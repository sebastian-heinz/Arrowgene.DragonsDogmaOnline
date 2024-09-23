using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanScoutEntryGetInviteListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_SCOUT_ENTRY_GET_INVITE_LIST_RES;

        public S2CClanClanScoutEntryGetInviteListRes()
        {
            InviteInfo = new List<CDataClanScoutEntryInviteInfo>();
        }

        public List<CDataClanScoutEntryInviteInfo> InviteInfo;

        public class Serializer : PacketEntitySerializer<S2CClanClanScoutEntryGetInviteListRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanScoutEntryGetInviteListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataClanScoutEntryInviteInfo>(buffer, obj.InviteInfo);
            }

            public override S2CClanClanScoutEntryGetInviteListRes Read(IBuffer buffer)
            {
                S2CClanClanScoutEntryGetInviteListRes obj = new S2CClanClanScoutEntryGetInviteListRes();
                ReadServerResponse(buffer, obj);
                obj.InviteInfo = ReadEntityList<CDataClanScoutEntryInviteInfo>(buffer);
                return obj;
            }
        }
    }
}
