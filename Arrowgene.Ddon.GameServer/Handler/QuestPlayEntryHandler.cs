using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestPlayEntryHandler : StructurePacketHandler<GameClient, C2SQuestPlayEntryReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestPlayEntryHandler));

        public QuestPlayEntryHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestPlayEntryReq> request)
        {
            client.Send(new S2CQuestPlayEntryRes());

            var ntc = new S2CQuestPlayEntryNtc()
            {
                CharacterId = client.Character.CharacterId
            };
            client.Party.SendToAll(ntc);
        }
    }
}
