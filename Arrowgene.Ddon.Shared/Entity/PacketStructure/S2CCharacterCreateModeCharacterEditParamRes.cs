using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterCreateModeCharacterEditParamRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_CREATE_MODE_CHARACTER_EDIT_PARAM_RES;

        public class Serializer : PacketEntitySerializer<S2CCharacterCreateModeCharacterEditParamRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterCreateModeCharacterEditParamRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CCharacterCreateModeCharacterEditParamRes Read(IBuffer buffer)
            {
                S2CCharacterCreateModeCharacterEditParamRes obj = new S2CCharacterCreateModeCharacterEditParamRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
