using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
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
                Server.PartyQuestContentManager.InitatePartyAbandonVote(client, client.Party, 60);
                Server.PartyQuestContentManager.VoteToAbandon(client.Character, client.Party.Id, VoteAnswer.Agree);
                client.Party.SendToAllExcept(new S2CQuestPlayInterruptNtc()
                {
                    CharacterId = client.Character.CharacterId,
                    DeadlineSec = 60
                }, client);
            }
            else
            {
                client.Party.SendToAll(new S2CQuestPlayInterruptAnswerNtc() { IsInterrupt = true });
            }

            return new S2CQuestPlayInterruptRes()
            {
                DeadlineSec = 60
            };
        }
    }
}
