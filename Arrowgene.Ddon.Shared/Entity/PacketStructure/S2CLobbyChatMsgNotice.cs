using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CLobbyChatMsgNotice {

        // Unidentified variables from the PS4 version:
        //  ucType (u8) (use CDataLobbyChatMsgType when identified)
        //  unHandleId (u32)

        public S2CLobbyChatMsgNotice() {
            unk0 = 0;
            unk1 = 0;
            characterBaseInfo = new S2CLobbyChatMsgNoticeCharacterBaseInfo();
            unk2 = 0;
            unk3 = 0;
            unk4 = 0;
            strMessage = string.Empty;
        }

        public byte unk0;
        public uint unk1;
        public S2CLobbyChatMsgNoticeCharacterBaseInfo characterBaseInfo;
        public byte unk2;
        public uint unk3;
        public uint unk4;
        public string strMessage;
    }

    public class S2CLobbyChatMsgNoticeSerializer : EntitySerializer<S2CLobbyChatMsgNotice> {
        public override void Write(IBuffer buffer, S2CLobbyChatMsgNotice obj)
        {
            WriteByte(buffer, obj.unk0);
            WriteUInt32(buffer, obj.unk1);
            WriteEntity<S2CLobbyChatMsgNoticeCharacterBaseInfo>(buffer, obj.characterBaseInfo);
            WriteByte(buffer, obj.unk2);
            WriteUInt32(buffer, obj.unk3);
            WriteUInt32(buffer, obj.unk4);
            WriteMtString(buffer, obj.strMessage);
        }

        public override S2CLobbyChatMsgNotice Read(IBuffer buffer)
        {
            S2CLobbyChatMsgNotice obj = new S2CLobbyChatMsgNotice();
            obj.unk0 = ReadByte(buffer);
            obj.unk1 = ReadUInt32(buffer);
            obj.characterBaseInfo = ReadEntity<S2CLobbyChatMsgNoticeCharacterBaseInfo>(buffer);
            obj.unk2 = ReadByte(buffer);
            obj.unk3 = ReadUInt32(buffer);
            obj.unk4 = ReadUInt32(buffer);
            obj.strMessage = ReadMtString(buffer);
            return obj;
        }
    }
}