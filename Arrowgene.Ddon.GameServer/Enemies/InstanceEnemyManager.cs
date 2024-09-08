using Arrowgene.Ddon.GameServer;
using Arrowgene.Ddon.GameServer.GatheringItems;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

public class InstanceEnemyManager : InstanceAssetManager<byte, Enemy, InstancedEnemy>
{
    private readonly DdonGameServer _Server;
    private Dictionary<StageId, ushort> _CurrentSubgroup { get; set; }

    private Dictionary<StageId, Dictionary<byte, InstancedEnemy>> _EnemyData;

    public InstanceEnemyManager(DdonGameServer server) : base()
    {
        _Server = server;
        _CurrentSubgroup  = new Dictionary<StageId, ushort>();
        _EnemyData = new Dictionary<StageId, Dictionary<byte, InstancedEnemy>>();
    }

    protected override List<Enemy> FetchAssetsFromRepository(StageId stage, byte setId)
    {
        // SetId is not used here, because the enemy data structure is flat, but the interface demands we have it.
        return _Server.AssetRepository.EnemySpawnAsset.Enemies.GetValueOrDefault((stage, (byte)0)) ?? new List<Enemy>();
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

    public void SetInstanceEnemy(StageId stageId, byte index, InstancedEnemy enemy)
    {
        lock (_EnemyData)
        {
            if (!_EnemyData.ContainsKey(stageId))
            {
                _EnemyData[stageId] = new Dictionary<byte, InstancedEnemy>();
            }

            if (!_EnemyData[stageId].ContainsKey(index))
            {
                _EnemyData[stageId][index] = enemy;
            }
        }
    }

    public InstancedEnemy GetInstanceEnemy(StageId stageId, byte index)
    {
        lock (_EnemyData)
        {
            if (!_EnemyData.ContainsKey(stageId))
            {
                return null;
            }

            if (!_EnemyData[stageId].ContainsKey(index))
            {
                return null;
            }
            return _EnemyData[stageId][index];
        }
    }

    public List<InstancedEnemy> GetInstancedEnemies(StageId stageId)
    {
        lock (_EnemyData)
        {
            if (!_EnemyData.ContainsKey(stageId))
            {
                return new List<InstancedEnemy>();
            }
            return _EnemyData[stageId].Select(x => x.Value).ToList();
        }
    }

    public bool HasInstanceEnemy(StageId stageId, byte index)
    {
        lock (_EnemyData)
        {
            if (!_EnemyData.ContainsKey(stageId))
            {
                return false;
            }
            return _EnemyData[stageId].ContainsKey(index);
        }
    }

    public void ResetEnemyNode(StageId stageId)
    {
        lock (_EnemyData)
        {
            if (_EnemyData.ContainsKey(stageId))
            {
                _EnemyData[stageId].Clear();
            }
        }
    }

    public override void Clear()
    {
        base.Clear();
        lock (_EnemyData)
        {
            _EnemyData.Clear();
        }
    }

    public ushort GetSubgroup(StageId stageId)
    {
        return _CurrentSubgroup.GetValueOrDefault(stageId);
    }
}
