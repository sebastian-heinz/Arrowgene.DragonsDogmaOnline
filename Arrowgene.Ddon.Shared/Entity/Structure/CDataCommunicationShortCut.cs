using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCommunicationShortCut
    {
        public uint PageNo { get; set; }
        public uint ButtonNo { get; set; }
        public byte Type { get; set; }
        public byte Category { get; set; }
        public uint Id { get; set; }

        public class Serializer : EntitySerializer<CDataCommunicationShortCut>
        {
            public override void Write(IBuffer buffer, CDataCommunicationShortCut obj)
            {
                WriteUInt32(buffer, obj.PageNo);
                WriteUInt32(buffer, obj.ButtonNo);
                WriteByte(buffer, obj.Type);
                WriteByte(buffer, obj.Category);
                WriteUInt32(buffer, obj.Id);
            }

            public override CDataCommunicationShortCut Read(IBuffer buffer)
            {
                CDataCommunicationShortCut obj = new CDataCommunicationShortCut();
                obj.PageNo = ReadUInt32(buffer);
                obj.ButtonNo = ReadUInt32(buffer);
                obj.Type = ReadByte(buffer);
                obj.Category = ReadByte(buffer);
                obj.Id = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
