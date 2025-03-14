using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataRewardItem
{
    public ItemId ItemId { get; set; }
    public ushort Num { get; set; }
    
    public class Serializer : EntitySerializer<CDataRewardItem>
    {
        public override void Write(IBuffer buffer, CDataRewardItem obj)
        {
            WriteUInt32(buffer, (uint) obj.ItemId);
            WriteUInt16(buffer, obj.Num);
        }

        public override CDataRewardItem Read(IBuffer buffer)
        {
            CDataRewardItem obj = new CDataRewardItem();
            obj.ItemId = (ItemId)ReadUInt32(buffer);
            obj.Num = ReadUInt16(buffer);
            return obj;
        }
    }
}
