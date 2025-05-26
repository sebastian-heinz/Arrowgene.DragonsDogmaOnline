using Arrowgene.Ddon.GameServer.Characters;
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
    public class JobEmblemAttachElementHandler : GameRequestPacketQueueHandler<C2SJobEmblemAttachElementReq, S2CJobEmblemAttachElementRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobEmblemAttachElementHandler));

        public JobEmblemAttachElementHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SJobEmblemAttachElementReq request)
        {
            var packets = new PacketQueue();
            var itemUpdateNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.EmblemStartAttach
            };

            if (request.EmblemUIDs.Count == 0)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INTERNAL_ERROR, $"Expected at least 1 emblem UID but found none");
            }

            var (storageType, storageInfo) = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, request.EmblemUIDs[0]);
            var emblemItem = storageInfo.Item2;

            var inheritanceChance = Server.JobEmblemManager.GetInheritanceChance(emblemItem, request.AttachChanceItems);
            bool isSuccess = (Random.Shared.NextDouble() <= inheritanceChance);
            bool isItemLost = !(request.PremiumCurrencyCost.Count > 0);
            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var item in request.AttachChanceItems)
                {
                    itemUpdateNtc.UpdateItemList.AddRange(Server.ItemManager.ConsumeItemByIdFromMultipleStorages(Server, client.Character, ItemManager.BothStorageTypes, (uint) item.ItemId, item.Num, connection));
                }

                foreach (var currency in request.PremiumCurrencyCost)
                {
                    itemUpdateNtc.UpdateWalletList.Add(Server.WalletManager.RemoveFromWallet(client.Character, (WalletType) currency.PointType, currency.Amount, connection));
                }

                if (isSuccess)
                {
                    var crests = request.JewelryUIDs.Select(x => client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, x).Item2.Item2.EquipElementParamList)
                        .SelectMany(x => x)
                        .ToList();
                    if (crests.Count == 0)
                    {
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INTERNAL_ERROR, "Failed to locate crest ids for emblem inheritance");
                    }

                    // Select a random crest to inherit if there are multiple
                    var crest = crests[Random.Shared.Next(0, crests.Count)];
                    foreach (var uid in request.EmblemUIDs)
                    {
                        var (storageType, storageInfo) = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, uid);
                        var (slotNo, item, _) = storageInfo;

                        ushort relativeSlotNo = slotNo;
                        CharacterCommon characterCommon = client.Character;
                        if (storageType == StorageType.PawnEquipment)
                        {
                            uint pawnId = Storages.DeterminePawnId(client.Character, storageType, relativeSlotNo);
                            characterCommon = client.Character.Pawns.Where(x => x.PawnId == pawnId).SingleOrDefault()
                                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_NOT_FOUNDED, "Unable to locate the pawn that has this emblem item equipped");
                            relativeSlotNo = EquipManager.DeterminePawnEquipSlot(relativeSlotNo);
                        }

                        var match = item.EquipElementParamList.Where(x => x.SlotNo == request.InheritanceSlot).FirstOrDefault();
                        if (match == null)
                        {
                            item.EquipElementParamList.Add(new CDataEquipElementParam()
                            {
                                SlotNo = request.InheritanceSlot,
                                CrestId = crest.CrestId,
                                Add = crest.Add
                            });
                        }
                        else
                        {
                            match.CrestId = crest.CrestId;
                            match.Add = crest.Add;
                        }

                        Server.Database.InsertCrest(characterCommon.CommonId, uid, request.InheritanceSlot, crest.CrestId, crest.Add, connection);
                        itemUpdateNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, item, storageType, relativeSlotNo, 1, 1));
                    }
                }

                if (isSuccess || (!isSuccess && isItemLost))
                {
                    foreach (var itemUID in request.JewelryUIDs)
                    {
                        itemUpdateNtc.UpdateItemList.AddRange(Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, ItemManager.EquipmentStorages, itemUID, 1, connection));
                    }
                }
            });

            client.Enqueue(new S2CJobEmblemAttachElementRes()
            {
                InheritanceResult = new()
                {
                    JobId = request.JobId,
                    IsSuccess = isSuccess,
                    IsItemLost = isItemLost,
                    EquipElementParamList = emblemItem.EquipElementParamList,
                },
                ItemUpdateResultList = itemUpdateNtc.UpdateItemList,
                UpdateWalletPointList = itemUpdateNtc.UpdateWalletList,
            }, packets);
            client.Enqueue(itemUpdateNtc, packets);

            return packets;
        }
    }
}
