using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDispelLotPlus
    {
        public CDataDispelLotPlus()
        {
        }

        public byte Plus { get; set; }
        public byte PlusRate { get; set; }

        public class Serializer : EntitySerializer<CDataDispelLotPlus>
        {
            public override void Write(IBuffer buffer, CDataDispelLotPlus obj)
            {
                WriteByte(buffer, obj.Plus);
                WriteByte(buffer, obj.PlusRate);
            }

            public override CDataDispelLotPlus Read(IBuffer buffer)
            {
                CDataDispelLotPlus obj = new CDataDispelLotPlus();
                obj.Plus = ReadByte(buffer);
                obj.PlusRate = ReadByte(buffer);
                return obj;
            }
        }
    }
}

