#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftStartEquipColorChangeHandler : GameRequestPacketHandler<C2SCraftStartEquipColorChangeReq, S2CCraftStartEquipColorChangeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartEquipColorChangeHandler));
        private readonly ItemManager _itemmanager;
        public CraftStartEquipColorChangeHandler(DdonGameServer server) : base(server)
        {
            _itemmanager = server.ItemManager;
        }

        public override S2CCraftStartEquipColorChangeRes Handle(GameClient client, C2SCraftStartEquipColorChangeReq request)
        {
            Character character = client.Character;
            uint charid = client.Character.CharacterId;
            string equipItemUID = request.EquipItemUID;
            List<CDataCraftColorant> colorList = request.CraftColorantList;
            var ramItem = character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, equipItemUID);
            var equipItem = ramItem.Item2.Item2;
            ClientItemInfo clientItemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, equipItem.ItemId);
            byte color = request.Color;
            List<CDataCraftColorant> colorlist = new List<CDataCraftColorant>(); // this is probably for consuming the dye
            uint craftpawnid = request.CraftMainPawnID;
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            CDataCurrentEquipInfo CurrentEquipInfo = new CDataCurrentEquipInfo()
            {
                ItemUId = equipItemUID,
            };
            var colorantList = colorList[0];
            string DyeUId = colorantList.ItemUID;

            if (!string.IsNullOrEmpty(DyeUId))
            {
                try
                {
                    var updateResults = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, DyeUId, 1);
                    updateCharacterItemNtc.UpdateItemList.AddRange(updateResults);
                }
                catch (NotEnoughItemsException)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_ITEM_NUM, "Client Item Desync has Occurred.");
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
                    CurrentEquipInfo.EquipSlot.CharacterId = charid;
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
                        charid,
                        storageType,
                        equipItem,
                        slotno
                    );
                    updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, equipItem, storageType, slotno, 1, 1));
                }
            }
            else
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INVALID_STORAGE_TYPE, $"Item with UID {equipItemUID} not found in {storageType}");
            }
            
            var res = new S2CCraftStartEquipColorChangeRes()
                {
                    ColorNo = color,
                    CurrentEquipInfo = CurrentEquipInfo
                };

            var craftInfo = Server.AssetRepository.CostExpScalingAsset.CostExpScalingInfo[clientItemInfo.Rank];
            uint totalCost = craftInfo.Cost;
            uint pawnExp = craftInfo.Exp;

            Pawn leadPawn = Server.CraftManager.FindPawn(client, request.CraftMainPawnID);
            List<Pawn> pawns = new List<Pawn> { leadPawn };
            pawns.AddRange(request.CraftSupportPawnIDList.Select(p => Server.CraftManager.FindPawn(client, p.PawnId)));
            List<uint> costPerformanceLevels = new List<uint>();

            foreach (Pawn pawn in pawns)
            {
                if (pawn != null)
                {
                    costPerformanceLevels.Add(CraftManager.GetPawnCostPerformanceLevel(pawn));
                }
                else
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_INVALID, "Couldn't find the Pawn ID.");
                }
            }

            CDataUpdateWalletPoint updateWalletPoint = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold,
                            Server.CraftManager.CalculateRecipeCost(totalCost, costPerformanceLevels));
            updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);
            client.Send(updateCharacterItemNtc);

            if (CraftManager.CanPawnExpUp(leadPawn))
            {
                double BonusExpMultiplier = Server.GpCourseManager.PawnCraftBonus();
                CraftManager.HandlePawnExpUpNtc(client, leadPawn, pawnExp, BonusExpMultiplier);
                if (CraftManager.CanPawnRankUp(leadPawn))
                {
                    CraftManager.HandlePawnRankUpNtc(client, leadPawn);
                }
            }
            else
            {
                CraftManager.HandlePawnExpUpNtc(client, leadPawn, 0, 0);
            }

            Server.Database.UpdatePawnBaseInfo(leadPawn);
            
            return res;
        }
    }
}
