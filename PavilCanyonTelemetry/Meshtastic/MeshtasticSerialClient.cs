using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Meshtastic.Protobufs;

namespace PavilCanyonTelemetry.Meshtastic;

internal sealed class MeshtasticSerialClient : IDisposable
{
    private const byte START1 = 0x94;
    private const byte START2 = 0xC3;
    private const int MAX_LEN = 512;

    private SerialPort? _port;
    private CancellationTokenSource? _cts;

    public bool IsConnected => _port?.IsOpen == true;

    public event Action<string>? DebugText;
    public event Action<FromRadio>? FromRadioReceived;
    public event Action<Exception>? ConnectionError;

    public void Connect(string portName)
    {
        if (IsConnected) return;
        _cts = new CancellationTokenSource();
        _port = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One)
        {
            Handshake = Handshake.None,
            ReadTimeout = 500,
            WriteTimeout = 500,
            DtrEnable = true,
            RtsEnable = true
        };
        _port.Open();
        _ = Task.Run(() => ReaderLoop(_cts.Token));
    }

    public void Disconnect()
    {
        try { _cts?.Cancel(); } catch { }
        try
        {
            if (_port != null)
            {
                if (_port.IsOpen) _port.Close();
                _port.Dispose();
            }
        }
        catch { }
        _port = null;
        _cts = null;
    }

    public void Send(ToRadio toRadio)
    {
        if (_port == null || !_port.IsOpen) throw new InvalidOperationException("Serial port not open");
        var payload = toRadio.ToByteArray();
        if (payload.Length > MAX_LEN) throw new InvalidOperationException("ToRadio payload too large");

        var header = new byte[] { START1, START2, (byte)((payload.Length >> 8) & 0xFF), (byte)(payload.Length & 0xFF) };
        _port.Write(header, 0, header.Length);
        _port.Write(payload, 0, payload.Length);
    }

    public void RequestConfigDump(uint wantConfigId) => Send(new ToRadio { WantConfigId = wantConfigId });

    public void SendText(uint destinationNodeNum, string text, bool wantAck)
    {
        var data = new Data { Portnum = PortNum.TextMessageApp, Payload = ByteString.CopyFromUtf8(text) };
        var pkt = new MeshPacket { To = destinationNodeNum, Channel = 0, WantAck = wantAck, Decoded = data };
        Send(new ToRadio { Packet = pkt });
    }

    private void ReaderLoop(CancellationToken ct)
    {
        try
        {
            if (_port == null) return;
            var state = 0;
            var header = new byte[4];
            byte[] payload = Array.Empty<byte>();
            var payloadLen = 0;
            var pos = 0;

            while (!ct.IsCancellationRequested)
            {
                int b;
                try { b = _port.ReadByte(); }
                catch (TimeoutException) { continue; }
                if (b < 0) continue;
                var by = (byte)b;

                switch (state)
                {
                    case 0:
                        if (by == START1) { header[0] = by; state = 1; }
                        else
                        {
                            if (Program.ourForm.GetCheckboxIncludeDebug() == true)
                            {
                                DebugText?.Invoke(((char)by).ToString());
                            }
                        }
                        break;
                    case 1:
                        if (by == START2) { header[1] = by; state = 2; }
                        else 
                        {
                            if (Program.ourForm.GetCheckboxIncludeDebug() == true)
                            {
                                DebugText?.Invoke(((char)header[0]).ToString()); DebugText?.Invoke(((char)by).ToString()); state = 0;
                            }
                        }
                        break;
                    case 2:
                        header[2] = by; state = 3; break;
                    case 3:
                        header[3] = by;
                        payloadLen = (header[2] << 8) | header[3];
                        if (payloadLen <= 0 || payloadLen > MAX_LEN) { state = 0; break; }
                        payload = new byte[payloadLen];
                        pos = 0;
                        state = 4;
                        break;
                    case 4:
                        payload[pos++] = by;
                        if (pos >= payloadLen)
                        {
                            try { FromRadioReceived?.Invoke(FromRadio.Parser.ParseFrom(payload)); }
                            catch (Exception ex) { ConnectionError?.Invoke(ex); }
                            state = 0;
                        }
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            ConnectionError?.Invoke(ex);
        }
    }

    public void Dispose() => Disconnect();
}
