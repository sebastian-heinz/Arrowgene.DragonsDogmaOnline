using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestQuestOrderHandler : GameStructurePacketHandler<C2SQuestQuestOrderReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestQuestOrderHandler));

        public QuestQuestOrderHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestQuestOrderReq> packet)
        {
            var res = new S2CQuestQuestOrderRes();

            QuestId questId = (QuestId)packet.Structure.QuestScheduleId;
            var quest = client.Party.QuestState.GetQuest(questId);
            if (client.Party.QuestState.GetActiveQuestIds().Contains(questId))
            {
                var questState = client.Party.QuestState.GetQuestState(questId);
                res.QuestProcessStateList = quest.ToCDataQuestList(questState.Step).QuestProcessStateList;
            }
            else
            {
                Logger.Debug($"Quest q{questId} inactive.");
            }

            client.Send(res);
        }
    }
}
