using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestPlayInterruptAnswerHandler : GameRequestPacketHandler<C2SQuestPlayInterruptAnswerReq, S2CQuestPlayInterruptAnswerRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestPlayInterruptAnswerHandler));

        public QuestPlayInterruptAnswerHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestPlayInterruptAnswerRes Handle(GameClient client, C2SQuestPlayInterruptAnswerReq packet)
        {
            Server.PartyQuestContentManager.VoteToAbandon(client.Character, client.Party.Id, packet.IsAnswer ? VoteAnswer.Agree : VoteAnswer.Disagree);
            if (Server.PartyQuestContentManager.AllMembersVoted(client.Party.Id))
            {
                if (Server.PartyQuestContentManager.VotedPassed(client.Party.Id))
                {
                    Server.PartyQuestContentManager.CancelVoteToAbandonTimer(client.Party.Id);
                    Server.PartyQuestContentManager.CancelTimer(client.Party.Id);
                    client.Party.SendToAll(new S2CQuestPlayInterruptAnswerNtc() { IsInterrupt = true });
                }
                else
                {
                    Server.PartyQuestContentManager.CancelVoteToAbandonTimer(client.Party.Id);
                    client.Party.SendToAll(new S2CQuestPlayInterruptAnswerNtc() { IsInterrupt = false });
                }
            }

            return new S2CQuestPlayInterruptAnswerRes();
        }
    }
}
