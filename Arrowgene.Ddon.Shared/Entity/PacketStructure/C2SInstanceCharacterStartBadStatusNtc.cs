using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceCharacterStartBadStatusNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_CHARACTER_START_BAD_STATUS_NTC;

        public C2SInstanceCharacterStartBadStatusNtc()
        {
        }

        public uint StatusId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SInstanceCharacterStartBadStatusNtc>
        {
            public override void Write(IBuffer buffer, C2SInstanceCharacterStartBadStatusNtc obj)
            {
                WriteUInt32(buffer, obj.StatusId);
            }

            public override C2SInstanceCharacterStartBadStatusNtc Read(IBuffer buffer)
            {
                C2SInstanceCharacterStartBadStatusNtc obj = new C2SInstanceCharacterStartBadStatusNtc();
                obj.StatusId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
