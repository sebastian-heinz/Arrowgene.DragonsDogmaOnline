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
            S2CPawnDeleteMyPawnRes res = new();
            int pawnIndex = request.SlotNo - 1;

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc
            {
                UpdateType = ItemNoticeType.DeletePawn
            };

            Pawn pawn = client.Character.Pawns[pawnIndex];
            Equipment pawnEquipment = client.Character.Storage.GetPawnEquipment(pawnIndex);
            List<Item> pawnStorageItems = [.. pawnEquipment.GetItems(EquipType.Performance), .. pawnEquipment.GetItems(EquipType.Visual)];

            Server.Database.ExecuteInTransaction(connection =>
            {
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
                            client.Character.Storage.GetStorage(StorageType.ItemPost), 0, connection));
                    }
                    else
                    {
                        updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.ConsumeItemByUId(Server, client.Character, StorageType.PawnEquipment, storageItem.UId, 1, connection));
                    }
                }

                // Later pawns in the list have to have their gear shuffled down to accomodate.
                Dictionary<Pawn, List<(Item? Item, ushort Slot, EquipType Type)>> nextPawnEquipment = [];
                var itemPost = client.Character.Storage.GetStorage(StorageType.ItemPost);
                for (int nextIndex = pawnIndex + 1; nextIndex < client.Character.Pawns.Count; nextIndex++)
                {
                    Pawn nextPawn = client.Character.Pawns[nextIndex];
                    nextPawnEquipment[nextPawn] = client.Character.Storage.GetPawnEquipment(nextIndex).GetItemsTuple();
                    foreach (var (item, slot, type) in nextPawnEquipment[nextPawn])
                    {
                        if (item is null) continue;
                        // Put equipped items in the item post, temporarily. We don't care about the updates because this is just bookkeeping for the DB.
                        Server.ItemManager.MoveItem(Server, client.Character, nextPawn.Equipment.Storage, item.UId, 1, itemPost, 0, connection);
                    }
                }

                client.Character.Pawns.Remove(pawn);
                Server.Database.DeletePawn(pawn.PawnId, connection);

                // Fix the shuffled pawns equipment in the DB.
                foreach (var (nextPawn, equipmentList) in nextPawnEquipment)
                {
                    // Their internal offset for their equipment storage needs to be adjusted so further operations don't throw equipment into the void.
                    int deltaOffset = EquipmentTemplate.TOTAL_EQUIP_SLOTS * 2;
                    int offset = pawnIndex++ * deltaOffset;
                    nextPawn.Equipment.SetOffset(offset);

                    foreach (var (item, slot, type) in equipmentList)
                    {
                        if (item is null) continue;
                        ushort newSlot = (ushort)(slot - deltaOffset);
                        
                        // Re-equip the stuff we put in the Item Post.
                        Server.ItemManager.MoveItem(Server, client.Character, itemPost, item.UId, 1, nextPawn.Equipment.Storage, newSlot, connection);
                    }
                }
            });

            client.Send(updateCharacterItemNtc);

            return res;
        }
    }
}
