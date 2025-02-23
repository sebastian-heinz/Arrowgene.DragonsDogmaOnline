using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class LibDdon
    {
        private static readonly LibDdon Instance = new LibDdon();

        private static DdonGameServer Server { get; set; } = null;
        public static HandlerUtils Handler { get; private set; } = new HandlerUtils();
        public static ItemUtils Item { get; private set; } = new ItemUtils();
        public static QuestUtils Quest { get; private set; } = new QuestUtils();
        public static EnemyUtils Enemy { get; private set; } = new EnemyUtils();
        public static CharacterUtils Character { get; private set; } = new CharacterUtils();

        public static void SetServer(DdonGameServer server)
        {
            // TODO: How to block this after being set one time without breaking tests
            Server = server;
        }

        public static T GetSetting<T>(string scriptName, string key)
        {
            return Server.GameSettings.Get<T>(scriptName, key);
        }

        // TODO: Remove this function once Server singleton is created
        public static void LoadQuest(IQuest scriptedQuest)
        {
            QuestManager.LoadScriptedQuest(Server, scriptedQuest);
        }

        public static GpCourseManager GetCourseManager()
        {
            return Server.GpCourseManager;
        }

        public class HandlerUtils
        {
            private Dictionary<string, object> HandlerCache = new Dictionary<string, object>();

            public T Get<T>()
            {
                string name = typeof(T).FullName;
                if (!HandlerCache.ContainsKey(name))
                {
                    HandlerCache[name] = Activator.CreateInstance(typeof(T), Server);
                }
                return (T)HandlerCache[name];
            }
        }

        public class QuestUtils
        {
            public void ApplyTimeExtension(GameClient client, uint amount)
            {
                if (BoardManager.BoardIdIsExm(client.Party.ContentId) && amount > 0)
                {
                    var newEndTime = Server.PartyQuestContentManager.ExtendTimer(client.Party.Id, amount);
                    client.Party.SendToAll(new S2CQuestPlayAddTimerNtc() { PlayEndDateTime = newEndTime });
                }
            }
        }

        public class ItemUtils
        {
            public IGameItem GetItemInterface(ItemId itemId)
            {
                return Server.ScriptManager.GameItemModule.GetItemInterface(itemId);
            }
        }

        public class EnemyUtils
        {
            public NamedParam GetNamedParam(uint paramId)
            {
                return Server.AssetRepository.NamedParamAsset.GetValueOrDefault(paramId, NamedParam.DEFAULT_NAMED_PARAM);
            }

            public DropsTable GetDropsTable(uint enemyId, ushort lv)
            {
                return Server.AssetRepository.QuestDropItemAsset.GetDropTable(enemyId, lv);
            }

            public InstancedEnemy Create(EnemyId enemyId, ushort lv, uint exp, byte index, bool assignDefaultDrops = true)
            {
                var enemy = new InstancedEnemy((uint)enemyId, lv, exp, index);
                if (assignDefaultDrops)
                {
                    enemy.DropsTable = GetDropsTable((uint)enemyId, lv);
                }
                return enemy;
            }

            public InstancedEnemy Create(EnemyId enemyId, ushort lv, uint exp, bool assignDefaultDrops = true)
            {
                return Create(enemyId, lv, exp, 0, assignDefaultDrops);
            }

            public InstancedEnemy CreateRandom(ushort lv, uint exp, byte index, List<EnemyId> enemyIds, bool assignDefaultDrops = true)
            {
                var dropTables = new Dictionary<EnemyId, DropsTable>();
                if (assignDefaultDrops)
                {
                    foreach (var enemyId in enemyIds)
                    {
                        dropTables.Add(enemyId, GetDropsTable((uint)enemyId, lv));
                    }
                }
                return new InstancedRandomEnemy(enemyIds, dropTables, lv, exp, index);
            }

            public InstancedEnemy CreateRandom(ushort lv, uint exp, List<EnemyId> enemyIds, bool assignDefaultDrops = true)
            {
                return CreateRandom(lv, exp, 0, enemyIds, assignDefaultDrops);
            }
        }

        public class CharacterUtils
        {
            public bool HasEquipped(CharacterCommon characterCommon, EquipType equipType, ItemId itemId)
            {
                return characterCommon.Equipment.GetItems(equipType).Exists(x => x?.ItemId == (uint)itemId);
            }

            public bool HasEquipped(CharacterCommon characterCommon, List<EquipType> equipTypes, ItemId itemId)
            {
                foreach (var equipType in equipTypes)
                {
                    if (HasEquipped(characterCommon, equipType, itemId))
                    {
                        return true;
                    }
                }
                return false;
            }

            public bool HasCrestEquipped(CharacterCommon characterCommon, EquipType equipType, ItemId crestId)
            {
                return characterCommon.Equipment.GetItems(equipType).Exists(x => x.EquipElementParamList.Any(y => y?.CrestId == (int)crestId));
            }

            public bool HasCrestEquipped(CharacterCommon characterCommon, List<EquipType> equipTypes, ItemId itemId)
            {
                foreach (var equipType in equipTypes)
                {
                    if (HasCrestEquipped(characterCommon, equipType, itemId))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
