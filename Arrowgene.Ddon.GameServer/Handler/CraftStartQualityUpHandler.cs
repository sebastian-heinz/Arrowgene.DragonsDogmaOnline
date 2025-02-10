#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Craft;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartQualityUpHandler : GameRequestPacketHandler<C2SCraftStartQualityUpReq, S2CCraftStartQualityUpRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartQualityUpHandler));


        public CraftStartQualityUpHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftStartQualityUpRes Handle(GameClient client, C2SCraftStartQualityUpReq request)
        {
            string equipItemUID = request.ItemUID;
            Character character = client.Character;
            var ramItem = character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, equipItemUID);
            Item equipItem = ramItem.Item2.Item2;
            ClientItemInfo itemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, equipItem.ItemId);
            byte itemRank = itemInfo.Rank;
            uint craftpawnid = request.CraftMainPawnID;
            string RefineMaterialUID = request.RefineUID;
            ushort AddStatusID = request.AddStatusID;
            uint pawnExp = 0;
            uint totalCost = Math.Min(100, (uint)itemRank) * 300;
            List<CDataItemUpdateResult> updateResults;
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();

            CDataAddStatusParam AddStat = new CDataAddStatusParam()
            {
                IsAddStat1 = false,
                IsAddStat2 = false,
                AdditionalStatus1 = 0,
                AdditionalStatus2 = 0,
            };
            List<CDataAddStatusParam> AddStatList = new List<CDataAddStatusParam>()
            {
                new CDataAddStatusParam(),
            };
            CDataCurrentEquipInfo CurrentEquipInfo = new CDataCurrentEquipInfo()
            {
                ItemUId = equipItemUID,
            };

            // TODO: Revisit AdditionalStatus down the line. It appears it might be apart of a larger system involving craig? 
            // Definitely a potential huge rabbit hole that I think we should deal with in a different PR.

            // Lead pawn is always owned by player.
            Pawn leadPawn = Server.CraftManager.FindPawn(client, request.CraftMainPawnID);

            List<CraftPawn> craftPawns = new()
            {
                new CraftPawn(leadPawn, CraftPosition.Leader)
            };
            craftPawns.AddRange(request.CraftSupportPawnIDList.Select(p => new CraftPawn(Server.CraftManager.FindPawn(client, p.PawnId), CraftPosition.Assistant)));
            craftPawns.AddRange(request.CraftMasterLegendPawnIDList.Select(p => new CraftPawn(Server.AssetRepository.PawnCraftMasterLegendAsset.Single(m => m.PawnId == p.PawnId))));

            double calculatedOdds = Server.CraftManager.CalculateEquipmentQualityIncreaseRate(craftPawns);

            uint plusValue = 0;
            bool isGreatSuccessEquipmentQuality = false;

            //TODO: Need to calculate the Gold cost and remove it. Cost is based on the Equipments IR.
            // Probably do what Crests do and make a JSON for it?
            if (!string.IsNullOrEmpty(RefineMaterialUID))
            {
                Item refineMaterialItem = Server.Database.SelectStorageItemByUId(RefineMaterialUID);
                CraftCalculationResult craftCalculationResult = Server.CraftManager.CalculateEquipmentQuality(refineMaterialItem, (uint)calculatedOdds, itemRank);
                plusValue = craftCalculationResult.CalculatedValue;
                isGreatSuccessEquipmentQuality = craftCalculationResult.IsGreatSuccess;
                pawnExp = craftCalculationResult.Exp;

                try
                {
                    updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, RefineMaterialUID, 1);
                    updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                }
                catch (NotEnoughItemsException)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_ITEM_NUM, "Client Item Desync has Occurred.");
                }
            }

            // TODO: figuring out what this is
            // I've tried plugging Crest IDs & Equipment ID/RandomQuality n such, and just random numbers Unk0 - Unk4 just don't seem to change anything.
            CDataS2CCraftStartQualityUpResUnk0 dummydata = new CDataS2CCraftStartQualityUpResUnk0()
            {
                IsGreatSuccess = isGreatSuccessEquipmentQuality
            };

            // Updating the item.
            equipItem.ItemId = equipItem.ItemId;
            equipItem.PlusValue = (byte)plusValue;
            equipItem.AddStatusParamList = equipItem.AddStatusParamList;

            var (storageType, foundItem) = character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, equipItemUID);
            if (foundItem != null)
            {
                var (slotno, item, itemnum) = foundItem;
                CharacterCommon characterCommon = null;
                if (storageType == StorageType.CharacterEquipment || storageType == StorageType.PawnEquipment)
                {
                    CurrentEquipInfo.EquipSlot.EquipSlotNo = EquipManager.DetermineEquipSlot(slotno);
                    CurrentEquipInfo.EquipSlot.EquipType = EquipManager.GetEquipTypeFromSlotNo(slotno);
                }

                if (storageType == StorageType.PawnEquipment)
                {
                    uint pawnId = Storages.DeterminePawnId(client.Character, storageType, slotno);
                    CurrentEquipInfo.EquipSlot.PawnId = pawnId;
                    characterCommon = client.Character.Pawns.SingleOrDefault(x => x.PawnId == pawnId);
                }
                else if (storageType == StorageType.CharacterEquipment)
                {
                    CurrentEquipInfo.EquipSlot.CharacterId = character.CharacterId;
                    characterCommon = character;
                }

                updateCharacterItemNtc.UpdateType = ItemNoticeType.StartEquipGradeUp;
                updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, slotno, 0, 0));

                if (foundItem != null)
                {
                    (slotno, item, itemnum) = foundItem;
                    Server.ItemManager.UpgradeStorageItem(Server, client, character.CharacterId, storageType, equipItem, slotno);
                    updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, slotno, 1, 1));
                }
                else
                {
                    Logger.Error($"Item with UID {equipItemUID} not found in {storageType}");
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_STORAGE_TYPE, $"Item with UID {equipItemUID} not found in {storageType}");
                }
            }

            uint cost = Server.CraftManager.CalculateRecipeCost(totalCost, itemInfo, craftPawns);
            CDataUpdateWalletPoint updateWalletPoint = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, cost)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_CRAFT_INSUFFICIENT_GOLD, $"Insufficient gold. {cost} > {Server.WalletManager.GetWalletAmount(client.Character, WalletType.Gold)}"); updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);
            updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);

            if (request.CraftMasterLegendPawnIDList.Count > 0)
            {
                uint totalGPcost = (uint)request.CraftMasterLegendPawnIDList.Sum(p => Server.AssetRepository.PawnCraftMasterLegendAsset.Single(m => m.PawnId == p.PawnId).RentalCost);
                CDataUpdateWalletPoint updateGP = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.GoldenGemstones, totalGPcost)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_GP_LACK_GP);
                updateCharacterItemNtc.UpdateWalletList.Add(updateGP);
            }

            var res = new S2CCraftStartQualityUpRes()
            {
                Unk0 = dummydata,
                AddStatusDataList = AddStatList,
                CurrentEquip = CurrentEquipInfo
            };

            if (Server.CraftManager.CanPawnExpUp(leadPawn))
            {
                double BonusExpMultiplier = Server.GpCourseManager.PawnCraftBonus();
                Server.CraftManager.HandlePawnExpUpNtc(client, leadPawn, pawnExp, BonusExpMultiplier);
                if (Server.CraftManager.CanPawnRankUp(leadPawn))
                {
                    Server.CraftManager.HandlePawnRankUpNtc(client, leadPawn);
                }
            }
            else
            {
                Server.CraftManager.HandlePawnExpUpNtc(client, leadPawn, 0, 0);
            }

            Server.Database.UpdatePawnBaseInfo(leadPawn);

            client.Send(updateCharacterItemNtc);
            return res;
        }
    }
}
