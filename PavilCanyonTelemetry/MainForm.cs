using System;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Meshtastic.Protobufs;
using PavilCanyonTelemetry.Logging;
using PavilCanyonTelemetry.Meshtastic;
using PavilCanyonTelemetry.Properties;
using PavilCanyonTelemetry.Utilities;
using System.Collections.Concurrent;

namespace PavilCanyonTelemetry;

public partial class MainForm : Form
{
    private readonly MeshtasticSerialClient _client = new();
    private readonly NodeDatabase _nodedb = new();
    private TelemetryLogger? _logger;
    private readonly System.Windows.Forms.Timer _refreshTimer;
    private uint _wantConfigId = 1;
    private readonly ConcurrentQueue<string> _uiQueue = new();
    private readonly System.Windows.Forms.Timer _uiFlushTimer = new();
    private const int MaxCharsInConsole = 500_000; // Prevent runaway memory
    private int _nodeDbDirty; // Flag is either 0 or 1

    public MainForm()
    {
        InitializeComponent();

        // Icon = new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppIcon.ico"));
        Icon = new System.Drawing.Icon(Resources.AppIcon, new Size(32, 32));
        BackColor = Color.FromArgb(12, 28, 34);
        txtConsole.BackColor = Color.FromArgb(10, 20, 28);
        txtConsole.ForeColor = Color.FromArgb(190, 230, 235);
        txtSend.BackColor = Color.FromArgb(16, 34, 44);
        txtSend.ForeColor = Color.FromArgb(220, 245, 250);

        _client.DebugText += s => { _uiQueue.Enqueue(HexUtil.BytesToPrintableString(Encoding.UTF8.GetBytes(s))); };
        _client.FromRadioReceived += fr => ProcessFromRadio(DateTime.Now, fr); 
        _client.ConnectionError += ex => Ui(() => AppendLine($"[ERROR] {ex.Message}"));

        // Start the User Interface flush timer
        _uiFlushTimer.Interval = 100; // 10 HZ UI updates
        _uiFlushTimer.Tick += (_, _) => FlushUiQueue();
        _uiFlushTimer.Start();

        lblDeviceIdValue.Text = Settings.Default.DeviceId;
        PopulatePorts();

        _refreshTimer = new System.Windows.Forms.Timer { Interval = (int)TimeSpan.FromMinutes(10).TotalMilliseconds };
        _refreshTimer.Tick += (_, _) => RequestRefresh();

        Shown += (_, _) =>
        {
            if (Settings.Default.AutoConnect)
            {
                var p = Settings.Default.SerialPort;
                if (!string.IsNullOrWhiteSpace(p) && cmbPorts.Items.Contains(p))
                {
                    cmbPorts.SelectedItem = p;
                    TryConnect();
                }
            }
        };
    }

    private string BuildConsoleLine(DateTime receivedAt, MeshPacket pkt)
    {
        bool bWasText = false;

        // NOTE: This method must NOT touch UI controls.
        // It can read _nodedb and parse payloads, but should only RETURN a string.

        var from = pkt.From;
        var to = pkt.To;

        _nodedb.TryGet(from, out var fromNi);
        _nodedb.TryGet(to, out var toNi);

        var fromName = NodeDatabase.GetDisplayName(fromNi);
        var toName = NodeDatabase.GetDisplayName(toNi);

        if (string.IsNullOrWhiteSpace(fromName)) fromName = $"!{from:x8}";
        if (string.IsNullOrWhiteSpace(toName)) toName = $"!{to:x8}";

        var portName =
            pkt.Decoded != null ? pkt.Decoded.Portnum.ToString()
            : (pkt.Encrypted != null && pkt.Encrypted.Length > 0 ? "ENCRYPTED" : "UNKNOWN");

        string? text = null;
        string? telemetrySummary = null;

        if (pkt.Decoded != null)
        {
            // Text packets
            if (pkt.Decoded.Portnum == PortNum.TextMessageApp ||
                pkt.Decoded.Portnum == PortNum.TextMessageCompressedApp)
            {
                text = pkt.Decoded.Payload.ToStringUtf8();

                // We check to see if the text only checkbox is set
                bWasText = true;
            }
            // Telemetry packets
            else if (pkt.Decoded.Portnum == PortNum.TelemetryApp)
            {
                try
                {
                    var tel = Telemetry.Parser.ParseFrom(pkt.Decoded.Payload);
                    var parts = new System.Collections.Generic.List<string>();

                    if (tel.DeviceMetrics != null)
                    {
                        if (tel.DeviceMetrics.BatteryLevel != 0) parts.Add($"Bat={tel.DeviceMetrics.BatteryLevel}%");
                        if (tel.DeviceMetrics.Voltage != 0) parts.Add($"V={tel.DeviceMetrics.Voltage:F2}");
                    }

                    if (tel.EnvironmentMetrics != null)
                    {
                        if (tel.EnvironmentMetrics.Temperature != 0) parts.Add($"T={tel.EnvironmentMetrics.Temperature:F1}C");
                        if (tel.EnvironmentMetrics.RelativeHumidity != 0) parts.Add($"RH={tel.EnvironmentMetrics.RelativeHumidity:F1}%");
                    }

                    telemetrySummary = parts.Count > 0 ? string.Join(" ", parts) : "Telemetry";
                }
                catch
                {
                    telemetrySummary = "Telemetry (parse failed)";
                }
            }
        }

        // basic hop display
        var hopLimit = pkt.HopLimit;
        var hopStart = pkt.HopStart;

        // RSSI is typically int-ish in Meshtastic protobufs; SNR is float-ish
        var line =
            $"[{receivedAt:HH:mm:ss}] {fromName}(!{from:x8}) -> {toName}(!{to:x8}) " +
            $"{portName} RSSI={pkt.RxRssi} SNR={pkt.RxSnr:F1} Hops={hopLimit}/{hopStart}";

        // Is the Text Only checkbox checked?
        if (Program.ourForm.checkBoxTextOnly.Checked && !bWasText)
        {
            // It s not text and also the checkbox for Text Only is checked so don't return valid string
            return null;
        }

        if (!string.IsNullOrWhiteSpace(text))
            line += $" : {text}";
        else if (!string.IsNullOrWhiteSpace(telemetrySummary))
            line += $" : {telemetrySummary}";

        return line;
    }

    private void ProcessFromRadio(DateTime receivedAt, FromRadio fr)
    {
        // Log to file is fine here (if your logger is thread-safe)
        _logger?.AppendLogLine($"{receivedAt:O} FromRadio: {fr}");

        // Update NodeDB (thread-safe dictionary recommended)
        if (fr.NodeInfo != null)
        {
            _nodedb.Upsert(fr.NodeInfo);

            // Tell UI it needs to rebuild the node radio buttons,
            // but don't do it here.
            System.Threading.Interlocked.Exchange(ref _nodeDbDirty, 1);
        }

        // Prepare strings for UI (enqueue only)
        if (fr.Packet != null)
        {
            var line = BuildConsoleLine(receivedAt, fr.Packet);

            // fredr
            if (line != null)
            {
                _uiQueue.Enqueue(line + Environment.NewLine);
            }
        }

        // If you need to update header (HW/FW/MyNode), do it with a UI request flag
        // OR enqueue a small action (see optional upgrade below).
    }

    private void FlushUiQueue()
    {
        // 1) Flush text in chunks
        if (!_uiQueue.IsEmpty)
        {
            var sb = new StringBuilder(64_000);

            while (_uiQueue.TryDequeue(out var chunk))
            {
                sb.Append(chunk);

                // keep one append bounded so UI stays responsive
                if (sb.Length >= 64_000)
                    break;
            }

            txtConsole.AppendText(sb.ToString());

            // Trim old text (optional but strongly recommended)
            if (txtConsole.TextLength > MaxCharsInConsole)
            {
                txtConsole.SelectionStart = 0;
                txtConsole.SelectionLength = txtConsole.TextLength - (MaxCharsInConsole / 2);
                txtConsole.SelectedText = "";
            }
        }

        // Refresh node list at most once per tick when dirty
        if (System.Threading.Interlocked.Exchange(ref _nodeDbDirty, 0) == 1)
        {
            RefreshNodeButtons();
        }
    }

    private void Ui(Action a)
    {
        if (IsDisposed) return;
        if (InvokeRequired) BeginInvoke(a);
        else a();
    }

    private void AppendLine(string line) => _uiQueue.Enqueue(line + Environment.NewLine);

    private void PopulatePorts()
    {
        cmbPorts.Items.Clear();
        foreach (var port in SerialPort.GetPortNames().OrderBy(p => p))
            cmbPorts.Items.Add(port);
        if (cmbPorts.Items.Count > 0 && cmbPorts.SelectedIndex < 0)
            cmbPorts.SelectedIndex = 0;
    }

    private void RequestRefresh()
    {
        if (!_client.IsConnected) return;
        _wantConfigId++;
        _client.RequestConfigDump(_wantConfigId);

        if (Program.ourForm.checkBoxTextOnly.Checked == false)
            AppendLine($"[INFO] Requested config refresh id={_wantConfigId}");
    }

    /// <summary>
    /// fredr
    /// </summary>
    /// <param name="receivedAt"></param>
    /// <param name="fr"></param>
    private void HandleFromRadio(DateTime receivedAt, FromRadio fr)
    {
        _logger?.AppendLogLine($"{receivedAt:O} FromRadio: {fr}");

        if (fr.Metadata != null)
        {
            lblHwValue.Text = string.IsNullOrWhiteSpace(fr.Metadata.HwModel.ToString()) ? "(unknown)" : fr.Metadata.HwModel.ToString();
            lblFwValue.Text = string.IsNullOrWhiteSpace(fr.Metadata.FirmwareVersion) ? "(unknown)" : fr.Metadata.FirmwareVersion;
        }

        if (fr.MyInfo != null)
            lblMyNodeValue.Text = $"!{fr.MyInfo.MyNodeNum:x8}";

        if (fr.NodeInfo != null)
        {
            _nodedb.Upsert(fr.NodeInfo);
            RefreshNodeButtons();
        }

        if (fr.Packet != null)
            HandleMeshPacket(receivedAt, fr.Packet);
    }

    private void HandleMeshPacket(DateTime receivedAt, MeshPacket pkt)
    {
        var from = pkt.From;
        var to = pkt.To;

        _nodedb.TryGet(from, out var fromNi);
        _nodedb.TryGet(to, out var toNi);

        var fromName = NodeDatabase.GetDisplayName(fromNi);
        var toName = NodeDatabase.GetDisplayName(toNi);

        var portnum = pkt.Decoded?.Portnum.ToString() ?? (pkt.Encrypted.Length > 0 ? "ENCRYPTED" : "UNKNOWN");

        string? text = null;
        string? telemetrySummary = null;

        if (pkt.Decoded != null)
        {
            if (pkt.Decoded.Portnum == PortNum.TextMessageApp || pkt.Decoded.Portnum == PortNum.TextMessageCompressedApp)
            {
                text = pkt.Decoded.Payload.ToStringUtf8();
            }
            else if (pkt.Decoded.Portnum == PortNum.TelemetryApp)
            {
                try
                {
                    var tel = Telemetry.Parser.ParseFrom(pkt.Decoded.Payload);
                    var parts = new System.Collections.Generic.List<string>();
                    if (tel.DeviceMetrics != null)
                    {
                        if (tel.DeviceMetrics.BatteryLevel != 0) parts.Add($"Bat={tel.DeviceMetrics.BatteryLevel}%");
                        if (tel.DeviceMetrics.Voltage != 0) parts.Add($"V={tel.DeviceMetrics.Voltage:F2}");
                    }
                    if (tel.EnvironmentMetrics != null)
                    {
                        if (tel.EnvironmentMetrics.Temperature != 0) parts.Add($"T={tel.EnvironmentMetrics.Temperature:F1}C");
                        if (tel.EnvironmentMetrics.RelativeHumidity != 0) parts.Add($"RH={tel.EnvironmentMetrics.RelativeHumidity:F1}%");
                    }
                    telemetrySummary = parts.Count > 0 ? string.Join(" ", parts) : "Telemetry";
                }
                catch { telemetrySummary = "Telemetry (parse failed)"; }
            }
        }

        var payloadHex = pkt.Decoded != null ? HexUtil.ToHex(pkt.Decoded.Payload.Span) : HexUtil.ToHex(pkt.Encrypted.Span);
        var isMatch = DeviceIdMatcher.MatchesLongName(Settings.Default.DeviceId, toName);

        var line = $"[{receivedAt:HH:mm:ss}] {fromName}(!{from:x8}) -> {toName}(!{to:x8}) {portnum} RSSI={pkt.RxRssi} SNR={pkt.RxSnr:F1} Hops={pkt.HopLimit}/{pkt.HopStart}";
        if (!string.IsNullOrWhiteSpace(text)) line += $" : {text}";
        else if (!string.IsNullOrWhiteSpace(telemetrySummary)) line += $" : {telemetrySummary}";
        AppendLine(line);

        _logger?.AppendCsvRow(receivedAt, from, to, fromName, toName, pkt.Channel, portnum, text, telemetrySummary,
            pkt.RxRssi, pkt.RxSnr, pkt.HopLimit, pkt.HopStart, pkt.WantAck, pkt.ViaMqtt, payloadHex, isMatch);
    }

    private void RefreshNodeButtons()
    {
        var selected = GetSelectedDestination();
        flowNodes.SuspendLayout();
        flowNodes.Controls.Clear();

        var rbBroadcast = new RadioButton { AutoSize = true, Text = "@LongFast (Broadcast)  !FFFFFFFF", Tag = 0xFFFFFFFFu, Checked = selected == 0xFFFFFFFFu };
        flowNodes.Controls.Add(rbBroadcast);

        foreach (var ni in _nodedb.Snapshot().OrderBy(n => n.User?.LongName ?? n.User?.ShortName ?? string.Empty))
        {
            var name = NodeDatabase.GetDisplayName(ni);
            if (string.IsNullOrWhiteSpace(name)) name = $"!{ni.Num:x8}";
            var rb = new RadioButton { AutoSize = true, Text = $"{name}  !{ni.Num:x8}", Tag = ni.Num, Checked = selected == ni.Num };
            flowNodes.Controls.Add(rb);
        }

        if (!flowNodes.Controls.OfType<RadioButton>().Any(r => r.Checked)) rbBroadcast.Checked = true;
        flowNodes.ResumeLayout();
    }

    private uint GetSelectedDestination()
    {
        var rb = flowNodes.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
        return rb?.Tag is uint u ? u : 0xFFFFFFFFu;
    }

    private void TryConnect()
    {
        if (_client.IsConnected) return;
        if (cmbPorts.SelectedItem == null) { AppendLine("[WARN] No COM port selected."); return; }

        var port = cmbPorts.SelectedItem.ToString()!;
        _client.Connect(port);
        Settings.Default.SerialPort = port;
        Settings.Default.Save();

        btnConnect.Enabled = false;
        btnDisconnect.Enabled = true;

        var stamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        var baseName = $"PavilCanyon-{stamp}";
        var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        _logger = new TelemetryLogger(dir, baseName);
        lblLogValue.Text = Path.GetFileName(_logger.LogPath);

        AppendLine($"[INFO] Connected to {port}. Logging to .\\Logs\\{baseName}.*");
        _client.RequestConfigDump(_wantConfigId);
        _refreshTimer.Start();
    }

    private void Disconnect()
    {
        _refreshTimer.Stop();
        _client.Disconnect();
        _logger?.Dispose();
        _logger = null;
        btnConnect.Enabled = true;
        btnDisconnect.Enabled = false;
        AppendLine("[INFO] Disconnected.");
    }

    private void menuExit_Click(object sender, EventArgs e) => Close();

    private void menuDeviceId_Click(object sender, EventArgs e)
    {
        var dlg = new DeviceIdForm(Settings.Default.DeviceId);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            Settings.Default.DeviceId = dlg.DeviceId;
            Settings.Default.Save();
            lblDeviceIdValue.Text = Settings.Default.DeviceId;
            AppendLine($"[INFO] Device ID (Long Name) set to '{Settings.Default.DeviceId}'");
        }
    }

    private void btnConnect_Click(object sender, EventArgs e)
    {
        try { TryConnect(); }
        catch (Exception ex) { AppendLine($"[ERROR] Connect failed: {ex.Message}"); }
    }

    private void btnDisconnect_Click(object sender, EventArgs e) => Disconnect();
    private void btnRefreshPorts_Click(object sender, EventArgs e) => PopulatePorts();
    private void btnClear_Click(object sender, EventArgs e) => txtConsole.Clear();

    private void txtSend_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.SuppressKeyPress = true;
        var msg = txtSend.Text.Trim();
        if (msg.Length == 0) return;
        var dest = GetSelectedDestination();
        var wantAck = dest != 0xFFFFFFFFu;

        try
        {
            _client.SendText(dest, msg, wantAck);
            var destLabel = dest == 0xFFFFFFFFu ? "@LongFast" : $"!{dest:x8}";
            AppendLine($"[TX] -> {destLabel} {(wantAck ? "(want_ack)" : string.Empty)}: {msg}");
            _logger?.AppendLogLine($"{DateTime.Now:O} TX to {destLabel}: {msg}");
            txtSend.Clear();
        }
        catch (Exception ex)
        {
            AppendLine($"[ERROR] Send failed: {ex.Message}");
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        try { Disconnect(); } catch { }
        base.OnFormClosing(e);
    }

    /// <summary>
    /// When the main form is clicked on
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void flowNodes_Paint(object sender, PaintEventArgs e)
    {

    }
}
