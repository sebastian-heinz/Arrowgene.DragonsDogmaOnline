using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobEmblemDetachElementHandler : GameRequestPacketQueueHandler<C2SJobEmblemDetachElementReq, S2CJobEmblemDetachElementRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobEmblemDetachElementHandler));

        public JobEmblemDetachElementHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SJobEmblemDetachElementReq request)
        {
            var packets = new PacketQueue();

            var response = new S2CJobEmblemDetachElementRes()
            {
                InheritanceResult = new()
                {
                    JobId = request.JobId,
                    IsSuccess = true,
                }
            };

            var updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.EmblemStartDetach
            };
            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var uid in request.EmblemUIDs)
                {
                    var (storageType, itemProps) = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, uid);
                    var (slotNo, item, amount) = itemProps;

                    ushort relativeSlotNo = slotNo;
                    CharacterCommon characterCommon = client.Character;
                    if (storageType == StorageType.PawnEquipment)
                    {
                        uint pawnId = Storages.DeterminePawnId(client.Character, storageType, relativeSlotNo);
                        characterCommon = client.Character.Pawns.Where(x => x.PawnId == pawnId).SingleOrDefault()
                            ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_NOT_FOUNDED, "Unable to locate the pawn that has this emblem item equipped");
                        relativeSlotNo = EquipManager.DeterminePawnEquipSlot(relativeSlotNo);
                    }

                    updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, item, storageType, relativeSlotNo, 0, 0));
                    item.EquipElementParamList.Where(x => x.SlotNo == request.InheritanceSlot).FirstOrDefault().CrestId = 0;
                    Server.Database.RemoveCrest(characterCommon.CommonId, uid, request.InheritanceSlot, connection);
                    updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(characterCommon, item, storageType, relativeSlotNo, 1, 1));

                    // note: This might be set multiple times but should all be the same
                    response.InheritanceResult.EquipElementParamList = item.EquipElementParamList;
                }
            });
            client.Enqueue(response, packets);
            client.Enqueue(updateCharacterItemNtc, packets);

            return packets;
        }
    }
}
