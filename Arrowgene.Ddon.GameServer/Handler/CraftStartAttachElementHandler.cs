using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartAttachElementHandler : GameRequestPacketHandler<C2SCraftStartAttachElementReq, S2CCraftStartAttachElementRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartAttachElementHandler));

        public CraftStartAttachElementHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftStartAttachElementRes Handle(GameClient client, C2SCraftStartAttachElementReq request)
        {
            var results = Server.Database.SelectEquipItemByCharacter(client.Character.CommonId);

            var result = new S2CCraftStartAttachElementRes()
            {
                // TODO: Use values extracted from client file craft_element_exp.cee.json
                // TODO: Level = itemrank - 1
                Gold = 100,
            };

            result.CurrentEquip.EquipSlot.CharacterId = client.Character.CharacterId;
            result.CurrentEquip.EquipSlot.PawnId = 0;
            result.CurrentEquip.EquipSlot.EquipSlotNo = 0;
            result.CurrentEquip.EquipSlot.EquipType = 0;

            Item item = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, request.EquipItemUId);
            foreach (var element in request.CraftElementList)
            {
                uint crestId = Server.ItemManager.LookupItemByUID(Server, element.ItemUId);

                Server.Database.InsertCrest(client.Character.CommonId, request.EquipItemUId, element.SlotNo, crestId, 0);
                result.EquipElementParamList.Add(new CDataWeaponCrestData()
                {
                    CrestId = crestId,
                    SlotNo = element.SlotNo,
                });

                item.WeaponCrestDataList.Add(new CDataWeaponCrestData()
                {
                    CrestId = crestId,
                    SlotNo = element.SlotNo,
                });

                // TODO: Consume crest item
            }

            // client.Character.Storage.SetStorageItem(item.Item2, item.Item3, StorageType.ItemBagEquipment, item.Item1);

#if false
            // Do I actually need this for anything other than the wallet?
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = (ushort) ItemNoticeType.StartAttachElement;
            updateCharacterItemNtc.UpdateWalletList.Add(Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, 100));

            updateCharacterItemNtc.UpdateItemList.Add(new CDataItemUpdateResult(item));
            client.Send(updateCharacterItemNtc);
#endif

            // TODO: Use values extracted from client file
            S2CCraftCraftExpUpNtc expNtc = new S2CCraftCraftExpUpNtc()
            {
                PawnId = request.CraftMainPawnId,
                AddExp = 100,
                ExtraBonusExp = 0,
                TotalExp = 100,
                CraftRankLimit = 0
            };
            client.Send(expNtc);

            return result;
        }
    }
}
