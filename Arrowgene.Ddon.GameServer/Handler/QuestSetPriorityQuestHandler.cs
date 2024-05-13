using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestSetPriorityQuestHandler : GameStructurePacketHandler<C2SQuestSetPriorityQuestReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestSetPriorityQuestHandler));
        
        public QuestSetPriorityQuestHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestSetPriorityQuestReq> packet)
        {
            client.Send(new S2CQuestSetPriorityQuestRes()
            {
                QuestScheduleId = packet.Structure.QuestScheduleId
            });

#if false
            // TODO: This does not clear properly currently
            // TODO: need to debug why
            S2CQuestSetPriorityQuestNtc ntc = new S2CQuestSetPriorityQuestNtc()
            {
                CharacterId = client.Character.CharacterId
            };

            ntc.PriorityQuestList.Add(new CDataPriorityQuest()
            {
                QuestScheduleId = packet.Structure.QuestScheduleId,
                QuestId = packet.Structure.QuestScheduleId,
                QuestAnnounceList = new List<CDataQuestAnnounce>()
                {
                    new CDataQuestAnnounce(){ AnnounceNo = 0} // accept?
                }
            });
            client.Party.SendToAll(ntc);
#endif
        }
    }
}
