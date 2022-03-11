using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataDeliveryItem
{
    public CDataDeliveryItem()
    {

    }
    
    public class Serializer : EntitySerializer<CDataDeliveryItem>
    {
        public override void Write(IBuffer buffer, CDataDeliveryItem obj)
        {

        }

        public override CDataDeliveryItem Read(IBuffer buffer)
        {
            CDataDeliveryItem obj = new CDataDeliveryItem();
            return obj;
        }
    }
}
