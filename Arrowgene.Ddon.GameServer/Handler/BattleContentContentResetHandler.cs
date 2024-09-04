using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Csv;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentContentResetHandler : GameRequestPacketHandler<C2SBattleContentContentResetReq, S2CBattleContentContentResetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentContentResetHandler));

        public BattleContentContentResetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentContentResetRes Handle(GameClient client, C2SBattleContentContentResetReq request)
        {
            // Add back equipment templates
            client.Character.EquipmentTemplate = new EquipmentTemplate(Server.AssetRepository.BitterblackMazeAsset.GenerateStarterEquipment(), Server.AssetRepository.BitterblackMazeAsset.GenerateStarterJobEquipment());

            List<CDataItemUpdateResult> updateItemList = null;
            Server.Database.ExecuteInTransaction(connection =>
            {
                // Remove all items from the player inventory
                updateItemList = Server.ItemManager.RemoveAllItemsFromInventory(client.Character, client.Character.Storage, ItemManager.AllItemStorages, connection);

                // Remove items equipped in the database
                Server.Database.DeleteAllEquipItems(client.Character.CommonId, connection);

                // Recreate starting items for player
                Server.Database.CreateItems(connection, client.Character);
            });

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.SwitchingStorage,
                UpdateItemList = updateItemList
            };
            client.Send(updateCharacterItemNtc);

            // Add back equipment
            client.Character.Equipment = client.Character.Storage.GetCharacterEquipment();

            // Reset EXP
            Server.ExpManager.ResetExpData(client, client.Character);

            // Set current job back to level 1 stats
            var jobResults = Server.JobManager.SetJob(client, client.Character, client.Character.Job);
            foreach (var otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Send((S2CJobChangeJobNtc)jobResults.jobNtc);
            }
            client.Send((S2CJobChangeJobNtc)jobResults.jobNtc);
            client.Send((S2CItemUpdateCharacterItemNtc)jobResults.itemNtc);

            // Reset progress
            client.Character.BbmProgress.StartTime = 0;
            client.Character.BbmProgress.ContentId = 0;
            client.Character.BbmProgress.Tier = 0;
            client.Character.BbmProgress.KilledDeath = false;
            Server.Database.UpdateBBMProgress(client.Character.CharacterId, client.Character.BbmProgress);

            // Update the situation information
            S2CBattleContentProgressNtc ntc2 = new S2CBattleContentProgressNtc();
            ntc2.BattleContentStatusList.Add(BitterblackMazeManager.GetUpdatedContentStatus(Server, client.Character));
            client.Send(ntc2);

            return new S2CBattleContentContentResetRes();
        }
    }
}
