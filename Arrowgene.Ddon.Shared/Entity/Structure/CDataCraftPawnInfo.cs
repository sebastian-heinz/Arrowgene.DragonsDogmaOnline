using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftPawnInfo
    {
        public CDataCraftPawnInfo() {
            Name = string.Empty;
        }
    
        public uint PawnId { get; set; }
        public string Name { get; set; }
    
        public class Serializer : EntitySerializer<CDataCraftPawnInfo>
        {
            public override void Write(IBuffer buffer, CDataCraftPawnInfo obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteMtString(buffer, obj.Name);
            }
        
            public override CDataCraftPawnInfo Read(IBuffer buffer)
            {
                CDataCraftPawnInfo obj = new CDataCraftPawnInfo();
                obj.PawnId = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                return obj;
            }
        }
    }
}