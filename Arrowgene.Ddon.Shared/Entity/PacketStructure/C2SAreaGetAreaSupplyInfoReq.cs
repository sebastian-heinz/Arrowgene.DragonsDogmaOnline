using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SAreaGetAreaSupplyInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_AREA_GET_AREA_SUPPLY_INFO_REQ;

        public QuestAreaId AreaId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SAreaGetAreaSupplyInfoReq>
        {
            public override void Write(IBuffer buffer, C2SAreaGetAreaSupplyInfoReq obj)
            {
                WriteUInt32(buffer, (uint)obj.AreaId);
            }

            public override C2SAreaGetAreaSupplyInfoReq Read(IBuffer buffer)
            {
                C2SAreaGetAreaSupplyInfoReq obj = new C2SAreaGetAreaSupplyInfoReq();
                obj.AreaId = (QuestAreaId)ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
