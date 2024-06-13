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
            bool isEquipped = _equipManager.IsItemEquipped(common, equipItemUID);


            // Can probably also changed Equipped Gear considering it asks for slot & info.
            if (isEquipped)
            {
                List<CDataCharacterEquipInfo> characterEquipList = common.Equipment.getEquipmentAsCDataCharacterEquipInfo(common.Job, EquipType.Performance)
                    .Union(common.Equipment.getEquipmentAsCDataCharacterEquipInfo(common.Job, EquipType.Visual))
                    .ToList();

                var equipInfo = characterEquipList.FirstOrDefault(info => info.EquipItemUId == equipItemUID);

                equipslot = equipInfo.EquipCategory;
                equiptype = equipInfo.EquipType;
            }

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
                IsGreatSuccess = true
            };

            List<CDataItemUpdateResult> itemUpdateResult = Server.ItemManager.AddItem(Server, client.Character, false, equipItemID, 1, 2);
            updateCharacterItemNtc.UpdateItemList.AddRange(itemUpdateResult);  

            var res = new S2CCraftStartQualityUpRes()
            {
                Unk0 = dummydata,
                CurrentEquip = CurrentEquipInfo
            };
            client.Send(res);
        }
    }
}