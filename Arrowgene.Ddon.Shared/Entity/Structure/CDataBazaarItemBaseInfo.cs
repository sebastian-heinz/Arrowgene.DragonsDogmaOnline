using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBazaarItemBaseInfo
    {
        public uint ItemId { get; set; }
        public ushort Num { get; set; }
        public uint Price { get; set; }
    
        public class Serializer : EntitySerializer<CDataBazaarItemBaseInfo>
        {
            public override void Write(IBuffer buffer, CDataBazaarItemBaseInfo obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt16(buffer, obj.Num);
                WriteUInt32(buffer, obj.Price);
            }
        
            public override CDataBazaarItemBaseInfo Read(IBuffer buffer)
            {
                CDataBazaarItemBaseInfo obj = new CDataBazaarItemBaseInfo();
                obj.ItemId = ReadUInt32(buffer);
                obj.Num = ReadUInt16(buffer);
                obj.Price = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}