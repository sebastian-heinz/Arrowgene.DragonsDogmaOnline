using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.GameServer.GatheringItems;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;

public class InstanceEnemyManager : InstanceAssetManager<byte, Enemy, InstancedEnemy>
{
    private readonly DdonGameServer _Server;
    private Dictionary<StageId, ushort> _CurrentSubgroup { get; set; }

    public InstanceEnemyManager(DdonGameServer server) : base()
    {
        _Server = server;
        _CurrentSubgroup  = new Dictionary<StageId, ushort>();
    }

    protected override List<Enemy> FetchAssetsFromRepository(StageId stage, byte subGroupId)
    {
        return _Server.AssetRepository.EnemySpawnAsset.Enemies.GetValueOrDefault((stage, subGroupId)) ?? new List<Enemy>();
    }

    protected override List<InstancedEnemy> InstanceAssets(List<Enemy> originals)
    {
        List<InstancedEnemy> filteredEnemyList = new List<InstancedEnemy>();

        // Calculate current game time
        long gameTimeMSec = _Server.WeatherManager.RealTimeToGameTimeMS(DateTimeOffset.UtcNow);

        foreach (Enemy original in originals)
        {
            // If end < start, it spans past midnight and needs special range handling
            if(original.SpawnTimeEnd < original.SpawnTimeStart)
            {
                // Morning range is 0 (midnight) to end time, Evening range is start time and onwards
                if(gameTimeMSec <= original.SpawnTimeEnd || gameTimeMSec >= original.SpawnTimeStart)
                {
                    filteredEnemyList.Add(new InstancedEnemy(original));
                }
            }
            else if(gameTimeMSec >= original.SpawnTimeStart && gameTimeMSec <= original.SpawnTimeEnd)
            {
                filteredEnemyList.Add(new InstancedEnemy(original));
            }
        }
        return filteredEnemyList;
    }

    public ushort GetInstanceSubgroupId(StageId stageId)
    {
        lock (_CurrentSubgroup)
        {
            if (!_CurrentSubgroup.ContainsKey(stageId))
            {
                return 0;
            }
            return _CurrentSubgroup[stageId];
        }
    }

    public void SetInstanceSubgroupId(StageId stageId, ushort subgroupId)
    {
        lock (_CurrentSubgroup)
        {
            _CurrentSubgroup[stageId] = subgroupId;
        }
    }

    public override void Clear()
    {
        base.Clear();
        lock (_CurrentSubgroup)
        {
            _CurrentSubgroup.Clear();
        }
    }
}
