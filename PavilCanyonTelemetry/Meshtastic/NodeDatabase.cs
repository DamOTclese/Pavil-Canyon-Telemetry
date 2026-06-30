using System.Collections.Concurrent;
using Meshtastic.Protobufs;

namespace PavilCanyonTelemetry.Meshtastic;

internal sealed class NodeDatabase
{
    private readonly ConcurrentDictionary<uint, NodeInfo> _nodes = new();

    public void Upsert(NodeInfo nodeInfo) => _nodes[nodeInfo.Num] = nodeInfo;
    public bool TryGet(uint nodeNum, out NodeInfo? nodeInfo) => _nodes.TryGetValue(nodeNum, out nodeInfo);
    public NodeInfo[] Snapshot() => System.Linq.Enumerable.ToArray(_nodes.Values);

    public static string GetDisplayName(NodeInfo? ni)
    {
        if (ni?.User == null) return string.Empty;
        if (!string.IsNullOrWhiteSpace(ni.User.LongName)) return ni.User.LongName;
        if (!string.IsNullOrWhiteSpace(ni.User.ShortName)) return ni.User.ShortName;
        return string.Empty;
    }
}
