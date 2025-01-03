using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    /// <summary>
    /// Disconnects the client while displaying the matching error message.
    /// Doesn't prompt a logout request, is a hard DC.
    /// </summary>
    public class S2CConnectionCriticalErrorNtc : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CONNECTION_CRITICAL_ERROR_NTC;

        public ErrorCode ErrorCode { get; set; }

        public class Serializer : PacketEntitySerializer<S2CConnectionCriticalErrorNtc>
        {
            public override void Write(IBuffer buffer, S2CConnectionCriticalErrorNtc obj)
            {
                WriteUInt32(buffer, (uint)obj.ErrorCode);
            }

            public override S2CConnectionCriticalErrorNtc Read(IBuffer buffer)
            {
                S2CConnectionCriticalErrorNtc obj = new S2CConnectionCriticalErrorNtc();
                obj.ErrorCode = (ErrorCode)ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
