using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanScoutEntryDisapproveInviteNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CLAN_CLAN_SCOUT_ENTRY_DISAPPROVE_INVITE_NTC;

        public uint InviteId;

        public class Serializer : PacketEntitySerializer<S2CClanClanScoutEntryDisapproveInviteNtc>
        {
            public override void Write(IBuffer buffer, S2CClanClanScoutEntryDisapproveInviteNtc obj)
            {
                WriteUInt32(buffer, obj.InviteId);
            }

            public override S2CClanClanScoutEntryDisapproveInviteNtc Read(IBuffer buffer)
            {
                S2CClanClanScoutEntryDisapproveInviteNtc obj = new S2CClanClanScoutEntryDisapproveInviteNtc();

                obj.InviteId = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
