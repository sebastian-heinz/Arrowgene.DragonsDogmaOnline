using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataDeliveredItemRecord
{
    public CDataDeliveredItemRecord()
    {

    }
    
    public class Serializer : EntitySerializer<CDataDeliveredItemRecord>
    {
        public override void Write(IBuffer buffer, CDataDeliveredItemRecord obj)
        {

        }

        public override CDataDeliveredItemRecord Read(IBuffer buffer)
        {
            CDataDeliveredItemRecord obj = new CDataDeliveredItemRecord();
            return obj;
        }
    }
}
