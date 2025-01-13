using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.GatheringItems.Generators
{
    internal class EpitaphRoadGatheringItemGenerator : IGatheringGenerator
    {
        private readonly DdonGameServer Server;

        public EpitaphRoadGatheringItemGenerator(DdonGameServer server)
        {
            Server = server;
        }

        public List<InstancedGatheringItem> Generate(GameClient client, StageId stageId, uint index)
        {   
            if (!StageManager.IsEpitaphRoadStageId(stageId))
            {
                return new(); 
            }
            return Server.EpitaphRoadManager.RollGatheringLoot(client, client.Character, stageId, index);
        }
    }
}
