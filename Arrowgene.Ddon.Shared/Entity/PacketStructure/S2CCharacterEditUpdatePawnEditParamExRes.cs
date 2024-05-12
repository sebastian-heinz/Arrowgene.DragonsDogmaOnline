using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterEditUpdatePawnEditParamExRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_EX_RES;

        public class Serializer : PacketEntitySerializer<S2CCharacterEditUpdatePawnEditParamExRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterEditUpdatePawnEditParamExRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CCharacterEditUpdatePawnEditParamExRes Read(IBuffer buffer)
            {
                S2CCharacterEditUpdatePawnEditParamExRes obj = new S2CCharacterEditUpdatePawnEditParamExRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}