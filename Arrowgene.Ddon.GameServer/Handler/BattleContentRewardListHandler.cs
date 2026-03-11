using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

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

            uint goldMarks = (uint)rewards.Values.Sum(x => x.GoldMarks);
            uint silverMarks = (uint)rewards.Values.Sum(x => x.SilverMarks);
            uint redMarks = (uint)rewards.Values.Sum(x => x.RedMarks);


            if (goldMarks > 0)
            {
                result.RewardParamList.Add(new CDataBattleContentRewardParam()
                {
                    WalletType = WalletType.GoldenDragonMark,
                    Amount = goldMarks
                });
            }

            if (silverMarks > 0)
            {
                result.RewardParamList.Add(new CDataBattleContentRewardParam()
                {
                    WalletType = WalletType.SilverDragonMark,
                    Amount = silverMarks
                });
            }

            if (redMarks > 0)
            {
                result.RewardParamList.Add(new CDataBattleContentRewardParam()
                {
                    WalletType = WalletType.RedDragonMark,
                    Amount = redMarks
                });
            }

            return result;
        }
    }
}
