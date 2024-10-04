using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanScoutEntryInviteInfo
    {
        public CDataClanScoutEntryInviteInfo()
        {
            ClanName = "";
            BaseInfo = new CDataCommunityCharacterBaseInfo();
        }

        public uint Id { get; set; }
        public uint ClanId { get; set; }
        public string ClanName { get; set; }
        public CDataCommunityCharacterBaseInfo BaseInfo { get; set; }
        public long CreateTime { get; set; }

        public class Serializer : EntitySerializer<CDataClanScoutEntryInviteInfo>
        {
            public override void Write(IBuffer buffer, CDataClanScoutEntryInviteInfo obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteUInt32(buffer, obj.ClanId);
                WriteMtString(buffer, obj.ClanName);
                WriteEntity<CDataCommunityCharacterBaseInfo>(buffer, obj.BaseInfo);
                WriteInt64(buffer, obj.CreateTime);
            }

            public override CDataClanScoutEntryInviteInfo Read(IBuffer buffer)
            {
                CDataClanScoutEntryInviteInfo obj = new CDataClanScoutEntryInviteInfo();
                obj.Id = ReadUInt32(buffer);
                obj.ClanId = ReadUInt32(buffer);
                obj.ClanName = ReadMtString(buffer);
                obj.BaseInfo = ReadEntity<CDataCommunityCharacterBaseInfo>(buffer);
                obj.CreateTime = ReadInt64(buffer);
                return obj;
            }
        }
    }
}
