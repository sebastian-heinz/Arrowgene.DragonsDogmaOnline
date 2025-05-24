#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Craft;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartEquipColorChangeHandler : GameRequestPacketQueueHandler<C2SCraftStartEquipColorChangeReq, S2CCraftStartEquipColorChangeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartEquipColorChangeHandler));
        private readonly ItemManager _itemmanager;
        public CraftStartEquipColorChangeHandler(DdonGameServer server) : base(server)
        {
            _itemmanager = server.ItemManager;
        }

        public override PacketQueue Handle(GameClient client, C2SCraftStartEquipColorChangeReq request)
        {
            PacketQueue queue = new();

            Character character = client.Character;
            uint charId = client.Character.CharacterId;
            string equipItemUID = request.EquipItemUID;
            var ramItem = character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, equipItemUID);
            var equipItem = ramItem.Item2.Item2;
            ClientItemInfo clientItemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, equipItem.ItemId);
            byte color = request.Color;
            uint craftPawnId = request.CraftMainPawnID;
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            CDataCurrentEquipInfo currentEquipInfo = new CDataCurrentEquipInfo()
            {
                ItemUId = equipItemUID,
            };
            string dyeUId = request.CraftColorantList.First().ItemUID;

            Server.Database.ExecuteInTransaction(connection =>
            {
                if (!string.IsNullOrEmpty(dyeUId))
                {
                    try
                    {
                        var updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, dyeUId, 1, connection);
                        updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                    }
                    catch (NotEnoughItemsException)
                    {
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_ITEM_NUM, "Client Item Desync has occurred.");
                    }
                }

                //Applying the Dye
                equipItem.Color = color;

                var (storageType, foundItem) = character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, equipItemUID);

                if (foundItem != null)
                {
                    var (slotno, item, itemnum) = foundItem;
                    CharacterCommon characterCommon = null;

                    if (storageType == StorageType.CharacterEquipment || storageType == StorageType.PawnEquipment)
                    {
                        currentEquipInfo.EquipSlot.EquipSlotNo = EquipManager.DetermineEquipSlot(slotno);
                        currentEquipInfo.EquipSlot.EquipType = EquipManager.GetEquipTypeFromSlotNo(slotno);
                    }

                    if (storageType == StorageType.PawnEquipment)
                    {
                        uint pawnId = Storages.DeterminePawnId(client.Character, storageType, slotno);
                        currentEquipInfo.EquipSlot.PawnId = pawnId;
                        characterCommon = client.Character.Pawns.SingleOrDefault(x => x.PawnId == pawnId)
                            ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_NOT_FOUNDED,
                            $"Couldn't find pawn ID {pawnId}.");
                    }
                    else if (storageType == StorageType.CharacterEquipment)
                    {
                        currentEquipInfo.EquipSlot.CharacterId = charId;
                        characterCommon = character;
                    }

                    updateCharacterItemNtc.UpdateType = ItemNoticeType.StartEquipColorChang;
                    updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, slotno, 0, 0));

                    if (foundItem != null)
                    {
                        (slotno, item, itemnum) = foundItem;
                        _itemmanager.UpgradeStorageItem(
                            Server,
                            client,
                            charId,
                            storageType,
                            equipItem,
                            slotno,
                            connection
                        );
                        updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, slotno, 1, 1));
                    }
                }
                else
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_STORAGE_TYPE, $"Item with UID {equipItemUID} not found in {storageType}");
                }
            
                client.Enqueue(new S2CCraftStartEquipColorChangeRes()
                    {
                        ColorNo = color,
                        CurrentEquipInfo = currentEquipInfo
                    }, queue);

                var craftInfo = Server.AssetRepository.CostExpScalingAsset.GetScalingInfo(clientItemInfo.Rank);
                uint totalCost = craftInfo.Cost;
                uint pawnExp = craftInfo.Exp;

                Pawn leadPawn = Server.CraftManager.FindPawn(client, request.CraftMainPawnID);
                List<CraftPawn> craftPawns = new()
                {
                    new CraftPawn(leadPawn, CraftPosition.Leader)
                };
                craftPawns.AddRange(request.CraftSupportPawnIDList.Select(p => new CraftPawn(Server.CraftManager.FindPawn(client, p.PawnId), CraftPosition.Assistant)));

                uint cost = Server.CraftManager.CalculateRecipeCost(totalCost, clientItemInfo, craftPawns);
                CDataUpdateWalletPoint updateWalletPoint = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, cost, connection)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_CRAFT_INSUFFICIENT_GOLD, $"Insufficient gold. {cost} > {Server.WalletManager.GetWalletAmount(client.Character, WalletType.Gold)}"); updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);

                updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);
                client.Enqueue(updateCharacterItemNtc, queue);

                if (CraftManager.CanPawnExpUp(leadPawn))
                {
                    double BonusExpMultiplier = Server.GpCourseManager.PawnCraftBonus();
                    client.Enqueue(CraftManager.HandlePawnExpUpNtc(client, leadPawn, pawnExp, BonusExpMultiplier), queue);
                    if (CraftManager.CanPawnRankUp(leadPawn))
                    {
                        client.Enqueue(CraftManager.HandlePawnRankUpNtc(client, leadPawn), queue);
                        queue.AddRange(Server.AchievementManager.HandlePawnCrafting(client, leadPawn, connection));
                    }
                }
                else
                {
                    client.Enqueue(CraftManager.HandlePawnExpUpNtc(client, leadPawn, 0, 0), queue);
                }

                Server.Database.UpdatePawnBaseInfo(leadPawn, connection);

                queue.AddRange(Server.AchievementManager.HandleChangeColor(client, connection));
            });

            return queue;
        }
    }
}
