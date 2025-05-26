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
    public class JobEmblemResetParamLevelHandler : GameRequestPacketQueueHandler<C2SJobEmblemResetParamLevelReq, S2CJobEmblemResetParamLevelRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobEmblemResetParamLevelHandler));

        public JobEmblemResetParamLevelHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SJobEmblemResetParamLevelReq request)
        {
            var packets = new PacketQueue();

            var updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.EmblemStatUpdate,
            };

            var emblemData = client.Character.JobEmblems[request.JobId];
            foreach (var statId in emblemData.StatLevels.Keys)
            {
                emblemData.StatLevels[statId] = 0;
            }
            emblemData.EmblemPointsUsed = Server.JobEmblemManager.CalculatedUsedPoints(emblemData);

            Server.Database.ExecuteInTransaction(connection =>
            {
                Server.Database.UpsertJobEmblemData(client.Character.CharacterId, emblemData, connection);

                foreach (var walletPoint in request.EmblemPointResetGGCostList)
                {
                    updateCharacterItemNtc.UpdateWalletList.Add(Server.WalletManager.RemoveFromWallet(client.Character, (WalletType)walletPoint.PointType, walletPoint.Amount, connection));
                }

                foreach (var playPoint in request.EmblemPointResetPPCostList)
                {
                    S2CJobUpdatePlayPointNtc playPointNtc = Server.PPManager.RemovePlayPoint2(client, request.JobId, playPoint.Amount, connectionIn: connection);
                    client.Enqueue(playPointNtc, packets);
                }
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

            client.Enqueue(new S2CJobEmblemResetParamLevelRes()
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
