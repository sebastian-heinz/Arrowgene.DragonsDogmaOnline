using Arrowgene.Ddon.GameServer.Characters;
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

                bool toBag = false;
                switch (item.StorageType)
                {
                    case 19:
                        toBag = true;
                        break;
                    case 20:
                        toBag = false;
                        break;
                    default:
                        throw new Exception($"Unexpected destination when exchanging items {item.StorageType}");
                }

                // Check for cost
                bool hasFullPayment = true;
                foreach (var payment in item.UIDList)
                {
                    hasFullPayment &= Server.ItemManager.HasItem(Server, client.Character, ItemManager.BothStorageTypes, payment.UId, payment.Num);
                }

                if (!hasFullPayment)
                {
                    // Probably a hacker or cheater
                    continue;
                }

                var updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
                {
                    UpdateType = 0x10a
                };

                // Consume payment
                foreach (var payment in item.UIDList)
                {
                    List<CDataItemUpdateResult> updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, payment.UId, payment.Num);
                    updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                }

                var purchase = AppraiseItem(appraisialItems[item.Id]);

                List<CDataItemUpdateResult> itemUpdateResults = Server.ItemManager.AddItem(Server, client.Character, toBag, purchase.ItemId, purchase.ItemNum);
                updateCharacterItemNtc.UpdateItemList.AddRange(itemUpdateResults);

                client.Send(updateCharacterItemNtc);

                res.DispelItemResultList.Add(purchase);
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

