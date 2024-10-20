using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanJoinNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_JOIN_NTC;

        public uint CharacterId;

        public class Serializer : PacketEntitySerializer<S2CClanClanJoinNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanJoinNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
            }

            public override S2CClanClanJoinNtc Read(IBuffer buffer)
            {
                S2CClanClanJoinNtc obj = new S2CClanClanJoinNtc();

                obj.CharacterId = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
