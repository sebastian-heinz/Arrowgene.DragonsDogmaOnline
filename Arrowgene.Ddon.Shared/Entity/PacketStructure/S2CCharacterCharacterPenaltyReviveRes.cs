using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterCharacterPenaltyReviveRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_CHARACTER_PENALTY_REVIVE_RES;

        public S2CCharacterCharacterPenaltyReviveRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CCharacterCharacterPenaltyReviveRes>
        {

            public override void Write(IBuffer buffer, S2CCharacterCharacterPenaltyReviveRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CCharacterCharacterPenaltyReviveRes Read(IBuffer buffer)
            {
                S2CCharacterCharacterPenaltyReviveRes obj = new S2CCharacterCharacterPenaltyReviveRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }

}
