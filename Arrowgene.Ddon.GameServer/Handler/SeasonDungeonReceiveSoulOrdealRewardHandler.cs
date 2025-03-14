using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonReceiveSoulOrdealRewardHandler : GameRequestPacketHandler<C2SSeasonDungeonReceiveSoulOrdealRewardReq, S2CSeasonDungeonReceiveSoulOrdealRewardRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeonReceiveSoulOrdealRewardHandler));

        public SeasonDungeonReceiveSoulOrdealRewardHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSeasonDungeonReceiveSoulOrdealRewardRes Handle(GameClient client, C2SSeasonDungeonReceiveSoulOrdealRewardReq request)
        {
            client.Send(new S2CSeasonDungeonSetOmStateNtc()
            {
                LayoutId = request.LayoutId,
                PosId = request.PosId,
                State = SoulOrdealOmState.RewardReceived
            });

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.SoulOrdealReward
            };

            var rewards = Server.EpitaphRoadManager.GetRewards(client, request.LayoutId.AsStageLayoutId(), request.PosId);
            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var reward in request.RewardList)
                {
                    var item = rewards.ItemRewards[(int)reward.RewardIndex];
                    if (Server.ItemManager.IsItemWalletPoint(item.ItemId))
                    {
                        (WalletType walletType, uint amount) = Server.ItemManager.ItemToWalletPoint(item.ItemId);
                        var result = Server.WalletManager.AddToWallet(client.Character, walletType, amount * reward.Amount, connectionIn: connection);
                        updateCharacterItemNtc.UpdateWalletList.Add(result);
                    }
                    else if (reward.Amount > 0)
                    {
                        var result = Server.ItemManager.AddItem(Server, client.Character, true, item.ItemId, reward.Amount, connectionIn: connection);
                        updateCharacterItemNtc.UpdateItemList.AddRange(result);
                    }
                }
            });
            client.Send(updateCharacterItemNtc);

            Server.EpitaphRoadManager.CollectTrialRewards(client, request.LayoutId.AsStageLayoutId(), request.PosId);

            return new S2CSeasonDungeonReceiveSoulOrdealRewardRes()
            {
                RewardList = rewards.ItemRewards.Select((x, index) => new CDataSoulOrdealRewardItem() { RewardIndex = (uint)index, Amount = 0 }).ToList()
            };
        }
    }
}
