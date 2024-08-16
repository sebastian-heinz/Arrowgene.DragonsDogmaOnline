using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SNpcGetNpcExtendedFacilityReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_NPC_GET_NPC_EXTENDED_FACILITY_REQ;

        public NpcId NpcId { get; set; }

        public C2SNpcGetNpcExtendedFacilityReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SNpcGetNpcExtendedFacilityReq>
        {
            public override void Write(IBuffer buffer, C2SNpcGetNpcExtendedFacilityReq obj)
            {
                WriteUInt32(buffer, (uint) obj.NpcId);
            }

            public override C2SNpcGetNpcExtendedFacilityReq Read(IBuffer buffer)
            {
                C2SNpcGetNpcExtendedFacilityReq obj = new C2SNpcGetNpcExtendedFacilityReq();
                obj.NpcId = (NpcId) ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
