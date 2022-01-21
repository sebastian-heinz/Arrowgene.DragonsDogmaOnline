using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLobbyChatMsgReq {

        public CDataLobbyChatMsgReq() {
            type = 0;
            unk2 = 0;
            unk3 = 0;
            unk4 = 0;
            unk5 = 0;
            strMessage = string.Empty;
        }

        public CDataLobbyChatMsgType.Type type; 
        public uint unk2; // Target ID?
        public byte unk3;
        public uint unk4;
        public uint unk5;
        public string strMessage;
    }

    public class CDataLobbyChatMsgReqSerializer : EntitySerializer<CDataLobbyChatMsgReq> {
        public override void Write(IBuffer buffer, CDataLobbyChatMsgReq obj)
        {
            WriteByte(buffer, (byte) obj.type);
            WriteUInt32(buffer, obj.unk2);
            WriteByte(buffer, obj.unk3);
            WriteUInt32(buffer, obj.unk4);
            WriteUInt32(buffer, obj.unk5);
            WriteMtString(buffer, obj.strMessage);
        }

        public override CDataLobbyChatMsgReq Read(IBuffer buffer)
        {
            CDataLobbyChatMsgReq obj = new CDataLobbyChatMsgReq();
            obj.type = (CDataLobbyChatMsgType.Type) buffer.ReadByte();
            obj.unk2 = buffer.ReadUInt32();
            obj.unk3 = buffer.ReadByte();
            obj.unk4 = buffer.ReadUInt32();
            obj.unk5 = buffer.ReadUInt32();
            obj.strMessage = buffer.ReadMtString();
            return obj;
        }
    }
}