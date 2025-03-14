using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SAreaAreaRankUpReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_AREA_AREA_RANK_UP_REQ;

        public QuestAreaId AreaId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SAreaAreaRankUpReq>
        {
            public override void Write(IBuffer buffer, C2SAreaAreaRankUpReq obj)
            {
                WriteUInt32(buffer, (uint)obj.AreaId);
            }

            public override C2SAreaAreaRankUpReq Read(IBuffer buffer)
            {
                C2SAreaAreaRankUpReq obj = new C2SAreaAreaRankUpReq();
                obj.AreaId = (QuestAreaId)ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
