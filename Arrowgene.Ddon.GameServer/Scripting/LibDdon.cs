using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
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
        public QuestUtils QuestFunctions { get; }

        private LibDdon()
        {
            QuestFunctions = new QuestUtils(this);
        }

        public static void SetServer(DdonGameServer server)
        {
            // TODO: How to block this after being set one time without breaking tests
            Instance.Server = server;
        }

        public static T GetSetting<T>(string scriptName, string key)
        {
            return Instance.Server.GameSettings.Get<T>(scriptName, key);
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
            var enemy = new InstancedEnemy((uint)enemyId, lv, exp, index);
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

        private Dictionary<string, object> HandlerCache = new Dictionary<string, object>();

        public static T GetHandler<T>()
        {
            string name = typeof(T).FullName;
            if (!Instance.HandlerCache.ContainsKey(name))
            {
                Instance.HandlerCache[name] = Activator.CreateInstance(typeof(T), Instance.Server);
            }
            return (T)Instance.HandlerCache[name];
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

        public static QuestUtils Quest()
        {
            return Instance.QuestFunctions;
        }

        public class QuestUtils
        {
            private LibDdon LibDdon;

            public QuestUtils(LibDdon libDdon)
            {
                LibDdon = libDdon;
            }

            public void ApplyTimeExtension(GameClient client, uint amount)
            {
                if (BoardManager.BoardIdIsExm(client.Party.ContentId) && amount > 0)
                {
                    var newEndTime = LibDdon.Server.PartyQuestContentManager.ExtendTimer(client.Party.Id, amount);
                    client.Party.SendToAll(new S2CQuestPlayAddTimerNtc() { PlayEndDateTime = newEndTime });
                }
            }
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
