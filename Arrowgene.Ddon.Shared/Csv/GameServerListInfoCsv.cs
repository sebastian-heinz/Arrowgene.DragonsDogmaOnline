using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv;

public class GameServerListInfoCsv : CsvReaderWriter<ServerInfo>
{
    protected override int NumExpectedItems => 12;
    protected override ServerInfo CreateInstance(string[] properties)
    {
        if (!ushort.TryParse(properties[0], out ushort id)) return null;
        string name = properties[1];
        string brief = properties[2];
        string trafficName = properties[3];
        if (!uint.TryParse(properties[4], out uint trafficLevel)) return null;
        if (!uint.TryParse(properties[5], out uint maxLoginNum)) return null;
        string address = properties[6];
        if (!ushort.TryParse(properties[7], out ushort port)) return null;
        if (!bool.TryParse(properties[8], out bool isHide)) return null;
        if (!ushort.TryParse(properties[9], out ushort rpcPort)) return null;
        string authToken = properties[10];
        if (!bool.TryParse(properties[11], out bool isHead)) return null;

        return new ServerInfo()
        {
            Id = id,
            Name = name,
            Brief = brief,
            TrafficName = trafficName,
            TrafficLevel = trafficLevel,
            MaxLoginNum = maxLoginNum,
            LoginNum = 0, // Current Players
            Addr = address,
            Port = port,
            IsHide = isHide,
            RpcPort = rpcPort,
            RpcAuthToken = authToken,
            IsHead = isHead
        };
    }
}
