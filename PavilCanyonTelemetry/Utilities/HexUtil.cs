using System;
using System.Text;

namespace PavilCanyonTelemetry.Utilities;

internal static class HexUtil
{
    public static string BytesToPrintableString(ReadOnlySpan<byte> bytes)
    {
        var sb = new StringBuilder(bytes.Length * 4);
        foreach (var b in bytes)
        {
            if (b >= 32 && b <= 126) sb.Append((char)b);
            else if (b == (byte)0x0d) sb.Append("\r");
            else if (b == (byte)0x0a) sb.Append("\n");
            else if (b == (byte)0x09) sb.Append("\t");
            else sb.Append($"<{b:X2}>");
        }
        return sb.ToString();
    }

    public static string ToHex(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length == 0) return string.Empty;
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes) sb.Append(b.ToString("X2"));
        return sb.ToString();
    }
}
