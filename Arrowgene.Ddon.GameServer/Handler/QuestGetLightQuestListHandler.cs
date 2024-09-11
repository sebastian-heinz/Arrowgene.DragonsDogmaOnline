using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetLightQuestList : GameRequestPacketHandler<C2SQuestGetLightQuestListReq, S2CQuestGetLightQuestListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetLightQuestList));
        
        public QuestGetLightQuestList(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetLightQuestListRes Handle(GameClient client, C2SQuestGetLightQuestListReq request)
        {
            S2CQuestGetLightQuestListRes res = new S2CQuestGetLightQuestListRes();

            res.BaseId = request.BaseId;

            // TODO: Investigate these values.
            res.NotCompleteQuestNum = 10;
            res.GpCompleteEnable = false;
            res.GpCompletePriceGp = 1;

            res.LightQuestList = new List<CDataLightQuestList>();
            var quests = QuestManager.GetQuestsByType(QuestType.Light).Where(x => x.Value.LightQuestDetail.BaseAreaPoint == request.BaseId);
            var completionStats = Server.Database.GetCompletedQuestsByType(client.Character.CommonId, QuestType.Light).ToDictionary(x => x.QuestId, x => x.ClearCount);
            foreach (var quest in quests)
            {
                var lightQuest = quest.Value.ToCDataLightQuestList(1); // Step 1 has the necessary info that the UI is looking for, not step 0.
                lightQuest.Detail.ClearNum = completionStats.GetValueOrDefault(quest.Key);
                res.LightQuestList.Add(lightQuest);
            }

            return res;
        }
    }
}
