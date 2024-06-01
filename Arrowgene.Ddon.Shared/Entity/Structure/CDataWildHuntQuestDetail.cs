using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWildHuntQuestDetail
    {
        public uint Unk1 { get; set; }
        public byte Unk2 { get; set; }

        public class Serializer : EntitySerializer<CDataWildHuntQuestDetail>
        {
            public override void Write(IBuffer buffer, CDataWildHuntQuestDetail obj)
            {
                WriteUInt32(buffer, obj.Unk1);
                WriteByte(buffer, obj.Unk2);
            }

            public override CDataWildHuntQuestDetail Read(IBuffer buffer)
            {
                CDataWildHuntQuestDetail obj = new CDataWildHuntQuestDetail
                {
                    Unk1 = ReadUInt32(buffer),
                    Unk2 = ReadByte(buffer)
                };
                return obj;
            }
        }
    }
}
