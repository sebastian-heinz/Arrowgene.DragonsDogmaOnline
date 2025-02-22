using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAreaInfoList
    {
        public uint AreaId { get; set; }
        public byte Weather { get; set; }
        public byte UndiscoveredQuestNum { get; set; }
        public byte HighDifficultyQuestNum { get; set; }
        public bool IsBonus { get; set; }

        public class Serializer : EntitySerializer<CDataAreaInfoList>
        {
            public override void Write(IBuffer buffer, CDataAreaInfoList obj)
            {
                WriteUInt32(buffer, obj.AreaId);
                WriteByte(buffer, obj.Weather);
                WriteByte(buffer, obj.UndiscoveredQuestNum);
                WriteByte(buffer, obj.HighDifficultyQuestNum);
                WriteBool(buffer, obj.IsBonus);
            }

            public override CDataAreaInfoList Read(IBuffer buffer)
            {
                CDataAreaInfoList obj = new CDataAreaInfoList();
                obj.AreaId = ReadUInt32(buffer);
                obj.Weather = ReadByte(buffer);
                obj.UndiscoveredQuestNum = ReadByte(buffer);
                obj.HighDifficultyQuestNum = ReadByte(buffer);
                obj.IsBonus = ReadBool(buffer);
                return obj;
            }
        }
    }
}
