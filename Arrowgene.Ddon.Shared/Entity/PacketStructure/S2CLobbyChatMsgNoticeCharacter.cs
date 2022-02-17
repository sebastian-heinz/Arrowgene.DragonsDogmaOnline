using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CLobbyChatMsgNoticeCharacterBaseInfo
    {
        public S2CLobbyChatMsgNoticeCharacterBaseInfo()
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

        public class Serializer : EntitySerializer<S2CLobbyChatMsgNoticeCharacterBaseInfo>
        {
            public override void Write(IBuffer buffer, S2CLobbyChatMsgNoticeCharacterBaseInfo obj)
            {
                WriteUInt32(buffer, obj.characterId);
                WriteMtString(buffer, obj.strFirstName);
                WriteMtString(buffer, obj.strLastName);
                WriteMtString(buffer, obj.strClanName);
            }

            public override S2CLobbyChatMsgNoticeCharacterBaseInfo Read(IBuffer buffer)
            {
                S2CLobbyChatMsgNoticeCharacterBaseInfo obj = new S2CLobbyChatMsgNoticeCharacterBaseInfo();
                obj.characterId = ReadByte(buffer);
                obj.strFirstName = ReadMtString(buffer);
                obj.strLastName = ReadMtString(buffer);
                obj.strClanName = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
