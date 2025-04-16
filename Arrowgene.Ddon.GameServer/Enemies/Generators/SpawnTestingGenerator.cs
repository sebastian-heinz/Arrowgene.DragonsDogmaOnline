using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Enemies.Generators
{
    public class SpawnTestingGenerator : IEnemySetGenerator
    {
        public static bool ToggledOn = false;

        private static readonly int MAX_ENEMIES = 20;

        public bool GetEnemySet(DdonGameServer server, GameClient client, StageLayoutId stageLayoutId, byte subGroupId, List<InstancedEnemy> instancedEnemySet, out QuestId questId)
        {
            if (!ToggledOn)
            {
                questId = 0;
                return false;
            }

            NamedParam dummyParam = server.AssetRepository.NamedParamAsset.GetValueOrDefault(1467u, NamedParam.DEFAULT_NAMED_PARAM);

            for (byte i = 0; i < MAX_ENEMIES; i++)
            {
                var dummyEnemy = new InstancedEnemy(EnemyId.DragonCrystalOfDarknessInvulnerableGround, (ushort)(stageLayoutId.GroupId * 100 + i), 0, i);
                dummyEnemy.Scale = 30;
                dummyEnemy.NamedEnemyParams = dummyParam;
                instancedEnemySet.Add(dummyEnemy);
            }
            questId = 0;
            return true;
        }
    }
}
