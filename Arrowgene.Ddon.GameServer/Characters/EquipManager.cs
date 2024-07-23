#nullable enable
using System;
using System.Collections.Generic;
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
                    characterToEquipTo.EquipmentTemplate.SetJobItem(null, characterToEquipTo.Job, changeEquipJobItem.EquipSlotNo);
                    server.Database.DeleteEquipJobItem(characterToEquipTo.CommonId, characterToEquipTo.Job, changeEquipJobItem.EquipSlotNo);
                }
                else
                {
                    // EQUIP
                    Item item = server.Database.SelectItem(changeEquipJobItem.EquipJobItemUId);
                    characterToEquipTo.EquipmentTemplate.SetJobItem(item, characterToEquipTo.Job, changeEquipJobItem.EquipSlotNo);
                    server.Database.ReplaceEquipJobItem(item.UId, characterToEquipTo.CommonId, characterToEquipTo.Job, changeEquipJobItem.EquipSlotNo);
                }
            }

            // Send packets informing of the update
            List<CDataEquipJobItem> equippedJobItems = characterToEquipTo.EquipmentTemplate.JobItemsAsCDataEquipJobItem(characterToEquipTo.Job);
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

        public void HandleChangeEquipList(DdonGameServer server, GameClient client, CharacterCommon characterToEquipTo, List<CDataCharacterEquipInfo> changeCharacterEquipList, ItemNoticeType updateType, List<StorageType> storageTypes, Action sendResponse)
        {
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = updateType
            };

            foreach (CDataCharacterEquipInfo changeCharacterEquipInfo in changeCharacterEquipList)
            {
                string itemUId = changeCharacterEquipInfo.EquipItemUId;
                EquipType equipType = changeCharacterEquipInfo.EquipType;
                byte equipSlot = changeCharacterEquipInfo.EquipCategory;
                ushort equipItemStorageSlot = characterToEquipTo.Equipment.GetStorageSlot(equipType, equipSlot);

                if(itemUId.Length == 0)
                {
                    // UNEQUIP

                    // Remove from equipment template
                    characterToEquipTo.EquipmentTemplate.SetEquipItem(null, characterToEquipTo.Job, equipType, equipSlot);
                    server.Database.DeleteEquipItem(characterToEquipTo.CommonId, characterToEquipTo.Job, equipType, equipSlot);

                    // Update storage
                    // TODO: Move to the other storage types if the first one is full
                    Storage destinationStorage = client.Character.Storage.getStorage(storageTypes[0]);
                    updateCharacterItemNtc.UpdateItemList.AddRange(server.ItemManager.MoveItem(server, client.Character, characterToEquipTo.Equipment.Storage, equipItemStorageSlot, 1, destinationStorage, 0));
                }
                else
                {
                    // EQUIP

                    // Set in equipment template
                    characterToEquipTo.EquipmentTemplate.SetEquipItem(server.Database.SelectItem(itemUId), characterToEquipTo.Job, equipType, equipSlot);
                    server.Database.ReplaceEquipItem(characterToEquipTo.CommonId, characterToEquipTo.Job, equipType, equipSlot, itemUId);

                    // Update storage, swapping if needed
                    Storage sourceStorage = client.Character.Storage.getStorage(storageTypes[0]);
                    updateCharacterItemNtc.UpdateItemList.AddRange(server.ItemManager.MoveItem(server, client.Character, sourceStorage, itemUId, 1, characterToEquipTo.Equipment.Storage, equipItemStorageSlot));
                }
            }

            sendResponse.Invoke();

            client.Send(updateCharacterItemNtc);

            // Notify other players
            if (characterToEquipTo is Character character)
            {
                S2CEquipChangeCharacterEquipNtc changeCharacterEquipNtc = new S2CEquipChangeCharacterEquipNtc()
                {
                    CharacterId = character.CharacterId,
                    EquipItemList = character.Equipment.AsCDataEquipItemInfo(EquipType.Performance),
                    VisualEquipItemList = character.Equipment.AsCDataEquipItemInfo(EquipType.Visual)
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
                    EquipItemList = pawn.Equipment.AsCDataEquipItemInfo(EquipType.Performance),
                    VisualEquipItemList = pawn.Equipment.AsCDataEquipItemInfo(EquipType.Visual),
                    // TODO: Unk0
                };

                foreach (Client otherClient in server.ClientLookup.GetAll())
                {
                    otherClient.Send(changePawnEquipNtc);
                }
            }
        }
    }
}
