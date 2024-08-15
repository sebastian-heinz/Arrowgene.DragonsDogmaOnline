using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceCharacterEndBadStatusNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_CHARACTER_END_BAD_STATUS_NTC;

        public C2SInstanceCharacterEndBadStatusNtc()
        {
        }

        public uint StatusId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SInstanceCharacterEndBadStatusNtc>
        {
            public override void Write(IBuffer buffer, C2SInstanceCharacterEndBadStatusNtc obj)
            {
                WriteUInt32(buffer, obj.StatusId);
            }

            public override C2SInstanceCharacterEndBadStatusNtc Read(IBuffer buffer)
            {
                C2SInstanceCharacterEndBadStatusNtc obj = new C2SInstanceCharacterEndBadStatusNtc();
                obj.StatusId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
