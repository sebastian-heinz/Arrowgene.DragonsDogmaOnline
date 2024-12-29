using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CDeleteCharacterInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.L2C_DELETE_CHARACTER_INFO_RES;

        public class Serializer : PacketEntitySerializer<L2CDeleteCharacterInfoRes>
        {
            public override void Write(IBuffer buffer, L2CDeleteCharacterInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override L2CDeleteCharacterInfoRes Read(IBuffer buffer)
            {
                L2CDeleteCharacterInfoRes obj = new L2CDeleteCharacterInfoRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
