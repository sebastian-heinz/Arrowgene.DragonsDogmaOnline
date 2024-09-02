using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPartyMember
    {
        public CDataPartyMember()
        {
            CharacterListElement = new CDataCharacterListElement();
            AnyValueList = new byte[8];
        }

        public CDataCharacterListElement CharacterListElement { get; set; }
        public byte MemberType { get; set; }
        public int MemberIndex { get; set; }
        public uint PawnId { get; set; }
        public bool IsLeader { get; set; }
        public bool IsPawn { get; set; }
        public bool IsPlayEntry { get; set; }
        public JoinState JoinState { get; set; }
        public byte[] AnyValueList { get; set; }
        public byte SessionStatus { get; set; }

        public class Serializer : EntitySerializer<CDataPartyMember>
        {
            public override void Write(IBuffer buffer, CDataPartyMember obj)
            {
                WriteEntity(buffer, obj.CharacterListElement);
                WriteByte(buffer, obj.MemberType);
                WriteInt32(buffer, obj.MemberIndex);
                WriteUInt32(buffer, obj.PawnId);
                WriteBool(buffer, obj.IsLeader);
                WriteBool(buffer, obj.IsPawn);
                WriteBool(buffer, obj.IsPlayEntry);
                WriteByte(buffer, (byte) obj.JoinState);
                WriteByteArray(buffer, obj.AnyValueList);
                WriteByte(buffer, obj.SessionStatus);
            }

            public override CDataPartyMember Read(IBuffer buffer)
            {
                CDataPartyMember obj = new CDataPartyMember();
                obj.CharacterListElement = ReadEntity<CDataCharacterListElement>(buffer);
                obj.MemberType = ReadByte(buffer);
                obj.MemberIndex = ReadInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.IsLeader = ReadBool(buffer);
                obj.IsPawn = ReadBool(buffer);
                obj.IsPlayEntry = ReadBool(buffer);
                obj.JoinState = (JoinState) ReadByte(buffer);
                obj.AnyValueList = ReadByteArray(buffer, obj.AnyValueList.Length);
                obj.SessionStatus = ReadByte(buffer);
                return obj;
            }
        }
    }
}
