using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartDetachElementHandler : GameRequestPacketHandler<C2SCraftStartDetachElementReq, S2CCraftStartDetachElementRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartDetachElementHandler));

        public CraftStartDetachElementHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftStartDetachElementRes Handle(GameClient client, C2SCraftStartDetachElementReq request)
        {
            var results = Server.Database.SelectEquipItemByCharacter(client.Character.CommonId);

            var result = new S2CCraftStartDetachElementRes();
            result.CurrentEquip.EquipSlot.CharacterId = client.Character.CharacterId;
            result.CurrentEquip.EquipSlot.PawnId = 0;
            result.CurrentEquip.EquipSlot.EquipSlotNo = 0;
            result.CurrentEquip.EquipSlot.EquipType = 0;

            Item item = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, request.EquipItemUId);
            foreach (var element in request.CraftElementList)
            {
                Server.Database.RemoveCrest(client.Character.CommonId, request.EquipItemUId, element.SlotNo);
                result.EquipElementParamList.Add(new CDataWeaponCrestData()
                {
                    CrestId = 0,
                    SlotNo = element.SlotNo,
                });

                item.WeaponCrestDataList = item.WeaponCrestDataList.Where(x => x.SlotNo != element.SlotNo).ToList();
            }
            // client.Character.Storage.SetStorageItem(item.Item2, item.Item3, StorageType.ItemBagEquipment, item.Item1);

            return result;
        }
    }
}
