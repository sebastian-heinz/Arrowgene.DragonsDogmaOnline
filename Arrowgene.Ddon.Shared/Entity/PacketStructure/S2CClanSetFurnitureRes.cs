using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanSetFurnitureRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_SET_FURNITURE_RES;

        public S2CClanSetFurnitureRes()
        {
        }


        public C2SClanSetFurnitureReq FurnitureUpdate { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanSetFurnitureRes>
        {
            public override void Write(IBuffer buffer, S2CClanSetFurnitureRes obj)
            {
                C2SClanSetFurnitureReq req = obj.FurnitureUpdate;
                WriteUInt64(buffer, 0);
                WriteUInt32(buffer, req.FurnitureId);
                WriteUInt32(buffer, req.LocationId);
            }

            public override S2CClanSetFurnitureRes Read(IBuffer buffer)
            {
                S2CClanSetFurnitureRes obj = new S2CClanSetFurnitureRes();
                return obj;
            }
        }

    }
}
