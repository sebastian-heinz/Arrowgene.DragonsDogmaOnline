using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDispelLotColor
    {
        public CDataDispelLotColor()
        {
        }

        public byte Color { get; set; }
        public byte ColorRate { get; set; }

        public class Serializer : EntitySerializer<CDataDispelLotColor>
        {
            public override void Write(IBuffer buffer, CDataDispelLotColor obj)
            {
                WriteByte(buffer, obj.Color);
                WriteByte(buffer, obj.ColorRate);
            }

            public override CDataDispelLotColor Read(IBuffer buffer)
            {
                CDataDispelLotColor obj = new CDataDispelLotColor();
                obj.Color = ReadByte(buffer);
                obj.ColorRate = ReadByte(buffer);
                return obj;
            }
        }
    }
}

