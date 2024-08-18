using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentRewardListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_REWARD_LIST_REQ;

        public C2SBattleContentRewardListReq()
        {
        }

        public GameMode GameMode {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SBattleContentRewardListReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentRewardListReq obj)
            {
                WriteUInt32(buffer, (uint) obj.GameMode);
            }

            public override C2SBattleContentRewardListReq Read(IBuffer buffer)
            {
                C2SBattleContentRewardListReq obj = new C2SBattleContentRewardListReq();
                obj.GameMode = (GameMode)ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
