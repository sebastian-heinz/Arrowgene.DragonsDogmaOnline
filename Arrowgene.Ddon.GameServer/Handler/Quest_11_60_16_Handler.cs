using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler;

public class Quest_11_60_16_Handler : PacketHandler<GameClient>
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestEndDistributionQuestCancelHandler));


    public Quest_11_60_16_Handler(DdonGameServer server) : base(server)
    {
    }

    public override PacketId Id => PacketId.C2S_QUEST_11_60_16_NTC;

    public override void Handle(GameClient client, IPacket packet)
    {
        // What to do?
    }
}
