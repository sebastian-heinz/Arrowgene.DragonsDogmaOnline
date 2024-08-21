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
            List<CDataUpdateWalletPoint> results = new List<CDataUpdateWalletPoint>();
            if (rewards.GoldMarks > 0)
            {
                results.Add(Server.WalletManager.AddToWallet(client.Character, WalletType.GoldenDragonMark, rewards.GoldMarks));
                rewards.GoldMarks = 0;
            }

            if (rewards.SilverMarks > 0)
            {
                results.Add(Server.WalletManager.AddToWallet(client.Character, WalletType.SilverDragonMark, rewards.SilverMarks));
                rewards.SilverMarks = 0;
            }

            if (rewards.RedMarks > 0)
            {
                results.Add(Server.WalletManager.AddToWallet(client.Character, WalletType.RedDragonMark, rewards.RedMarks));
                rewards.RedMarks = 0;
            }

            if (results.Count > 0)
            {
                S2CItemUpdateCharacterItemNtc updateWalletNtc = new S2CItemUpdateCharacterItemNtc();
                updateWalletNtc.UpdateType = ItemNoticeType.Default;
                updateWalletNtc.UpdateWalletList = results;
                client.Send(updateWalletNtc);
            }

            Server.Database.UpdateBBMRewards(client.Character.CharacterId, rewards);

            // Update Situation Data
            S2CBattleContentProgressNtc progressNtc = new S2CBattleContentProgressNtc();
            progressNtc.BattleContentStatusList.Add(BitterblackMazeManager.GetUpdatedContentStatus(Server, client.Character));
            client.Send(progressNtc);

            return new S2CBattleContentGetRewardRes();
        }
    }
}
