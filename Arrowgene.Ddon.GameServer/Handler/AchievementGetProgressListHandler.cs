using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler;

public class AchievementGetProgressListHandler : GameRequestPacketQueueHandler<C2SAchievementGetProgressListReq, S2CAchievementGetProgressListRes>
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AchievementGetProgressListHandler));

    public AchievementGetProgressListHandler(DdonGameServer server) : base(server)
    {
    }

    public override PacketQueue Handle(GameClient client, C2SAchievementGetProgressListReq request)
    {
        return Server.AchievementManager.CalculateProgress(client);
    }
}
