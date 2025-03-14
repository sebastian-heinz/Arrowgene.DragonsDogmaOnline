#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class EquipManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipManager));

        public static readonly List<EquipSlot> EnsembleSlots = new List<EquipSlot>()
        {
            EquipSlot.ArmorLeg,
            EquipSlot.ArmorHelm,
            EquipSlot.ArmorArm,
            EquipSlot.WearBody,
            EquipSlot.WearLeg,
            EquipSlot.Accessory
        };

        private static readonly List<EquipSlot> ItemRankSlots = new List<EquipSlot>()
        {
            EquipSlot.WepMain,
            EquipSlot.ArmorHelm,
            EquipSlot.ArmorBody,
            EquipSlot.WearBody,
            EquipSlot.ArmorArm,
            EquipSlot.ArmorLeg,
            EquipSlot.WearLeg,
            EquipSlot.Accessory
        };

        private static readonly byte SLOTS = EquipmentTemplate.TOTAL_EQUIP_SLOTS;

        public static EquipType GetEquipTypeFromSlotNo(ushort slotNo)
        {
            ushort relativeSlotNo = slotNo;
            if (slotNo > (SLOTS * 2))
            {
                relativeSlotNo = DeterminePawnEquipSlot(slotNo);
            }

            return (relativeSlotNo > SLOTS) ? EquipType.Visual : EquipType.Performance;
        }

        public static ushort DeterminePawnEquipSlot(ushort slotNo)
        {
            int pawnIndex = (slotNo - 1) / (SLOTS * 2);
            var relativeSlotNo = ((slotNo) - (pawnIndex * (SLOTS * 2)));
            return (ushort)((relativeSlotNo > SLOTS) ? relativeSlotNo - SLOTS : relativeSlotNo);
        }

        public static ushort DetermineEquipSlot(ushort slotNo)
        {
            return (ushort) ((slotNo > SLOTS) ? (slotNo - SLOTS) : slotNo);
        }

        public PacketQueue EquipJobItem(DdonGameServer server, GameClient client, CharacterCommon characterToEquipTo, List<CDataChangeEquipJobItem> changeEquipJobItems)
        {
            PacketQueue packetQueue = new PacketQueue();
            foreach (CDataChangeEquipJobItem changeEquipJobItem in changeEquipJobItems)
            {
                if (changeEquipJobItem.EquipJobItemUId.Length == 0)
                {
                    // UNEQUIP
                    // Remove from equipment
                    characterToEquipTo.EquipmentTemplate.SetJobItem(null, characterToEquipTo.Job, changeEquipJobItem.EquipSlotNo);
                    server.Database.DeleteEquipJobItem(characterToEquipTo.CommonId, characterToEquipTo.Job, changeEquipJobItem.EquipSlotNo);
                }
                else
                {
                    // EQUIP
                    Item item = server.Database.SelectStorageItemByUId(changeEquipJobItem.EquipJobItemUId);
                    characterToEquipTo.EquipmentTemplate.SetJobItem(item, characterToEquipTo.Job, changeEquipJobItem.EquipSlotNo);
                    server.Database.ReplaceEquipJobItem(item.UId, characterToEquipTo.CommonId, characterToEquipTo.Job, changeEquipJobItem.EquipSlotNo);
                }
            }

            // Send packets informing of the update
            List<CDataEquipJobItem> equippedJobItems = characterToEquipTo.EquipmentTemplate.JobItemsAsCDataEquipJobItem(characterToEquipTo.Job);
            if (characterToEquipTo is Character character)
            {
                client.Enqueue(new S2CEquipChangeCharacterEquipJobItemRes()
                {
                    EquipJobItemList = equippedJobItems
                }, packetQueue);

                client.Party.EnqueueToAll(new S2CEquipChangeCharacterEquipJobItemNtc()
                {
                    CharacterId = character.CharacterId,
                    EquipJobItemList = equippedJobItems
                }, packetQueue);
            }
            else if (characterToEquipTo is Pawn pawn)
            {
                client.Enqueue(new S2CEquipChangePawnEquipJobItemRes()
                {
                    PawnId = pawn.PawnId,
                    EquipJobItemList = equippedJobItems
                }, packetQueue  );

                client.Party.EnqueueToAll(new S2CEquipChangePawnEquipJobItemNtc()
                {
                    CharacterId = client.Character.CharacterId,
                    PawnId = pawn.PawnId,
                    EquipJobItemList = equippedJobItems
                }, packetQueue);
            }
            else
            {
                throw new Exception("Unknown character type");
            }

            return packetQueue;
        }

        public PacketQueue HandleChangeEquipList(DdonGameServer server, GameClient client, CharacterCommon characterToEquipTo, List<CDataCharacterEquipInfo> changeCharacterEquipList, ItemNoticeType updateType, List<StorageType> storageTypes, DbConnection? connectionIn = null)
        {
            PacketQueue queue = new();

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = updateType
            };

            List<(EquipType, EquipSlot)> forceRemovals = new List<(EquipType, EquipSlot)>();

            foreach (CDataCharacterEquipInfo changeCharacterEquipInfo in changeCharacterEquipList)
            {
                string itemUId = changeCharacterEquipInfo.EquipItemUId;
                EquipType equipType = changeCharacterEquipInfo.EquipType;
                byte equipSlot = changeCharacterEquipInfo.EquipCategory;
                ushort equipItemStorageSlot = characterToEquipTo.Equipment.GetStorageSlot(equipType, equipSlot);

                if (itemUId.Length == 0)
                {
                    // UNEQUIP

                    // Remove from equipment template
                    characterToEquipTo.EquipmentTemplate.SetEquipItem(null, characterToEquipTo.Job, equipType, equipSlot);

                    server.Database.DeleteEquipItem(characterToEquipTo.CommonId, characterToEquipTo.Job, equipType, equipSlot, connectionIn);

                    // Update storage
                    // TODO: Move to the other storage types if the first one is full
                    Storage destinationStorage = client.Character.Storage.GetStorage(storageTypes[0]);
                    updateCharacterItemNtc.UpdateItemList.AddRange(
                        server.ItemManager.MoveItem(server, client.Character, characterToEquipTo.Equipment.Storage, equipItemStorageSlot, 1, destinationStorage, 0, connectionIn)
                    );

                    if (equipType == EquipType.Performance && equipSlot == (byte)EquipSlot.Lantern)
                    {
                        queue.AddRange(server.ItemManager.StopLantern(client, true));
                    }
                }
                else
                {
                    // EQUIP

                    // Set in equipment template
                    //TODO: Move this lookup to memory instead of the DB if possible.
                    characterToEquipTo.EquipmentTemplate.SetEquipItem(server.Database.SelectStorageItemByUId(itemUId, connectionIn), characterToEquipTo.Job, equipType, equipSlot);

                    server.Database.ReplaceEquipItem(characterToEquipTo.CommonId, characterToEquipTo.Job, equipType, equipSlot, itemUId, connectionIn);
                    
                    // Update storage, swapping if needed
                    var result = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, itemUId);
                    Storage sourceStorage = client.Character.Storage.GetStorage(result.Item1);
                    updateCharacterItemNtc.UpdateItemList.AddRange(
                        server.ItemManager.MoveItem(server, client.Character, sourceStorage, itemUId, 1, characterToEquipTo.Equipment.Storage, equipItemStorageSlot, connectionIn)
                    );

                    //Check for ensemble requirements
                    ClientItemInfo itemInfo = server.ItemManager.LookupInfoByUID(server, itemUId);
                    if (itemInfo.SubCategory == ItemSubCategory.EquipEnsemble)
                    {
                        foreach (EquipSlot slot in EnsembleSlots)
                        {
                            var foo = characterToEquipTo.EquipmentTemplate.GetEquipItem(characterToEquipTo.Job, equipType, (byte)slot);
                            if (foo is null) continue;
                            forceRemovals.Add((equipType, slot));
                        }
                    }
                    else if (EnsembleSlots.Contains((EquipSlot)itemInfo.EquipSlot!))
                    {
                        var currentBody = characterToEquipTo.EquipmentTemplate.GetEquipItem(characterToEquipTo.Job, equipType, (byte)EquipSlot.ArmorBody);
                        if (currentBody != null)
                        {
                            ClientItemInfo bodyInfo = server.ItemManager.LookupInfoByItem(server, currentBody);
                            if (bodyInfo.SubCategory == ItemSubCategory.EquipEnsemble)
                            {
                                forceRemovals.Add((equipType, EquipSlot.ArmorBody));
                            }
                        }
                    }
                }
            }

            //Post-process to handle the overrides.
            foreach ((EquipType, EquipSlot) force in forceRemovals)
            {
                EquipType equipType = force.Item1;
                EquipSlot slot = force.Item2;

                //Handle template and DB
                characterToEquipTo.EquipmentTemplate.SetEquipItem(null, characterToEquipTo.Job, equipType, (byte)slot);
                server.Database.DeleteEquipItem(characterToEquipTo.CommonId, characterToEquipTo.Job, equipType, (byte)slot, connectionIn);

                // Update storage
                // TODO: Move to the other storage types if the first one is full
                Storage destinationStorage = client.Character.Storage.GetStorage(storageTypes[0]);
                updateCharacterItemNtc.UpdateItemList.AddRange(server.ItemManager.MoveItem(
                    server,
                    client.Character,
                    characterToEquipTo.Equipment.Storage,
                    characterToEquipTo.Equipment.GetStorageSlot(equipType, (byte)slot),
                    1,
                    destinationStorage,
                    0,
                    connectionIn
                ));

                //Handle the client.
                changeCharacterEquipList.Add(new CDataCharacterEquipInfo()
                {
                    EquipItemUId = string.Empty,
                    EquipCategory = (byte)slot,
                    EquipType = equipType
                });
            }

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

                client.Enqueue(updateCharacterItemNtc, queue);
                server.ClientLookup.EnqueueToAll(changeCharacterEquipNtc, queue);

                return queue;
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

                client.Enqueue(updateCharacterItemNtc, queue);
                server.ClientLookup.EnqueueToAll(changePawnEquipNtc, queue);

                return queue;
            }

            throw new ResponseErrorException(ErrorCode.ERROR_CODE_FAIL); //TODO: Find a better code.
        }

        public void GetEquipTypeandSlot(Equipment equipment, string uid, out EquipType equipType, out byte equipSlot)
        {
            for (int i = 0; i < SLOTS * 2; i++)
            {
                var tuple = equipment.Storage.Items[equipment.Offset + i];
                if (tuple?.Item1?.UId == uid)
                {
                    equipSlot = (byte)(i + 1);
                    equipType = equipSlot <= SLOTS ? EquipType.Performance : EquipType.Visual;
                    return;
                }
            }
            throw new Exception("Item not found");
        }

        public List<(EquipType, EquipSlot)> CleanGenderedEquipTemplates(DdonGameServer server, CharacterCommon character, DbConnection? connectionIn = null)
        {
            List<(EquipType, EquipSlot)> forceRemovals = new List<(EquipType, EquipSlot)>();
            //Clean equip templates.
            foreach (JobId job in Enum.GetValues(typeof(JobId)))
            {
                foreach (EquipType equipType in Enum.GetValues(typeof(EquipType)))
                {
                    foreach (EquipSlot equipSlot in Enumerable.Range(1, 15))
                    {
                        Item? item = character.EquipmentTemplate.GetEquipItem(job, equipType, (byte)equipSlot);
                        if (item is null) continue;
                        ClientItemInfo itemInfo = server.ItemManager.LookupInfoByItem(server, item);
                        if (itemInfo.Gender == Gender.Any) continue;
                        if ((character.EditInfo.Sex == 1 && itemInfo.Gender == Gender.Female)
                            || (character.EditInfo.Sex == 2 && itemInfo.Gender == Gender.Male))
                        {
                            character.EquipmentTemplate.SetEquipItem(null, job, equipType, (byte)equipSlot);
                            server.Database.DeleteEquipItem(character.CommonId, job, equipType, (byte)equipSlot, connectionIn);

                            if (job == character.Job)
                            {
                                forceRemovals.Add((equipType, equipSlot));
                            }
                        }
                    }
                }
            }

            return forceRemovals;
        }

        private bool CharacterHasEnsembleEquipped(DdonGameServer server, CharacterCommon characterCommon)
        {
            foreach (EquipSlot slot in EnsembleSlots)
            {
                var equip = characterCommon.EquipmentTemplate.GetEquipItem(characterCommon.Job, EquipType.Performance, slot);
                if (equip == null)
                {
                    continue;
                }

                var itemInfo = server.ItemManager.LookupInfoByItemID(server, equip.ItemId);
                if (itemInfo.SubCategory == ItemSubCategory.EquipEnsemble)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanMeetStorageRequirements(DdonGameServer server, GameClient client, CharacterCommon characterToEquipTo, List<CDataCharacterEquipInfo> changeCharacterEquipList, List<StorageType> storageTypes)
        {
            int slotsRequired = 0;
            int freeSlots = storageTypes.Sum(x => client.Character.Storage.GetStorage(x).EmptySlots());

            // Check regular removals
            slotsRequired += changeCharacterEquipList.Sum(x => x.EquipItemUId.Length == 0 ? 1 : 0);

            // Check removals caused by equipped an ensemble
            foreach (var changeItem in changeCharacterEquipList)
            {
                // This is actually a removal, so skip it.
                if (changeItem.EquipItemUId.Length == 0)
                {
                    continue;
                }

                var itemInfo = server.ItemManager.LookupInfoByUID(server, changeItem.EquipItemUId);
                if (itemInfo.SubCategory == ItemSubCategory.EquipEnsemble)
                {
                    foreach (EquipSlot slot in EnsembleSlots)
                    {
                        var equip = characterToEquipTo.EquipmentTemplate.GetEquipItem(characterToEquipTo.Job, changeItem.EquipType, slot);
                        if (equip == null && (byte)slot == changeItem.EquipCategory)
                        {
                            // Equipping an ensemble into an open body slot frees one storage space.
                            slotsRequired += -1;
                        }
                        else if (equip != null)
                        {
                            // Equipped an ensemble will force some other item to be removed, taking up one storage space.
                            slotsRequired += 1;
                        }
                    }
                }
            }

            return slotsRequired <= freeSlots;
        }

        public uint CalculateItemRank(DdonGameServer server, CharacterCommon characterCommon)
        {
            uint itemRank = 0;

            if (CharacterHasEnsembleEquipped(server, characterCommon))
            {
                var mainHand = characterCommon.EquipmentTemplate.GetEquipItem(characterCommon.Job, EquipType.Performance, EquipSlot.WepMain);
                itemRank = mainHand is not null ? server.ItemManager.LookupInfoByItemID(server, mainHand.ItemId).Rank : 0u;

                foreach (EquipSlot slot in EnsembleSlots)
                {
                    var equip = characterCommon.EquipmentTemplate.GetEquipItem(characterCommon.Job, EquipType.Performance, slot);
                    if (equip == null)
                    {
                        continue;
                    }

                    var itemInfo = server.ItemManager.LookupInfoByItemID(server, equip.ItemId);
                    if (itemInfo.SubCategory == ItemSubCategory.EquipEnsemble)
                    {
                        itemRank += itemInfo.Rank;
                        break;
                    }
                }

                itemRank = itemRank / 2;
            }
            else
            {
                foreach (EquipSlot slot in ItemRankSlots)
                {
                    var equip = characterCommon.EquipmentTemplate.GetEquipItem(characterCommon.Job, EquipType.Performance, slot);
                    if (equip != null)
                    {
                        var itemInfo = server.ItemManager.LookupInfoByItemID(server, equip.ItemId);
                        itemRank += itemInfo.Rank;
                    }
                }

                itemRank = itemRank / 8;
            }

            return itemRank > 0 ? itemRank : 1;
        }

        public static bool HasLantern(CharacterCommon characterCommon)
        {
            return characterCommon.EquipmentTemplate.GetEquipItem(characterCommon.Job, EquipType.Performance, EquipSlot.Lantern) != null;
        }
    }
}
