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

                // TODO: unequip pawn and store items in player storage instead of deleting
                if (request.IsKeepEquip)
                {
                }
                CDataItemUpdateResult ntcData = Server.ItemManager.ConsumeItemByUId(Server, client.Character, StorageType.PawnEquipment, storageItem.UId, 1);
                updateItems.Add(ntcData);
            }

            updateCharacterItemNtc.UpdateItemList.AddRange(updateItems);
            client.Send(updateCharacterItemNtc);

            client.Character.Pawns.Remove(pawn);
            Server.Database.DeletePawn(pawn.PawnId);

            // TODO: send some form of NTC to notify client

            return res;
        }
    }
}
