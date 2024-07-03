#nullable enable
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartQualityUpHandler : GameStructurePacketHandler<C2SCraftStartQualityUpReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartQualityUpHandler));
        private static readonly List<StorageType> STORAGE_TYPES = new List<StorageType> {
            StorageType.ItemBagConsumable, StorageType.ItemBagMaterial, StorageType.ItemBagEquipment, StorageType.ItemBagJob, 
            StorageType.StorageBoxNormal, StorageType.StorageBoxExpansion, StorageType.StorageChest
        };

        private readonly ItemManager _itemManager;
        private readonly EquipManager _equipManager;
        private readonly Random _random;

        public CraftStartQualityUpHandler(DdonGameServer server) : base(server)
        {
            _itemManager = Server.ItemManager;
            _equipManager = Server.EquipManager;
            _random = new Random();
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftStartQualityUpReq> packet)
        {

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();    
            updateCharacterItemNtc.UpdateType = 0;
            string equipItemUID = packet.Structure.Unk0;
            uint equipItemID = _itemManager.LookupItemByUID(Server, equipItemUID);
            Character common = client.Character;
            ushort equipslot = 0;
            byte equiptype = 0;
            uint charid = client.Character.CharacterId;
            uint pawnid = packet.Structure.CraftMainPawnID;
            var equipItem = Server.Database.SelectItem(equipItemUID);
            byte currentPlusValue = equipItem.PlusValue;
            bool isEquipped = _equipManager.IsItemEquipped(common, equipItemUID);
            bool dogreatsucess = _random.Next(5) == 0; // 1 in 5 chance to be true, someone said it was 20%.
            string RefineMaterial = packet.Structure.Unk1;
            byte RandomQuality = 0;
            int D100 =  _random.Next(100);
            S2CContextGetLobbyPlayerContextNtc lobbyPlayerContextNtc = new S2CContextGetLobbyPlayerContextNtc();

            // Check if a refinematerial is set (shouldn't be possible to have annything run without it but heyho)
            if (!string.IsNullOrEmpty(RefineMaterial))
            {
                // Remove Refinement material (and increase odds of better Stars)
                foreach (var craftMaterial in packet.Structure.CraftMaterialList)
                {
                    try
                    {
                        List<CDataItemUpdateResult> updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, STORAGE_TYPES, RefineMaterial, 1);
                        updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                        D100 = D100 + 10;
                    }
                    catch (NotEnoughItemsException e)
                    {
                        Logger.Exception(e);
                        client.Send(new S2CCraftStartCraftRes()
                        {
                            Result = 1
                        });
                        return;
                    }
                }
            }

            
            var thresholds = new (int Threshold, int Quality)[]
            {
                (75, 2),
                (25, 1),
                (0, 0)  // This should always be the last one to catch all remaining cases
            };

            RandomQuality = (byte)thresholds.First(t => D100 >= t.Threshold).Quality;

            if (dogreatsucess)
            {
                RandomQuality = 3;
            }

            Item QualityUpItem = new Item()
            {
                ItemId = equipItemID,
                Unk3 = 0,   // Safety setting,
                Color = 0,
                PlusValue = RandomQuality,
                WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                ArmorCrestDataList = new List<CDataArmorCrestData>(),
                EquipElementParamList = new List<CDataEquipElementParam>()
            };
            

            if (isEquipped)
            {
                List<CDataCharacterEquipInfo> characterEquipList = common.Equipment.getEquipmentAsCDataCharacterEquipInfo(common.Job, EquipType.Performance)
                    .Union(common.Equipment.getEquipmentAsCDataCharacterEquipInfo(common.Job, EquipType.Visual))
                    .ToList();

                var equipInfo = characterEquipList.FirstOrDefault(info => info.EquipItemUId == equipItemUID);
                equipslot = equipInfo.EquipCategory;
                equiptype = equipInfo.EquipType;

                _equipManager.ReplaceEquippedItem(Server, client, common, StorageType.Unk14, equipItemUID, QualityUpItem.UId, QualityUpItem.ItemId, QualityUpItem, (EquipType)equiptype, (byte)equipslot);
                lobbyPlayerContextNtc = new S2CContextGetLobbyPlayerContextNtc();
                GameStructure.S2CContextGetLobbyPlayerContextNtc(lobbyPlayerContextNtc, common);
                client.Send(lobbyPlayerContextNtc);
            }
            else
            {
                Logger.Debug($"Attempting to find {equipItemUID}");
                StorageType storageType = FindItemByUID(common, equipItemUID).StorageType ?? throw new Exception("Item not found in any storage type");
                Logger.Debug($"this is it {storageType}");
                // TODO: Looks like this is mostly fine but some potential issues if items share UIDs? I need to explore this further.
                // unfortunately this didn't fix the bug of not losing the item icon when you change quality lol

                // TODO: Sometimes an error happens, need to look into that.
                ushort slotno = 0;
                uint itemnum = 0;
                Item item;
                var foundItem = common.Storage.getStorage(StorageType.ItemBagEquipment).findItemByUId(equipItemUID);

                switch (storageType)
                {
                    case StorageType.ItemBagEquipment:
                        foundItem = common.Storage.getStorage(StorageType.ItemBagEquipment).findItemByUId(equipItemUID);
                        break;
                    case StorageType.StorageBoxNormal:
                        foundItem = common.Storage.getStorage(StorageType.StorageBoxNormal).findItemByUId(equipItemUID);
                        break;
                    case StorageType.StorageBoxExpansion:
                        foundItem = common.Storage.getStorage(StorageType.StorageBoxExpansion).findItemByUId(equipItemUID);
                        break;
                    case StorageType.StorageChest:
                        foundItem = common.Storage.getStorage(StorageType.StorageChest).findItemByUId(equipItemUID);
                        break;
                    default:
                        Logger.Debug($"Bruh this found an item in {storageType}, not cool.");
                        break;
                }

                if (foundItem != null)
                {
                    (slotno, item, itemnum) = foundItem;
                    _itemManager.ReplaceStorageItem(
                        Server,
                        client,
                        common,
                        charid,
                        storageType,
                        QualityUpItem,
                        QualityUpItem.UId,
                        (byte)slotno
                    );
                    Logger.Debug($"Your Slot is: {slotno}, in {storageType} hopefully thats right?");
                }
                else
                {
                    Logger.Error($"Item with UID {equipItemUID} not found in {storageType}");
                }

                List<CDataItemUpdateResult> updateResults = Server.ItemManager.ReplaceStorageItem(Server, client, common, charid, storageType, QualityUpItem, QualityUpItem.UId, (byte)slotno);
                updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
            
            };

            List<CDataArmorCrestData> ArmorCrestDataList = new List<CDataArmorCrestData>();
            ArmorCrestDataList.Add(new CDataArmorCrestData { u0 = 0, u1 = 0, u2 = 0, u3 = 0 });

            CDataEquipSlot EquipmentSlot = new CDataEquipSlot()
            {
                Unk0 = charid,
                Unk1 = pawnid,
                Unk2 = equiptype, // type
                Unk3 = equipslot, // slot
            };
            CDataCurrentEquipInfo CurrentEquipInfo = new CDataCurrentEquipInfo()
            {
                ItemUID = equipItemUID,
                EquipSlot = EquipmentSlot
            };

            // figuring out what this is
            CDataS2CCraftStartQualityUpResUnk0 dummydata = new CDataS2CCraftStartQualityUpResUnk0()
            {
                Unk0 = 0,
                Unk1 = 0,
                Unk2 = 0,
                Unk3 = 0,
                Unk4 = 0,
                IsGreatSuccess = dogreatsucess
            };

            var res = new S2CCraftStartQualityUpRes()
            {
                Unk0 = dummydata,
                ArmorCrestDataList = ArmorCrestDataList,
                CurrentEquip = CurrentEquipInfo
            };
            client.Send(res);
            client.Send(updateCharacterItemNtc);
        }
        private (StorageType? StorageType, (ushort SlotNo, Item Item, uint ItemNum)?) FindItemByUID(Character character, string itemUID)
        {
            foreach (var storageType in STORAGE_TYPES)
            {
                var foundItem = character.Storage.getStorage(storageType).findItemByUId(itemUID);
                if (foundItem != null)
                {
                    return (storageType, (foundItem.Item1, foundItem.Item2, foundItem.Item3));
                }
            }
            return (null, null);
        }
    }
}