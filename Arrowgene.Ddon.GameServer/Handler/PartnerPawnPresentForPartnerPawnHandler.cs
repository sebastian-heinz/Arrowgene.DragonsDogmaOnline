using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartnerPawnPresentForPartnerPawnHandler : GameRequestPacketQueueHandler<C2SPartnerPawnPresentForPartnerPawnReq, S2CPartnerPawnPresentForPartnerPawnRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartnerPawnPresentForPartnerPawnHandler));

        public PartnerPawnPresentForPartnerPawnHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SPartnerPawnPresentForPartnerPawnReq request)
        {
            CDataPartnerPawnData partnerPawnData = new CDataPartnerPawnData();
            var packets = new PacketQueue();

            List<CDataItemUpdateResult> itemUpdateResults = new List<CDataItemUpdateResult>();
            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var item in request.ItemUIDList)
                {
                    uint itemId = Server.ItemManager.LookupItemByUID(Server, item.ItemUID);
                    var searchResult = client.Character.Storage.FindItemByUIdInStorage(ItemManager.BothStorageTypes, item.ItemUID);
                    var itemUpdate = Server.ItemManager.ConsumeItemByUId(Server, client.Character, searchResult.Item1, item.ItemUID, item.Num, connectionIn: connection)
                        ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_QUEST_DONT_HAVE_DELIVERY_ITEM);

                    itemUpdateResults.Add(itemUpdate);
                    packets.AddRange(Server.PartnerPawnManager.UpdateLikabilityIncreaseAction(client, PartnerPawnAffectionAction.Gift, connection));
                }

                partnerPawnData = Server.PartnerPawnManager.GetCDataPartnerPawnData(client, connection);
            });

            packets.Enqueue(client, new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.QuestDelivery,
                UpdateItemList = itemUpdateResults
            });

            packets.Enqueue(client, new S2CPartnerPawnPresentForPartnerPawnRes()
            {
                PartnerInfo = partnerPawnData
            });

            return packets;
        }
    }
}
