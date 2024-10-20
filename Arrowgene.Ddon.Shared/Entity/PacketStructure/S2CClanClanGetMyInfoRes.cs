using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanGetMyInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_GET_MY_INFO_RES;

        public S2CClanClanGetMyInfoRes()
        {
            ClanParam = new CDataClanParam();
        }
        public CDataClanParam ClanParam { get; set; }
        public long LeaveTime { get; set; } 

        public class Serializer : PacketEntitySerializer<S2CClanClanGetMyInfoRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanGetMyInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity<CDataClanParam>(buffer, obj.ClanParam);
                WriteInt64(buffer, obj.LeaveTime);
            }

            public override S2CClanClanGetMyInfoRes Read(IBuffer buffer)
            {
                S2CClanClanGetMyInfoRes obj = new S2CClanClanGetMyInfoRes();
                ReadServerResponse(buffer, obj);
                obj.ClanParam = ReadEntity<CDataClanParam>(buffer);
                obj.LeaveTime = ReadInt64(buffer);
                return obj;
            }
        }
    }
}
