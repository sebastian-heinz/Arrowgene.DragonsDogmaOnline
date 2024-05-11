using System;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Enemies;
using Arrowgene.Ddon.GameServer.GatheringItems;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;

public class InstanceEnemyManager : InstanceAssetManager<byte, Enemy, InstancedEnemy>
{
    // Taken from the pcaps. A few days before DDOn release. Wednesday, 26 August 2015 15:00:00
    private static readonly long ORIGINAL_REAL_TIME_SEC = 0x55DDD470;
    // defining how many hours there are in a day (24) This comes from the same pcaps
    private static readonly uint GAME_DAY_LENGTH = 24;
    // defining how long a full 24 hours cycle is in real world time, (90 minutes) This comes from the same pcaps
    private static readonly uint GAME_DAY_LENGTH_REAL_TIME = 90;

    private readonly AssetRepository _assetRepository;

    public InstanceEnemyManager(AssetRepository assetRepository) : base()
    {
        this._assetRepository = assetRepository;
    }

    protected override List<Enemy> FetchAssetsFromRepository(StageId stage, byte subGroupId)
    {
        return _assetRepository.EnemySpawnAsset.Enemies.GetValueOrDefault((stage, subGroupId)) ?? new List<Enemy>();
    }

    protected override List<InstancedEnemy> InstanceAssets(List<Enemy> originals)
    {
        List<InstancedEnemy> filteredEnemyList = new List<InstancedEnemy>();
        foreach(Enemy original in originals)
        {
            // Calculate current game time
            long gameTimeMSec = calcGameTimeMSec(DateTimeOffset.Now, ORIGINAL_REAL_TIME_SEC, GAME_DAY_LENGTH_REAL_TIME, GAME_DAY_LENGTH);
            
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

    // Magical game time calculation obtained from the PS4 client, can be found in ServerGameTimeGetBaseinfoHandler
    private long calcGameTimeMSec(DateTimeOffset realTime, long originalRealTimeSec, uint gameTimeOneDayMin, uint gameTimeDayHour)
    {
        long result = 1440 * (realTime.Millisecond + 1000 * (realTime.ToUnixTimeSeconds() - originalRealTimeSec)) / gameTimeOneDayMin
                    % (3600000 * gameTimeDayHour);
        return result;
    }
}
