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

        public class Serializer : EntitySerializer<CDataCharacterMessage>
        {
            public override void Write(IBuffer buffer, CDataCharacterMessage obj)
            {
                WriteUInt32(buffer, obj.MessageNo);
                WriteMtString(buffer, obj.Message);
                WriteUInt32(buffer, obj.Emotion);
                WriteBool(buffer, obj.EmotoChat);
            }

            public override CDataCharacterMessage Read(IBuffer buffer)
            {
                CDataCharacterMessage obj = new CDataCharacterMessage();
                obj.MessageNo = ReadUInt32(buffer);
                obj.Message = ReadMtString(buffer);
                obj.Emotion = ReadUInt32(buffer);
                obj.EmotoChat = ReadBool(buffer);
                return obj;
            }
        }
    }
}
