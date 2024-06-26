using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCharacterSwitchGameModeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CHARACTER_SWITCH_GAME_MODE_REQ;

        public int Unk0 {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SCharacterSwitchGameModeReq>
        {
            public override void Write(IBuffer buffer, C2SCharacterSwitchGameModeReq obj)
            {
                WriteInt32(buffer, obj.Unk0);
            }

            public override C2SCharacterSwitchGameModeReq Read(IBuffer buffer)
            {
                C2SCharacterSwitchGameModeReq obj = new C2SCharacterSwitchGameModeReq();
                obj.Unk0 = ReadInt32(buffer);
                return obj;
            }
        }
    }
}
