using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler;

public class QuestEndDistributionQuestCancelHandler : GameRequestPacketHandler<C2SQuestEndDistributionQuestCancelReq, S2CQuestEndDistributionQuestCancelRes>
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestEndDistributionQuestCancelHandler));

    public QuestEndDistributionQuestCancelHandler(DdonGameServer server) : base(server)
    {
    }

    public override S2CQuestEndDistributionQuestCancelRes Handle(GameClient client, C2SQuestEndDistributionQuestCancelReq request)
    {
        // TODO: Implement.
        return new();
    }
}
