using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CProfileGetCharacterProfileRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PROFILE_GET_CHARACTER_PROFILE_RES;

        public S2CProfileGetCharacterProfileRes()
        {
            CharacterName = new CDataCharacterName();
            ClanParam = new CDataClanParam();
            JobLevelList = new List<CDataJobBaseInfo>();
            MatchingProfile = new CDataMatchingProfile();
            ArisenProfile = new CDataArisenProfile();
            OnlineId = string.Empty;
        }

        public uint CharacterId { get; set; }
        public CDataCharacterName CharacterName { get; set; }
        public JobId JobId { get; set; }
        public byte JobLevel { get; set; }
        public CDataClanParam ClanParam { get; set; }
        public uint ClanMemberRank { get; set; }
        public List<CDataJobBaseInfo> JobLevelList { get; set; }
        public CDataMatchingProfile MatchingProfile { get; set; }
        public CDataArisenProfile ArisenProfile { get; set; }
        public string OnlineId { get; set; }
        public uint Unk0 { get; set; }
        

        public class Serializer : PacketEntitySerializer<S2CProfileGetCharacterProfileRes>
        {
            public override void Write(IBuffer buffer, S2CProfileGetCharacterProfileRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntity<CDataCharacterName>(buffer, obj.CharacterName);
                WriteByte(buffer, (byte) obj.JobId);
                WriteByte(buffer, obj.JobLevel);
                WriteEntity<CDataClanParam>(buffer, obj.ClanParam);
                WriteUInt32(buffer, obj.ClanMemberRank);
                WriteEntityList<CDataJobBaseInfo>(buffer, obj.JobLevelList);
                WriteEntity<CDataMatchingProfile>(buffer, obj.MatchingProfile);
                WriteEntity<CDataArisenProfile>(buffer, obj.ArisenProfile);
                WriteMtString(buffer, obj.OnlineId);
                WriteUInt32(buffer, obj.Unk0);
            }

            public override S2CProfileGetCharacterProfileRes Read(IBuffer buffer)
            {
                S2CProfileGetCharacterProfileRes obj = new S2CProfileGetCharacterProfileRes();
                ReadServerResponse(buffer, obj);
                obj.CharacterId = ReadUInt32(buffer);
                obj.CharacterName = ReadEntity<CDataCharacterName>(buffer);
                obj.JobId = (JobId) ReadByte(buffer);
                obj.JobLevel = ReadByte(buffer);
                obj.ClanParam = ReadEntity<CDataClanParam>(buffer);
                obj.ClanMemberRank = ReadUInt32(buffer);
                obj.JobLevelList = ReadEntityList<CDataJobBaseInfo>(buffer);
                obj.MatchingProfile = ReadEntity<CDataMatchingProfile>(buffer);
                obj.ArisenProfile = ReadEntity<CDataArisenProfile>(buffer);
                obj.OnlineId = ReadMtString(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}