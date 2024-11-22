using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanBaseReleaseRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_BASE_RELEASE_RES;

        public S2CClanClanBaseReleaseRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CClanClanBaseReleaseRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanBaseReleaseRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CClanClanBaseReleaseRes Read(IBuffer buffer)
            {
                S2CClanClanBaseReleaseRes obj = new S2CClanClanBaseReleaseRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
