using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterEditUpdateCharacterEditParamRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_EDIT_UPDATE_CHARACTER_EDIT_PARAM_RES;

        public class Serializer : PacketEntitySerializer<S2CCharacterEditUpdateCharacterEditParamRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterEditUpdateCharacterEditParamRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CCharacterEditUpdateCharacterEditParamRes Read(IBuffer buffer)
            {
                S2CCharacterEditUpdateCharacterEditParamRes obj = new S2CCharacterEditUpdateCharacterEditParamRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}