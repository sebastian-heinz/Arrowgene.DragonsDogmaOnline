using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class LibDdon
    {
        private static readonly LibDdon Instance = new LibDdon();

        private DdonGameServer Server { get; set; } = null;

        private LibDdon()
        {
        }

        public static void SetServer(DdonGameServer server)
        {
            // TODO: How to block this after being set one time without breaking tests
            Instance.Server = server;
        }

        public static NamedParam GetNamedParam(uint paramId)
        {
            return Instance.Server.AssetRepository.NamedParamAsset.GetValueOrDefault(paramId, NamedParam.DEFAULT_NAMED_PARAM);
        }

        public static DropsTable GetDropsTable(uint enemyId, ushort lv)
        {
            return Instance.Server.AssetRepository.QuestDropItemAsset.GetDropTable(enemyId, lv);
        }

        public static InstancedEnemy CreateEnemy(uint enemyId, ushort lv, uint exp, byte index, bool assignDefaultDrops = true)
        {
            var enemy = new InstancedEnemy(enemyId, lv, exp, index);

            if (assignDefaultDrops)
            {
                enemy.DropsTable = LibDdon.GetDropsTable(enemyId, lv);
            }

            return enemy;
        }
    }
}
