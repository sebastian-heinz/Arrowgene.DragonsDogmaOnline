using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer
{
    public class BazaarManager
    {
        private static readonly double TAXES = 0.05; // 5%, value taken from the ingame menu
        private static ulong LAST_BAZAAR_ID = 0;

        // TODO: Make it configurable
        private static readonly TimeSpan EXHIBITION_TIME_SPAN = TimeSpan.FromDays(7);
        private static readonly TimeSpan COOLDOWN_TIME_SPAN = TimeSpan.FromDays(1);

        public BazaarManager(DdonGameServer server)
        {
            Server = server;
            Exhibitions = new List<BazaarExhibition>();
        }

        private DdonGameServer Server;

        private List<BazaarExhibition> Exhibitions;

        public ulong Exhibit(GameClient client, StorageType storageType, string itemUID, ushort num, uint price, byte _flag)
        {
            // TODO: Figure out what _flag is for

            uint totalPrice = num*price;

            CDataItemUpdateResult itemUpdateResult = Server.ItemManager.ConsumeItemByUId(Server, client.Character, storageType, itemUID, num);

            S2CItemUpdateCharacterItemNtc itemUpdateNtc = new S2CItemUpdateCharacterItemNtc();
            itemUpdateNtc.UpdateItemList.Add(itemUpdateResult);
            client.Send(itemUpdateNtc);

            DateTimeOffset now = DateTimeOffset.UtcNow;

            BazaarExhibition exhibition = new BazaarExhibition();
            exhibition.CharacterId = client.Character.CharacterId;
            exhibition.Info.ItemInfo.BazaarId = GenerateBazaarId();
            exhibition.Info.ItemInfo.Sequence = 0; // TODO: Figure out
            exhibition.Info.ItemInfo.ItemBaseInfo.ItemId = itemUpdateResult.ItemList.ItemId;
            exhibition.Info.ItemInfo.ItemBaseInfo.Num = num;
            exhibition.Info.ItemInfo.ItemBaseInfo.Price = price;
            exhibition.Info.ItemInfo.ExhibitionTime = now;
            exhibition.Info.State = BazaarExhibitionState.OnSale;
            exhibition.Info.Proceeds = (uint)Math.Clamp(totalPrice - totalPrice*TAXES, 0, uint.MaxValue);
            exhibition.Info.Expire = now.Add(EXHIBITION_TIME_SPAN);

            // TODO: Save in DB
            Exhibitions.Add(exhibition);

            return exhibition.Info.ItemInfo.BazaarId;
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
            
            // TODO: Update DB
            exhibition.Info.ItemInfo.ItemBaseInfo.Price = newPrice;
            exhibition.Info.ItemInfo.ExhibitionTime = now;
            exhibition.Info.Expire = now.Add(EXHIBITION_TIME_SPAN);
            // TODO: Send NTC
            return exhibition.Info.ItemInfo.BazaarId;
        }

        public void Cancel(GameClient client, ulong bazaarId)
        {
            BazaarExhibition exhibition = GetExhibitionByBazaarId(bazaarId);

            if(exhibition.Info.State != BazaarExhibitionState.OnSale)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_BAZAAR_STATE_CHANGED);
            }

            Exhibitions.Remove(exhibition);

            // TODO: Verify if items are supposed to go to the storage box
            List<CDataItemUpdateResult> itemUpdateResults = Server.ItemManager.AddItem(Server, client.Character, false, exhibition.Info.ItemInfo.ItemBaseInfo.ItemId, exhibition.Info.ItemInfo.ItemBaseInfo.Num);

            S2CItemUpdateCharacterItemNtc itemUpdateNtc = new S2CItemUpdateCharacterItemNtc();
            itemUpdateNtc.UpdateItemList.AddRange(itemUpdateResults);
            client.Send(itemUpdateNtc);
        }

        public uint ReceiveProceeds(GameClient client)
        {
            // TODO: Fetch from DB
            List<BazaarExhibition> exhibitionsToReceive = Exhibitions
                .Where(exhibition => exhibition.Info.State == BazaarExhibitionState.Sold)
                .ToList();
            
            uint totalProceeds = (uint) exhibitionsToReceive.Sum(exhibition => exhibition.Info.Proceeds);
            Server.WalletManager.AddToWalletNtc(client, client.Character, WalletType.Gold, totalProceeds);

            // TODO: Save to DB
            DateTimeOffset now = DateTimeOffset.UtcNow;
            foreach (BazaarExhibition exhibition in exhibitionsToReceive)
            {
                exhibition.Info.State = BazaarExhibitionState.Idle;
                exhibition.Info.Expire = now.Add(COOLDOWN_TIME_SPAN);
            }

            return totalProceeds;
        }

        public BazaarExhibition GetExhibitionByBazaarId(ulong bazaarId)
        {
            return Exhibitions.Where(exhibition => exhibition.Info.ItemInfo.BazaarId == bazaarId).Single();
        }

        public List<BazaarExhibition> GetExhibitionsByCharacter(Character character)
        {
            // TODO: Clear on DB
            Exhibitions.RemoveAll(exhibition => exhibition.Info.State == BazaarExhibitionState.Idle && exhibition.Info.Expire < DateTimeOffset.Now);
            // TODO: Fetch from DB
            return Exhibitions.Where(exhibition => exhibition.CharacterId == character.CharacterId).ToList();
        }

        public List<BazaarExhibition> GetActiveExhibitionsForItemId(uint itemId, Character filterOutCharacter = null)
        {
            // TODO: Fetch from DB
            return Exhibitions
                .Where(
                    exhibition => exhibition.Info.ItemInfo.ItemBaseInfo.ItemId == itemId 
                    && exhibition.Info.State == BazaarExhibitionState.OnSale
                    && exhibition.Info.Expire.CompareTo(DateTimeOffset.Now) > 0
                    && (filterOutCharacter == null || filterOutCharacter.CharacterId != exhibition.CharacterId)
                )
                .ToList();
        }

        public void SetExhibitionState(ulong bazaarId, BazaarExhibitionState state)
        {
            // TODO: Update DB
            GetExhibitionByBazaarId(bazaarId).Info.State = state;
        }

        private ulong GenerateBazaarId()
        {
            return LAST_BAZAAR_ID++;
        }
    }
}