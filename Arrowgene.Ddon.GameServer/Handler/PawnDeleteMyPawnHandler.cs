using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

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

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc
            {
                UpdateType = ItemNoticeType.DeletePawn
            };
            Server.Database.ExecuteInTransaction(connection =>
            {
                Pawn pawn = client.Character.Pawns[pawnIndex];
                Equipment pawnEquipment = client.Character.Storage.GetPawnEquipment(pawnIndex);
                List<Item> pawnStorageItems = new List<Item>(pawnEquipment.GetItems(EquipType.Performance).ToArray());
                pawnStorageItems.AddRange(pawnEquipment.GetItems(EquipType.Visual).ToArray());

                foreach (Item storageItem in pawnStorageItems)
                {
                    if (storageItem == null)
                    {
                        continue;
                    }

                    // UI indicates that holding a locked item should prevent pawn deletion.
                    if (storageItem.SafetySetting > 0)
                    {
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_SAFETY_SETTING);
                    }

                    if (request.IsKeepEquip)
                    {
                        updateCharacterItemNtc.UpdateItemList.AddRange(Server.ItemManager.MoveItem(Server, client.Character, pawn.Equipment.Storage, storageItem.UId, 1,
                            client.Character.Storage.GetStorage(StorageType.StorageBoxNormal), 0, connection));
                    }
                    else
                    {
                        updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.ConsumeItemByUId(Server, client.Character, StorageType.PawnEquipment, storageItem.UId, 1, connection));
                    }
                }

                // Later pawns in the list have to have their gear shuffled down to accomodate.
                // Or be lazy and just throw them all into the item post.
                // TODO: Stop being lazy and actually put them in their proper equip slots.
                for (int nextIndex = pawnIndex + 1; nextIndex < client.Character.Pawns.Count; nextIndex++)
                {
                    Pawn nextPawn = client.Character.Pawns[nextIndex];
                    Equipment nextEquipment = client.Character.Storage.GetPawnEquipment(nextIndex);
                    List<Item> nextStorageItems = new List<Item>(nextEquipment.GetItems(EquipType.Performance).ToArray());
                    nextStorageItems.AddRange(nextEquipment.GetItems(EquipType.Visual).ToArray());

                    foreach (Item nextItem in nextStorageItems)
                    {
                        updateCharacterItemNtc.UpdateItemList.AddRange(Server.ItemManager.MoveItem(Server, client.Character, nextPawn.Equipment.Storage, nextItem.UId, 1,
                            client.Character.Storage.GetStorage(StorageType.ItemPost), 0, connection));
                    }
                }

                client.Character.Pawns.Remove(pawn);
                Server.Database.DeletePawn(pawn.PawnId, connection);
            });

            client.Send(updateCharacterItemNtc);

            return res;
        }
    }
}
