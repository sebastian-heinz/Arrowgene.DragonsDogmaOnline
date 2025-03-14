using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestSubstoryStartAnnounceNtc : IPacketStructure
    {
        public S2CQuestSubstoryStartAnnounceNtc()
        {
        }

        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }

        public PacketId Id => PacketId.S2C_QUEST_11_92_16_NTC;

        public class Serializer : PacketEntitySerializer<S2CQuestSubstoryStartAnnounceNtc> {
            public override void Write(IBuffer buffer, S2CQuestSubstoryStartAnnounceNtc obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override S2CQuestSubstoryStartAnnounceNtc Read(IBuffer buffer)
            {
                var obj = new S2CQuestSubstoryStartAnnounceNtc();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
