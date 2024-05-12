using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataFriendInfo
    {
        public CDataCharacterListElement CharacterListElement { get; set; } = new();
        public byte PendingStatus { get; set; } = 0;
        public UInt32 UnFriendNo { get; set; } = 0;
        public bool IsFavorite { get; set; } = false;

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
