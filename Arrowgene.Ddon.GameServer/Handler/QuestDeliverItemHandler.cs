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
    public class QuestDeliverItemHandler : GameRequestPacketHandler<C2SQuestDeliverItemReq, S2CQuestDeliverItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestDeliverItemHandler));

        public QuestDeliverItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestDeliverItemRes Handle(GameClient client, C2SQuestDeliverItemReq request)
        {
            S2CQuestDeliverItemRes res = new S2CQuestDeliverItemRes()
            {
                QuestScheduleId = request.QuestScheduleId,
                ProcessNo = request.ProcessNo,
            };

            var quest = QuestManager.GetQuestByScheduleId(request.QuestScheduleId);
            var questStateManager = QuestManager.GetQuestStateManager(client, quest);
            var questState = questStateManager.GetQuestState(request.QuestScheduleId);

            Dictionary<uint, CDataDeliveredItem> deliveredItems = new Dictionary<uint, CDataDeliveredItem>();
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

                    if (!deliveredItems.ContainsKey(itemId))
                    {
                        deliveredItems[itemId] = new CDataDeliveredItem()
                        {
                            ItemId = itemId,
                            ItemNum = 0,
                            NeedNum = 0
                        };
                    }
                    deliveredItems[itemId].ItemNum += (ushort)item.Num;
                }

                foreach (var deliveredItem in deliveredItems.Values)
                {
                    // Do this check in the transaction so the DB rolls back if something goes wrong.
                    deliveredItem.NeedNum = (ushort)questState.UpdateDeliveryRequest(deliveredItem.ItemId, deliveredItem.ItemNum);
                }
            });

            client.Send(new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.QuestDelivery,
                UpdateItemList = itemUpdateResults
            });

            S2CQuestDeliverItemNtc ntc = new S2CQuestDeliverItemNtc()
            {
                DeliveredItemRecord = new CDataDeliveredItemRecord()
                {
                    CharacterId = client.Character.CharacterId,
                    QuestScheduleId = request.QuestScheduleId,
                    ProcessNo = request.ProcessNo,
                    DeliveredItemList = deliveredItems.Values.ToList()
                }
            };

            if (quest.IsPersonal)
            {
                client.Send(ntc);
            }
            else
            {
                client.Party.SendToAll(ntc);
            }

            return res;
        }
    }
}
