using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Appraisal;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class DispelExchangeDispelItemHandler : GameRequestPacketHandler<C2SDispelExchangeDispelItemReq, S2CDispelExchangeDispelItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DispelExchangeDispelItemHandler));

        public DispelExchangeDispelItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CDispelExchangeDispelItemRes Handle(GameClient client, C2SDispelExchangeDispelItemReq request)
        {
            var res = new S2CDispelExchangeDispelItemRes();

            var appraisialItems = Server.AssetRepository.SpecialShopAsset.AppraisalItems;
            foreach (var item in request.GetDispelItemList)
            {
                if (!appraisialItems.ContainsKey(item.Id))
                {
                    // TODO: Throw an exception?
                    continue;
                }

                bool sendToItemBag;
                switch (item.StorageType)
                {
                    case 19:
                        // If item.StorageType is 19: Send to corresponding item bag
                        sendToItemBag = true;
                        break;
                    case 20:
                        // If item.StorageType is 20: Send to storage 
                        sendToItemBag = false;
                        break;
                    default:
                        throw new Exception($"Unexpected destination when exchanging items {item.StorageType}");
                }

                var appraisialItem = appraisialItems[item.Id];
                res.DispelItemResultList.Add(AppraiseItem(appraisialItem));
            }

            return res;
        }

        private CDataDispelResultInfo AppraiseItem(AppraisalItem item)
        {
            var lotResult = item.LootPool[(new Random()).Next(0, item.LootPool.Count)];

            var itemResult = new CDataDispelResultInfo()
            {
                ItemId = lotResult.ItemId,
                ItemNum = lotResult.Amount
            };

            for (int i = 0; i < lotResult.Slots; i++)
            {
                itemResult.EquipElementParamList.Add(new CDataEquipElementParam()
                {
                    SlotNo = (byte) i,
                    CrestId = 13662,
                    Add = 3,
                });
            }

            return itemResult;
        }
    }
}

