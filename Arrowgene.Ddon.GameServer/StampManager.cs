using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer
{
    public class StampManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StampManager));

        public StampManager(DdonGameServer server)
        {
            Server = server;
        }

        public static readonly int MAX_DAILY_STAMP = 8;
        private static readonly int TOTAL_STAMP_SLOT = 10;

        /// <summary>
        /// Grace period *after* their their regular daily reset.
        /// </summary>
        private static readonly TimeSpan DAILY_STAMP_GRACE_PERIOD = new TimeSpan(1, 0, 0, 0);

        /// <summary>
        /// JST hour of the reset.
        /// </summary>
        private static readonly int STAMP_RESET_HOUR = 5;

        private DdonGameServer Server;

        public List<CDataStampBonusAsset> GetDailyStampAssets()
        {
            return Server.AssetRepository.StampBonusAsset.Where(x => 1 <= x.StampNum && x.StampNum <= 8).OrderBy(x => x.StampNum).ToList();
        }

        public List<CDataStampBonusAsset> GetTotalStampAssets()
        {
            return Server.AssetRepository.StampBonusAsset.Where(x => x.StampNum > MAX_DAILY_STAMP).OrderBy(x => x.StampNum).ToList(); ;
        }

        public List<CDataStampBonusAsset> GetTotalStampAssetsWindow(ushort totalStamps)
        {
            var totalList = GetTotalStampAssets();

            // Return only the ten stamps surrounding the current position.
            if (totalList.Count > TOTAL_STAMP_SLOT)
            {
                // Find position of the last stamp checkpoint we've passed.
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
            S2CItemUpdateCharacterItemNtc ntc = new();
            PacketQueue queue = new();

            foreach (var entry in stamps)
            {
                foreach (var bonus in entry.StampBonus)
                {
                    // Currency
                    // Only the first five (GP, RP, BO, Silver Tickets, GP) seem to be valid items for display.
                    if (bonus.BonusType <= 5)
                    {
                        ntc.UpdateWalletList.Add(Server.WalletManager.AddToWallet(client.Character, (WalletType)bonus.BonusType, bonus.BonusValue));
                    }
                    else 
                    {
                        var (bonusQueue, isSpecial) = Server.ItemManager.HandleSpecialItem(client, ntc, (ItemId)bonus.BonusType, bonus.BonusValue);
                        if (isSpecial)
                        {
                            queue.AddRange(bonusQueue);
                        }
                        else
                        {
                            ntc.UpdateItemList.AddRange(Server.ItemManager.AddItem(Server, client.Character, StorageType.ItemPost, bonus.BonusType, bonus.BonusValue));
                        }
                    }
                }
            }

            if (ntc.UpdateWalletList.Any() || ntc.UpdateItemList.Any())
            {
                client.Send(ntc);
            }
            queue.Send();
        }

        static public bool CanResetConsecutiveStamp(CharacterStampBonus stampData)
        {
            return RelativeSpanToReset(stampData.LastStamp) > DAILY_STAMP_GRACE_PERIOD;
        }

        static public bool CanStamp(DateTime lastStamp)
        {
            return RelativeSpanToReset(lastStamp).TotalSeconds > 0;
        }

        public void UpdateStamp(Character character)
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo jstZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            DateTime jstNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, jstZone);

            character.StampBonus.LastStamp = jstNow;
            character.StampBonus.ConsecutiveStamp += 1;
            character.StampBonus.TotalStamp += 1;

            Server.Database.UpdateCharacterStampData(character.CharacterId, character.StampBonus);
        }

        /// <summary>
        /// Calculate the timespan from/until the reset.
        /// If negative, their stamp reset hasn't happened yet.
        /// If positive, their stamp reset has happened.
        /// </summary>
        static private TimeSpan RelativeSpanToReset(DateTime lastStamp)
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo jstZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            DateTime jstNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, jstZone);

            DateTime lastStampReset; // Always in JST
            if (lastStamp.Hour < STAMP_RESET_HOUR)
            {
                lastStampReset = lastStamp.Date.AddHours(STAMP_RESET_HOUR);
            }
            else
            {
                lastStampReset = lastStamp.Date.AddDays(1).AddHours(STAMP_RESET_HOUR);
            }

            return jstNow - lastStampReset;
        }
    }
}
