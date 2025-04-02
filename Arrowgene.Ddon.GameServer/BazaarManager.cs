using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer
{
    public class BazaarManager
    {
        private static readonly double TAXES = 0.05; // 5%, value taken from the ingame menu

        public BazaarManager(DdonGameServer server)
        {
            Server = server;
        }

        private DdonGameServer Server;

        public ulong Exhibit(GameClient client, StorageType storageType, string itemUID, ushort num, uint price, byte _flag)
        {
            // TODO: Figure out what _flag is for

            CDataItemUpdateResult itemUpdateResult = Server.ItemManager.ConsumeItemByUId(Server, client.Character, storageType, itemUID, num);

            S2CItemUpdateCharacterItemNtc itemUpdateNtc = new S2CItemUpdateCharacterItemNtc();
            itemUpdateNtc.UpdateItemList.Add(itemUpdateResult);
            client.Send(itemUpdateNtc);

            DateTimeOffset now = DateTimeOffset.UtcNow;

            BazaarExhibition exhibition = new BazaarExhibition();
            exhibition.CharacterId = client.Character.CharacterId;
            exhibition.Info.ItemInfo.Sequence = 0; // TODO: Figure out
            exhibition.Info.ItemInfo.ItemBaseInfo.ItemId = itemUpdateResult.ItemList.ItemId;
            exhibition.Info.ItemInfo.ItemBaseInfo.Num = num;
            exhibition.Info.ItemInfo.ItemBaseInfo.Price = price;
            exhibition.Info.ItemInfo.ExhibitionTime = now;
            exhibition.Info.State = BazaarExhibitionState.OnSale;
            exhibition.Info.Proceeds = calculateProceeds(exhibition.Info.ItemInfo.ItemBaseInfo);
            exhibition.Info.Expire = now.AddSeconds(Server.GameSettings.GameServerSettings.BazaarExhibitionTimeSeconds);

            ulong bazaarId = Server.Database.InsertBazaarExhibition(exhibition);
            return bazaarId;
        }

        public ulong ReExhibit(ulong bazaarId, uint newPrice)
        {
            // TODO: Fetch from DB
            BazaarExhibition exhibition = GetExhibitionByBazaarId(bazaarId);

            if(exhibition.Info.State != BazaarExhibitionState.OnSale)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_BAZAAR_STATE_CHANGED);
            }

            DateTimeOffset now = DateTimeOffset.UtcNow;
            exhibition.Info.ItemInfo.ItemBaseInfo.Price = newPrice;
            exhibition.Info.ItemInfo.ExhibitionTime = now;
            exhibition.Info.Proceeds = calculateProceeds(exhibition.Info.ItemInfo.ItemBaseInfo);
            exhibition.Info.Expire = now.AddSeconds(Server.GameSettings.GameServerSettings.BazaarExhibitionTimeSeconds);
            Server.Database.UpdateBazaarExhibiton(exhibition);

            return exhibition.Info.ItemInfo.BazaarId;
        }

        public void Cancel(GameClient client, ulong bazaarId)
        {
            BazaarExhibition exhibition = GetExhibitionByBazaarId(bazaarId);

            if(exhibition.Info.State != BazaarExhibitionState.OnSale)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_BAZAAR_STATE_CHANGED);
            }

            Server.Database.DeleteBazaarExhibition(exhibition.Info.ItemInfo.BazaarId);

            // TODO: Verify if items are supposed to go to the storage box
            List<CDataItemUpdateResult> itemUpdateResults = Server.ItemManager.AddItem(Server, client.Character, false, exhibition.Info.ItemInfo.ItemBaseInfo.ItemId, exhibition.Info.ItemInfo.ItemBaseInfo.Num);

            S2CItemUpdateCharacterItemNtc itemUpdateNtc = new S2CItemUpdateCharacterItemNtc();
            itemUpdateNtc.UpdateItemList.AddRange(itemUpdateResults);
            client.Send(itemUpdateNtc);
        }

        public void Proceeds(GameClient client, ulong bazaarId, List<CDataItemStorageIndicateNum> itemStorageIndicateNumList)
        {
            BazaarExhibition exhibition = Server.BazaarManager.GetExhibitionByBazaarId(bazaarId);
            
            uint totalItemAmount = (uint) itemStorageIndicateNumList.Select(x => (int) x.ItemNum).Sum();
            if(exhibition.Info.ItemInfo.ItemBaseInfo.Num != totalItemAmount)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_BAZAAR_INTERNAL_ERROR);
            }

            uint totalPrice = exhibition.Info.ItemInfo.ItemBaseInfo.Price * exhibition.Info.ItemInfo.ItemBaseInfo.Num;

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = 0;

            // UPDATE INVENTORY
            foreach (CDataItemStorageIndicateNum itemStorageIndicateNum in itemStorageIndicateNumList)
            {
                bool sendToItemBag;
                switch(itemStorageIndicateNum.StorageType) {
                    case 19:
                        sendToItemBag = true;
                        break;
                    case 20:
                        sendToItemBag = false;
                        break;
                    default:
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_BAZAAR_INTERNAL_ERROR, "Unexpected destination when buying goods: "+itemStorageIndicateNum.StorageType);
                }
                List<CDataItemUpdateResult> itemUpdateResult = Server.ItemManager.AddItem(Server, client.Character, sendToItemBag, exhibition.Info.ItemInfo.ItemBaseInfo.ItemId, itemStorageIndicateNum.ItemNum);
                updateCharacterItemNtc.UpdateItemList.AddRange(itemUpdateResult);
            }

            CDataUpdateWalletPoint updateWalletPoint = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, totalPrice);
            if (updateWalletPoint is null)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_BAZAAR_INTERNAL_ERROR, "Insufficient funds.");
            }

            updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);

            Server.BazaarManager.SetExhibitionState(exhibition.Info.ItemInfo.BazaarId, BazaarExhibitionState.Sold);

            // Notify seller
            GameClient seller = Server.ClientLookup.GetClientByCharacterId(exhibition.CharacterId);
            if(seller != null)
            {
                seller.Send(new S2CBazaarProceedsNtc()
                {
                    BazaarId = exhibition.Info.ItemInfo.BazaarId,
                    ItemId = exhibition.Info.ItemInfo.ItemBaseInfo.ItemId,
                    Proceeds = exhibition.Info.Proceeds
                });
            }
            
            // Send packets
            client.Send(updateCharacterItemNtc);
        }

        public uint ReceiveProceeds(GameClient client)
        {
            List<BazaarExhibition> exhibitionsToReceive = GetSoldExhibitionsByCharacter(client.Character);

            uint totalProceeds = (uint) exhibitionsToReceive.Sum(exhibition => exhibition.Info.Proceeds);
            client.Send(Server.WalletManager.AddToWalletNtc(client, client.Character, WalletType.Gold, totalProceeds));

            DateTimeOffset now = DateTimeOffset.UtcNow;
            foreach (BazaarExhibition exhibition in exhibitionsToReceive)
            {
                exhibition.Info.State = BazaarExhibitionState.Idle;
                ulong totalCooldown;
                try
                {
                    totalCooldown = Server.GameSettings.GameServerSettings.BazaarCooldownTimeSeconds - Server.GpCourseManager.BazaarReExhibitShorten();
                }
                catch (OverflowException _)
                {
                    totalCooldown = 0;
                }
                exhibition.Info.Expire = now.AddSeconds(totalCooldown);
                Server.Database.UpdateBazaarExhibiton(exhibition);
            }

            return (uint) totalProceeds;
        }

        public void NotifySoldExhibitions(GameClient client)
        {
            List<BazaarExhibition> soldExhibitions = GetSoldExhibitionsByCharacter(client.Character);
            foreach (BazaarExhibition soldExhibition in soldExhibitions)
            {    
                client.Send(new S2CBazaarProceedsNtc()
                {
                    BazaarId = soldExhibition.Info.ItemInfo.BazaarId,
                    ItemId = soldExhibition.Info.ItemInfo.ItemBaseInfo.ItemId,
                    Proceeds = soldExhibition.Info.Proceeds
                });
            }
        }

        public BazaarExhibition GetExhibitionByBazaarId(ulong bazaarId)
        {
            return Server.Database.SelectBazaarExhibitionByBazaarId(bazaarId);
        }

        public List<BazaarExhibition> GetExhibitionsByCharacter(Character character)
        {
            return Server.Database.FetchCharacterBazaarExhibitions(character.CharacterId);
        }

        public List<BazaarExhibition> GetActiveExhibitionsForItemId(uint itemId, Character filterOutCharacter, DbConnection? connectionIn = null)
        {
            return Server.Database.SelectActiveBazaarExhibitionsByItemIdExcludingOwn(itemId, filterOutCharacter.CharacterId, connectionIn);
        }

        public List<BazaarExhibition> GetActiveExhibitionsForItemIds(List<uint> itemIds, Character filterOutCharacter)
        {
            return Server.Database.SelectActiveBazaarExhibitionsByItemIdsExcludingOwn(itemIds, filterOutCharacter.CharacterId);
        }

        private void SetExhibitionState(ulong bazaarId, BazaarExhibitionState state)
        {
            BazaarExhibition exhibition = GetExhibitionByBazaarId(bazaarId);
            exhibition.Info.State = state;
            Server.Database.UpdateBazaarExhibiton(exhibition);
        }

        private List<BazaarExhibition> GetSoldExhibitionsByCharacter(Character character)
        {
            return GetExhibitionsByCharacter(character)
                .Where(exhibition => exhibition.Info.State == BazaarExhibitionState.Sold)
                .ToList();
        }

        private uint calculateProceeds(CDataBazaarItemBaseInfo itemBaseInfo)
        {
            uint totalPrice = itemBaseInfo.Num*itemBaseInfo.Price;
            uint taxDeduction = (uint)(totalPrice * TAXES);

            //Minimum proceeds are 1 because the client UI won't let the player receive them if the total proceeds are less than 1.
            return Math.Clamp(totalPrice - taxDeduction, 1, uint.MaxValue); 
        }
    }
}
