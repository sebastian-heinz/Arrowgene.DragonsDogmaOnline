using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Appraisal;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class DispelExchangeDispelItemHandler : GameRequestPacketQueueHandler<C2SDispelExchangeDispelItemReq, S2CDispelExchangeDispelItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DispelExchangeDispelItemHandler));

        public DispelExchangeDispelItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SDispelExchangeDispelItemReq request)
        {
            PacketQueue queue = new();
            var res = new S2CDispelExchangeDispelItemRes();
            var appraisalItems = Server.AssetRepository.SpecialShopAsset.AppraisalItems;

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var item in request.GetDispelItemList)
                {
                    if (!appraisalItems.ContainsKey(item.Id))
                    {
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_NOT_FOUND);
                    }

                    bool toBag = false;
                    toBag = item.StorageType switch
                    {
                        19 => true,
                        20 => false,
                        _ => throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_STORAGE_TYPE, $"Unexpected destination when exchanging items {item.StorageType}"),
                    };

                    // Check for cost
                    bool hasFullPayment = true;
                    foreach (var payment in item.UIDList)
                    {
                        hasFullPayment &= Server.ItemManager.HasItem(Server, client.Character, ItemManager.BothStorageTypes, payment.ItemUID, payment.Num);
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
                        List<CDataItemUpdateResult> updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, payment.ItemUID, payment.Num, connection);
                        updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                    }

                    var purchase = AppraiseItem(client.Character, appraisalItems[item.Id]);

                    var (specialQueue, isSpecial) = Server.ItemManager.HandleSpecialItem(client, updateCharacterItemNtc, (ItemId)purchase.ItemId, purchase.ItemNum, connection);
                    if (isSpecial)
                    {
                        queue.AddRange(specialQueue);
                    }
                    else
                    {
                        updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.AddNewItem(Server, client.Character, toBag, purchase.ToItem(), purchase.ItemNum, connection));
                    }

                    client.Enqueue(updateCharacterItemNtc, queue);
                    res.DispelItemResultList.Add(purchase);

                    queue.AddRange(Server.AchievementManager.HandleAppraisal(client, connection));
                }
            });

            client.Enqueue(res, queue);

            return queue;
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
