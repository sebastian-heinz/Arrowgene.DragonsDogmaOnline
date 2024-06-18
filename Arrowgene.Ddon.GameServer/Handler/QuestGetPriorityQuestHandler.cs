using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using YamlDotNet.Serialization;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetPriorityQuestHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetPriorityQuestHandler));


        public QuestGetPriorityQuestHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_QUEST_GET_PRIORITY_QUEST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            // client.Send(GameFull.Dump_144);
            S2CQuestGetPriorityQuestRes res = new S2CQuestGetPriorityQuestRes();

            var partyLeader = client.Party.Leader;

            CDataPriorityQuestSetting setting = new CDataPriorityQuestSetting();
            setting.CharacterId = partyLeader.Client.Character.CharacterId;

            var priorityQuests = Server.Database.GetPriorityQuests(partyLeader.Client.Character.CommonId);
            foreach (var questId in priorityQuests)
            {
                var quest = QuestManager.GetQuest(questId);
                var questState = client.Party.QuestState.GetQuestState(quest);
                setting.PriorityQuestList.Add(quest.ToCDataPriorityQuest(questState.Step));
            }

            res.PriorityQuestSettingsList.Add(setting);

            client.Send(res);
        }
    }
}
