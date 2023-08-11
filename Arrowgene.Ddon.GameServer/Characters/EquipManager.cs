#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class EquipManager
    {
        public void EquipJobItem(DdonGameServer server, GameClient client, CharacterCommon characterToEquipTo, List<CDataChangeEquipJobItem> changeEquipJobItems)
        {
            foreach (CDataChangeEquipJobItem changeEquipJobItem in changeEquipJobItems)
            {
                if(changeEquipJobItem.EquipJobItemUId.Length == 0)
                {
                    // UNEQUIP
                    // Remove from equipment
                    characterToEquipTo.Equipment.SetJobItem(null, characterToEquipTo.Job, changeEquipJobItem.EquipSlotNo);
                    server.Database.DeleteEquipJobItem(characterToEquipTo.CommonId, characterToEquipTo.Job, changeEquipJobItem.EquipSlotNo);
                }
                else
                {
                    // EQUIP
                    Item item = server.Database.SelectItem(changeEquipJobItem.EquipJobItemUId);
                    characterToEquipTo.Equipment.SetJobItem(item, characterToEquipTo.Job, changeEquipJobItem.EquipSlotNo);
                    server.Database.ReplaceEquipJobItem(item.UId, characterToEquipTo.CommonId, characterToEquipTo.Job, changeEquipJobItem.EquipSlotNo);
                }
            }

            // Send packets informing of the update
            List<CDataEquipJobItem> equippedJobItems = characterToEquipTo.Equipment.getJobItemsAsCDataEquipJobItem(characterToEquipTo.Job);
            if(characterToEquipTo is Character character)
            {
                client.Send(new S2CEquipChangeCharacterEquipJobItemRes() 
                {
                    EquipJobItemList = equippedJobItems
                });

                client.Party.SendToAll(new S2CEquipChangeCharacterEquipJobItemNtc()
                {
                    CharacterId = character.CharacterId,
                    EquipJobItemList = equippedJobItems
                });
            } 
            else if (characterToEquipTo is Pawn pawn)
            {
                client.Send(new S2CEquipChangePawnEquipJobItemRes() 
                {
                    PawnId = pawn.PawnId,
                    EquipJobItemList = equippedJobItems
                });

                client.Party.SendToAll(new S2CEquipChangePawnEquipJobItemNtc()
                {
                    CharacterId = client.Character.CharacterId,
                    PawnId = pawn.PawnId,
                    EquipJobItemList = equippedJobItems
                });
            }
            else
            {
                throw new Exception("Unknown character type");
            }
        }

        public void HandleChangeEquipList(DdonGameServer server, GameClient client, CharacterCommon characterToEquipTo, List<CDataCharacterEquipInfo> changeCharacterEquipList, ushort updateType, StorageType storageType, Action sendResponse)
        {
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new()
            {
                UpdateType = updateType
            };

            foreach (CDataCharacterEquipInfo changeCharacterEquipInfo in changeCharacterEquipList)
            {
                string itemUId = changeCharacterEquipInfo.EquipItemUId;
                EquipType equipType = (EquipType) changeCharacterEquipInfo.EquipType;
                byte equipSlot = changeCharacterEquipInfo.EquipCategory;

                uint characterId, pawnId;
                if(characterToEquipTo is Character character1)
                {
                    characterId = character1.CharacterId;
                    pawnId = 0;
                }
                else if(characterToEquipTo is Pawn pawn)
                {
                    characterId = pawn.CharacterId;
                    pawnId = pawn.PawnId; 
                }
                else
                {
                    throw new Exception("Unknown character type");
                }

                if(itemUId.Length == 0)
                {
                    this.UnequipItem(server, client, characterToEquipTo, updateCharacterItemNtc, equipType, equipSlot, storageType, characterId, pawnId);
                }
                else
                {
                    this.EquipItem(server, client, characterToEquipTo, updateCharacterItemNtc, equipType, equipSlot, storageType, itemUId, characterId, pawnId);
                }
            }

            sendResponse.Invoke();

            // Notify other players
            if(characterToEquipTo is Character character)
            {
                S2CEquipChangeCharacterEquipNtc changeCharacterEquipNtc = new S2CEquipChangeCharacterEquipNtc()
                {
                    CharacterId = character.CharacterId,
                    EquipItemList = characterToEquipTo.Equipment.getEquipmentAsCDataEquipItemInfo(characterToEquipTo.Job, EquipType.Performance),
                    VisualEquipItemList = characterToEquipTo.Equipment.getEquipmentAsCDataEquipItemInfo(characterToEquipTo.Job, EquipType.Visual)
                    // TODO: Unk0
                };

                foreach (Client otherClient in server.ClientLookup.GetAll())
                {
                    otherClient.Send(changeCharacterEquipNtc);
                }
            } 
            else if(characterToEquipTo is Pawn pawn)
            {
                S2CEquipChangePawnEquipNtc changePawnEquipNtc = new S2CEquipChangePawnEquipNtc()
                {
                    CharacterId = pawn.CharacterId,
                    PawnId = pawn.PawnId,
                    EquipItemList = characterToEquipTo.Equipment.getEquipmentAsCDataEquipItemInfo(characterToEquipTo.Job, EquipType.Performance),
                    VisualEquipItemList = characterToEquipTo.Equipment.getEquipmentAsCDataEquipItemInfo(characterToEquipTo.Job, EquipType.Visual)
                    // TODO: Unk0
                };

                foreach (Client otherClient in server.ClientLookup.GetAll())
                {
                    otherClient.Send(changePawnEquipNtc);
                }
            }
            
            client.Send(updateCharacterItemNtc);
        }

        private void UnequipItem(DdonGameServer server, GameClient client, CharacterCommon characterToEquipTo, S2CItemUpdateCharacterItemNtc updateCharacterItemNtc, EquipType equipType, byte equipSlot, StorageType storageType, uint characterId, uint pawnId)
        {
            // Find in equipment the item to unequip
            Item item = characterToEquipTo.Equipment.GetEquipItem(characterToEquipTo.Job, equipType, equipSlot) ?? throw new Exception("No item found in this slot");

            characterToEquipTo.Equipment.SetEquipItem(null, characterToEquipTo.Job, equipType, equipSlot);
            server.Database.DeleteEquipItem(characterToEquipTo.CommonId, characterToEquipTo.Job, equipType, equipSlot, item.UId);
            
            ushort dstSlotNo = client.Character.Storage.addStorageItem(item, 1, storageType);
            server.Database.InsertStorageItem(client.Character.CharacterId, storageType, dstSlotNo, item.UId, 1);

            updateCharacterItemNtc.UpdateItemList.Add(new CDataItemUpdateResult() {
                UpdateItemNum = 0,
                ItemList = new CDataItemList() {
                    ItemUId = item.UId,
                    ItemId = item.ItemId,
                    ItemNum = 0,
                    Unk3 = item.Unk3,
                    StorageType = StorageType.Unk14,
                    SlotNo = 1,
                    Color = item.Color,
                    PlusValue = item.PlusValue,
                    Bind = true,
                    EquipPoint = 0,
                    EquipCharacterID = characterId,
                    EquipPawnID = pawnId,
                    WeaponCrestDataList = item.WeaponCrestDataList,
                    ArmorCrestDataList = item.ArmorCrestDataList,
                    EquipElementParamList = item.EquipElementParamList
                }
            });
            updateCharacterItemNtc.UpdateItemList.Add(new CDataItemUpdateResult() {
                UpdateItemNum = 1,
                ItemList = new CDataItemList() {
                    ItemUId = item.UId,
                    ItemId = item.ItemId,
                    ItemNum = 1,
                    Unk3 = item.Unk3,
                    StorageType = storageType,
                    SlotNo = dstSlotNo,
                    Color = item.Color,
                    PlusValue = item.PlusValue,
                    Bind = true,
                    EquipPoint = 0,
                    EquipCharacterID = 0,
                    EquipPawnID = 0,
                    WeaponCrestDataList = item.WeaponCrestDataList,
                    ArmorCrestDataList = item.ArmorCrestDataList,
                    EquipElementParamList = item.EquipElementParamList
                }
            });
        }

        private void EquipItem(DdonGameServer server, GameClient client, CharacterCommon characterToEquipTo, S2CItemUpdateCharacterItemNtc updateCharacterItemNtc, EquipType equipType, byte equipSlot, StorageType srcStorageType, string itemUId, uint characterId, uint pawnId)
        {
            // Find in the bag the item to equip
            var tuple = client.Character.Storage.getStorage(srcStorageType).Items
                .Select((item, index) => new { item, slot = (ushort) (index+1)})
                .Where(tuple => tuple.item?.Item1.UId == itemUId)
                .First();
            Item item = tuple.item!.Item1;
            uint itemNum = tuple.item.Item2;
            ushort srcSlotNo = tuple.slot;

            Item? itemInSlot = characterToEquipTo.Equipment.GetEquipItem(characterToEquipTo.Job, (EquipType) equipType, equipSlot);
            if(itemInSlot != null)
            {
                // When equipping over an already equipped slot, unequip item first
                this.UnequipItem(server, client, characterToEquipTo, updateCharacterItemNtc, equipType, equipSlot, srcStorageType, characterId, pawnId);
            }

            characterToEquipTo.Equipment.SetEquipItem(item, characterToEquipTo.Job, (EquipType) equipType, equipSlot);
            server.Database.InsertEquipItem(characterToEquipTo.CommonId, characterToEquipTo.Job, equipType, equipSlot, item.UId);

            // Find slot from which the item will be taken
            client.Character.Storage.setStorageItem(null, 0, srcStorageType, tuple.slot);
            server.Database.DeleteStorageItem(client.Character.CharacterId, srcStorageType, tuple.slot);

            updateCharacterItemNtc.UpdateItemList.Add(new CDataItemUpdateResult() {
                UpdateItemNum = 0, // TODO: ?
                ItemList = new CDataItemList() {
                    ItemUId = item.UId,
                    ItemId = item.ItemId,
                    ItemNum = 0,
                    Unk3 = item.Unk3,
                    StorageType = srcStorageType,
                    SlotNo = srcSlotNo,
                    Color = item.Color,
                    PlusValue = item.PlusValue,
                    Bind = true,
                    EquipPoint = 0,
                    EquipCharacterID = characterId,
                    EquipPawnID = pawnId,
                    WeaponCrestDataList = item.WeaponCrestDataList,
                    ArmorCrestDataList = item.ArmorCrestDataList,
                    EquipElementParamList = item.EquipElementParamList
                }
            });
            updateCharacterItemNtc.UpdateItemList.Add(new CDataItemUpdateResult() {
                UpdateItemNum = 1,
                ItemList = new CDataItemList() {
                    ItemUId = item.UId,
                    ItemId = item.ItemId,
                    ItemNum = 1,
                    Unk3 = item.Unk3,
                    StorageType = StorageType.Unk14,
                    SlotNo = 1,
                    Color = item.Color,
                    PlusValue = item.PlusValue,
                    Bind = true,
                    EquipPoint = 0,
                    EquipCharacterID = characterId,
                    EquipPawnID = pawnId,
                    WeaponCrestDataList = item.WeaponCrestDataList,
                    ArmorCrestDataList = item.ArmorCrestDataList,
                    EquipElementParamList = item.EquipElementParamList
                }
            });
        }
    }
}