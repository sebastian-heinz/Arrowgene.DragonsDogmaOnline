using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

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
            // client.Send(new S2CQuestGetLightQuestListRes());

            S2CQuestGetLightQuestListRes res = new S2CQuestGetLightQuestListRes();
            foreach (var quest in client.Character.Quests)
            {
                if (quest.Value.QuestType == QuestType.PersonalQuest || quest.Value.QuestType == QuestType.ExtremeMissions)
                {
                    res.LightQuestList.Add(new CDataLightQuestList()
                    {
                        Param = new CDataQuestList()
                        {
                            QuestScheduleId = (uint)quest.Key,
                            QuestId = (uint)quest.Key
                        }
                    });
                }
            }

            client.Send(res);
        }
    }
}
