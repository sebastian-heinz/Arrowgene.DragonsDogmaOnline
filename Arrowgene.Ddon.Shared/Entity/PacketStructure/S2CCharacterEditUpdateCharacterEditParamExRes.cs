using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterEditUpdateCharacterEditParamExRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_EX_RES;

        public class Serializer : PacketEntitySerializer<S2CCharacterEditUpdateCharacterEditParamExRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterEditUpdateCharacterEditParamExRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CCharacterEditUpdateCharacterEditParamExRes Read(IBuffer buffer)
            {
                S2CCharacterEditUpdateCharacterEditParamExRes obj = new S2CCharacterEditUpdateCharacterEditParamExRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}