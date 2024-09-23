using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanScoutEntrySearchReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_SCOUT_ENTRY_SEARCH_REQ;

        public class Serializer : PacketEntitySerializer<C2SClanClanScoutEntrySearchReq>
        {

            public override void Write(IBuffer buffer, C2SClanClanScoutEntrySearchReq obj)
            {
            }

            public override C2SClanClanScoutEntrySearchReq Read(IBuffer buffer)
            {
                return new C2SClanClanScoutEntrySearchReq();
            }
        }
    }
}
