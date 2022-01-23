using Arrowgene.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLobbyMemberInfo
    {
        public CDataLobbyMemberInfo()
        {
            CharacterID = 0;
            FirstName = "";
            LastName = "";
            ClanName = "";
            PawnID = 0;
            Unk0 = 0;
            Unk1 = 0;
            Unk2 = 0;
        }

        public uint CharacterID;
        public string FirstName;
        public string LastName;
        public string ClanName;
        public uint PawnID;

        /*
        Possible names:
            Platform
            ClientVersion
            SessionStatus
            OnlineStatus
        */
        public byte Unk0;
        public byte Unk1;
        public byte Unk2;
    }

    public class CDataLobbyMemberInfoSerializer : EntitySerializer<CDataLobbyMemberInfo>
    {
        public override void Write(IBuffer buffer, CDataLobbyMemberInfo obj)
        {
            buffer.WriteUInt32(obj.CharacterID);
            buffer.WriteMtString(obj.FirstName);
            buffer.WriteMtString(obj.LastName);
            buffer.WriteMtString(obj.ClanName);
            buffer.WriteUInt32(obj.PawnID);
            buffer.WriteByte(obj.Unk0);
            buffer.WriteByte(obj.Unk1);
            buffer.WriteByte(obj.Unk2);
        }

        public override CDataLobbyMemberInfo Read(IBuffer buffer)
        {
            CDataLobbyMemberInfo obj = new CDataLobbyMemberInfo();
            obj.CharacterID = ReadUInt32(buffer);
            obj.FirstName = ReadMtString(buffer);
            obj.LastName = ReadMtString(buffer);
            obj.ClanName = ReadMtString(buffer);
            obj.PawnID = ReadUInt32(buffer);
            obj.Unk0 = ReadByte(buffer);
            obj.Unk1 = ReadByte(buffer);
            obj.Unk2 = ReadByte(buffer);
            return obj;
        }
    }


}
