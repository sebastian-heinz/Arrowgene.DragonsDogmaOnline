using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Core.Tokens;
using static Arrowgene.Ddon.GameServer.Characters.ExmManager;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class ExmManager
    {
        private DdonGameServer _Server;

        private Dictionary<ulong, CDataEntryItem> _ContentData;
        private Dictionary<uint, ulong> _CharacterIdToContentId;
        private Dictionary<ulong, HashSet<uint>> _ContentIdToCharacterIds;
        private Dictionary<ulong, Quest> _ContentIdToQuest;
        private Dictionary<ulong, TimerState> _ContentTimers;
        private uint EntryItemIdCounter;
        private Stack<uint> _FreeEntryItemIds;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ExmManager));

        internal class TimerState
        {
            public DateTime TimeStart {  get; set; }
            public TimeSpan Duration {  get; set; }
            public Timer Timer { get; set; }
        }

        public ExmManager(DdonGameServer server)
        {
            _Server = server;
            _ContentData = new Dictionary<ulong, CDataEntryItem>();
            _CharacterIdToContentId = new Dictionary<uint, ulong>();
            _ContentIdToCharacterIds = new Dictionary<ulong, HashSet<uint>>();
            _ContentIdToQuest = new Dictionary<ulong, Quest>();
            _ContentTimers = new Dictionary<ulong, TimerState>();

            // ID tracking
            EntryItemIdCounter = 1;
            _FreeEntryItemIds = new Stack<uint>();
            _FreeEntryItemIds.Push(EntryItemIdCounter);
        }

        public bool HasContentId(ulong contentId)
        {
            lock (_ContentData)
            {
                return _ContentData.ContainsKey(contentId);
            }
        }

        public List<CDataEntryItem> GetListOfRecruitingContent()
        {
            lock (_ContentData)
            {
                return _ContentData.Values.ToList();
            }
        }

        public uint NumGroupsRecruitingForContent(uint questId)
        {
            // TODO: Implement mechanism to search for this
            return 0;
        }

        public uint NumGroupsRecruitingForContent(QuestId questId)
        {
            return NumGroupsRecruitingForContent((uint)questId);
        }

        public bool CreateGroupForContent(ulong id, CDataEntryItem entryItem)
        {
            lock (_ContentData)
            {
                if (_ContentData.ContainsKey(id))
                {
                    // Group is already registered
                    return false;
                }

                _ContentData[id] = entryItem;

                return true;
            }
        }

        public CDataEntryItem GetEntryItemDataForContent(ulong id)
        {
            lock (_ContentData)
            {
                if (!_ContentData.ContainsKey(id))
                {
                    return null;
                }
                return _ContentData[id];
            }
        }

        public CDataEntryItem GetEntryItemDataForCharacter(uint characterId)
        {
            lock (_ContentData)
            {
                ulong id = _CharacterIdToContentId[characterId];
                return GetEntryItemDataForContent(id);
            }
        }

        public CDataEntryItem GetEntryItemDataForCharacter(Character character)
        {
            return GetEntryItemDataForCharacter(character.CharacterId);
        }

        public bool RemoveGroupForContent(ulong contentId)
        {
            lock (_ContentData)
            {
                if (!_ContentData.ContainsKey(contentId))
                {
                    return false;
                }

                if (_ContentTimers.ContainsKey(contentId))
                {
                    CancelTimer(contentId);
                }

                if (_ContentIdToCharacterIds.ContainsKey(contentId))
                {
                    foreach (var characterId in _ContentIdToCharacterIds[contentId])
                    {
                        _CharacterIdToContentId.Remove(characterId);
                    }
                    _ContentIdToCharacterIds.Remove(contentId);
                }
                _ContentIdToQuest.Remove(contentId);

                var data = _ContentData[contentId];
                ReclaimEntryItemId(data.Id);

                return _ContentData.Remove(contentId);
            }
        }

        public ulong GetContentIdForCharacter(uint characterId)
        {
            lock (_ContentData)
            {
                if (!_CharacterIdToContentId.ContainsKey(characterId))
                {
                    return 0;
                }
                return _CharacterIdToContentId[characterId];
            }
        }

        public ulong GetContentIdForCharacter(Character character)
        {
            return GetContentIdForCharacter(character.CharacterId);
        }

        public List<uint> GetCharacterIdsForContent(ulong id)
        {
            lock (_ContentData)
            {
                if (!_ContentIdToCharacterIds.ContainsKey(id))
                {
                    return new List<uint>();
                }
                return _ContentIdToCharacterIds[id].ToList();
            }
        }

        public bool AddCharacterToContentGroup(ulong id, uint characterId)
        {
            lock (_ContentData)
            {
                if (!_ContentData.ContainsKey(id))
                {
                    return false;
                }

                _CharacterIdToContentId[characterId] = id;
                if (!_ContentIdToCharacterIds.ContainsKey(id))
                {
                    _ContentIdToCharacterIds[id] = new HashSet<uint>();
                }

                return _ContentIdToCharacterIds[id].Add(characterId);
            }
        }

        public bool AddCharacterToContentGroup(ulong id, Character character)
        {
            return AddCharacterToContentGroup(id, character.CharacterId);
        }

        public bool RemoveCharacterFromContentGroup(uint characterId)
        {
            lock (_ContentData)
            {
                if (!_ContentIdToCharacterIds.ContainsKey(characterId))
                {
                    return false;
                }

                ulong id = _CharacterIdToContentId[characterId];
                _CharacterIdToContentId.Remove(characterId);

                if (!_ContentIdToCharacterIds.ContainsKey(id))
                {
                    return true;
                }

                _ContentIdToCharacterIds[id].Remove(characterId);
            }

            return true;
        }

        public bool RemoveCharacterFromContentGroup(Character character)
        {
            return RemoveCharacterFromContentGroup(character.CharacterId);
        }

        public bool AddQuestToContent(ulong id, Quest quest)
        {
            lock (_ContentData)
            {
                _ContentIdToQuest[id] = quest;
                return true;
            }
        }

        public bool RemoveQuestFromContent(ulong id)
        {
            lock (_ContentData)
            {
                if (!_ContentIdToQuest.ContainsKey(id))
                {
                    return false;
                }
                return _ContentIdToQuest.Remove(id);
            }
        }

        public Quest GetQuestForContent(ulong id)
        {
            lock (_ContentData)
            {
                if (!_ContentIdToQuest.ContainsKey(id))
                {
                    return null;
                }
                return _ContentIdToQuest[id];
            }
        }

        public uint GenerateEntryItemId()
        {
            lock (_FreeEntryItemIds)
            {
                if (_FreeEntryItemIds.Count == 0)
                {
                    EntryItemIdCounter = EntryItemIdCounter + 1;
                    _FreeEntryItemIds.Push(EntryItemIdCounter);
                }
                return _FreeEntryItemIds.Pop();
            }
        }

        private void ReclaimEntryItemId(uint id)
        {
            lock (_FreeEntryItemIds)
            {
                _FreeEntryItemIds.Push(id);
            }
        }

        public void StartTimer(ulong contentId, GameClient client, uint playtimeInSeconds)
        {
            lock (_ContentData)
            {
                _ContentTimers[contentId] = new TimerState();

                var timerState = _ContentTimers[contentId];
                timerState.Duration = TimeSpan.FromSeconds(playtimeInSeconds);
                timerState.TimeStart = DateTime.Now;
                timerState.Timer = new Timer(task =>
                {
                    TimeSpan alreadyElapsed = DateTime.Now.Subtract(timerState.TimeStart);
                    if (alreadyElapsed > timerState.Duration)
                    {
                        Logger.Info($"Timer expired for ContentId={contentId}");
                        client.Party.SendToAll(new S2CQuestPlayTimeupNtc());
                        CancelTimer(contentId);
                    }
                }, null, 0, 1000);
                Logger.Info($"Starting {playtimeInSeconds} second timer for ContentId={contentId}");
            }
        }

        public ulong ExtendTimer(ulong contentId, uint amountInSeconds)
        {
            lock (_ContentTimers)
            {
                Logger.Info($"Extending time by {amountInSeconds} seconds for ContentId={contentId}");
                _ContentTimers[contentId].Duration += TimeSpan.FromSeconds(amountInSeconds);
                return (ulong) ((DateTimeOffset)(_ContentTimers[contentId].TimeStart + _ContentTimers[contentId].Duration)).ToUnixTimeSeconds();
            }
        }

        public (TimeSpan Elapsed, TimeSpan MaximumDuration) CancelTimer(ulong contentId)
        {
            lock (_ContentData)
            {
                if (_ContentTimers.ContainsKey(contentId))
                {
                    Logger.Info($"Canceling timer for ContentId={contentId}");
                    _ContentTimers[contentId].Timer.Dispose();

                    var timerState = _ContentTimers[contentId];

                    TimeSpan elapsed = DateTime.Now.Subtract(timerState.TimeStart);
                    var results = (elapsed, timerState.Duration);
                    _ContentTimers.Remove(contentId);
                    return results;
                }
                return (TimeSpan.Zero, TimeSpan.Zero);
            }
        }
    }
}
