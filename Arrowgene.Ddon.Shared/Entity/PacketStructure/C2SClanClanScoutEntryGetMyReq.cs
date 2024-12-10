using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanScoutEntryGetMyReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_SCOUT_ENTRY_GET_MY_REQ;

        public C2SClanClanScoutEntryGetMyReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanScoutEntryGetMyReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanScoutEntryGetMyReq obj)
            {
            }

            public override C2SClanClanScoutEntryGetMyReq Read(IBuffer buffer)
            {
                C2SClanClanScoutEntryGetMyReq obj = new C2SClanClanScoutEntryGetMyReq();
                return obj;
            }
        }
    }
}
