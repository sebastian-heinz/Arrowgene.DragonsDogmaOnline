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

            foreach (var questId in partyLeader.Client.Character.PriorityQuests)
            {
                // TODO: Currently we are using the questId as the schedule
                // TODO: ID but we might want to make that unique at some point.
                setting.PriorityQuestList.Add(new CDataPriorityQuest()
                {
                    QuestId = (uint) questId,
                    QuestScheduleId = (uint) questId,
                });
            }

            res.PriorityQuestSettingsList.Add(setting);

            client.Send(res);
        }
    }
}
