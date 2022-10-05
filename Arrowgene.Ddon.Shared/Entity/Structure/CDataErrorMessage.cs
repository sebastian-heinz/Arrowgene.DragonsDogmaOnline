using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataErrorMessage
    {
        public CDataErrorMessage()
        {
            MessageId = 0;
            ErrorId = ErrorCode.ERROR_CODE_SUCCESS;
            Message = string.Empty;
            DevelopMessage = string.Empty;
        }

        public uint MessageId { get; set; }
        public ErrorCode ErrorId { get; set; }
        public string Message { get; set; }
        public string DevelopMessage { get; set; }

        public class Serializer : EntitySerializer<CDataErrorMessage>
        {
            public override void Write(IBuffer buffer, CDataErrorMessage obj)
            {
                WriteUInt32(buffer, obj.MessageId);
                WriteUInt32(buffer, (uint)obj.ErrorId);
                WriteMtString(buffer, obj.Message);
                WriteMtString(buffer, obj.DevelopMessage);
            }

            public override CDataErrorMessage Read(IBuffer buffer)
            {
                CDataErrorMessage obj = new CDataErrorMessage();
                obj.MessageId = ReadUInt32(buffer);
                obj.ErrorId = (ErrorCode)ReadUInt32(buffer);
                obj.Message = ReadMtString(buffer);
                obj.DevelopMessage = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
