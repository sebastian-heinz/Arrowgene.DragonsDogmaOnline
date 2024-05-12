using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;


namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemUseBagItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_USE_BAG_ITEM_RES;        

        public class Serializer : PacketEntitySerializer<S2CItemUseBagItemRes>
        {

            public override void Write(IBuffer buffer, S2CItemUseBagItemRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CItemUseBagItemRes Read(IBuffer buffer)
            {
                S2CItemUseBagItemRes obj = new S2CItemUseBagItemRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }

        }
    }
}
