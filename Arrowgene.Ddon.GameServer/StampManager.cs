using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer
{
    public class StampManager
    {
        public StampManager(DdonGameServer server)
        {
            Server = server;
        }

        public static int MAX_DAILY_STAMP = 8;
        public static int TOTAL_STAMP_SLOT = 10;
        public static int DAILY_STAMP_GRACE_PERIOD = 1; //Login every X days or lose your consecutive stamp.
        public static int STAMP_RESET_HOUR = 20; //5AM JST = 8PM UTC = 1 PM PST = 4 PM EST

        private DdonGameServer Server;

        public List<CDataStampBonusAsset> GetDailyStampAssets()
        {
            return Server.AssetRepository.StampBonusAsset.Where(x => 1 <= x.StampNum && x.StampNum <= 8).OrderBy(x => x.StampNum).ToList();
        }

        public List<CDataStampBonusAsset> GetTotalStampAssets()
        {
            return Server.AssetRepository.StampBonusAsset.Where(x => x.StampNum > 8).OrderBy(x => x.StampNum).ToList(); ;
        }

        public List<CDataStampBonusAsset> GetTotalStampAssetsWindow(ushort totalStamps)
        {
            var totalList = GetTotalStampAssets();

            //Return only the ten stamps surrounding the current position.
            if (totalList.Count > TOTAL_STAMP_SLOT)
            {
                //Find position of the last stamp checkpoint we've passed.
                int position = totalList.FindLastIndex(x => x.StampNum <= totalStamps);

                if (position == -1) return totalList.Take(TOTAL_STAMP_SLOT).ToList();

                int left = Math.Clamp(position - 6, 0, totalList.Count - TOTAL_STAMP_SLOT);
                return totalList.Skip(left).Take(TOTAL_STAMP_SLOT).ToList();
            }
            else return totalList;
        }

        public bool CanDailyStamp(CharacterStampBonus stampData)
        {
            return CanStamp(stampData.LastStamp);
        }

        public bool CanTotalStamp(CharacterStampBonus stampData)
        { 
            return CanDailyStamp(stampData) && GetTotalStampAssets().Where(x => x.StampNum == (stampData.TotalStamp + 1)).Any();
        }

        public void HandleStampBonuses(GameClient client, IEnumerable<CDataStampBonusAsset> stamps)
        {
            List<CDataItemUpdateResult> totalItems = new List<CDataItemUpdateResult>();
            List<CDataUpdateWalletPoint> totalWallet = new List<CDataUpdateWalletPoint>();
            foreach (var entry in stamps)
            {
                foreach (var bonus in entry.StampBonus)
                {
                    //Currency
                    //Only the first five (GP, RP, BO, Silver Tickets, GP) seem to be valid items for display.
                    if (bonus.BonusType <= 5)
                    {
                        totalWallet.Add(Server.WalletManager.AddToWallet(client.Character, (WalletType)bonus.BonusType, bonus.BonusValue));
                    }
                    else
                    {
                        totalItems = totalItems.Concat(Server.ItemManager.AddItem(Server, client.Character, StorageType.ItemPost, bonus.BonusType, bonus.BonusValue)).ToList();
                    }
                }
            }

            if (totalItems.Any() || totalWallet.Any())
            {
                client.Send(new S2CItemUpdateCharacterItemNtc()
                {
                    UpdateItemList = totalItems,
                    UpdateWalletList = totalWallet,
                    UpdateType = ItemNoticeType.StampBonus
                });
            }
        }

        static public bool CanResetConsecutiveStamp(CharacterStampBonus stampData)
        {
            DateTime lastReset = GetLastStampReset();
            DateTime lastStamp = stampData.LastStamp;
            return (lastReset - lastStamp).TotalDays >= (DAILY_STAMP_GRACE_PERIOD + 1);
        }

        static private DateTime GetLastStampReset()
        {
            DateTime lastReset = DateTime.Today.AddHours(STAMP_RESET_HOUR); 
            if (lastReset > DateTime.Now)
            {
                lastReset = lastReset.AddDays(-1);
            }
            return lastReset;
        }

        static public bool CanStamp(DateTime lastStamp)
        {
            return lastStamp < GetLastStampReset();
        }
    }
}
