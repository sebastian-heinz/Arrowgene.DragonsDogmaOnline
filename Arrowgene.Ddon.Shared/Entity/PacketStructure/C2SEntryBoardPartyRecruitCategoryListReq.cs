using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEntryBoardPartyRecruitCategoryListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ENTRY_BOARD_PARTY_RECRUIT_CATEGORY_LIST_REQ;

        public C2SEntryBoardPartyRecruitCategoryListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardPartyRecruitCategoryListReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardPartyRecruitCategoryListReq obj)
            {
            }

            public override C2SEntryBoardPartyRecruitCategoryListReq Read(IBuffer buffer)
            {
                C2SEntryBoardPartyRecruitCategoryListReq obj = new C2SEntryBoardPartyRecruitCategoryListReq();
                return obj;
            }
        }
    }
}
