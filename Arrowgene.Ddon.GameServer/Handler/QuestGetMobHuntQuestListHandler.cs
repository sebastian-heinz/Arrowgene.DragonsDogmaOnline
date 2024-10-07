using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Data.Entity.Core.Mapping;
using System.Dynamic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetMobHuntQuestListHandler : GameRequestPacketHandler<C2SQuestGetMobHuntQuestListReq, S2CQuestGetMobHuntQuestListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetMobHuntQuestListHandler));

        public QuestGetMobHuntQuestListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetMobHuntQuestListRes Handle(GameClient client, C2SQuestGetMobHuntQuestListReq request)
        {
            var quests = QuestManager.GetQuestsByType(QuestType.WildHunt);

            return new S2CQuestGetMobHuntQuestListRes()
            {
                ConfidenceLevel = 0,
                WildHuntCount = 3,
                QuestList = quests
                    .Where(x => !client.Party.QuestState.HasActiveQuest(x.Value.QuestId))
                    .Select(x => x.Value.ToCDataQuestMobHuntQuestInfo(0))
                    .ToList()
            };
        }
    }
}

