using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SAreaGetAreaQuestHintListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_AREA_GET_AREA_QUEST_HINT_LIST_REQ;

        public QuestAreaId AreaId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SAreaGetAreaQuestHintListReq>
        {
            public override void Write(IBuffer buffer, C2SAreaGetAreaQuestHintListReq obj)
            {
                WriteUInt32(buffer, (uint)obj.AreaId);
            }

            public override C2SAreaGetAreaQuestHintListReq Read(IBuffer buffer)
            {
                C2SAreaGetAreaQuestHintListReq obj = new C2SAreaGetAreaQuestHintListReq();
                obj.AreaId = (QuestAreaId)ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
