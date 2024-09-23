using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEntryBoardListParam
    {
        public ulong BoardId {  get; set; } // Board Entry ID?
        public ushort SortieMin { get; set; }
        public ushort NoPartyMembers { get; set; }
        public ushort Unk3 { get; set; }
        public ushort TimeOut { get; set; } // Looks like maximum time in seconds
        public ushort Unk5 { get; set; } // Level or item level?
        public bool Unk6 {  get; set; }
        public uint Unk7 { get; set; }


        public class Serializer : EntitySerializer<CDataEntryBoardListParam>
        {
            public override void Write(IBuffer buffer, CDataEntryBoardListParam obj)
            {
                WriteUInt64(buffer, obj.BoardId);
                WriteUInt16(buffer, obj.SortieMin);
                WriteUInt16(buffer, obj.NoPartyMembers);
                WriteUInt16(buffer, obj.Unk3);
                WriteUInt16(buffer, obj.TimeOut);
                WriteUInt16(buffer, obj.Unk5);
                WriteBool(buffer, obj.Unk6);
                WriteUInt32(buffer, obj.Unk7);
            }

            public override CDataEntryBoardListParam Read(IBuffer buffer)
            {
                CDataEntryBoardListParam obj = new CDataEntryBoardListParam();
                obj.BoardId = ReadUInt64(buffer);
                obj.SortieMin = ReadUInt16(buffer);
                obj.NoPartyMembers = ReadUInt16(buffer);
                obj.Unk3 = ReadUInt16(buffer);
                obj.TimeOut = ReadUInt16(buffer);
                obj.Unk5 = ReadUInt16(buffer);
                obj.Unk6 = ReadBool(buffer);
                obj.Unk7 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
