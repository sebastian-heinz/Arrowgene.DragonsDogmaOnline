using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataRecycleItemLot
    {
        public ItemId ItemId { get; set; }
        public uint Amount { get; set; }
        public bool Unk2 { get; set; }
        

        public class Serializer : EntitySerializer<CDataRecycleItemLot>
        {
            public override void Write(IBuffer buffer, CDataRecycleItemLot obj)
            {
                WriteUInt32(buffer, (uint) obj.ItemId);
                WriteUInt32(buffer, obj.Amount);
                WriteBool(buffer, obj.Unk2);
            }

            public override CDataRecycleItemLot Read(IBuffer buffer)
            {
                CDataRecycleItemLot obj = new CDataRecycleItemLot();
                obj.ItemId = (ItemId) ReadUInt32(buffer);
                obj.Amount = ReadUInt32(buffer);
                obj.Unk2 = ReadBool(buffer);
                return obj;
            }
        }
    }
}
