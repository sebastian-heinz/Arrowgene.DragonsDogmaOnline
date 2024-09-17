using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.Clan;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanMemberInfo
    {
        public CDataClanMemberInfo()
        {
            CharacterListElement = new CDataCharacterListElement();
        }

        public ClanMemberRank Rank { get; set; }
        public long Created { get; set; }
        public long LastLoginTime { get; set; }
        public long LeaveTime { get; set; }
        public uint Permission { get; set; }
        public CDataCharacterListElement CharacterListElement { get; set; }
    
        public class Serializer : EntitySerializer<CDataClanMemberInfo>
        {
            public override void Write(IBuffer buffer, CDataClanMemberInfo obj)
            {
                WriteUInt32(buffer, (uint)obj.Rank);
                WriteInt64(buffer, obj.Created);
                WriteInt64(buffer, obj.LastLoginTime);
                WriteInt64(buffer, obj.LeaveTime);
                WriteUInt32(buffer, obj.Permission);
                WriteEntity<CDataCharacterListElement>(buffer, obj.CharacterListElement);
            }
        
            public override CDataClanMemberInfo Read(IBuffer buffer)
            {
                CDataClanMemberInfo obj = new CDataClanMemberInfo();
                obj.Rank = (ClanMemberRank)ReadUInt32(buffer);
                obj.Created = ReadInt64(buffer);
                obj.LastLoginTime = ReadInt64(buffer);
                obj.LeaveTime = ReadInt64(buffer);
                obj.Permission = ReadUInt32(buffer);
                obj.CharacterListElement = ReadEntity<CDataCharacterListElement>(buffer);
                return obj;
            }
        }
    }
}
