using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterSwitchGameModeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_SWITCH_GAME_MODE_REQ;

        public GameMode GameMode {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SCharacterSwitchGameModeReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterSwitchGameModeReq obj)
            {
                WriteInt32(buffer, (int) obj.GameMode);
            }

            public override C2SCharacterSwitchGameModeReq Read(IBuffer buffer)
            {
                C2SCharacterSwitchGameModeReq obj = new C2SCharacterSwitchGameModeReq();
                obj.GameMode = (GameMode) ReadInt32(buffer);
                return obj;
            }
        }
    }
}
