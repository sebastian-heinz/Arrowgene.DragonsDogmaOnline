using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class S2CGpGetCapRes : ServerResponse
{
    public override PacketId Id => PacketId.S2C_GP_GET_CAP_RES;

    public uint CAP { get; set; }

    public class Serializer : PacketEntitySerializer<S2CGpGetCapRes>
    {
        public override void Write(IBuffer buffer, S2CGpGetCapRes obj)
        {
            WriteServerResponse(buffer, obj);

            WriteUInt32(buffer, obj.CAP);
        }

        public override S2CGpGetCapRes Read(IBuffer buffer)
        {
            var obj = new S2CGpGetCapRes();

            ReadServerResponse(buffer, obj);

            obj.CAP = ReadUInt32(buffer);

            return obj;
        }
    }
}
