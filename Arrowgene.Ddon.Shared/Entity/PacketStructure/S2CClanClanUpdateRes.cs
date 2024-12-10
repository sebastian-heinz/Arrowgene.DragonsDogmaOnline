using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CClanClanUpdateRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CLAN_CLAN_UPDATE_RES;

        public S2CClanClanUpdateRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CClanClanUpdateRes>
        {
            public override void Write(IBuffer buffer, S2CClanClanUpdateRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CClanClanUpdateRes Read(IBuffer buffer)
            {
                S2CClanClanUpdateRes obj = new S2CClanClanUpdateRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
