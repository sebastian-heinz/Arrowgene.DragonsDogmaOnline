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
    public class CraftStartAttachElementHandler : GameRequestPacketQueueHandler<C2SCraftStartAttachElementReq, S2CCraftStartAttachElementRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftStartAttachElementHandler));

        public CraftStartAttachElementHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SCraftStartAttachElementReq request)
        {
            PacketQueue queue = new();
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();

            var (storageType, itemProps) = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, request.EquipItemUId);
            var (slotNo, item, amount) = itemProps;

            ClientItemInfo clientItemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, item.ItemId);
            var result = new S2CCraftStartAttachElementRes();

            ushort relativeSlotNo = slotNo;
            CharacterCommon characterCommon = null;
            if (storageType == StorageType.CharacterEquipment)
            {
                characterCommon = client.Character;
                result.CurrentEquip.EquipSlot.CharacterId = client.Character.CharacterId;
                result.CurrentEquip.EquipSlot.PawnId = 0;
            }
            else if (storageType == StorageType.PawnEquipment)
            {
                uint pawnId = Storages.DeterminePawnId(client.Character, storageType, relativeSlotNo);
                characterCommon = client.Character.Pawns.Where(x => x.PawnId == pawnId).SingleOrDefault();
                relativeSlotNo = EquipManager.DeterminePawnEquipSlot(relativeSlotNo);
                result.CurrentEquip.EquipSlot.CharacterId = 0;
                result.CurrentEquip.EquipSlot.PawnId = pawnId;
            }

            if (storageType == StorageType.CharacterEquipment || storageType == StorageType.PawnEquipment)
            {
                result.CurrentEquip.EquipSlot.EquipSlotNo = EquipManager.DetermineEquipSlot(relativeSlotNo);
                result.CurrentEquip.EquipSlot.EquipType = EquipManager.GetEquipTypeFromSlotNo(relativeSlotNo);
            }

            var craftInfo = Server.AssetRepository.CostExpScalingAsset.GetScalingInfo(clientItemInfo.Rank);
            uint totalCost = (uint)(craftInfo.Cost * request.CraftElementList.Count);
            uint pawnExp = (uint)(craftInfo.Exp * request.CraftElementList.Count);

            updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, item, storageType, relativeSlotNo, 0, 0));
            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var element in request.CraftElementList)
                {
                    uint crestId = Server.ItemManager.LookupItemByUID(Server, element.ItemUId);

                    Server.Database.InsertCrest(client.Character.CommonId, request.EquipItemUId, element.SlotNo, crestId, 0, connection);
                    result.EquipElementParamList.Add(new CDataEquipElementParam()
                    {
                        CrestId = crestId,
                        SlotNo = element.SlotNo,
                    });

                    item.EquipElementParamList.Add(new CDataEquipElementParam()
                    {
                        CrestId = crestId,
                        SlotNo = element.SlotNo,
                    });

                    // Consume the crest
                    updateCharacterItemNtc.UpdateItemList.AddRange(Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, element.ItemUId, 1, connection));
                }

                Pawn leadPawn = Server.CraftManager.FindPawn(client, request.CraftMainPawnId);
                List<CraftPawn> craftPawns = new()
                {
                    new CraftPawn(leadPawn, CraftPosition.Leader)
                };
                craftPawns.AddRange(request.CraftSupportPawnIDList.Select(p => new CraftPawn(Server.CraftManager.FindPawn(client, p.PawnId), CraftPosition.Assistant)));

                uint cost = Server.CraftManager.CalculateRecipeCost(totalCost, clientItemInfo, craftPawns);
                updateCharacterItemNtc.UpdateType = ItemNoticeType.StartAttachElement;
                CDataUpdateWalletPoint updateWalletPoint = Server.WalletManager.RemoveFromWallet(client.Character, WalletType.Gold, cost, connection)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_CRAFT_INSUFFICIENT_GOLD, $"Insufficient gold. {cost} > {Server.WalletManager.GetWalletAmount(client.Character, WalletType.Gold)}");

                updateCharacterItemNtc.UpdateWalletList.Add(updateWalletPoint);
                updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, item, storageType, relativeSlotNo, 1, 1));
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

                queue.AddRange(Server.AchievementManager.HandleMountCrest(client, connection));
            });

            client.Enqueue(result, queue);
            return queue;
        }
    }
}
