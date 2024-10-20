using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanBaseReleaseStateUpdateNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_BASE_RELEASE_STATE_UPDATE_NTC;

        public byte State;

        public class Serializer : PacketEntitySerializer<S2CClanClanBaseReleaseStateUpdateNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanBaseReleaseStateUpdateNtc obj)
            {
                WriteByte(buffer, obj.State);
            }

            public override S2CClanClanBaseReleaseStateUpdateNtc Read(IBuffer buffer)
            {
                S2CClanClanBaseReleaseStateUpdateNtc obj = new S2CClanClanBaseReleaseStateUpdateNtc();

                obj.State = ReadByte(buffer);

                return obj;
            }
        }
    }
}
