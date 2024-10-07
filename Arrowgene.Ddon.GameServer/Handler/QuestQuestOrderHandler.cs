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

            if (QuestManager.IsQuestEnabled(packet.Structure.QuestScheduleId))
            {
                uint step;
                var quest = client.Party.QuestState.GetQuest(packet.Structure.QuestScheduleId);
                if (client.Party.QuestState.GetActiveQuestScheduleIds().Contains(packet.Structure.QuestScheduleId))
                {
                    var questState = client.Party.QuestState.GetQuestState(quest.QuestScheduleId);
                    step = questState.Step;
                 }
                else
                {
                    step = 0;
                    client.Party.QuestState.AddNewQuest(quest, 0);
                }
                res.QuestProcessStateList = quest.ToCDataQuestList(step).QuestProcessStateList;
            }
            else
            {
                Logger.Debug($"Quest '{packet.Structure.QuestScheduleId}' inactive.");
            }

            client.Send(res);
        }
    }
}
