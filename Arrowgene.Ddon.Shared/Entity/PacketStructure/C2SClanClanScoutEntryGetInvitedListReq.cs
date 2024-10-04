using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanScoutEntryGetInvitedListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_SCOUT_ENTRY_GET_INVITED_LIST_REQ;

        public C2SClanClanScoutEntryGetInvitedListReq()
        {

        }

        public class Serializer : PacketEntitySerializer<C2SClanClanScoutEntryGetInvitedListReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanScoutEntryGetInvitedListReq obj)
            {
            }

            public override C2SClanClanScoutEntryGetInvitedListReq Read(IBuffer buffer)
            {
                C2SClanClanScoutEntryGetInvitedListReq obj = new C2SClanClanScoutEntryGetInvitedListReq();
                return obj;
            }
        }
    }
}
