using System;

namespace PavilCanyonTelemetry.Utilities;

internal static class DeviceIdMatcher
{
    public static bool MatchesLongName(string configuredLongName, string? packetDestinationLongName)
    {
        if (string.IsNullOrWhiteSpace(configuredLongName)) return false;
        if (string.IsNullOrWhiteSpace(packetDestinationLongName)) return false;
        return string.Equals(configuredLongName.Trim(), packetDestinationLongName.Trim(), StringComparison.OrdinalIgnoreCase);
    }
}
