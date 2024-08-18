using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentCharacterInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_CHARACTER_INFO_REQ;

        public C2SBattleContentCharacterInfoReq()
        {
        }

        public GameMode GameMode {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SBattleContentCharacterInfoReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentCharacterInfoReq obj)
            {
                WriteUInt32(buffer, (uint) obj.GameMode);
            }

            public override C2SBattleContentCharacterInfoReq Read(IBuffer buffer)
            {
                C2SBattleContentCharacterInfoReq obj = new C2SBattleContentCharacterInfoReq();
                obj.GameMode = (GameMode)ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

