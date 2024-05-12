using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CGetErrorMessageListNtc : IPacketStructure
    {
        public L2CGetErrorMessageListNtc()
        {
            ErrorMessages = new List<CDataErrorMessage>();
        }

        public List<CDataErrorMessage> ErrorMessages { get; set; }
        public PacketId Id => PacketId.L2C_GET_ERROR_MESSAGE_LIST_NTC;

        public class Serializer : PacketEntitySerializer<L2CGetErrorMessageListNtc>
        {
            public override void Write(IBuffer buffer, L2CGetErrorMessageListNtc obj)
            {
                WriteEntityList<CDataErrorMessage>(buffer, obj.ErrorMessages);
            }

            public override L2CGetErrorMessageListNtc Read(IBuffer buffer)
            {
                L2CGetErrorMessageListNtc obj = new L2CGetErrorMessageListNtc();
                obj.ErrorMessages = ReadEntityList<CDataErrorMessage>(buffer);
                return obj;
            }
        }
    }
}
