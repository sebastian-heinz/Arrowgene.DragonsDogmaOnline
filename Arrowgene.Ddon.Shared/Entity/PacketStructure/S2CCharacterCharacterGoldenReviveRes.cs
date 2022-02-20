using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterCharacterGoldenReviveRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_CHARACTER_GOLDEN_REVIVE_RES;

        /**
         * The amount of Golden Gemstones the player has after being revived
         * (Normally, current amount - 1)
        */ 
        public uint GP { get; set; }

        public S2CCharacterCharacterGoldenReviveRes()
        {
            GP = 0;
        }

        public class Serializer : PacketEntitySerializer<S2CCharacterCharacterGoldenReviveRes>
        {

            public override void Write(IBuffer buffer, S2CCharacterCharacterGoldenReviveRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.GP);
            }

            public override S2CCharacterCharacterGoldenReviveRes Read(IBuffer buffer)
            {
                S2CCharacterCharacterGoldenReviveRes obj = new S2CCharacterCharacterGoldenReviveRes();
                ReadServerResponse(buffer, obj);
                obj.GP = ReadUInt32(buffer);
                return obj;
            }
        }
    }

}
