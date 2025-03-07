using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CConnectionLoginRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CONNECTION_LOGIN_RES;

        public string OneTimeToken { get; set; } = string.Empty;
        public bool IsCogLogin { get; set; }

        public class Serializer : PacketEntitySerializer<S2CConnectionLoginRes>
        {

            public override void Write(IBuffer buffer, S2CConnectionLoginRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt16(buffer, obj.IsCogLogin ? (ushort) 1 : (ushort) 0);
                WriteMtString(buffer, obj.OneTimeToken);
            }

            public override S2CConnectionLoginRes Read(IBuffer buffer)
            {
                S2CConnectionLoginRes obj = new S2CConnectionLoginRes();
                ReadServerResponse(buffer, obj);
                ushort isCogLogin = ReadUInt16(buffer);
                obj.IsCogLogin = isCogLogin != 0;
                obj.OneTimeToken = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
