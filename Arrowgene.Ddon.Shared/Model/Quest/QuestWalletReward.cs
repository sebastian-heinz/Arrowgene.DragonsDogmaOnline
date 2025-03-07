using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestWalletReward
    {
        public WalletType WalletType { get; set; }
        public uint Amount { get; set; }

        public static QuestWalletReward Create(WalletType walletType, uint amount)
        {
            return new QuestWalletReward()
            {
                WalletType = walletType,
                Amount = amount,
            };
        }

        public CDataWalletPoint AsCDataWalletPoint()
        {
            return new CDataWalletPoint()
            {
                Type = WalletType,
                Value = Amount,
            };
        }
    }
}
