using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler;

public class Quest_11_60_16_Handler : StructurePacketHandler<GameClient, C2S_QUEST_11_60_16_NTC>
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestEndDistributionQuestCancelHandler));

    public Quest_11_60_16_Handler(DdonGameServer server) : base(server)
    {
    }

    public override void Handle(GameClient client, StructurePacket<C2S_QUEST_11_60_16_NTC> packet)
    {
        // What to do?
    }
}
