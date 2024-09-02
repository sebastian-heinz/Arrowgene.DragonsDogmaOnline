using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMDataCraftMaterial
    {
        public uint ItemId { get; set; }
        public uint Num { get; set; }
        public uint SortNo { get; set; }
        public bool IsSp { get; set; }
    
        public class Serializer : EntitySerializer<CDataMDataCraftMaterial>
        {
            public override void Write(IBuffer buffer, CDataMDataCraftMaterial obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.Num);
                WriteUInt32(buffer, obj.SortNo);
                WriteBool(buffer, obj.IsSp);
            }
        
            public override CDataMDataCraftMaterial Read(IBuffer buffer)
            {
                CDataMDataCraftMaterial obj = new CDataMDataCraftMaterial();
                obj.ItemId = ReadUInt32(buffer);
                obj.Num = ReadUInt32(buffer);
                obj.SortNo = ReadUInt32(buffer);
                obj.IsSp = ReadBool(buffer);
                return obj;
            }
        }
    }
}