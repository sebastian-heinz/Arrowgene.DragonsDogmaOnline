using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataCommunicationShortCut
    {
        public uint PageNo;
        public uint ButtonNo;
        public byte Type;
        public byte Category;
        public uint ID;
    }

    public class CDataCommunicationShortCutSerializer : EntitySerializer<CDataCommunicationShortCut>
    {
        public override void Write(IBuffer buffer, CDataCommunicationShortCut obj)
        {
            WriteUInt32(buffer, obj.PageNo);
            WriteUInt32(buffer, obj.ButtonNo);
            WriteByte(buffer, obj.Type);
            WriteByte(buffer, obj.Category);
            WriteUInt32(buffer, obj.ID);
        }

        public override CDataCommunicationShortCut Read(IBuffer buffer)
        {
            CDataCommunicationShortCut obj = new CDataCommunicationShortCut();
            obj.PageNo = ReadUInt32(buffer);
            obj.ButtonNo = ReadUInt32(buffer);
            obj.Type = ReadByte(buffer);
            obj.Category = ReadByte(buffer);
            obj.ID = ReadUInt32(buffer);
            return obj;
        }
    }
}
