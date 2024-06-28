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

            // Check if a refinematerial is set
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

            // Someone said a GreatSuccess gave 2 Crests instead of 1? so I guess take the result and Add 1? But Clamp to 3.
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
                S2CContextGetLobbyPlayerContextNtc lobbyPlayerContextNtc = new S2CContextGetLobbyPlayerContextNtc();
                GameStructure.S2CContextGetLobbyPlayerContextNtc(lobbyPlayerContextNtc, client.Character);
                client.Send(lobbyPlayerContextNtc);
            }
            else
            {
                List<CDataItemUpdateResult> AddItemResult;
                List<CDataItemUpdateResult> RemoveItemResult;
                RemoveItemResult = _itemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, STORAGE_TYPES, equipItemUID, 1);
                bool isBagItem =! RemoveItemResult.Any(result => result.ItemList.StorageType == StorageType.StorageBoxNormal ||
                                                                result.ItemList.StorageType == StorageType.StorageBoxExpansion);
                AddItemResult = _itemManager.AddItem(Server, client.Character, isBagItem, equipItemID, 1, RandomQuality);
                updateCharacterItemNtc.UpdateItemList.AddRange(AddItemResult);
                updateCharacterItemNtc.UpdateItemList.AddRange(RemoveItemResult);
                //TODO: When we figure out swapping items directly correctly, do that here too. Its almost done in CraftStartEquipGradeUpHandler in the Equipped section, just need client to reflect accurately.
                //TODO: bug caused by doing it this way, the end screen will show a blank item because the item it wants to show is consumed 
            }


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
    }
}