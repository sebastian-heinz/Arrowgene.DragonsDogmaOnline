using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobEmblemUpdateParamLevelHandler : GameRequestPacketQueueHandler<C2SJobEmblemUpdateParamLevelReq, S2CJobEmblemUpdateParamLevelRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobEmblemUpdateParamLevelHandler));

        public JobEmblemUpdateParamLevelHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SJobEmblemUpdateParamLevelReq request)
        {
            var packets = new PacketQueue();
            var updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.EmblemStatUpdate,
            };

            var emblemData = client.Character.JobEmblems[request.JobId];
            foreach (var param in request.UpdatedEmblemParamList)
            {
                emblemData.StatLevels[param.StatId] = param.Add;
            }
            emblemData.EmblemPointsUsed = Server.JobEmblemManager.CalculatedUsedPoints(emblemData);

            Server.Database.ExecuteInTransaction(connection =>
            {
                Server.Database.UpsertJobEmblemData(client.Character.CharacterId, emblemData, connection);
            });

            foreach (var uid in request.EmblemUIDs)
            {
                (StorageType storageType, Tuple<ushort, Item, uint> itemProps) = client.Character.Storage.FindItemByUIdInStorage(ItemManager.EquipmentStorages, uid);
                var (slotNo, item, amount) = itemProps;

                updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(client.Character, item, storageType, slotNo, 0, 0));
                item.EquipStatParamList = Server.JobEmblemManager.GetEquipStatParamList(emblemData);
                updateCharacterItemNtc.UpdateItemList.Add(Server.ItemManager.CreateItemUpdateResult(client.Character, item, storageType, slotNo, 1, 1));
            }
            client.Enqueue(updateCharacterItemNtc, packets);

            client.Enqueue(new S2CJobEmblemUpdateParamLevelRes()
            {
                JobId = request.JobId,
                EquipStatParamList = Server.JobEmblemManager.GetEquipStatParamList(emblemData),
                EmblemStatParamList = emblemData.GetEmblemStatParamList(),
                EmblemPoints = new CDataJobEmblemPoints()
                {
                    JobId = request.JobId,
                    Amount = Server.JobEmblemManager.GetAvailableEmblemPoints(emblemData),
                    MaxAmount = Server.JobEmblemManager.MaxEmblemPointsForLevel(emblemData)
                }
            }, packets);

            return packets;
        }
    }
}
