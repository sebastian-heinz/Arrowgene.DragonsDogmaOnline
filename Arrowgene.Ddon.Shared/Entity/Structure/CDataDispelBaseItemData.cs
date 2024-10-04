using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDispelBaseItemData
    {
        public CDataDispelBaseItemData()
        {
        }

        public uint ItemId { get; set; }
        public uint Num {  get; set; }

        public class Serializer : EntitySerializer<CDataDispelBaseItemData>
        {
            public override void Write(IBuffer buffer, CDataDispelBaseItemData obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.Num);
            }

            public override CDataDispelBaseItemData Read(IBuffer buffer)
            {
                CDataDispelBaseItemData obj = new CDataDispelBaseItemData();
                obj.ItemId = ReadUInt32(buffer);
                obj.Num = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

