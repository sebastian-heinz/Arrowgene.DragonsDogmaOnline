using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CServerTimeUpdateNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SERVER_TIME_UPDATE_NTC;

        public S2CServerTimeUpdateNtc()
        {
        }

        public byte Moon { get; set; }
        public byte Second { get; set; }
        public byte Minute { get; set; }
        public byte Hour { get; set; }
        public byte MonthDay { get; set; }
        public byte Month { get; set; }
        public byte Weekday { get; set; }
        public ushort Year { get; set; }

        public class Serializer : PacketEntitySerializer<S2CServerTimeUpdateNtc>
        {
            public override void Write(IBuffer buffer, S2CServerTimeUpdateNtc obj)
            {
                WriteByte(buffer, obj.Moon);
                WriteByte(buffer, obj.Second);
                WriteByte(buffer, obj.Minute);
                WriteByte(buffer, obj.Hour);
                WriteByte(buffer, obj.MonthDay);
                WriteByte(buffer, obj.Month);
                WriteByte(buffer, obj.Weekday);
                WriteUInt16(buffer, obj.Year);
            }

            public override S2CServerTimeUpdateNtc Read(IBuffer buffer)
            {
                S2CServerTimeUpdateNtc obj = new S2CServerTimeUpdateNtc();
                obj.Moon = ReadByte(buffer);
                obj.Second = ReadByte(buffer);
                obj.Minute = ReadByte(buffer);
                obj.Hour = ReadByte(buffer);
                obj.MonthDay = ReadByte(buffer);
                obj.Month = ReadByte(buffer);
                obj.Weekday = ReadByte(buffer);
                obj.Year = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
