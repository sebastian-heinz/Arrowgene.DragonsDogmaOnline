using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class LibDdon
    {
        private LibDdon()
        {
        }

        private static DdonGameServer Server { get; set; } = null;
        public static LibDdonItemUtils Item { get; private set; } = new LibDdonItemUtils();
        public static LibDdonQuestUtils Quest { get; private set; } = new LibDdonQuestUtils();
        public static LibDdonEnemyUtils Enemy { get; private set; } = new LibDdonEnemyUtils();
        public static LibDdonCharacterUtils Character { get; private set; } = new LibDdonCharacterUtils();
        public static LibDdonTimeUtils GameTime { get; private set; } = new LibDdonTimeUtils();
        public static LibDdonCraftUtils Crafting { get; private set; } = new LibDdonCraftUtils();
        public static LibDdonChatUtils ChatMgr { get; private set; } = new LibDdonChatUtils();
        public static AssetRepository Assets
        { 
            get
            {
                return Server.AssetRepository;
            }
        }

        public static void SetServer(DdonGameServer server)
        {
            // TODO: How to block this after being set one time without breaking tests
            Server = server;
        }

        public static T GetSetting<T>(string scriptName, string key)
        {
            return Server.GameSettings.Get<T>(scriptName, key);
        }

        private static Dictionary<string, object> HandlerCache = new Dictionary<string, object>();
        public static T GetHandler<T>()
        {
            string name = typeof(T).FullName;
            if (!HandlerCache.ContainsKey(name))
            {
                HandlerCache[name] = Activator.CreateInstance(typeof(T), Server);
            }
            return (T)HandlerCache[name];
        }

        // TODO: Remove this function once Server singleton is created
        public static void LoadQuest(IQuest scriptedQuest)
        {
            QuestManager.LoadScriptedQuest(Server, scriptedQuest);
        }

        public static void RegenerateQuest(Quest quest)
        {
            LoadQuest(quest.BackingObject);
        }

        public static GpCourseManager GetCourseManager()
        {
            return Server.GpCourseManager;
        }

        public static EpitaphRoadManager GetEpitaphRoadManager()
        {
            return Server.EpitaphRoadManager;
        }

        public static List<IMonsterSpotInfo> GetCautionSpots()
        {
            var result = new List<IMonsterSpotInfo>();
            foreach (var item in Server.ScriptManager.MonsterCautionSpotModule.EnemyGroups)
            {
                result.AddRange(item.Value);
            }
            return result;
        }

        public class LibDdonQuestUtils
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

        public class LibDdonItemUtils
        {
            public IGameItem GetItemInterface(ItemId itemId)
            {
                return Server.ScriptManager.GameItemModule.GetItemInterface(itemId);
            }

            public ClientItemInfo GetClientItemInfo(ItemId itemId)
            {
                return Server.ItemManager.LookupInfoByItemID(Server, (uint) itemId);
            }

            public GatheringItem CreateDropItem(ItemId itemId, uint minAmount, uint maxAmount, double dropChance)
            {
                return new GatheringItem()
                {
                    ItemId = itemId,
                    ItemNum = minAmount,
                    MaxItemNum = maxAmount,
                    DropChance = dropChance
                };
            }
        }

        public class LibDdonEnemyUtils
        {
            public NamedParam GetNamedParam(uint paramId)
            {
                return Server.AssetRepository.NamedParamAsset.GetValueOrDefault(paramId, NamedParam.DEFAULT_NAMED_PARAM);
            }

            public DropsTable GetDropsTable(EnemyId enemyId, ushort lv)
            {
                return Server.AssetRepository.QuestDropItemAsset.GetDropTable((uint)enemyId, lv);
            }

            public DropsTable GetDropsTable(InstancedEnemy enemy)
            {
                return GetDropsTable((EnemyId)enemy.EnemyId, enemy.Lv);
            }

            public InstancedEnemy Create(EnemyId enemyId, ushort lv, uint exp, byte index, bool assignDefaultDrops = true)
            {
                var enemy = new InstancedEnemy(enemyId, lv, exp, index);
                if (assignDefaultDrops)
                {
                    enemy.DropsTable = GetDropsTable(enemyId, lv);
                }

                return enemy;
            }

            public InstancedEnemy Create(EnemyId enemyId, ushort lv, uint exp, bool assignDefaultDrops = true)
            {
                return Create(enemyId, lv, exp, 0, assignDefaultDrops);
            }

            public InstancedEnemy CreateAuto(EnemyId enemyId, ushort lv, byte index, bool isBoss = false, bool assignDefaultDrops = true, EnemyExpScheme scheme = EnemyExpScheme.Automatic)
            {
                return Create(enemyId, lv, 0, index, assignDefaultDrops)
                    .SetIsBoss(isBoss)
                    .SetExpScheme(scheme);
            }

            public InstancedEnemy CreateAuto(EnemyId enemyId, ushort lv, bool isBoss = false, bool assignDefaultDrops = true, EnemyExpScheme scheme = EnemyExpScheme.Automatic)
            {
                return CreateAuto(enemyId, lv, 0, isBoss, assignDefaultDrops, scheme);
            }


            public InstancedEnemy CreateRandom(ushort lv, uint exp, byte index, List<(EnemyId EnemyId, uint NamedParamId)> enemies, bool assignDefaultDrops = true)
            {
                var dropTables = new Dictionary<EnemyId, DropsTable>();
                
                foreach (var enemyParams in enemies)
                {
                    if (assignDefaultDrops)
                    {
                        dropTables.Add(enemyParams.EnemyId, GetDropsTable(enemyParams.EnemyId, lv));
                    }
                    else
                    {
                        dropTables.Add(enemyParams.EnemyId, new());
                    }
                }

                // Convert Id's into object with stats
                var updatedList = enemies.Select(x => (x.EnemyId, GetNamedParam(x.NamedParamId))).ToList();
                return new InstancedRandomEnemy(updatedList, dropTables, lv, exp, index);
            }

            public InstancedEnemy CreateRandom(ushort lv, uint exp, byte index, List<EnemyId> enemyIds, bool assignDefaultDrops = true)
            {
                var enemies = enemyIds.Select(x => (x, 2298u)).ToList();
                return CreateRandom(lv, exp, index, enemies, assignDefaultDrops);
            }

            public InstancedEnemy CreateRandom(ushort lv, uint exp, List<EnemyId> enemyIds, bool assignDefaultDrops = true)
            {
                return CreateRandom(lv, exp, 0, enemyIds, assignDefaultDrops);
            }
        }

        public class LibDdonCharacterUtils
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

        public class LibDdonTimeUtils
        {
            public long GetCurrentGameTime()
            {
                return Server.GameTimeManager.GameTimeNow();
            }

            public bool IsNightTime()
            {
                return Server.GameTimeManager.IsNightTime();
            }
            public bool IsDayTime()
            {
                return Server.GameTimeManager.IsDayTime();
            }

            public long ConvertToGameTime(string time)
            {
                return Server.GameTimeManager.ConvertTimeToMilliseconds(time);
            }
        }

        public class LibDdonCraftUtils
        {
            public CraftManager GetCraftManager()
            {
                return Server.CraftManager;
            }
        }

        public class LibDdonChatUtils
        {
            public void SendMessageToParty(GameClient client, LobbyChatMsgType msgType, string message)
            {
                Server.ChatManager.BroadcastMessageToParty(client.Party, msgType, message);
            }

            public void SendMessageToPlayer(GameClient client, LobbyChatMsgType msgType, string message)
            {
                Server.ChatManager.SendMessage(message, string.Empty, string.Empty, msgType, new List<GameClient>() { client });
            }
        }
    }
}
