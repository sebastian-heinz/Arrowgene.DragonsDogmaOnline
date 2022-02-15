using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Enemy;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetEnemySetListHandler : StructurePacketHandler<GameClient, C2SInstanceGetEnemySetListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetEnemySetListHandler));

        private readonly EnemyManager _enemyManager;

        public InstanceGetEnemySetListHandler(DdonGameServer server) : base(server)
        {
            _enemyManager = server.EnemyManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetEnemySetListReq> request)
        {
            // Hidell plains seems to be stageID 1, layerNo 0, and then every spawn is a different groupID
            StageId stageId = StageId.FromStageLayoutId(request.Structure.LayoutId);
            byte subGroupId = request.Structure.SubGroupId;
            client.Stage = stageId;

            List<EnemySpawn> spawns = _enemyManager.GetSpawns(stageId, subGroupId);


            S2CInstanceGetEnemySetListRes response = new S2CInstanceGetEnemySetListRes();
            response.LayoutId = stageId.ToStageLayoutId();
            response.SubGroupId = subGroupId;
            response.RandomSeed = CryptoRandom.Instance.GetRandomUInt32();

            for (byte i = 0; i < spawns.Count; i++)
            {
                EnemySpawn spawn = spawns[i];
                CDataLayoutEnemyData enemy = new CDataLayoutEnemyData();
                enemy.PositionIndex = i;
                enemy.EnemyInfo = spawn.Enemy;
                response.EnemyList.Add(enemy);
            }

            client.Send(response);
        }
    }
}
