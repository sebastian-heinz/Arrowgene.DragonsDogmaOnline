using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CGetErrorMessageListNtc : IPacketStructure
    {
        public L2CGetErrorMessageListNtc()
        {
            ErrorCodes = new List<ClientErrorCode>();
        }

        public L2CGetErrorMessageListNtc(List<ClientErrorCode> errorCodes)
        {
            ErrorCodes = errorCodes;
        }

        public List<ClientErrorCode> ErrorCodes { get; set; }
        public PacketId Id => PacketId.L2C_GET_ERROR_MESSAGE_LIST_NTC;

        public class Serializer : EntitySerializer<L2CGetErrorMessageListNtc>
        {
            public override void Write(IBuffer buffer, L2CGetErrorMessageListNtc obj)
            {
                WriteUInt32(buffer, (uint) obj.ErrorCodes.Count);
                for (int i = 0; i < obj.ErrorCodes.Count; i++)
                {
                    ClientErrorCode errorCode = obj.ErrorCodes[i];
                    WriteUInt32(buffer, errorCode.MessageId);
                    WriteUInt32(buffer, errorCode.ErrorId);

                    if (errorCode.ErrorCode.Length > 0)
                    {
                        // writing error codes for dev purpose, not proper translation
                        WriteMtString(buffer, errorCode.ErrorCode);
                    }
                    else if (errorCode.MessageEn.Length > 0)
                    {
                        // try english next
                        WriteMtString(buffer, errorCode.MessageEn);
                    }
                    else
                    {
                        // try use japanese
                        WriteMtString(buffer, errorCode.MessageJp);
                    }

                    WriteUInt16(buffer, 0);
                }
            }

            public override L2CGetErrorMessageListNtc Read(IBuffer buffer)
            {
                L2CGetErrorMessageListNtc obj = new L2CGetErrorMessageListNtc();
                uint count = ReadUInt32(buffer);
                for (int i = 0; i < count; i++)
                {
                    ClientErrorCode errorCode = new ClientErrorCode();
                    errorCode.MessageId = ReadUInt32(buffer);
                    errorCode.Id = (int) ReadUInt32(buffer);
                    errorCode.ErrorCode = ReadMtString(buffer);
                    ReadUInt16(buffer);
                    obj.ErrorCodes.Add(errorCode);
                }

                return obj;
            }
        }
    }
}
