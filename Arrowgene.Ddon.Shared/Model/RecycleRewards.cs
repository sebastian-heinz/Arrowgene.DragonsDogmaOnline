using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model
{
    public class RecycleRewards
    {
        public RecycleRewards()
        {
            WalletRewards = new List<(WalletType WalletType, uint Amount)>();
            ItemRewards = new List<(ItemId ItemId, uint Amount)>();
        }

        public uint NumRewards { get; set; }
        public List<(WalletType WalletType, uint Amount)> WalletRewards { get; set; }
        public List<(ItemId ItemId, uint Amount)> ItemRewards { get; set; } // Total possible pool of rewards?

        public void AddWalletReward(WalletType walletType, uint amount)
        {
            WalletRewards.Add((walletType, amount));
        }

        public void AddWalletReward(ItemId itemId, uint amount)
        {
            ItemRewards.Add((itemId, amount));
        }
    }
}
