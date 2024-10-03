using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGameTimeBaseInfo
    {
        public CDataGameTimeBaseInfo()
        {
            GameTimeOneDayMin=0x5A; // Taken from the pcaps
            GameTimeYearMonth=0;
            GameTimeMonthDay=0;
            GameTimeDayHour=0x18; // Taken from the pcaps
            GameTimeWeekDay=0;
            GameTimeMoonAge=0;
            OriginalRealTimeSec=0x55DDD470; // Taken from the pcaps. A few days before DDOn release
            OriginalGameTimeSec=0x22C2ED000; // Taken from the pcaps.
            OriginalWeek=0;
            OriginalMoonAge=0;
        }

        public uint GameTimeOneDayMin { get; set; } // RL minutes it takes for a GT day to pass? Can't be 0
        public uint GameTimeYearMonth { get; set; }
        public uint GameTimeMonthDay { get; set; }
        public uint GameTimeDayHour { get; set; } // Hours in a day. Can't be 0
        public uint GameTimeWeekDay { get; set; }
        public uint GameTimeMoonAge { get; set; }
        public long OriginalRealTimeSec { get; set; } // Timestamp in unix time
        public long OriginalGameTimeSec { get; set; } // Timestamp in unix time
        public uint OriginalWeek { get; set; }
        public uint OriginalMoonAge { get; set; }

        public class Serializer : EntitySerializer<CDataGameTimeBaseInfo>
        {
            public override void Write(IBuffer buffer, CDataGameTimeBaseInfo obj)
            {
                WriteUInt32(buffer, obj.GameTimeOneDayMin);
                WriteUInt32(buffer, obj.GameTimeYearMonth);
                WriteUInt32(buffer, obj.GameTimeMonthDay);
                WriteUInt32(buffer, obj.GameTimeDayHour);
                WriteUInt32(buffer, obj.GameTimeWeekDay);
                WriteUInt32(buffer, obj.GameTimeMoonAge);
                WriteInt64(buffer, obj.OriginalRealTimeSec);
                WriteInt64(buffer, obj.OriginalGameTimeSec);
                WriteUInt32(buffer, obj.OriginalWeek);
                WriteUInt32(buffer, obj.OriginalMoonAge);
            }

            public override CDataGameTimeBaseInfo Read(IBuffer buffer)
            {
                CDataGameTimeBaseInfo obj = new CDataGameTimeBaseInfo();
                obj.GameTimeOneDayMin = ReadUInt32(buffer);
                obj.GameTimeYearMonth = ReadUInt32(buffer);
                obj.GameTimeMonthDay = ReadUInt32(buffer);
                obj.GameTimeDayHour = ReadUInt32(buffer);
                obj.GameTimeWeekDay = ReadUInt32(buffer);
                obj.GameTimeMoonAge = ReadUInt32(buffer);
                obj.OriginalRealTimeSec = ReadInt64(buffer);
                obj.OriginalGameTimeSec = ReadInt64(buffer);
                obj.OriginalWeek = ReadUInt32(buffer);
                obj.OriginalMoonAge = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
