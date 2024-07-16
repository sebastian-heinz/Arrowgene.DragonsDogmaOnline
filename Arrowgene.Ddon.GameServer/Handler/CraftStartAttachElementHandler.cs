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
                Gold = 100,
            };

            result.CurrentEquip.EquipSlot.CharacterId = client.Character.CharacterId;
            result.CurrentEquip.EquipSlot.PawnId = 0;
            result.CurrentEquip.EquipSlot.EquipSlotNo = 0;
            result.CurrentEquip.EquipSlot.EquipType = 0;

#if false
            var match = results.Select(x => x).Where(x => x.UId == request.EquipItemUId).ToList();
            if (match.Count > 0)
            {
                var equipInfo = match[0];
                result.CurrentEquip.EquipSlot.CharacterId = client.Character.CharacterId;
                result.CurrentEquip.EquipSlot.EquipSlotNo = equipInfo.EquipSlot;
                result.CurrentEquip.EquipSlot.EquipType = equipInfo.EquipType;
                result.CurrentEquip.EquipSlot.PawnId = 0; // TODO: Handle Pawns?
            }
#endif

            foreach (var element in request.CraftElementList)
            {
                result.EquipElementParamList.Add(new CDataWeaponCrestData()
                {
                    CrestId = Server.ItemManager.LookupItemByUID(Server, element.ItemUId),
                    SlotNo = element.SlotNo,
                });
            }

#if false
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = (ushort) ItemNoticeType.StartAttachElement;
            updateCharacterItemNtc.UpdateWalletList.Add(Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, 100));
            client.Send(updateCharacterItemNtc);
#endif

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
