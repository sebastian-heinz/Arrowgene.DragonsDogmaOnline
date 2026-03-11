using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentGetRewardHandler : GameRequestPacketHandler<C2SBattleContentGetRewardReq, S2CBattleContentGetRewardRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentGetRewardHandler));

        public BattleContentGetRewardHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentGetRewardRes Handle(GameClient client, C2SBattleContentGetRewardReq request)
        {
            // Claim marks
            var rewards = Server.Database.SelectBBMRewards(client.Character.CharacterId);
            List<CDataUpdateWalletPoint> results = [];

            uint goldMarks = (uint)rewards.Values.Sum(x => x.GoldMarks);
            uint silverMarks = (uint)rewards.Values.Sum(x => x.SilverMarks);
            uint redMarks = (uint)rewards.Values.Sum(x => x.RedMarks);

            if (goldMarks > 0)
            {
                results.Add(Server.WalletManager.AddToWallet(client.Character, WalletType.GoldenDragonMark, goldMarks));
            }

            if (silverMarks > 0)
            {
                results.Add(Server.WalletManager.AddToWallet(client.Character, WalletType.SilverDragonMark, silverMarks));
            }

            if (redMarks > 0)
            {
                results.Add(Server.WalletManager.AddToWallet(client.Character, WalletType.RedDragonMark, redMarks));
            }

            foreach(var reward in rewards.Values)
            {
                reward.GoldMarks = 0;
                reward.SilverMarks = 0;
                reward.RedMarks = 0;
                Server.Database.UpdateBBMRewards(client.Character.CharacterId, reward);
            }

            if (results.Count > 0)
            {
                S2CItemUpdateCharacterItemNtc updateWalletNtc = new()
                {
                    UpdateType = ItemNoticeType.Default,
                    UpdateWalletList = results
                };
                client.Send(updateWalletNtc);
            }

            // Update Situation Data
            S2CBattleContentProgressNtc progressNtc = new S2CBattleContentProgressNtc();
            progressNtc.BattleContentStatusList.Add(BitterblackMazeManager.GetUpdatedContentStatus(Server, client.Character));
            client.Send(progressNtc);

            return new S2CBattleContentGetRewardRes();
        }
    }
}
