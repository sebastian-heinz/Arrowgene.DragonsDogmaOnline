using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGPShopDisplayType
    {
        public uint ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public uint InGameUrlID { get; set; }
    
        public class Serializer : EntitySerializer<CDataGPShopDisplayType>
        {
            public override void Write(IBuffer buffer, CDataGPShopDisplayType obj)
            {
                WriteUInt32(buffer, obj.ID);
                WriteMtString(buffer, obj.Name);
                WriteUInt32(buffer, obj.InGameUrlID);
            }
        
            public override CDataGPShopDisplayType Read(IBuffer buffer)
            {
                CDataGPShopDisplayType obj = new CDataGPShopDisplayType();
                obj.ID = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.InGameUrlID = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
