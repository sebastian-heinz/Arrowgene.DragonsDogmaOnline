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
            if (client.Party.Clients.Count > 1)
            {
                Server.ContentManager.InitatePartyAbandonVote(client.Party, 60);
                Server.ContentManager.VoteToAbandon(client.Character, client.Party.Id, true);
            }

            client.Party.SendToAll(new S2CQuestPlayInterruptNtc()
            {
                DeadlineSec = 60
            });

            return new S2CQuestPlayInterruptRes()
            {
                DeadlineSec = 60
            };
        }
    }
}
