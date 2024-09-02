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
using System.Diagnostics;

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
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_NOT_FOUND);
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
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_STORAGE_TYPE, $"Unexpected destination when exchanging items {item.StorageType}");
                }

                // Check for cost
                bool hasFullPayment = true;
                foreach (var payment in item.UIDList)
                {
                    hasFullPayment &= Server.ItemManager.HasItem(Server, client.Character, ItemManager.BothStorageTypes, payment.UId, payment.Num);
                }

                if (!hasFullPayment)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_DISPEL_LACK_MONEY);
                }

                var updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
                {
                    UpdateType = ItemNoticeType.GetDispelItem
                };

                // Consume payment
                foreach (var payment in item.UIDList)
                {
                    List<CDataItemUpdateResult> updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, payment.UId, payment.Num);
                    updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                }

                var purchase = AppraiseItem(client.Character, appraisialItems[item.Id]);

                List<CDataItemUpdateResult> itemUpdateResults = Server.ItemManager.AddItem(Server, client.Character, toBag, purchase.ItemId, purchase.ItemNum);
                if (itemUpdateResults.Count != 1)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INTERNAL_ERROR);
                }

                var newItem = client.Character.Storage.FindItemByUIdInStorage(ItemManager.BothStorageTypes, itemUpdateResults[0].ItemList.ItemUId).Item2.Item2;
                if (purchase.EquipElementParamList.Count > 0)
                {
                    foreach (var elementParam in purchase.EquipElementParamList)
                    {
                        Server.Database.InsertCrest(client.Character.CommonId, itemUpdateResults[0].ItemList.ItemUId, elementParam.SlotNo, elementParam.CrestId, elementParam.Add);
                        newItem.EquipElementParamList.Add(elementParam);
                    }

                    itemUpdateResults[0].ItemList.EquipElementParamList = purchase.EquipElementParamList;
                }

                updateCharacterItemNtc.UpdateItemList.AddRange(itemUpdateResults);

                client.Send(updateCharacterItemNtc);

                res.DispelItemResultList.Add(purchase);
            }

            return res;
        }

        private CDataDispelResultInfo AppraiseItem(Character character, AppraisalItem item)
        {
            var lotResult = item.LootPool[Random.Shared.Next(0, item.LootPool.Count)];

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
                        equipElement.CrestId = AppraisalManager.RollBitterBlackMazeEarringCrest(character.Job);
                        equipElement.Add = AppraisalManager.RollBitterBlackMazeEarringPercent(character.Job);
                        break;
                }

                itemResult.EquipElementParamList.Add(equipElement);
            }

            return itemResult;
        }
    }
}
