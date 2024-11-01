using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataFriendInfo
    {
        public CDataFriendInfo()
        {
            CharacterListElement = new();
        }

        public CDataCharacterListElement CharacterListElement { get; set; }
        public byte PendingStatus { get; set; } 
        public uint FriendNo { get; set; }
        public bool IsFavorite { get; set; }

        public class Serializer : EntitySerializer<CDataFriendInfo>
        {
            public override void Write(IBuffer buffer, CDataFriendInfo obj)
            {
                WriteEntity(buffer, obj.CharacterListElement);
                WriteByte(buffer, obj.PendingStatus);
                WriteUInt32(buffer, obj.FriendNo);
                WriteBool(buffer, obj.IsFavorite);
            }

            public override CDataFriendInfo Read(IBuffer buffer)
            {
                CDataFriendInfo obj = new CDataFriendInfo();
                obj.CharacterListElement = ReadEntity<CDataCharacterListElement>(buffer);
                obj.PendingStatus = ReadByte(buffer);
                obj.FriendNo = ReadUInt32(buffer);
                obj.IsFavorite = ReadBool(buffer);
                return obj;
            }
        }
    }
}
