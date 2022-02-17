using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CLobbyChatMsgNotice : IPacketStructure {

        // Unidentified variables from the PS4 version:
        //  ucType (u8) (use CDataLobbyChatMsgType when identified)
        //  unHandleId (u32)

        public PacketId Id => PacketId.S2C_LOBBY_LOBBY_CHAT_MSG_NTC;

        public byte Unk0;
        public uint Unk1;
        public S2CLobbyChatMsgNoticeCharacterBaseInfo CharacterBaseInfo;
        public byte Unk2;
        public uint Unk3;
        public uint Unk4;
        public string StrMessage;

        public S2CLobbyChatMsgNotice() {
            Unk0 = 0;
            Unk1 = 0;
            CharacterBaseInfo = new S2CLobbyChatMsgNoticeCharacterBaseInfo();
            Unk2 = 0;
            Unk3 = 0;
            Unk4 = 0;
            StrMessage = string.Empty;
        }

        public class Serializer : EntitySerializer<S2CLobbyChatMsgNotice> {
            static Serializer()
            {
                Id = PacketId.S2C_LOBBY_LOBBY_CHAT_MSG_NTC;
            }
            
            public override void Write(IBuffer buffer, S2CLobbyChatMsgNotice obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteEntity<S2CLobbyChatMsgNoticeCharacterBaseInfo>(buffer, obj.CharacterBaseInfo);
                WriteByte(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteUInt32(buffer, obj.Unk4);
                WriteMtString(buffer, obj.StrMessage);
            }

            public override S2CLobbyChatMsgNotice Read(IBuffer buffer)
            {
                S2CLobbyChatMsgNotice obj = new S2CLobbyChatMsgNotice();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.CharacterBaseInfo = ReadEntity<S2CLobbyChatMsgNoticeCharacterBaseInfo>(buffer);
                obj.Unk2 = ReadByte(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                obj.StrMessage = ReadMtString(buffer);
                return obj;
            }
        }

    }

}
