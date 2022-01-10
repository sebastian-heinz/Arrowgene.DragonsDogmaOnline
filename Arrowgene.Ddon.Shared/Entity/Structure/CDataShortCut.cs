using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataShortCut
    {
        public uint PageNo;
        public uint ButtonNo;
        public uint ShortcutID;
        public uint U32Data;
        public uint F32Data;
        public byte ExexType;
    }

    public class CDataShortCutSerializer : EntitySerializer<CDataShortCut>
    {
        public override void Write(IBuffer buffer, CDataShortCut obj)
        {
            WriteUInt32(buffer, obj.PageNo);
            WriteUInt32(buffer, obj.ButtonNo);
            WriteUInt32(buffer, obj.ShortcutID);
            WriteUInt32(buffer, obj.U32Data);
            WriteUInt32(buffer, obj.F32Data);
            WriteByte(buffer, obj.ExexType);
        }

        public override CDataShortCut Read(IBuffer buffer)
        {
            CDataShortCut obj = new CDataShortCut();
            obj.PageNo = ReadUInt32(buffer);
            obj.ButtonNo = ReadUInt32(buffer);
            obj.ShortcutID = ReadUInt32(buffer);
            obj.U32Data = ReadUInt32(buffer);
            obj.F32Data = ReadUInt32(buffer);
            obj.ExexType = ReadByte(buffer);
            return obj;
        }
    }
}
