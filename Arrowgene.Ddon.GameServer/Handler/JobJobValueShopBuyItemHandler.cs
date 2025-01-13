using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobJobValueShopBuyItemHandler : GameRequestPacketHandler<C2SJobJobValueShopBuyItemReq, S2CJobJobValueShopBuyItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobGetPlayPointListHandler));

        private readonly ItemManager _itemManager;
        private readonly PlayPointManager _playPointManager;

        public JobJobValueShopBuyItemHandler(DdonGameServer server) : base(server)
        {
            _itemManager = server.ItemManager;
            _playPointManager = server.PPManager;
        }

        public override S2CJobJobValueShopBuyItemRes Handle(GameClient client, C2SJobJobValueShopBuyItemReq packet)
        {
            uint boughtAmount = packet.Num;

            bool sendToItemBag;
            switch (packet.StorageType)
            {
                case 19:
                    // If packet.Structure.Destination is 19: Send to corresponding item bag
                    sendToItemBag = true;
                    break;
                case 20:
                    // If packet.Structure.Destination is 20: Send to storage 
                    sendToItemBag = false;
                    break;
                default:
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_STORAGE_TYPE, $"Unexpected destination when buying goods: {packet.StorageType}");
            }

            CDataJobValueShopItem boughtListing = Server.AssetRepository.JobValueShopAsset.Where(
                x => x.Item1 == packet.JobId
                && x.Item2.LineupId == packet.LineupId).First().Item2;

            List<CDataItemUpdateResult> itemUpdateResults = _itemManager.AddItem(Server, client.Character, sendToItemBag, boughtListing.ItemId, boughtAmount);
            boughtAmount = (uint)itemUpdateResults.Select(result => result.UpdateItemNum).Sum();

            if (boughtAmount > 0)
            {
                client.Send(new S2CItemUpdateCharacterItemNtc()
                {
                    UpdateItemList = itemUpdateResults
                });
            }

            if (packet.Price > 0)
            {
                _playPointManager.RemovePlayPoint(client, packet.Price, packet.JobId);
            }

            return new S2CJobJobValueShopBuyItemRes()
            {
                JobId = packet.JobId,
                JobValueType = packet.JobValueType,
                Value = boughtAmount * packet.Price
            };
        }
    }
}
