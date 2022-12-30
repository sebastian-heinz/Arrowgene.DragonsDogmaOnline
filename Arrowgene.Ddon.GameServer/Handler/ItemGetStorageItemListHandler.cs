using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemGetStorageItemListHandler : GameStructurePacketHandler<C2SItemGetStorageItemListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemGetStorageItemListHandler));


        public ItemGetStorageItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SItemGetStorageItemListReq> packet)
        {
            List<byte> storageTypes = packet.Structure.StorageList.Select(cDataCommonU8 => cDataCommonU8.Value).ToList();
            client.Send(new S2CItemGetStorageItemListRes()
            {
                ItemList = client.Character.CharacterEquipItemListDictionary[client.Character.Job]
                    .Where(equipItem => storageTypes.Contains(equipItem.EquipType))
                    .Select(equipItem => new CDataItemList()
                    {
                        ItemUId = equipItem.EquipItemUId,
                        ItemId = equipItem.ItemId,
                        ItemNum = 1,
                        Unk3 = 0,
                        StorageType = 0xB, //equipItem.EquipType, // TODO: Check if equip type is the same as storage type
                        SlotNo = equipItem.EquipSlot,
                        Unk6 = equipItem.Color,
                        Unk7 = equipItem.PlusValue,
                        Bind = true,
                        Unk9 = 0,
                        Unk10 = client.Character.Id,
                        Unk11 = 0,
                        WeaponCrestDataList = equipItem.WeaponCrestDataList,
                        ArmorCrestDataList = equipItem.ArmorCrestDataList,
                        EquipElementParamList = equipItem.EquipElementParamList
                    })
                    .ToList()
            });
        }
    }
}
