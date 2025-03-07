using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestBoardReloadNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2CQuestBoardReloadNtc;

        public S2CQuestBoardReloadNtc()
        {
        }

        public bool Unk0 { get; set; } // When Unk0 is true and Unk1 is false, prints about a normal quest board
        public bool Unk1 { get; set; } // When Set to true, mentions to special quest board
        public uint Count { get; set; } // Count is printed when Unk1 is true

        public class Serializer : PacketEntitySerializer<S2CQuestBoardReloadNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestBoardReloadNtc obj)
            {
                WriteBool(buffer, obj.Unk0);
                WriteBool(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Count);
            }

            public override S2CQuestBoardReloadNtc Read(IBuffer buffer)
            {
                S2CQuestBoardReloadNtc obj = new S2CQuestBoardReloadNtc();
                obj.Unk0 = ReadBool(buffer);
                obj.Unk1 = ReadBool(buffer);
                obj.Count = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

