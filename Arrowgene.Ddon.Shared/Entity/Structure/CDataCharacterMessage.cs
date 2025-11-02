using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterMessage
    {
        public uint MessageNo { get; set; }
        public string Message { get; set; } = "";
        public uint Emotion { get; set; }
        public bool EmotoChat { get; set; }

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
