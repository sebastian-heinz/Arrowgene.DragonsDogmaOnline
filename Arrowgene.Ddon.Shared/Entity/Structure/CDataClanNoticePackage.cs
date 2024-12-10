using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanNoticePackage
    {
        public CDataClanNoticePackage()
        {
            Text = string.Empty;
        }

        public byte Type { get; set; } // Might be ClanHistoryType
        public uint Value1 { get; set; }
        public uint Value2 { get; set; }
        public uint Value3 { get; set; }
        public string Text { get; set; }


        public class Serializer : EntitySerializer<CDataClanNoticePackage>
        {
            public override void Write(IBuffer buffer, CDataClanNoticePackage obj)
            {
                WriteByte(buffer, obj.Type);
                WriteUInt32(buffer, obj.Value1);
                WriteUInt32(buffer, obj.Value2);
                WriteUInt32(buffer, obj.Value3);
                WriteMtString(buffer, obj.Text);
            }

            public override CDataClanNoticePackage Read(IBuffer buffer)
            {
                CDataClanNoticePackage obj = new CDataClanNoticePackage();
                obj.Type = ReadByte(buffer);
                obj.Value1 = ReadUInt32(buffer);
                obj.Value2 = ReadUInt32(buffer);
                obj.Value3 = ReadUInt32(buffer);
                obj.Text = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
