using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

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
            ulong bazaarId = 0;
            S2CItemUpdateCharacterItemNtc itemUpdateNtc = new S2CItemUpdateCharacterItemNtc();

            Server.Database.ExecuteInTransaction(connection =>
            {
                CDataItemUpdateResult itemUpdateResult = Server.ItemManager.ConsumeItemByUId(Server, client.Character, storageType, itemUID, num, connection);

                itemUpdateNtc.UpdateItemList.Add(itemUpdateResult);

                DateTimeOffset now = DateTimeOffset.UtcNow;

                BazaarExhibition exhibition = new BazaarExhibition();
                exhibition.CharacterId = client.Character.CharacterId;
                exhibition.Info.ItemInfo.Sequence = 0; // TODO: Figure out
                exhibition.Info.ItemInfo.ItemBaseInfo.ItemId = itemUpdateResult.ItemList.ItemId;
                exhibition.Info.ItemInfo.ItemBaseInfo.Num = num;
                exhibition.Info.ItemInfo.ItemBaseInfo.Price = price;
                exhibition.Info.ItemInfo.ExhibitionTime = now;
                exhibition.Info.State = BazaarExhibitionState.OnSale;
                exhibition.Info.Proceeds = CalculateProceeds(exhibition.Info.ItemInfo.ItemBaseInfo);
                exhibition.Info.Expire = now.AddSeconds(Server.GameSettings.GameServerSettings.BazaarExhibitionTimeSeconds);

                bazaarId = Server.Database.InsertBazaarExhibition(exhibition, connection);
            });

            client.Send(itemUpdateNtc);
            return bazaarId;
        }

        public ulong ReExhibit(ulong bazaarId, uint newPrice)
        {
            // TODO: Fetch from DB
            BazaarExhibition exhibition = new();
            Server.Database.ExecuteInTransaction(connection =>
            {
                exhibition = GetExhibitionByBazaarId(bazaarId, connection);

                if (exhibition.Info.State != BazaarExhibitionState.OnSale)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_BAZAAR_STATE_CHANGED);
                }

                DateTimeOffset now = DateTimeOffset.UtcNow;
                exhibition.Info.ItemInfo.ItemBaseInfo.Price = newPrice;
                exhibition.Info.ItemInfo.ExhibitionTime = now;
                exhibition.Info.Proceeds = CalculateProceeds(exhibition.Info.ItemInfo.ItemBaseInfo);
                exhibition.Info.Expire = now.AddSeconds(Server.GameSettings.GameServerSettings.BazaarExhibitionTimeSeconds);
                Server.Database.UpdateBazaarExhibiton(exhibition, connection);
            });
            
            return exhibition.Info.ItemInfo.BazaarId;
        }

        public void Cancel(GameClient client, ulong bazaarId)
        {
            S2CItemUpdateCharacterItemNtc itemUpdateNtc = new S2CItemUpdateCharacterItemNtc();

            Server.Database.ExecuteInTransaction(connection =>
            {
                BazaarExhibition exhibition = GetExhibitionByBazaarId(bazaarId, connection);

                if (exhibition.Info.State != BazaarExhibitionState.OnSale)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_BAZAAR_STATE_CHANGED);
                }

                Server.Database.DeleteBazaarExhibition(exhibition.Info.ItemInfo.BazaarId, connection);

                // TODO: Verify if items are supposed to go to the storage box
                List<CDataItemUpdateResult> itemUpdateResults = Server.ItemManager.AddItem(
                    Server, 
                    client.Character, 
                    false, 
                    exhibition.Info.ItemInfo.ItemBaseInfo.ItemId, 
                    exhibition.Info.ItemInfo.ItemBaseInfo.Num, 
                    connectionIn:connection
                );

                itemUpdateNtc.UpdateItemList.AddRange(itemUpdateResults);
            });
            
            client.Send(itemUpdateNtc);
        }

        public void Proceeds(GameClient client, ulong bazaarId, List<CDataItemStorageIndicateNum> itemStorageIndicateNumList)
        {
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new()
            {
                UpdateType = ItemNoticeType.BazaarProceeds
            };

            Server.Database.ExecuteInTransaction(connection =>
            {
                BazaarExhibition exhibition = Server.BazaarManager.GetExhibitionByBazaarId(bazaarId, connection);

                uint totalItemAmount = (uint)itemStorageIndicateNumList.Sum(x => (int)x.ItemNum);
                if (exhibition.Info.ItemInfo.ItemBaseInfo.Num != totalItemAmount)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_BAZAAR_INTERNAL_ERROR);
                }

                uint totalPrice = exhibition.Info.ItemInfo.ItemBaseInfo.Price * exhibition.Info.ItemInfo.ItemBaseInfo.Num;


                // UPDATE INVENTORY
                foreach (CDataItemStorageIndicateNum itemStorageIndicateNum in itemStorageIndicateNumList)
                {
                    var sendToItemBag = itemStorageIndicateNum.StorageType switch
                    {
                        19 => true,
                        20 => false,
                        _ => throw new ResponseErrorException(ErrorCode.ERROR_CODE_BAZAAR_INTERNAL_ERROR, "Unexpected destination when buying goods: " + itemStorageIndicateNum.StorageType),
                    };
                    List<CDataItemUpdateResult> itemUpdateResult = Server.ItemManager.AddItem(Server, client.Character, sendToItemBag, exhibition.Info.ItemInfo.ItemBaseInfo.ItemId, itemStorageIndicateNum.ItemNum, connectionIn:connection);
                    updateCharacterItemNtc.UpdateItemList.AddRange(itemUpdateResult);
                }

                CDataUpdateWalletPoint updateWalletPoint = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, totalPrice, connection)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_BAZAAR_INTERNAL_ERROR, "Insufficient funds.");
                updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);

                Server.BazaarManager.SetExhibitionState(exhibition.Info.ItemInfo.BazaarId, BazaarExhibitionState.Sold, connection);

                // Notify seller
                Server.RpcManager.AnnounceCharacterPacket(new S2CBazaarProceedsNtc()
                {
                    BazaarId = exhibition.Info.ItemInfo.BazaarId,
                    ItemId = exhibition.Info.ItemInfo.ItemBaseInfo.ItemId,
                    Proceeds = exhibition.Info.Proceeds
                }, exhibition.CharacterId);
            });

            client.Send(updateCharacterItemNtc);
        }

        public uint ReceiveProceeds(GameClient client)
        {
            uint totalProceeds = 0;
            S2CItemUpdateCharacterItemNtc notice = new()
            {
                UpdateType = ItemNoticeType.BazaarProceeds
            };

            DateTimeOffset newExpire = DateTimeOffset.UtcNow;
            try
            {
                newExpire.AddSeconds(Server.GameSettings.GameServerSettings.BazaarCooldownTimeSeconds - Server.GpCourseManager.BazaarReExhibitShorten());
            }
            catch (OverflowException) { }   

            Server.Database.ExecuteInTransaction(connection =>
            {
                List<BazaarExhibition> exhibitionsToReceive = GetSoldExhibitionsByCharacter(client.Character, connection);

                totalProceeds = (uint)exhibitionsToReceive.Sum(exhibition => exhibition.Info.Proceeds);
                notice.UpdateWalletList.Add(Server.WalletManager.AddToWallet(client.Character, WalletType.Gold, totalProceeds, connectionIn: connection));

                foreach (BazaarExhibition exhibition in exhibitionsToReceive)
                {
                    exhibition.Info.State = BazaarExhibitionState.Idle;
                    exhibition.Info.Expire = newExpire;
                    Server.Database.UpdateBazaarExhibiton(exhibition, connection);
                }
            });

            client.Send(notice);
            return totalProceeds;
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

        public BazaarExhibition GetExhibitionByBazaarId(ulong bazaarId, DbConnection? connectionIn = null)
        {
            return Server.Database.SelectBazaarExhibitionByBazaarId(bazaarId, connectionIn);
        }

        public List<BazaarExhibition> GetExhibitionsByCharacter(Character character, DbConnection? connectionIn = null)
        {
            return Server.Database.FetchCharacterBazaarExhibitions(character.CharacterId, connectionIn);
        }

        public List<BazaarExhibition> GetActiveExhibitionsForItemId(uint itemId, Character filterOutCharacter, DbConnection? connectionIn = null)
        {
            return Server.Database.SelectActiveBazaarExhibitionsByItemIdExcludingOwn(itemId, filterOutCharacter.CharacterId, connectionIn);
        }

        public List<BazaarExhibition> GetActiveExhibitionsForItemIds(List<uint> itemIds, Character filterOutCharacter, DbConnection? connectionIn = null)
        {
            return Server.Database.SelectActiveBazaarExhibitionsByItemIdsExcludingOwn(itemIds, filterOutCharacter.CharacterId, connectionIn);
        }

        private void SetExhibitionState(ulong bazaarId, BazaarExhibitionState state, DbConnection? connectionIn = null)
        {
            BazaarExhibition exhibition = GetExhibitionByBazaarId(bazaarId, connectionIn);
            exhibition.Info.State = state;
            Server.Database.UpdateBazaarExhibiton(exhibition);
        }

        private List<BazaarExhibition> GetSoldExhibitionsByCharacter(Character character, DbConnection? connectionIn = null)
        {
            return [.. GetExhibitionsByCharacter(character, connectionIn).Where(exhibition => exhibition.Info.State == BazaarExhibitionState.Sold)];
        }

        private uint CalculateProceeds(CDataBazaarItemBaseInfo itemBaseInfo)
        {
            uint totalPrice = itemBaseInfo.Num*itemBaseInfo.Price;
            uint taxDeduction = (uint)(totalPrice * TAXES);

            //Minimum proceeds are 1 because the client UI won't let the player receive them if the total proceeds are less than 1.
            return Math.Clamp(totalPrice - taxDeduction, 1, uint.MaxValue); 
        }
    }
}
