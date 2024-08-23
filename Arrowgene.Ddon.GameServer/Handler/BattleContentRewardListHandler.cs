using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentRewardListHandler : GameRequestPacketHandler<C2SBattleContentRewardListReq, S2CBattleContentRewardListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentRewardListHandler));

        public BattleContentRewardListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentRewardListRes Handle(GameClient client, C2SBattleContentRewardListReq request)
        {
            var rewards = Server.Database.SelectBBMRewards(client.Character.CharacterId);
            var result = new S2CBattleContentRewardListRes();

            if (rewards.GoldMarks > 0)
            {
                result.RewardParamList.Add(new CDataBattleContentRewardParam()
                {
                    WalletType = WalletType.GoldenDragonMark,
                    Amount = rewards.GoldMarks
                });
            }

            if (rewards.SilverMarks > 0)
            {
                result.RewardParamList.Add(new CDataBattleContentRewardParam()
                {
                    WalletType = WalletType.SilverDragonMark,
                    Amount = rewards.SilverMarks
                });
            }

            if (rewards.RedMarks > 0)
            {
                result.RewardParamList.Add(new CDataBattleContentRewardParam()
                {
                    WalletType = WalletType.RedDragonMark,
                    Amount = rewards.RedMarks
                });
            }

            return result;
        }
    }
}
