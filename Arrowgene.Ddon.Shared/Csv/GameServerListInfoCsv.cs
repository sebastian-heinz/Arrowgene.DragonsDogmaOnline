using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv;

public class GameServerListInfoCsv : CsvReaderWriter<ServerInfo>
{
    protected override int NumExpectedItems => 11;
    protected override ServerInfo CreateInstance(string[] properties)
    {
        if (!ushort.TryParse(properties[0], out ushort id)) return null;
        string name = properties[1];
        string brief = properties[2];
        if (!uint.TryParse(properties[3], out uint maxLoginNum)) return null;
        string address = properties[4];
        if (!ushort.TryParse(properties[5], out ushort port)) return null;
        if (!bool.TryParse(properties[6], out bool isHide)) return null;
        if (!ushort.TryParse(properties[7], out ushort rpcPort)) return null;
        string authToken = properties[8];
        if (!ushort.TryParse(properties[9], out ushort loginId)) return null;
        if (!bool.TryParse(properties[10], out bool preventLogin)) return null;

        return new ServerInfo()
        {
            Id = id,
            Name = name,
            Brief = brief,
            MaxLoginNum = maxLoginNum,
            LoginNum = 0, // Current Players
            Addr = address,
            Port = port,
            IsHide = isHide,
            RpcPort = rpcPort,
            RpcAuthToken = authToken,
            LoginId = loginId,
            PreventLogin = preventLogin,
        };
    }
}
