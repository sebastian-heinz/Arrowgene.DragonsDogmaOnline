using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftPawnList
    {
        public uint PawnId { get; set; }
        public uint CraftExp { get; set; }
        public uint CraftPoint { get; set; }
        public uint CraftRankLimit { get; set; }
    
        public class Serializer : EntitySerializer<CDataCraftPawnList>
        {
            public override void Write(IBuffer buffer, CDataCraftPawnList obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.CraftExp);
                WriteUInt32(buffer, obj.CraftPoint);
                WriteUInt32(buffer, obj.CraftRankLimit);
            }
        
            public override CDataCraftPawnList Read(IBuffer buffer)
            {
                CDataCraftPawnList obj = new CDataCraftPawnList();
                obj.PawnId = ReadUInt32(buffer);
                obj.CraftExp = ReadUInt32(buffer);
                obj.CraftPoint = ReadUInt32(buffer);
                obj.CraftRankLimit = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}