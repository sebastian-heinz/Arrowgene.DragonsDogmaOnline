using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

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
            Server.ContentManager.VoteToAbandon(client.Character, client.Party.Id, packet.IsAnswer);
            if (Server.ContentManager.VoteToAbandonPassed(client.Party.Id))
            {
                Server.ContentManager.CancelVoteToAbandonTimer(client.Party.Id);

                // TODO: There is supposed to be an NTC which
                // reports the response of the cancel operation
                // but I can't find it. This packet will give similar results
                // which basically allows the party to quit.
                Server.ContentManager.CancelTimer(client.Party.Id);
                client.Party.SendToAll(new S2CQuestPlayTimeupNtc());
            }

            return new S2CQuestPlayInterruptAnswerRes();
        }
    }
}
