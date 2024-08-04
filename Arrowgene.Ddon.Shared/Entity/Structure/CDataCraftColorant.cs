using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftColorant
    {

        public CDataCraftColorant()
        {
            ItemUID = string.Empty;
        }

        public string ItemUID { get; set; }
        public byte ItemNum { get; set; }

        public class Serializer : EntitySerializer<CDataCraftColorant>
        {
            public override void Write(IBuffer buffer, CDataCraftColorant obj)
            {
                WriteMtString(buffer, obj.ItemUID);
                WriteByte(buffer, obj.ItemNum);
            }

            public override CDataCraftColorant Read(IBuffer buffer)
            {
                CDataCraftColorant obj = new CDataCraftColorant();
                obj.ItemUID = ReadMtString(buffer);
                obj.ItemNum = ReadByte(buffer);
                return obj;
            }
        }
    }
}
