using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGatheringItemListUnk1Unk2
    {
        public uint Unk0 { get; set; }
        public ushort Unk1 { get; set; }

        public class Serializer : EntitySerializer<CDataGatheringItemListUnk1Unk2>
        {
            public override void Write(IBuffer buffer, CDataGatheringItemListUnk1Unk2 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt16(buffer, obj.Unk1);
            }
        
            public override CDataGatheringItemListUnk1Unk2 Read(IBuffer buffer)
            {
                CDataGatheringItemListUnk1Unk2 obj = new CDataGatheringItemListUnk1Unk2();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}