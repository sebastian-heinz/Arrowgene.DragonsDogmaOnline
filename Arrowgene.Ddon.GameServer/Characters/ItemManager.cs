#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class ItemManager
    {
        public CDataItemUpdateResult AddItem(DdonServer<GameClient> server, Character character, bool itemBag, uint itemId, uint num)
        {
            if(itemBag)
            {
                ClientItemInfo clientItemInfo = ClientItemInfo.GetInfoForItemId(server.AssetRepository.ClientItemInfos, itemId);
                return AddItem(server.Database, character, clientItemInfo.StorageType, itemId, num, clientItemInfo.StackLimit);
            }
            else
            {
                // TODO: Does the Storage Box have an item cap?
                return AddItem(server.Database, character, StorageType.StorageBoxNormal, itemId, num);
            }

        }

        public CDataItemUpdateResult AddItem(IDatabase database, Character character, StorageType destinationStorageType, uint itemId, uint num, uint stackLimit = UInt32.MaxValue)
        {
            var tuple = character.Storage.getStorage(destinationStorageType).Items
                .Select((item, index) => new {item = item, slot = (ushort) (index+1)})
                .Where(tuple => tuple.item?.Item1.ItemId == itemId)
                .FirstOrDefault();

            Item? item = tuple?.item?.Item1;
            ushort slot = tuple?.slot ?? 0;
            uint oldItemNum = tuple?.item?.Item2 ?? 0;
            uint newItemNum = Math.Min(stackLimit, oldItemNum + num);
            uint addedItems = newItemNum - oldItemNum;
            
            if (item == null)
            {
                item = new Item() {
                    ItemId = itemId,
                    Unk3 = 0,
                    Color = 0,
                    PlusValue = 0,
                    WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                    ArmorCrestDataList = new List<CDataArmorCrestData>(),
                    EquipElementParamList = new List<CDataEquipElementParam>()
                };
                database.InsertItem(item);
                slot = character.Storage.addStorageItem(item, newItemNum, destinationStorageType);
            } else
            {
                character.Storage.setStorageItem(item, newItemNum, destinationStorageType, slot);
            }

            database.ReplaceStorageItem(character.CharacterId, destinationStorageType, slot, item.UId, newItemNum);

            CDataItemUpdateResult result = new CDataItemUpdateResult();
            result.ItemList.ItemUId = item.UId;
            result.ItemList.ItemId = item.ItemId;
            result.ItemList.ItemNum = newItemNum;
            result.ItemList.Unk3 = item.Unk3;
            result.ItemList.StorageType = destinationStorageType;
            result.ItemList.SlotNo = slot;
            result.ItemList.Color = item.Color; // ?
            result.ItemList.PlusValue = item.PlusValue; // ?
            result.ItemList.Bind = false;
            result.ItemList.EquipPoint = 0;
            result.ItemList.EquipCharacterID = 0;
            result.ItemList.EquipPawnID = 0;
            result.ItemList.WeaponCrestDataList = item.WeaponCrestDataList;
            result.ItemList.ArmorCrestDataList = item.ArmorCrestDataList;
            result.ItemList.EquipElementParamList = item.EquipElementParamList;
            result.UpdateItemNum = (int) addedItems;
            return result;
        }
    }
}