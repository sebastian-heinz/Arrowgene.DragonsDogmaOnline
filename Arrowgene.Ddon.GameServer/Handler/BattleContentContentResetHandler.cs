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
            // Reset Inventory
            var updateItemList = RemoveAllItemsFromInventory(client.Character, client.Character.Storage, ItemManager.ItemBagStorageTypes);

            Server.Database.DeleteAllStorageItems(client.Character.ContentCharacterId);
            client.Character.Storage.Clear();

            // Remove items equipped in the database
            Server.Database.DeleteAllEquipItems(client.Character.CommonId);

            // Flush Storage
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.SwitchingStorage,
                UpdateItemList = updateItemList
            };
            client.Send(updateCharacterItemNtc);

            // Add back equipment templates
            client.Character.EquipmentTemplate = new EquipmentTemplate(Server.AssetRepository.BitterblackMazeAsset.StarterEquipment, Server.AssetRepository.BitterblackMazeAsset.JobEquipment);

            // Recreate starting items for player
            Server.Database.CreateItems(client.Character);

            // Add back equipment
            client.Character.Equipment = client.Character.Storage.GetCharacterEquipment();

            // Set current job back to level 1 stats
            var jobResults = Server.JobManager.SetJob(client, client.Character, client.Character.Job);
            client.Send((S2CJobChangeJobNtc) jobResults.jobNtc);

            // Reset EXP
            Server.ExpManager.ResetExpData(client, client.Character);

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

        private List<CDataItemUpdateResult> RemoveAllItemsFromInventory(Character character, Storages storages, List<StorageType> storageTypes)
        {
            var results = new List<CDataItemUpdateResult>();
            foreach (var storageType in storageTypes)
            {
                for (int i = 0; i < character.Storage.GetStorage(storageType).Items.Count; i++)
                {
                    ushort slotNo = (ushort)(i + 1);

                    var storageItem = storages.GetStorage(storageType).GetItem(slotNo);
                    if (storageItem != null)
                    {
                        results.Add(Server.ItemManager.CreateItemUpdateResult(null, storageItem.Item1, storageType, slotNo, 0, 0));
                        results.Add(Server.ItemManager.CreateItemUpdateResult(null, storageItem.Item1, storageType, slotNo, 0, storageItem.Item2));
                    }
                }
            }

            return results;
        }
    }
}
