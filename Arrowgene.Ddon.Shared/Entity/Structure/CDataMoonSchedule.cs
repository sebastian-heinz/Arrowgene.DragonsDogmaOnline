using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMoonSchedule
    {
        public CDataMoonSchedule()
        {
            MoonAge=0;
            BeginTimeSec=0;
            EndTimeSec=0;
        }

        public byte MoonAge { get; set; } // Probably Age = Phase?
        public long BeginTimeSec { get; set; }
        public long EndTimeSec { get; set; }

        public class Serializer : EntitySerializer<CDataMoonSchedule>
        {
            public override void Write(IBuffer buffer, CDataMoonSchedule obj)
            {
                WriteByte(buffer, obj.MoonAge);
                WriteInt64(buffer, obj.BeginTimeSec);
                WriteInt64(buffer, obj.EndTimeSec);
            }

            public override CDataMoonSchedule Read(IBuffer buffer)
            {
                CDataMoonSchedule obj = new CDataMoonSchedule();
                obj.MoonAge = ReadByte(buffer);
                obj.BeginTimeSec = ReadInt64(buffer);
                obj.EndTimeSec = ReadInt64(buffer);
                return obj;
            }
        }
    }
}