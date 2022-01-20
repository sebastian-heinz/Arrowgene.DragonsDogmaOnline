using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLobbyChatMsgNotice {

        // Unidentified variables from the PS4 version:
        //  ucType (u8) (use CDataLobbyChatMsgType when identified)
        //  unHandleId (u32)

        public CDataLobbyChatMsgNotice() {
            unk0 = 0;
            unk1 = 0;
            characterBaseInfo = new CDataLobbyChatMsgNoticeCharacterBaseInfo();
            unk2 = 0;
            unk3 = 0;
            unk4 = 0;
            strMessage = string.Empty;
        }

        public byte unk0;
        public uint unk1;
        public CDataLobbyChatMsgNoticeCharacterBaseInfo characterBaseInfo;
        public byte unk2;
        public uint unk3;
        public uint unk4;
        public string strMessage;
    }

    public class CDataLobbyChatMsgNoticeSerializer : EntitySerializer<CDataLobbyChatMsgNotice> {
        public override void Write(IBuffer buffer, CDataLobbyChatMsgNotice obj)
        {
            WriteByte(buffer, obj.unk0);
            WriteUInt32(buffer, obj.unk1);
            WriteEntity<CDataLobbyChatMsgNoticeCharacterBaseInfo>(buffer, obj.characterBaseInfo);
            WriteByte(buffer, obj.unk2);
            WriteUInt32(buffer, obj.unk3);
            WriteUInt32(buffer, obj.unk4);
            WriteMtString(buffer, obj.strMessage);
        }

        public override CDataLobbyChatMsgNotice Read(IBuffer buffer)
        {
            CDataLobbyChatMsgNotice obj = new CDataLobbyChatMsgNotice();
            obj.unk0 = ReadByte(buffer);
            obj.unk1 = ReadUInt32(buffer);
            obj.characterBaseInfo = ReadEntity<CDataLobbyChatMsgNoticeCharacterBaseInfo>(buffer);
            obj.unk2 = ReadByte(buffer);
            obj.unk3 = ReadUInt32(buffer);
            obj.unk4 = ReadUInt32(buffer);
            obj.strMessage = ReadMtString(buffer);
            return obj;
        }
    }
}