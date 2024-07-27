using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Handler
{
    internal class JobJobValueShopBuyItemHandler : GameStructurePacketHandler<C2SJobJobValueShopBuyItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobGetPlayPointListHandler));

        private readonly ItemManager _itemManager;
        private readonly PlayPointManager _playPointManager;

        public JobJobValueShopBuyItemHandler(DdonGameServer server) : base(server)
        {
            _itemManager = server.ItemManager;
            _playPointManager = server.PPManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SJobJobValueShopBuyItemReq> packet)
        {
            uint boughtAmount = packet.Structure.Num;

            bool sendToItemBag;
            switch (packet.Structure.StorageType)
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
                    throw new Exception("Unexpected destination when buying goods: " + packet.Structure.StorageType);
            }

            CDataJobValueShopItem boughtListing = Server.AssetRepository.JobValueShopAsset.Where(
                x => x.Item1 == packet.Structure.JobId
                && x.Item2.LineupId == packet.Structure.LineupId).First().Item2;

            List<CDataItemUpdateResult> itemUpdateResults = _itemManager.AddItem(Server, client.Character, sendToItemBag, boughtListing.ItemId, boughtAmount);
            boughtAmount = (uint)itemUpdateResults.Select(result => result.UpdateItemNum).Sum();

            var totalPrice = boughtAmount * packet.Structure.Price;

            if (boughtAmount == 0)
            {
                client.Send(new S2CItemUpdateCharacterItemNtc()
                {
                    UpdateType = ItemNoticeType.ShopGoods_buy
                });
            }
            else
            {
                client.Send(new S2CItemUpdateCharacterItemNtc()
                {
                    UpdateType = ItemNoticeType.ShopGoods_buy,
                    UpdateItemList = itemUpdateResults
                });
            }

            if (totalPrice > 0)
            {
                _playPointManager.RemovePlayPoint(client, boughtAmount * packet.Structure.Price);
            }

            client.Send(new S2CJobJobValueShopBuyItemRes()
            {
                JobId = packet.Structure.JobId,
                JobValueType = packet.Structure.JobValueType,
                Value = boughtAmount * packet.Structure.Price
            });

        }
    }
}
