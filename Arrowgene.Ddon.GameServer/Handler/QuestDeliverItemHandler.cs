using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Party;
using System.Diagnostics;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestDeliverItemHandler : GameStructurePacketHandler<C2SQuestDeliverItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestDeliverItemHandler));

        public QuestDeliverItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestDeliverItemReq> packet)
        {
            S2CQuestDeliverItemRes res = new S2CQuestDeliverItemRes()
            {
                QuestScheduleId = packet.Structure.QuestScheduleId,
                ProcessNo = packet.Structure.ProcessNo,
            };



            QuestId questId = (QuestId)packet.Structure.QuestScheduleId;
            var questState = client.Party.QuestState.GetQuestState(questId);

            Dictionary<uint, CDataDeliveredItem> deliveredItems = new Dictionary<uint, CDataDeliveredItem>();
            List<CDataItemUpdateResult> itemUpdateResults = new List<CDataItemUpdateResult>();
            foreach (var item in packet.Structure.ItemUIDList)
            {
                uint itemId = Server.ItemManager.LookupItemByUID(Server, item.UId);
                var itemUpdate = Server.ItemManager.ConsumeItemByUIdFromItemBag(Server, client.Character, item.UId, item.Num);
                if (itemUpdate == null)
                {
                    res.Error = (uint)ErrorCode.ERROR_CODE_QUEST_DONT_HAVE_DELIVERY_ITEM;
                    client.Send(res);
                    return;
                }
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
                deliveredItems[itemId].ItemNum += (ushort) item.Num;
            }

            foreach (var deliveredItem in deliveredItems.Values)
            {
                deliveredItem.NeedNum = (ushort) questState.UpdateDeliveryRequest(deliveredItem.ItemId, deliveredItem.ItemNum);
            }

            client.Send(new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = (ushort) ItemNoticeType.Default,
                UpdateItemList = itemUpdateResults
            });

            S2CQuestDeliverItemNtc ntc = new S2CQuestDeliverItemNtc()
            {
                DeliveredItemRecord = new CDataDeliveredItemRecord()
                {
                    CharacterId = client.Character.CharacterId,
                    QuestScheduleId = packet.Structure.QuestScheduleId,
                    ProcessNo = packet.Structure.ProcessNo,
                    DeliveredItemList = deliveredItems.Values.ToList()
                }
            };

            client.Party.SendToAll(ntc);

            client.Send(res);
        }
    }
}
