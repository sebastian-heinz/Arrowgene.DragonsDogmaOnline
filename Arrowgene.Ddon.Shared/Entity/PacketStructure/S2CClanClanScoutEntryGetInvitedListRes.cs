using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanScoutEntryGetInvitedListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_SCOUT_ENTRY_GET_INVITED_LIST_RES;

        public S2CClanClanScoutEntryGetInvitedListRes()
        {
            InviteInfo = new List<CDataClanScoutEntryInviteInfo>();
        }

        public List<CDataClanScoutEntryInviteInfo> InviteInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CClanClanScoutEntryGetInvitedListRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanScoutEntryGetInvitedListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataClanScoutEntryInviteInfo>(buffer, obj.InviteInfo);
            }

            public override S2CClanClanScoutEntryGetInvitedListRes Read(IBuffer buffer)
            {
                S2CClanClanScoutEntryGetInvitedListRes obj = new S2CClanClanScoutEntryGetInvitedListRes();
                ReadServerResponse(buffer, obj);
                obj.InviteInfo = ReadEntityList<CDataClanScoutEntryInviteInfo>(buffer);
                return obj;
            }
        }
    }
}
