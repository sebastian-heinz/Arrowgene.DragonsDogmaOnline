using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    /// <summary>
    /// Disconnects the client while displaying the matching error message.
    /// Prompts the client to disconnect itself via C2S_CONNECTION_LOGOUT_REQ.
    /// </summary>
    public class S2CConnectionErrorNtc : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CONNECTION_ERROR_NTC;

        public ErrorCode ErrorCode { get; set; }

        public class Serializer : PacketEntitySerializer<S2CConnectionErrorNtc>
        {
            public override void Write(IBuffer buffer, S2CConnectionErrorNtc obj)
            {
                WriteUInt32(buffer, (uint)obj.ErrorCode);
            }

            public override S2CConnectionErrorNtc Read(IBuffer buffer)
            {
                S2CConnectionErrorNtc obj = new S2CConnectionErrorNtc();
                obj.ErrorCode = (ErrorCode)ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
