using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLobbyChatMsgNoticeCharacterBaseInfo
    {
        public CDataLobbyChatMsgNoticeCharacterBaseInfo()
        {
            characterId = 0;
            strFirstName = string.Empty;
            strLastName = string.Empty;
            strClanName = string.Empty;
        }

        public uint characterId;
        public string strFirstName;
        public string strLastName;
        public string strClanName;
    }

    public class CDataLobbyChatMsgNoticeCharacterSerializer : EntitySerializer<CDataLobbyChatMsgNoticeCharacterBaseInfo> {
        public override void Write(IBuffer buffer, CDataLobbyChatMsgNoticeCharacterBaseInfo obj)
        {
            WriteUInt32(buffer, obj.characterId);
            WriteMtString(buffer, obj.strFirstName);
            WriteMtString(buffer, obj.strLastName);
            WriteMtString(buffer, obj.strClanName);
        }

        public override CDataLobbyChatMsgNoticeCharacterBaseInfo Read(IBuffer buffer)
        {
            CDataLobbyChatMsgNoticeCharacterBaseInfo obj = new CDataLobbyChatMsgNoticeCharacterBaseInfo();
            obj.characterId = ReadByte(buffer);
            obj.strFirstName = ReadMtString(buffer);
            obj.strLastName = ReadMtString(buffer);
            obj.strClanName = ReadMtString(buffer);
            return obj;
        }
    }
}