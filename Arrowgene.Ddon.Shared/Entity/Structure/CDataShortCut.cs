using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataShortCut
    {
        public uint PageNo { get; set; }
        public uint ButtonNo { get; set; }
        public uint ShortcutId { get; set; }
        public uint U32Data { get; set; }
        public uint F32Data { get; set; }
        public byte ExexType { get; set; }

        public class Serializer : EntitySerializer<CDataShortCut>
        {
            public override void Write(IBuffer buffer, CDataShortCut obj)
            {
                WriteUInt32(buffer, obj.PageNo);
                WriteUInt32(buffer, obj.ButtonNo);
                WriteUInt32(buffer, obj.ShortcutId);
                WriteUInt32(buffer, obj.U32Data);
                WriteUInt32(buffer, obj.F32Data);
                WriteByte(buffer, obj.ExexType);
            }

            public override CDataShortCut Read(IBuffer buffer)
            {
                CDataShortCut obj = new CDataShortCut();
                obj.PageNo = ReadUInt32(buffer);
                obj.ButtonNo = ReadUInt32(buffer);
                obj.ShortcutId = ReadUInt32(buffer);
                obj.U32Data = ReadUInt32(buffer);
                obj.F32Data = ReadUInt32(buffer);
                obj.ExexType = ReadByte(buffer);
                return obj;
            }
        }
    }
}
