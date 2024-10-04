using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanScoutEntryGetInviteListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_SCOUT_ENTRY_GET_INVITE_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SClanClanScoutEntryGetInviteListReq>
        {

            public override void Write(IBuffer buffer, C2SClanClanScoutEntryGetInviteListReq obj)
            {
            }

            public override C2SClanClanScoutEntryGetInviteListReq Read(IBuffer buffer)
            {
                return new C2SClanClanScoutEntryGetInviteListReq();
            }
        }
    }
}
