using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanSetFurnitureRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_SET_FURNITURE_RES;

        public S2CClanSetFurnitureRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CClanSetFurnitureRes>
        {
            public override void Write(IBuffer buffer, S2CClanSetFurnitureRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CClanSetFurnitureRes Read(IBuffer buffer)
            {
                S2CClanSetFurnitureRes obj = new S2CClanSetFurnitureRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
