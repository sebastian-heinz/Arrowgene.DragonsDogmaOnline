using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CAreaGetAreaSupplyRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_AREA_GET_AREA_SUPPLY_RES;

        public class Serializer : PacketEntitySerializer<S2CAreaGetAreaSupplyRes>
        {
            public override void Write(IBuffer buffer, S2CAreaGetAreaSupplyRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CAreaGetAreaSupplyRes Read(IBuffer buffer)
            {
                S2CAreaGetAreaSupplyRes obj = new S2CAreaGetAreaSupplyRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
