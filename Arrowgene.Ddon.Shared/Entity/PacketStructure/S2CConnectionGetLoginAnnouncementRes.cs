using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CConnectionGetLoginAnnouncementRes : ServerResponse
    {
        public S2CConnectionGetLoginAnnouncementRes()
        {
            Message = string.Empty;
        }

        public override PacketId Id => PacketId.S2C_CONNECTION_GET_LOGIN_ANNOUNCEMENT_RES;

        public string Message { get; set; }

        public class Serializer : PacketEntitySerializer<S2CConnectionGetLoginAnnouncementRes>
        {
            public override void Write(IBuffer buffer, S2CConnectionGetLoginAnnouncementRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteMtString(buffer, obj.Message);
            }

            public override S2CConnectionGetLoginAnnouncementRes Read(IBuffer buffer)
            {
                S2CConnectionGetLoginAnnouncementRes obj = new S2CConnectionGetLoginAnnouncementRes();
                ReadServerResponse(buffer, obj);
                obj.Message = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
