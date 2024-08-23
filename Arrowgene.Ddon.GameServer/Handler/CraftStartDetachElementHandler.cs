using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
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
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();

            var (storageType, itemProps) = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, request.EquipItemUId);
            var (slotNo, item, amount) = itemProps;

            var result = new S2CCraftStartDetachElementRes();

            ushort relativeSlotNo = slotNo;
            CharacterCommon characterCommon = null;
            if (storageType == StorageType.CharacterEquipment)
            {
                characterCommon = client.Character;
                result.CurrentEquip.EquipSlot.CharacterId = client.Character.CharacterId;
                result.CurrentEquip.EquipSlot.PawnId = 0;
            }
            else if (storageType == StorageType.PawnEquipment)
            {
                uint pawnId = Storages.DeterminePawnId(client.Character, storageType, relativeSlotNo);
                characterCommon = client.Character.Pawns.Where(x => x.PawnId == pawnId).SingleOrDefault();
                relativeSlotNo = EquipManager.DeterminePawnEquipSlot(relativeSlotNo);
                result.CurrentEquip.EquipSlot.CharacterId = 0;
                result.CurrentEquip.EquipSlot.PawnId = pawnId;

            }

            if (storageType == StorageType.CharacterEquipment || storageType == StorageType.PawnEquipment)
            {
                result.CurrentEquip.EquipSlot.EquipSlotNo = EquipManager.DetermineEquipSlot(relativeSlotNo);
                result.CurrentEquip.EquipSlot.EquipType = EquipManager.GetEquipTypeFromSlotNo(relativeSlotNo);
            }

            updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, item, storageType, relativeSlotNo, 0, 0));
            foreach (var element in request.CraftElementList)
            {
                Server.Database.RemoveCrest(client.Character.CommonId, request.EquipItemUId, element.SlotNo);
                result.EquipElementParamList.Add(new CDataEquipElementParam()
                {
                    CrestId = 0,
                    SlotNo = element.SlotNo,
                });

                item.EquipElementParamList = item.EquipElementParamList.Where(x => x.SlotNo != element.SlotNo).ToList();
            }

            updateCharacterItemNtc.UpdateType = ItemNoticeType.StartDetachElement;
            updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, item, storageType, relativeSlotNo, 1, 1));
            client.Send(updateCharacterItemNtc);

            return result;
        }
    }
}
