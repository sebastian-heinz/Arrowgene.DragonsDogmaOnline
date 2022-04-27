using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SLobbyChatMsgReq : IPacketStructure {

        public PacketId Id => PacketId.C2S_LOBBY_LOBBY_CHAT_MSG_REQ;

        public LobbyChatMsgType Type { get; set; } 
        public uint Unk1 { get; set; } // Target ID?
        public byte Unk2 { get; set; }
        public uint Unk3 { get; set; }
        public uint Unk4 { get; set; }
        public string StrMessage { get; set; }

        public C2SLobbyChatMsgReq() {
            Type = 0;
            Unk1 = 0;
            Unk2 = 0;
            Unk3 = 0;
            Unk4 = 0;
            StrMessage = string.Empty;
        }

        public class Serializer : PacketEntitySerializer<C2SLobbyChatMsgReq> {
            
            
            public override void Write(IBuffer buffer, C2SLobbyChatMsgReq obj)
            {
                WriteByte(buffer, (byte) obj.Type);
                WriteUInt32(buffer, obj.Unk1);
                WriteByte(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteUInt32(buffer, obj.Unk4);
                WriteMtString(buffer, obj.StrMessage);
            }

            public override C2SLobbyChatMsgReq Read(IBuffer buffer)
            {
                C2SLobbyChatMsgReq obj = new C2SLobbyChatMsgReq();
                obj.Type = (LobbyChatMsgType) ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadByte(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                obj.StrMessage = ReadMtString(buffer);
                return obj;
            }
        }

    }
}
