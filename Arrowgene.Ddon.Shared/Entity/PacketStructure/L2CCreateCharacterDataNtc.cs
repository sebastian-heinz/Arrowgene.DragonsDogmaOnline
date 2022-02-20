using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class L2CCreateCharacterDataNtc : ServerResponse
    {
        public override PacketId Id => PacketId.L2C_CREATE_CHARACTER_DATA_NTC;

        public class Serializer : PacketEntitySerializer<L2CCreateCharacterDataNtc>
        {
            public override void Write(IBuffer buffer, L2CCreateCharacterDataNtc obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override L2CCreateCharacterDataNtc Read(IBuffer buffer)
            {
                L2CCreateCharacterDataNtc obj = new L2CCreateCharacterDataNtc();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
