using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetLightQuestList : GameStructurePacketHandler<C2SQuestGetLightQuestListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetLightQuestList));
        
        public QuestGetLightQuestList(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestGetLightQuestListReq> packet)
        {
            S2CQuestGetLightQuestListRes res = new S2CQuestGetLightQuestListRes();

            res.BaseId = packet.Structure.BaseId;
            res.NotCompleteQuestNum = 0;
            res.GpCompleteEnable = false;
            res.GpCompletePriceGp = 10;
            res.LightQuestList = new List<CDataLightQuestList>();

            var activeQuests = client.Party.QuestState.GetActiveQuestIds();
            var quests = QuestManager.GetQuestsByType(QuestType.Light);
            Logger.Info($"{quests.Count}");
            foreach (var quest in quests)
            {
                if (activeQuests.Contains(quest.Key))
                {
                    continue;
                }
                if (!QuestManager.IsBoardQuest(quest.Key))
                {
                    continue;
                }
                res.LightQuestList.Add(quest.Value.ToCDataLightQuestList(0));
            }

            client.Send(res);
        }
    }
}
