using System.Collections.Generic;
using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetEnemySetListHandler : StructurePacketHandler<GameClient, C2SInstanceGetEnemySetListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetEnemySetListHandler));
        
        private long calcGameTimeMSec(DateTimeOffset realTime, long originalRealTimeSec, uint gameTimeOneDayMin, uint gameTimeDayHour)
        {
            long result = (1440 * (realTime.Millisecond + 1000 * (realTime.ToUnixTimeSeconds() - originalRealTimeSec)) / gameTimeOneDayMin)
                        % (3600000 * gameTimeDayHour);
            return result;
        }


        public InstanceGetEnemySetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetEnemySetListReq> request)
        {
            StageId stageId = StageId.FromStageLayoutId(request.Structure.LayoutId);
            byte subGroupId = request.Structure.SubGroupId;
            client.Character.Stage = stageId;

            List<Enemy> spawns = Server.AssetRepository.EnemySpawnAsset.Enemies.GetValueOrDefault((stageId, subGroupId)) ?? new List<Enemy>();
            
            // TODO test
            // spawns.AddRange(_enemyManager.GetSpawns(new StageId(1,0,15), 0));

            S2CInstanceGetEnemySetListRes response = new S2CInstanceGetEnemySetListRes();
            response.LayoutId = stageId.ToStageLayoutId();
            response.SubGroupId = subGroupId;
            response.RandomSeed = CryptoRandom.Instance.GetRandomUInt32();

            for (byte i = 0; i < spawns.Count; i++)
            {
                Enemy spawn = spawns[i];
                CDataLayoutEnemyData enemy = new CDataLayoutEnemyData
                {
                    PositionIndex = i,
                    EnemyInfo = spawn.asCDataStageLayoutEnemyPresetEnemyInfoClient()
                };
                // Get spawn time from the enemy object
                string spawnTime = spawn.SpawnTime;
                long startMilliseconds, endMilliseconds;
                ConvertSpawnTimeToMilliseconds(spawnTime, out startMilliseconds, out endMilliseconds);

                // Calculate current game time
                long gameTimeMSec = calcGameTimeMSec(DateTimeOffset.Now, 0x55DDD470, 90, 24);
                
                long dayMilliseconds = 3600000 * 24;
                // Check if the current game time falls within the spawn time range
                if (endMilliseconds < startMilliseconds)
                {
                    endMilliseconds = endMilliseconds + dayMilliseconds;
                    if (gameTimeMSec <= startMilliseconds && gameTimeMSec <= endMilliseconds)
                    {
                        response.EnemyList.Add(enemy);
                    }
                }

                if (gameTimeMSec >= startMilliseconds && gameTimeMSec <= endMilliseconds)
                {
                    // The enemy is allowed to spawn
                    response.EnemyList.Add(enemy);
                    Logger.Debug("this funky boi lives");
                }
                else
                {
                    // The enemy is not allowed to spawn
                    Logger.Debug("tard couldn't spawn");
                }
            }

            client.Send(response);
        }

        private void ConvertSpawnTimeToMilliseconds(string spawnTime, out long startMilliseconds, out long endMilliseconds)
        {
            // Split the spawnTime string at the comma to get start and end times
            string[] spawnTimes = spawnTime.Split(',');

            // Split the start time at the colon to get hours and minutes
            string[] startTimeComponents = spawnTimes[0].Split(':');
            int startHours = int.Parse(startTimeComponents[0]);
            int startMinutes = int.Parse(startTimeComponents[1]);

            // Split the end time at the colon to get hours and minutes
            string[] endTimeComponents = spawnTimes[1].Split(':');
            int endHours = int.Parse(endTimeComponents[0]);
            int endMinutes = int.Parse(endTimeComponents[1]);

            // Convert hours and minutes into milliseconds
            startMilliseconds = (startHours * 3600000) + (startMinutes * 60000);
            endMilliseconds = (endHours * 3600000) + (endMinutes * 60000);
        }
    }
}
