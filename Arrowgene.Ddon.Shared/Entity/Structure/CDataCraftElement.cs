using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftElement
    {
        public CDataCraftElement()
        {
            ItemUID = string.Empty;
        }

        string ItemUID { get; set; }
        ushort SlotNo { get; set; }

        public class Serializer : EntitySerializer<CDataCraftElement>
        {
            public override void Write(IBuffer buffer, CDataCraftElement obj)
            {
                WriteMtString(buffer, obj.ItemUID);
                WriteUInt16(buffer, obj.SlotNo);
            }

            public override CDataCraftElement Read(IBuffer buffer)
            {
                    CDataCraftElement obj = new CDataCraftElement();
                    obj.ItemUID = ReadMtString(buffer);
                    obj.SlotNo = ReadUInt16(buffer);
                    return obj;
                
            }
        }

    }
}