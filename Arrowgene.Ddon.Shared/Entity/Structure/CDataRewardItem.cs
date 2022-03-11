using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataRewardItem
{
    public CDataRewardItem()
    {

    }
    
    public class Serializer : EntitySerializer<CDataRewardItem>
    {
        public override void Write(IBuffer buffer, CDataRewardItem obj)
        {

        }

        public override CDataRewardItem Read(IBuffer buffer)
        {
            CDataRewardItem obj = new CDataRewardItem();
            return obj;
        }
    }
}
