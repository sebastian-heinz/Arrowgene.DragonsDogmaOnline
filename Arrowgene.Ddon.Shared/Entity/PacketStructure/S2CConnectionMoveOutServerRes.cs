using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CConnectionMoveOutServerRes : ServerResponse
    {
        public string SessionKey { get; set; } = string.Empty;

        public override PacketId Id => PacketId.S2C_CONNECTION_MOVE_OUT_SERVER_RES;

        public class Serializer : PacketEntitySerializer<S2CConnectionMoveOutServerRes>
        {

            public override void Write(IBuffer buffer, S2CConnectionMoveOutServerRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteMtString(buffer, obj.SessionKey);
            }

            public override S2CConnectionMoveOutServerRes Read(IBuffer buffer)
            {
                S2CConnectionMoveOutServerRes obj = new S2CConnectionMoveOutServerRes();
                ReadServerResponse(buffer, obj);
                obj.SessionKey = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
