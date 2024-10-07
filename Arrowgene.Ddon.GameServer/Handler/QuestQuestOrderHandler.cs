using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
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

            if (QuestManager.IsQuestEnabled(questId))
            {
                var quest = QuestManager.GetQuest(questId);

                uint step = 0;
                if (client.Party.QuestState.GetActiveQuestIds().Contains(questId))
                {
                    var questState = client.Party.QuestState.GetQuestState(questId);
                    step = questState.Step;
                }
                else
                {
                    client.Party.QuestState.AddNewQuest(quest, 0);
                }
                res.QuestProcessStateList = quest.ToCDataQuestList(step).QuestProcessStateList;
            }
            else
            {
                Logger.Debug($"Quest q{questId} inactive.");
            }

            client.Send(res);
        }
    }
}
