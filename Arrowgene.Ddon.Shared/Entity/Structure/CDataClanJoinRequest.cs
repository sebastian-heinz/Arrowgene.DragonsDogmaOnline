using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanJoinRequest
    {
        public CDataClanJoinRequest()
        {
            ClanName = "";
            BaseInfo = new CDataCommunityCharacterBaseInfo();
        }

        public uint ClanId { get; set; }
        public string ClanName { get; set; }
        public CDataCommunityCharacterBaseInfo BaseInfo { get; set; }
        public long CreateTime { get; set; }

        public class Serializer : EntitySerializer<CDataClanJoinRequest>
        {
            public override void Write(IBuffer buffer, CDataClanJoinRequest obj)
            {
                WriteUInt32(buffer, obj.ClanId);
                WriteMtString(buffer, obj.ClanName);
                WriteEntity<CDataCommunityCharacterBaseInfo>(buffer, obj.BaseInfo);
                WriteInt64(buffer, obj.CreateTime);
            }

            public override CDataClanJoinRequest Read(IBuffer buffer)
            {
                CDataClanJoinRequest obj = new CDataClanJoinRequest();
                obj.ClanId = ReadUInt32(buffer);
                obj.ClanName = ReadMtString(buffer);
                obj.BaseInfo = ReadEntity<CDataCommunityCharacterBaseInfo>(buffer);
                obj.CreateTime = ReadInt64(buffer);
                return obj;
            }
        }
    }
}
