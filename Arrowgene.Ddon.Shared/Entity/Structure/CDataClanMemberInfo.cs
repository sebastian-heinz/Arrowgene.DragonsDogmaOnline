using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.Clan;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataClanMemberInfo
    {
        public CDataClanMemberInfo()
        {
            CharacterListElement = new CDataCharacterListElement();
            Created = DateTimeOffset.MinValue;
            LastLoginTime = DateTimeOffset.MinValue;
            LeaveTime = DateTimeOffset.MinValue;
        }

        public ClanMemberRank Rank { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastLoginTime { get; set; }
        public DateTimeOffset LeaveTime { get; set; }
        public uint Permission { get; set; }
        public CDataCharacterListElement CharacterListElement { get; set; }
    
        public class Serializer : EntitySerializer<CDataClanMemberInfo>
        {
            public override void Write(IBuffer buffer, CDataClanMemberInfo obj)
            {
                WriteUInt32(buffer, (uint)obj.Rank);
                WriteInt64(buffer, obj.Created.ToUnixTimeSeconds());
                WriteInt64(buffer, obj.LastLoginTime.ToUnixTimeSeconds());
                WriteInt64(buffer, obj.LeaveTime.ToUnixTimeSeconds());
                WriteUInt32(buffer, obj.Permission);
                WriteEntity<CDataCharacterListElement>(buffer, obj.CharacterListElement);
            }
        
            public override CDataClanMemberInfo Read(IBuffer buffer)
            {
                CDataClanMemberInfo obj = new CDataClanMemberInfo();
                obj.Rank = (ClanMemberRank)ReadUInt32(buffer);
                obj.Created = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.LastLoginTime = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.LeaveTime = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
                obj.Permission = ReadUInt32(buffer);
                obj.CharacterListElement = ReadEntity<CDataCharacterListElement>(buffer);
                return obj;
            }
        }
    }
}
