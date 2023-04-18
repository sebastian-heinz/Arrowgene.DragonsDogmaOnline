using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnHp
    {
        public uint PawnId { get; set; }
        public uint Hp { get; set; }
    
        public class Serializer : EntitySerializer<CDataPawnHp>
        {
            public override void Write(IBuffer buffer, CDataPawnHp obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.Hp);
            }
        
            public override CDataPawnHp Read(IBuffer buffer)
            {
                CDataPawnHp obj = new CDataPawnHp();
                obj.PawnId = ReadUInt32(buffer);
                obj.Hp = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}