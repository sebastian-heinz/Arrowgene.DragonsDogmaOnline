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

        public void HandleChangeEquipList(DdonGameServer server, GameClient client, CharacterCommon characterToEquipTo, List<CDataCharacterEquipInfo> changeCharacterEquipList, ItemNoticeType updateType, List<StorageType> storageTypes, Action sendResponse)
        {
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = updateType
            };

            foreach (CDataCharacterEquipInfo changeCharacterEquipInfo in changeCharacterEquipList)
            {
                string itemUId = changeCharacterEquipInfo.EquipItemUId;
                EquipType equipType = (EquipType) changeCharacterEquipInfo.EquipType;
                byte equipSlot = changeCharacterEquipInfo.EquipCategory;
                ushort storageSlot = equipSlot;
                
                if(equipType == EquipType.Visual)
                {
                    storageSlot += Equipment.TOTAL_EQUIP_SLOTS;
                }

                uint characterId, pawnId;
                StorageType equipmentStorageType;
                if(characterToEquipTo is Character character1)
                {
                    characterId = character1.CharacterId;
                    pawnId = 0;
                    equipmentStorageType = StorageType.CharacterEquipment;
                }
                else if(characterToEquipTo is Pawn pawn)
                {
                    characterId = pawn.CharacterId;
                    pawnId = pawn.PawnId; 
                    equipmentStorageType = StorageType.PawnEquipment;
                    storageSlot = (ushort)(storageSlot + client.Character.Pawns.IndexOf(pawn)*Equipment.TOTAL_EQUIP_SLOTS*2);
                }
                else
                {
                    throw new Exception("Unknown character type");
                }

                if(itemUId.Length == 0)
                {
                    // Unequip
                    // TODO: Move to the other storage types if the first one is full
                    StorageType destinationStorageType = storageTypes[0];
                    Item? equippedItem = characterToEquipTo.Equipment.SetEquipItem(null, characterToEquipTo.Job, equipType, equipSlot);
                    if(equippedItem == null)
                    {
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_NOT_FOUND);
                    }
                    server.Database.DeleteEquipItem(characterToEquipTo.CommonId, characterToEquipTo.Job, equipType, equipSlot);
                    // Update storage
                    updateCharacterItemNtc.UpdateItemList.AddRange(server.ItemManager.MoveItem(server, client.Character, equipmentStorageType, equippedItem.UId, 1, destinationStorageType, 0));
                }
                else
                {
                    // Equip
                    StorageType sourceStorageType = storageTypes[0];
                    characterToEquipTo.Equipment.SetEquipItem(server.Database.SelectItem(itemUId), characterToEquipTo.Job, equipType, equipSlot);
                    server.Database.ReplaceEquipItem(characterToEquipTo.CommonId, characterToEquipTo.Job, equipType, equipSlot, itemUId);
                    // Update storage
                    updateCharacterItemNtc.UpdateItemList.AddRange(server.ItemManager.MoveItem(server, client.Character, sourceStorageType, itemUId, 1, equipmentStorageType, storageSlot));
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
        }        
    }
}
