using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnDeleteMyPawnHandler : GameRequestPacketHandler<C2SPawnDeleteMyPawnReq, S2CPawnDeleteMyPawnRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnDeleteMyPawnHandler));

        public PawnDeleteMyPawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnDeleteMyPawnRes Handle(GameClient client, C2SPawnDeleteMyPawnReq request)
        {
            S2CPawnDeleteMyPawnRes res = new S2CPawnDeleteMyPawnRes();
            int pawnIndex = request.SlotNo - 1;
            Pawn pawn = client.Character.Pawns[pawnIndex];
            Equipment pawnEquipment = client.Character.Storage.GetPawnEquipment(pawnIndex);
            List<Item> pawnStorageItems = new List<Item>(pawnEquipment.GetItems(EquipType.Performance).ToArray());
            pawnStorageItems.AddRange(pawnEquipment.GetItems(EquipType.Visual).ToArray());

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc
            {
                UpdateType = ItemNoticeType.DeletePawn
            };
            List<CDataItemUpdateResult> updateItems = new List<CDataItemUpdateResult>();
            foreach (Item storageItem in pawnStorageItems)
            {
                if (storageItem == null)
                {
                    continue;
                }

                if (request.IsKeepEquip)
                {
                    updateItems.AddRange(Server.ItemManager.MoveItem(Server, client.Character, pawn.Equipment.Storage, storageItem.UId, 1,
                        client.Character.Storage.GetStorage(StorageType.StorageBoxNormal), 0));
                }
                else
                {
                    updateItems.Add(Server.ItemManager.ConsumeItemByUId(Server, client.Character, StorageType.PawnEquipment, storageItem.UId, 1));
                }
            }

            updateCharacterItemNtc.UpdateItemList.AddRange(updateItems);
            client.Send(updateCharacterItemNtc);

            client.Character.Pawns.Remove(pawn);
            Server.Database.DeletePawn(pawn.PawnId);

            return res;
        }
    }
}
