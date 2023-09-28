using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterEditUpdatePawnEditParamRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_EDIT_UPDATE_PAWN_EDIT_PARAM_RES;

        public class Serializer : PacketEntitySerializer<S2CCharacterEditUpdatePawnEditParamRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterEditUpdatePawnEditParamRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CCharacterEditUpdatePawnEditParamRes Read(IBuffer buffer)
            {
                S2CCharacterEditUpdatePawnEditParamRes obj = new S2CCharacterEditUpdatePawnEditParamRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}