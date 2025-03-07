using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMDataCraftMaterial
    {
        public ItemId ItemId { get; set; }
        public uint Num { get; set; }
        public uint SortNo { get; set; }
        public bool IsSp { get; set; }
    
        public class Serializer : EntitySerializer<CDataMDataCraftMaterial>
        {
            public override void Write(IBuffer buffer, CDataMDataCraftMaterial obj)
            {
                WriteUInt32(buffer, (uint)obj.ItemId);
                WriteUInt32(buffer, obj.Num);
                WriteUInt32(buffer, obj.SortNo);
                WriteBool(buffer, obj.IsSp);
            }
        
            public override CDataMDataCraftMaterial Read(IBuffer buffer)
            {
                CDataMDataCraftMaterial obj = new CDataMDataCraftMaterial();
                obj.ItemId = (ItemId)ReadUInt32(buffer);
                obj.Num = ReadUInt32(buffer);
                obj.SortNo = ReadUInt32(buffer);
                obj.IsSp = ReadBool(buffer);
                return obj;
            }
        }
    }
}
