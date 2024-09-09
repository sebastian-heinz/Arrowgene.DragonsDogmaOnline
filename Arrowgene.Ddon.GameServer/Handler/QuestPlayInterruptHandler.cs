using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestPlayInterruptHandler : GameRequestPacketHandler<C2SQuestPlayInterruptReq, S2CQuestPlayInterruptRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestPlayEntryHandler));

        public QuestPlayInterruptHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestPlayInterruptRes Handle(GameClient client, C2SQuestPlayInterruptReq request)
        {
            client.Party.SendToAll(new S2CQuestPlayInterruptNtc()
            {
                CharacterId = client.Character.CharacterId,
                DeadlineSec = 60
            });

            return new S2CQuestPlayInterruptRes()
            {
                DeadlineSec = 60
            };
        }
    }
}
