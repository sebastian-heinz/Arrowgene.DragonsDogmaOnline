using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterSwitchGameModeRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CHARACTER_SWITCH_GAME_MODE_RES;
        public int Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCharacterSwitchGameModeRes>
        {
            public override void Write(IBuffer buffer, S2CCharacterSwitchGameModeRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteInt32(buffer, obj.Unk0);
            }

            public override S2CCharacterSwitchGameModeRes Read(IBuffer buffer)
            {
                S2CCharacterSwitchGameModeRes obj = new S2CCharacterSwitchGameModeRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadInt32(buffer);
                return obj;
            }
        }
    }
}
