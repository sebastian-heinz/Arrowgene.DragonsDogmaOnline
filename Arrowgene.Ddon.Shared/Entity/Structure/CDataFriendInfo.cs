using System;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataFriendInfo
    {
        public CDataFriendInfo()
        {
            CharacterListElement = new CDataCharacterListElement();
        }

        public CDataCharacterListElement CharacterListElement { get; set; }
        public byte PendingStatus { get; set; }
        public UInt32 UnFriendNo { get; set; }
        public bool IsFavorite { get; set; }

        public class Serializer : EntitySerializer<CDataFriendInfo>
        {
            public override void Write(IBuffer buffer, CDataFriendInfo obj)
            {
                WriteEntity(buffer, obj.CharacterListElement);
                WriteByte(buffer, obj.PendingStatus);
                WriteUInt32(buffer, obj.UnFriendNo);
                WriteBool(buffer, obj.IsFavorite);
            }

            public override CDataFriendInfo Read(IBuffer buffer)
            {
                CDataFriendInfo obj = new CDataFriendInfo();
                obj.CharacterListElement = ReadEntity<CDataCharacterListElement>(buffer);
                obj.PendingStatus = ReadByte(buffer);
                obj.UnFriendNo = ReadUInt32(buffer);
                obj.IsFavorite = ReadBool(buffer);
                return obj;
            }
        }
    }
}
