using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentGetRewardReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_GET_REWARD_REQ;

        public C2SBattleContentGetRewardReq()
        {
        }

        public GameMode GameMode {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SBattleContentGetRewardReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentGetRewardReq obj)
            {
                WriteUInt32(buffer, (uint) obj.GameMode);
            }

            public override C2SBattleContentGetRewardReq Read(IBuffer buffer)
            {
                C2SBattleContentGetRewardReq obj = new C2SBattleContentGetRewardReq();
                obj.GameMode = (GameMode) ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
