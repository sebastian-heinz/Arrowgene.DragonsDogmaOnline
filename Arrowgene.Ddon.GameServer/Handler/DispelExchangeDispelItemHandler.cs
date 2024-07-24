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
                    UpdateType = ItemNoticeType.ShopGoods_buy
                };

                // Consume payment
                foreach (var payment in item.UIDList)
                {
                    List<CDataItemUpdateResult> updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, payment.UId, payment.Num);
                    updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                }

                var purchase = AppraiseItem(client.Character, appraisialItems[item.Id]);

                List<CDataItemUpdateResult> itemUpdateResults = Server.ItemManager.AddItem(Server, client.Character, toBag, purchase.ItemId, purchase.ItemNum);

                if (purchase.EquipElementParamList.Count > 0)
                {
                    foreach (var elementParam in purchase.EquipElementParamList)
                    {
                        // TODO: Uncomment after crest PR is merged
                        // Server.Database.InsertCrest(client.Character.CommonId, itemUpdateResults[0].ItemList.ItemUId, elementParam.SlotNo, elementParam.CrestId, elementParam.Add);
                    }

                    itemUpdateResults[0].ItemList.WeaponCrestDataList = purchase.EquipElementParamList;
                }

                updateCharacterItemNtc.UpdateItemList.AddRange(itemUpdateResults);

                client.Send(updateCharacterItemNtc);

                res.DispelItemResultList.Add(purchase);
            }

            return res;
        }

        private CDataDispelResultInfo AppraiseItem(Character character, AppraisalItem item)
        {
            var lotResult = item.LootPool[(new Random()).Next(0, item.LootPool.Count)];

            var itemResult = new CDataDispelResultInfo()
            {
                ItemId = lotResult.ItemId,
                ItemNum = lotResult.Amount
            };

            for (int i = 0; i < lotResult.Crests.Count; i++)
            {
                var appraisalCrest = lotResult.Crests[i];

                var equipElement = new CDataEquipElementParam()
                {
                    SlotNo = (byte)(i + 1)
                };

                switch (appraisalCrest.CrestType)
                {
                    case AppraisalCrestType.Imbued:
                        equipElement.CrestId = appraisalCrest.CrestId;
                        equipElement.Add = appraisalCrest.Amount;
                        break;
                    case AppraisalCrestType.CrestLottery:
                        equipElement.CrestId = AppraisalManager.RollCrestLottery(appraisalCrest.CrestLottery);
                        equipElement.Add = appraisalCrest.Amount;
                        break;
                    case AppraisalCrestType.DragonTrinketAlpha:
                        equipElement.CrestId = AppraisalManager.RollDragonTrinketsAlpha(appraisalCrest.JobId);
                        equipElement.Add = appraisalCrest.Amount;
                        break;
                    case AppraisalCrestType.DragonTrinketBeta:
                        equipElement.CrestId = AppraisalManager.RollDragonTrinketsBeta(appraisalCrest.JobId);
                        equipElement.Add = appraisalCrest.Amount;
                        break;
                    case AppraisalCrestType.BitterBlackEarring:
                        // equipElement.CrestId = BitterBlackManager.RollEarringCrest(character.Job);
                        // equipElement.Add = BitterBlackManager.RollEarringPercent(character.Job);
                        break;
                    case AppraisalCrestType.BitterBlackBracelet:
                        // equipElement.CrestId = BitterBlackManager.RollBraceletCrest();
                        break;
                }

                itemResult.EquipElementParamList.Add(equipElement);
            }

            return itemResult;
        }
    }
}

