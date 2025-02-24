using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Linq;

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

        // TODO: Remove this function once Server singleton is created
        public static void LoadQuest(IQuest scriptedQuest)
        {
            QuestManager.LoadScriptedQuest(Instance.Server, scriptedQuest);
        }

        public static NamedParam GetNamedParam(uint paramId)
        {
            return Instance.Server.AssetRepository.NamedParamAsset.GetValueOrDefault(paramId, NamedParam.DEFAULT_NAMED_PARAM);
        }

        public static DropsTable GetDropsTable(uint enemyId, ushort lv)
        {
            return Instance.Server.AssetRepository.QuestDropItemAsset.GetDropTable(enemyId, lv);
        }

        public static InstancedEnemy CreateEnemy(EnemyId enemyId, ushort lv, uint exp, byte index, bool assignDefaultDrops = true)
        {
            var enemy = new InstancedEnemy((uint) enemyId, lv, exp, index);
            if (assignDefaultDrops)
            {
                enemy.DropsTable = LibDdon.GetDropsTable((uint)enemyId, lv);
            }
            return enemy;
        }

        public static InstancedEnemy CreateRandomEnemy(ushort lv, uint exp, byte index, List<EnemyId> enemyIds, bool assignDefaultDrops = true)
        {
            var dropTables = new Dictionary<EnemyId, DropsTable>();
            if (assignDefaultDrops)
            {
                foreach (var enemyId in enemyIds)
                {
                    dropTables.Add(enemyId, LibDdon.GetDropsTable((uint)enemyId, lv));
                }
            }
            return new InstancedRandomEnemy(enemyIds, dropTables, lv, exp, index);
        }

        public static T GetSetting<T>(string scriptName, string key)
        {
            return Instance.Server.GameLogicSettings.Get<T>(scriptName, key);
        }

        public static void SetSetting<T>(string scriptName, string key, T value)
        {
            Instance.Server.GameLogicSettings.Set(scriptName, key, value);
        }

        private Dictionary<string, object> HandlerCache = new Dictionary<string, object>();

        public static T GetHandler<T>()
        {
            string name = typeof(T).FullName;
            if (!Instance.HandlerCache.ContainsKey(name))
            {
                Instance.HandlerCache[name] = Activator.CreateInstance(typeof(T), Instance.Server);
            }
            return (T) Instance.HandlerCache[name];
        }

        public static IDatabase Database()
        {
            return Instance.Server.Database;
        }

        public static IGameItem GetGameItem(ItemId itemId)
        {
            return Instance.Server.ScriptManager.GameItemModule.GetItemInterface(itemId);
        }

        public static bool CharacterHasEquipped(CharacterCommon characterCommon, EquipType equipType, ItemId itemId)
        {
            return characterCommon.Equipment.GetItems(equipType).Exists(x => x?.ItemId == (uint)itemId);
        }

        public static bool CharacterHasEquipped(CharacterCommon characterCommon, List<EquipType> equipTypes, ItemId itemId)
        {
            foreach (var equipType in equipTypes)
            {
                if (CharacterHasEquipped(characterCommon, equipType, itemId))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CharacterHasCrestEquipped(CharacterCommon characterCommon, EquipType equipType, ItemId crestId)
        {
            return characterCommon.Equipment.GetItems(equipType).Exists(x => x.EquipElementParamList.Any(y => y?.CrestId == (int)crestId));
        }

        public static bool CharacterHasCrestEquipped(CharacterCommon characterCommon, List<EquipType> equipTypes, ItemId itemId)
        {
            foreach (var equipType in equipTypes)
            {
                if (CharacterHasCrestEquipped(characterCommon, equipType, itemId))
                {
                    return true;
                }
            }
            return false;
        }

        public static GpCourseManager GetCourseManager()
        {
            return Instance.Server.GpCourseManager;
        }
    }

    public static class InstancedEnemyUtils
    {
        public static InstancedEnemy SetNamedEnemyParams(this InstancedEnemy enemy, uint namedParamId)
        {
            return enemy.SetNamedEnemyParams(LibDdon.GetNamedParam(namedParamId));
        }
    }
}
