using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SLobbyChatMsgReq {

        public C2SLobbyChatMsgReq() {
            type = 0;
            unk2 = 0;
            unk3 = 0;
            unk4 = 0;
            unk5 = 0;
            strMessage = string.Empty;
        }

        public Structure.CDataLobbyChatMsgType.Type type; 
        public uint unk2; // Target ID?
        public byte unk3;
        public uint unk4;
        public uint unk5;
        public string strMessage;
    }

    public class C2SLobbyChatMsgReqSerializer : EntitySerializer<C2SLobbyChatMsgReq> {
        public override void Write(IBuffer buffer, C2SLobbyChatMsgReq obj)
        {
            WriteByte(buffer, (byte) obj.type);
            WriteUInt32(buffer, obj.unk2);
            WriteByte(buffer, obj.unk3);
            WriteUInt32(buffer, obj.unk4);
            WriteUInt32(buffer, obj.unk5);
            WriteMtString(buffer, obj.strMessage);
        }

        public override C2SLobbyChatMsgReq Read(IBuffer buffer)
        {
            C2SLobbyChatMsgReq obj = new C2SLobbyChatMsgReq();
            obj.type = (Structure.CDataLobbyChatMsgType.Type) ReadByte(buffer);
            obj.unk2 = ReadUInt32(buffer);
            obj.unk3 = ReadByte(buffer);
            obj.unk4 = ReadUInt32(buffer);
            obj.unk5 = ReadUInt32(buffer);
            obj.strMessage = ReadMtString(buffer);
            return obj;
        }
    }
}
