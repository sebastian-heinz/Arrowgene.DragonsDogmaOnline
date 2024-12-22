using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SAreaGetSpotInfoListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_AREA_GET_SPOT_INFO_LIST_REQ;

        public uint AreaId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SAreaGetSpotInfoListReq>
        {
            public override void Write(IBuffer buffer, C2SAreaGetSpotInfoListReq obj)
            {
                WriteUInt32(buffer, obj.AreaId);
            }

            public override C2SAreaGetSpotInfoListReq Read(IBuffer buffer)
            {
                C2SAreaGetSpotInfoListReq obj = new C2SAreaGetSpotInfoListReq();
                obj.AreaId = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
