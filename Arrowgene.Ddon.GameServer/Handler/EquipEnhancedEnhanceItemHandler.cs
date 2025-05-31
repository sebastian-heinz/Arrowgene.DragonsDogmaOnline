using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.GameServer.Shop;
using Arrowgene.Ddon.GameServer.Utils;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EquipEnhancedEnhanceItemHandler : GameRequestPacketQueueHandler<C2SEquipEnhancedEnhanceItemReq, S2CEquipEnhancedEnhanceItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipEnhancedEnhanceItemHandler));

        public EquipEnhancedEnhanceItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SEquipEnhancedEnhanceItemReq request)
        {
            PacketQueue packets = new();

            var updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();

            var category = Server.AssetRepository.LimitBreakAsset.GetCategoryForIndex(request.CategoryIndex) ??
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CRAFT_RECIPE_CATEGORY_UNKNOWN, $"Failed to locate the category listing '{request.CategoryIndex}' used in the limit break lottery");

            var (storageType, itemProps) = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, request.UpgradeItemUID);
            var (slotNo, item, amount) = itemProps;

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var itemCost in request.UpgradeItemCostList)
                {
                    updateCharacterItemNtc.UpdateItemList.AddRange(Server.ItemManager.ConsumeItemByIdFromMultipleStorages(Server, client.Character, ItemManager.AllItemStorages, (uint) itemCost.ItemId, itemCost.Amount, connection));
                }

                bool forceGreatSuccess = false;
                foreach (var walletCost in request.UpgradeWalletCost)
                {
                    updateCharacterItemNtc.UpdateWalletList.Add(Server.WalletManager.RemoveFromWallet(client.Character, walletCost.Type, walletCost.Value, connection));

                    forceGreatSuccess |= category.PremiumCurrencies.Contains(walletCost.Type);
                }

                var statRolls = category.StatLottery.OrderBy(x => Random.Shared.Next()).First();
                var statRoll = forceGreatSuccess ?
                    statRolls.Rolls[Random.Shared.Next((int) statRolls.MinGreatSuccessIndex, statRolls.Rolls.Count)] :
                    statRolls.Rolls.GetWeightedRandomElement(Server.GameSettings.GameServerSettings.EquipmentLimitBreakBias);

                var newAddStatusParam = new CDataAddStatusParam()
                {
                    AdditionalStatus1 = statRoll,
                    IsAddStat1 = true
                };

                Server.Database.UpsertEquipmentLimitBreakRecord(client.Character.CharacterId, item.UId, newAddStatusParam, connection);

                item.AddStatusParamList.Clear();
                item.AddStatusParamList.Add(newAddStatusParam);

                ushort relativeSlotNo = slotNo;
                CharacterCommon characterCommon = client.Character;
                if (storageType == StorageType.PawnEquipment)
                {
                    uint pawnId = Storages.DeterminePawnId(client.Character, storageType, relativeSlotNo);
                    characterCommon = client.Character.Pawns.Where(x => x.PawnId == pawnId).SingleOrDefault()
                        ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_NOT_FOUNDED, "Unable to locate the pawn that has this emblem item equipped");
                    relativeSlotNo = EquipManager.DeterminePawnEquipSlot(relativeSlotNo);
                }

                updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, item, storageType, relativeSlotNo, 1, 1));
                updateCharacterItemNtc.UpdateType = ItemNoticeType.GatherEquipItem;
                
                packets.Enqueue(client, updateCharacterItemNtc);

                packets.Enqueue(client, new S2CEquipEnhancedEnhanceItemRes()
                {
                    Unk0 = request.UpgradeItemUID,
                    Unk1 = 0,
                    Unk2 = 0,
                    Unk4 = 0,
                    Unk3 = 0,
                });
            });

            return packets;
        }
    }
}
