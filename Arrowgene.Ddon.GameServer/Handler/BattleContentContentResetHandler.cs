using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentContentResetHandler : GameRequestPacketHandler<C2SBattleContentContentResetReq, S2CBattleContentContentResetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentContentResetHandler));
        
        //Helper function to get the list of items to send to the client after a reset
        private List<CDataItemUpdateResult> GetRefreshInventoryList(Character character, StorageType storageType) 
        {
            List<CDataItemUpdateResult> result = new List<CDataItemUpdateResult>();
            for (int i = 0; i < character.Storage.GetStorage(storageType).Items.Count; i++) 
            {
                ushort slotNo = (ushort)(i + 1);
                var storageItem = character.Storage.GetStorage(storageType).GetItem(slotNo);
                if(storageItem != null) 
                {
                    result.Add(Server.ItemManager.CreateItemUpdateResult(null, storageItem.Item1, storageType, slotNo, storageItem.Item2, storageItem.Item2));
                }
                else
                 {
                    Item item = new Item()
                    {
                        ItemId = 0,
                        UId = ""
                    };
                    result.Add(Server.ItemManager.CreateItemUpdateResult(null, item, storageType, slotNo, 0, 0));
                }
            }
            return result;
        }
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

                // Add starter job items for Bitterblack Maze characters
                if(client.Character.GameMode == GameMode.BitterblackMaze) 
                {
                    Server.Database.CreateListItems(connection, client.Character, StorageType.ItemBagJob, Server.AssetRepository.BitterblackMazeAsset.GenerateStarterJobItems());
                }
            });

            //job items added for Bitterblack Maze needs to be updated to the Client
            updateItemList.AddRange(GetRefreshInventoryList(client.Character, StorageType.ItemBagJob));
            
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
            jobResults.Send();

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
