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
using Arrowgene.Ddon.Shared.Csv;
using System.Diagnostics;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetEnemySetListHandler : StructurePacketHandler<GameClient, C2SInstanceGetEnemySetListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetEnemySetListHandler));
        private static readonly long ORIGINAL_REAL_TIME_SEC = 0x55DDD470; // Taken from the pcaps. A few days before DDOn release. Wednesday, 26 August 2015 15:00:00
        
        private long calcGameTimeMSec(DateTimeOffset realTime, long originalRealTimeSec, uint gameTimeOneDayMin, uint gameTimeDayHour)
        {
            long result = (1440 * (realTime.Millisecond + 1000 * (realTime.ToUnixTimeSeconds() - originalRealTimeSec)) / gameTimeOneDayMin)
                        % (3600000 * gameTimeDayHour);
            return result;
        }
        const int gameDayLength = 24;
        const int gameDayLengthRealTime = 90;
        private readonly EnemySpawnAssetDeserializer _deserializer; // Add this

        public InstanceGetEnemySetListHandler(DdonGameServer server) : base(server)
        {
            _deserializer = new EnemySpawnAssetDeserializer(); // Initialize the deserializer
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

                // Calculate current game time
                long gameTimeMSec = calcGameTimeMSec(DateTimeOffset.Now, ORIGINAL_REAL_TIME_SEC, gameDayLengthRealTime, gameDayLength);
                
                // Call the deserializer to get start and end milliseconds
                long startMilliseconds, endMilliseconds;
                _deserializer.ConvertSpawnTimeToMilliseconds(spawn.SpawnTime, out startMilliseconds, out endMilliseconds);
                
                Logger.Debug($"start and end milliseconds, {startMilliseconds} {endMilliseconds}");
                
                // If end < start, it spans past midnight and needs special range handling
                if(endMilliseconds < startMilliseconds)
                {
                    if(gameTimeMSec <= endMilliseconds // Morning range is 0 (midnight) to end time
                        || gameTimeMSec >= startMilliseconds) // Evening range is start time and onwards
                    {
                        response.EnemyList.Add(enemy);
                    }
                }
                else if(gameTimeMSec >= startMilliseconds && gameTimeMSec <= endMilliseconds)
                {
                    response.EnemyList.Add(enemy);
                }
            }

            client.Send(response);
        }
    }
}
