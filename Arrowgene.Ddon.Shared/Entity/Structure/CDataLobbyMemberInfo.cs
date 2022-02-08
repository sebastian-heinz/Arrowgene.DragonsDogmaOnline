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
            WriteUInt32(buffer, obj.CharacterID);
            WriteMtString(buffer, obj.FirstName);
            WriteMtString(buffer, obj.LastName);
            WriteMtString(buffer, obj.ClanName);
            WriteUInt32(buffer, obj.PawnID);
            WriteByte(buffer, obj.Unk0);
            WriteByte(buffer, obj.Unk1);
            WriteByte(buffer, obj.Unk2);
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
