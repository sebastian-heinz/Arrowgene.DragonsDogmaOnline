using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftStartCraftRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_START_CRAFT_RES;

        public class Serializer : PacketEntitySerializer<S2CCraftStartCraftRes>
        {
            public override void Write(IBuffer buffer, S2CCraftStartCraftRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CCraftStartCraftRes Read(IBuffer buffer)
            {
                S2CCraftStartCraftRes obj = new S2CCraftStartCraftRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}