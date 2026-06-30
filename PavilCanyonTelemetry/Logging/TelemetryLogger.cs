using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace PavilCanyonTelemetry.Logging;

internal sealed class TelemetryLogger : IDisposable
{
    private readonly StreamWriter _log;
    private readonly StreamWriter _csvAll;
    private StreamWriter? _csvMatched;
    private readonly object _gate = new();

    public string BasePath { get; }
    public string LogPath { get; }

    public TelemetryLogger(string directory, string baseFileName)
    {
        Directory.CreateDirectory(directory);
        BasePath = Path.Combine(directory, baseFileName);
        LogPath = BasePath + ".log.txt";
        var csvPath = BasePath + ".csv";

        _log = new StreamWriter(LogPath, append: false, Encoding.UTF8) { AutoFlush = true };
        _csvAll = new StreamWriter(csvPath, append: false, Encoding.UTF8) { AutoFlush = true };
        WriteCsvHeader(_csvAll);
        _log.WriteLine($"PavilCanyonTelemetry log started {DateTime.Now:O}");
    }

    public void AppendLogLine(string line)
    {
        lock (_gate) { _log.WriteLine(line); }
    }

    public void AppendCsvRow(DateTime receivedAt, uint from, uint to, string fromName, string toName,
        uint channel, string portnum, string? text, string? telemetrySummary,
        int? rssi, float? snr, uint? hopLimit, uint? hopStart, bool? wantAck, bool? viaMqtt,
        string payloadHex, bool isMatch)
    {
        lock (_gate)
        {
            _csvAll.WriteLine(ToCsv(receivedAt, from, to, fromName, toName, channel, portnum, text, telemetrySummary,
                rssi, snr, hopLimit, hopStart, wantAck, viaMqtt, payloadHex, isMatch));

            if (isMatch)
            {
                if (_csvMatched == null)
                {
                    _csvMatched = new StreamWriter(BasePath + "-Device.csv", append: false, Encoding.UTF8) { AutoFlush = true };
                    WriteCsvHeader(_csvMatched);
                }
                _csvMatched.WriteLine(ToCsv(receivedAt, from, to, fromName, toName, channel, portnum, text, telemetrySummary,
                    rssi, snr, hopLimit, hopStart, wantAck, viaMqtt, payloadHex, isMatch));
            }
        }
    }

    private static void WriteCsvHeader(StreamWriter sw)
    {
        sw.WriteLine("ReceivedAt,From,To,FromName,ToName,Channel,PortNum,Text,TelemetrySummary,RxRssi,RxSnr,HopLimit,HopStart,WantAck,ViaMqtt,PayloadHex,DeviceIdMatch");
    }

    private static string ToCsv(DateTime receivedAt, uint from, uint to, string fromName, string toName,
        uint channel, string portnum, string? text, string? telemetrySummary,
        int? rssi, float? snr, uint? hopLimit, uint? hopStart, bool? wantAck, bool? viaMqtt,
        string payloadHex, bool isMatch)
    {
        static string Q(string s)
        {
            s ??= string.Empty;
            return "\"" + s.Replace("\"", "\"\"") + "\"";
        }

        return string.Join(",", new[]
        {
            Q(receivedAt.ToString("O", CultureInfo.InvariantCulture)),
            Q($"!{from:x8}"),
            Q($"!{to:x8}"),
            Q(fromName),
            Q(toName),
            channel.ToString(CultureInfo.InvariantCulture),
            Q(portnum),
            Q(text ?? string.Empty),
            Q(telemetrySummary ?? string.Empty),
            rssi?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
            snr?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
            hopLimit?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
            hopStart?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
            wantAck?.ToString() ?? string.Empty,
            viaMqtt?.ToString() ?? string.Empty,
            Q(payloadHex),
            isMatch ? "true" : "false"
        });
    }

    public void Dispose()
    {
        lock (_gate)
        {
            _csvMatched?.Dispose();
            _csvAll.Dispose();
            _log.Dispose();
        }
    }
}
