using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanHistoryElement
    {
        public CDataClanHistoryElement()
        {
            CharacterName = new CDataCharacterName();
            FreeText = "";
        }

        public long Date { get; set; }
        public byte Type { get; set; }
        public CDataCharacterName CharacterName { get; set; }
        public uint Value1 { get; set; }
        public uint Value2 { get; set; }
        public uint Value3 { get; set; }
        public string FreeText { get; set; }


        public class Serializer : EntitySerializer<CDataClanHistoryElement>
        {
            public override void Write(IBuffer buffer, CDataClanHistoryElement obj)
            {
                WriteInt64(buffer, obj.Date);
                WriteByte(buffer, obj.Type);
                WriteEntity<CDataCharacterName>(buffer, obj.CharacterName);
                WriteUInt32(buffer, obj.Value1);
                WriteUInt32(buffer, obj.Value2);
                WriteUInt32(buffer, obj.Value3);
                WriteMtString(buffer, obj.FreeText);
            }

            public override CDataClanHistoryElement Read(IBuffer buffer)
            {
                CDataClanHistoryElement obj = new CDataClanHistoryElement();
                obj.Date = ReadInt64(buffer);
                obj.Type = ReadByte(buffer);
                obj.CharacterName = ReadEntity<CDataCharacterName>(buffer);
                obj.Value1 = ReadUInt32(buffer);
                obj.Value2 = ReadUInt32(buffer);
                obj.Value3 = ReadUInt32(buffer);
                obj.FreeText = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
