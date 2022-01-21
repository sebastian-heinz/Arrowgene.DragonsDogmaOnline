using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterMessage
    {
        public CDataCharacterMessage()
        {
            MessageNo = 0;
            Message = "";
            Emotion = 0;
            EmotoChat = false;
        }

        public uint MessageNo;
        public string Message;
        public uint Emotion;
        public bool EmotoChat;
    }

    public class CDataCharacterMessageSerializer : EntitySerializer<CDataCharacterMessage>
    {
        public override void Write(IBuffer buffer, CDataCharacterMessage obj)
        {
            WriteUInt32(buffer, obj.MessageNo);
            WriteMtString(buffer, obj.Message);
            WriteUInt32(buffer, obj.Emotion);
            WriteByte(buffer, obj.EmotoChat ? (byte)1 : (byte)0);
        }

        public override CDataCharacterMessage Read(IBuffer buffer)
        {
            CDataCharacterMessage obj = new CDataCharacterMessage();
            obj.MessageNo = ReadUInt32(buffer);
            obj.Message = ReadMtString(buffer);
            obj.Emotion = ReadUInt32(buffer);
            obj.EmotoChat = ReadByte(buffer) == 1;
            return obj;
        }
    }
}
