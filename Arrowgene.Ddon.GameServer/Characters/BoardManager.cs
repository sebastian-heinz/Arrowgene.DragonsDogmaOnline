using Arrowgene.Ddon.GameServer.Utils;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class BoardManager
    {
        private DdonGameServer _Server;

        private Dictionary<ulong, Dictionary<uint, GroupData>> _Boards;
        private Dictionary<uint, GroupData> _Groups;
        private Dictionary<uint, uint> _CharacterToEntryIdMap;
        private UniqueIdPool _EntryItemIdPool;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BoardManager));

        public static readonly ushort PARTY_BOARD_TIMEOUT = 3600;
        public static readonly ushort ENTRY_BOARD_READY_TIMEOUT = 120;

        public class GroupData
        {
            public ulong BoardId {  get; set; }
            public CDataEntryItem EntryItem {  get; set; }
            public string Password {  get; set; }
            public uint PartyLeaderCharacterId {  get; set; }
            public HashSet<uint> Members {  get; set; }
            public Dictionary<uint, bool> MemberReadyState {  get; set; }
            public bool IsInRecreate {  get; set; }
            public ContentStatus ContentStatus {  get; set; }
            public uint RecruitmentTimerId {  get; set; }
            public uint ReadyUpTimerId {  get; set; }

            public GroupData()
            {
                EntryItem = new CDataEntryItem();
                Password = string.Empty;
                Members = new HashSet<uint>();
                MemberReadyState = new Dictionary<uint, bool>();

                ContentStatus = ContentStatus.Recruiting;
            }

            public void ResetReadyState()
            {
                foreach (var characterId in Members)
                {
                    MemberReadyState[characterId] = false;
                }
            }
        }

        public BoardManager(DdonGameServer server)
        {
            _Server = server;
            _Boards = new Dictionary<ulong, Dictionary<uint, GroupData>>();
            _Groups = new Dictionary<uint, GroupData>();
            _CharacterToEntryIdMap = new Dictionary<uint, uint>();
            _EntryItemIdPool = new UniqueIdPool(1);
        }

        public GroupData CreateNewGroup(ulong boardId, CDataEntryItemParam createParam, string password, uint leaderCharacterId)
        {
            var data = new GroupData()
            {
                BoardId = boardId,
                Password = password,
                PartyLeaderCharacterId = leaderCharacterId,
            };
            data.EntryItem.Param = createParam;
            data.EntryItem.Id = _EntryItemIdPool.GenerateId();

            // TODO: Quest Manager look up min/max

            lock (_Boards)
            {
                if (!_Boards.ContainsKey(boardId))
                {
                    _Boards[boardId] = new Dictionary<uint, GroupData>();
                }

                if (_Boards[boardId].ContainsKey(data.EntryItem.Id))
                {
                    Logger.Error($"EntryItemId={data.EntryItem.Id} is already in use (unable to assign)");
                    return null;
                }

                if (_Groups.ContainsKey(data.EntryItem.Id))
                {
                    Logger.Error($"EntryItemId={data.EntryItem.Id} is already in use (cleaned up improperly?)");
                    return null;
                }

                _Boards[boardId][data.EntryItem.Id] = data;
                _Groups[data.EntryItem.Id] = data;

                AddCharacterToGroup(data.EntryItem.Id, leaderCharacterId);

                Logger.Info($"Allocating EntryId={data.EntryItem.Id}");
            }

            return data;
        }

        public GroupData CreateNewGroup(ulong boardId, CDataEntryItemParam createParam, string password, Character leaderCharacter)
        {
            return CreateNewGroup(boardId, createParam, password, leaderCharacter.CharacterId);
        }

        public bool RemoveGroup(uint entryItemId)
        {
            lock (_Boards)
            {
                if (!_Groups.ContainsKey(entryItemId))
                {
                    return false;
                }

                var data = _Groups[entryItemId];
                _Groups.Remove(entryItemId);

                if (_Boards.ContainsKey(data.BoardId) && _Boards[data.BoardId].ContainsKey(entryItemId))
                {
                    _Boards[data.BoardId].Remove(entryItemId);
                }

                foreach (var characterId in data.Members)
                {
                    if (_CharacterToEntryIdMap.ContainsKey(characterId))
                    {
                        _CharacterToEntryIdMap.Remove(characterId);
                    }
                }

                if (data.RecruitmentTimerId != 0)
                {
                    _Server.TimerManager.CancelTimer(data.RecruitmentTimerId);
                }
                
                if (data.ReadyUpTimerId != 0)
                {
                    _Server.TimerManager.CancelTimer(data.ReadyUpTimerId);
                }

                _EntryItemIdPool.ReclaimId(data.EntryItem.Id);
                Logger.Info($"Reclaiming EntryId={data.EntryItem.Id}");
            }

            return true;
        }

        public GroupData RecreateGroup(uint characterId)
        {
            lock (_Boards)
            {
                uint entryItemId = GetEntryItemIdForCharacter(characterId);
                if (entryItemId == 0)
                {
                    return null;
                }

                var data = GetGroupData(entryItemId);
                data.ResetReadyState();
                data.IsInRecreate = true;
                data.ContentStatus = ContentStatus.Recruiting;

                return data;
            }
        }

        public GroupData RecreateGroup(Character character)
        {
            return RecreateGroup(character.CharacterId);
        }

        public List<GroupData> GetGroupsForBoardId(ulong boardId)
        {
            lock (_Boards)
            {
                if (!_Boards.ContainsKey(boardId))
                {
                    return new List<GroupData>();
                }
                return _Boards[boardId].Values.ToList();
            }
        }

        public bool AddCharacterToGroup(uint entryItemId, uint characterId)
        {
            lock (_Boards)
            {
                if (!_Groups.ContainsKey(entryItemId))
                {
                    return false;
                }

                var data = _Groups[entryItemId];
                if (data.Members.Count == data.EntryItem.Param.MaxEntryNum)
                {
                    return false;
                }

                data.Members.Add(characterId);

                _CharacterToEntryIdMap[characterId] = entryItemId;

                data.MemberReadyState[characterId] = false;

                return true;
            }
        }

        public bool AddCharacterToGroup(uint entryItemId, Character character)
        {
            return AddCharacterToGroup(entryItemId, character.CharacterId);
        }

        public bool RemoveCharacterFromGroup(uint characterId)
        {
            lock (_Boards)
            {
                uint entryItemId = GetEntryItemIdForCharacter(characterId);
                if (entryItemId == 0)
                {
                    // Character was not part of a board group
                    return false;
                }

                var data = GetGroupData(entryItemId);
                if (data == null)
                {
                    return false;
                }
                
                if (data.Members.Contains(characterId))
                {
                    data.Members.Remove(characterId);
                }

                if (data.MemberReadyState.ContainsKey(characterId))
                {
                    data.MemberReadyState.Remove(characterId);
                }

                if (_CharacterToEntryIdMap.ContainsKey(characterId))
                {
                    _CharacterToEntryIdMap.Remove(characterId);
                }

                if (data.Members.Count == 0)
                {
                    RemoveGroup(data.EntryItem.Id);
                }

                return true;
            }
        }

        public bool RemoveCharacterFromGroup(Character character)
        {
            return RemoveCharacterFromGroup(character.CharacterId);
        }

        public void SetGroupMemberReadyState(uint characterId, bool isReady)
        {
            lock (_Boards)
            {
                if (!_CharacterToEntryIdMap.ContainsKey(characterId))
                {
                    return;
                }

                uint entryItemId = _CharacterToEntryIdMap[characterId];
                if (!_Groups.ContainsKey(entryItemId))
                {
                    return;
                }

                var data = _Groups[entryItemId];
                data.MemberReadyState[characterId] = isReady;
            }
        }

        public void SetGroupMemberReadyState(Character character, bool isReady)
        {
            SetGroupMemberReadyState(character.CharacterId, isReady);
        }

        public uint NumGroupMembersReady(uint entryItemId)
        {
            lock (_Boards)
            {
                var data = GetGroupData(entryItemId);
                uint count = 0;
                foreach (var (characterId, isReady) in data.MemberReadyState)
                {
                    if (isReady)
                    {
                        count += 1;
                    }
                }
                return count;
            }
        }

        public bool AllGroupMembersReady(uint entryItemId)
        {
            lock (_Boards)
            {
                var data = GetGroupData(entryItemId);
                uint numReady = NumGroupMembersReady(entryItemId);
                return numReady == data.Members.Count;
            }
        }

        public GroupData GetGroupData(uint entryItemId)
        {
            lock (_Boards)
            {
                if (!_Groups.ContainsKey(entryItemId))
                {
                    return null;
                }
                return _Groups[entryItemId];
            }
        }

        public GroupData GetGroupDataForCharacter(uint characterId)
        {
            lock (_Boards)
            {
                uint entryItemId = GetEntryItemIdForCharacter(characterId);
                return GetGroupData(entryItemId);
            }
        }

        public GroupData GetGroupDataForCharacter(Character character)
        {
            return GetGroupDataForCharacter(character.CharacterId);
        }

        public uint GetEntryItemIdForCharacter(uint characterId)
        {
            lock (_Boards)
            {
                if (!_CharacterToEntryIdMap.ContainsKey(characterId))
                {
                    return 0;
                }
                return _CharacterToEntryIdMap[characterId];
            }
        }

        public uint GetEntryItemIdForCharacter(Character character)
        {
            return GetEntryItemIdForCharacter(character.CharacterId);
        }

        public bool StartRecruitmentTimer(uint entryItemId, uint timeoutInSeconds)
        {
            lock (_Boards)
            {
                var data = GetGroupData(entryItemId);
                if (data == null)
                {
                    return false;
                }

                data.RecruitmentTimerId = _Server.TimerManager.CreateTimer(timeoutInSeconds, () =>
                {
                    lock (_Boards)
                    {
                        foreach (var characterId in data.Members)
                        {
                            var memberClient = _Server.ClientLookup.GetClientByCharacterId(characterId);
                            if (memberClient != null)
                            {
                                memberClient.Send(new S2CEntryBoardEntryBoardItemLeaveNtc() { LeaveType = EntryBoardLeaveType.EntryBoardTimeUp });
                            }
                        }
                    }
                });

                if (!_Server.TimerManager.StartTimer(data.RecruitmentTimerId))
                {
                    _Server.TimerManager.CancelTimer(data.RecruitmentTimerId);
                    return false;
                }

                return true;
            }
        }

        public ulong GetRecruitmentTimeLeft(uint entryItemId)
        {
            lock (_Boards)
            {
                var data = GetGroupData(entryItemId);
                if (data == null)
                {
                    return 0;
                }
                return _Server.TimerManager.GetTimeLeftInSeconds(data.RecruitmentTimerId);
            }
        }

        public bool CancelRecruitmentTimer(uint entryItemId)
        {
            lock (_Boards)
            {
                var data = GetGroupData(entryItemId);
                if (data == null)
                {
                    return false;
                }

                _Server.TimerManager.CancelTimer(data.RecruitmentTimerId);
                data.RecruitmentTimerId = 0;
                return true;
            }
        }

        public bool StartReadyUpTimer(uint entryItemId, uint timeoutInSeconds)
        {
            lock (_Boards)
            {
                var data = GetGroupData(entryItemId);
                if (data == null)
                {
                    return false;
                }

                data.ReadyUpTimerId = _Server.TimerManager.CreateTimer(timeoutInSeconds, () =>
                {
                    lock (_Boards)
                    {
                        RestartRecruitment(entryItemId);
                    }
                });

                if (!_Server.TimerManager.StartTimer(data.ReadyUpTimerId))
                {
                    _Server.TimerManager.CancelTimer(data.ReadyUpTimerId);
                    return false;
                }

                return true;
            }
        }

        public bool RestartRecruitment(uint entryItemId)
        {
            lock (_Boards)
            {
                var data = GetGroupData(entryItemId);
                if (data == null)
                {
                    return false;
                }

                foreach (var characterId in data.Members)
                {
                    var memberClient = _Server.ClientLookup.GetClientByCharacterId(characterId);
                    if (memberClient != null)
                    {
                        memberClient.Send(new S2CEntryBoardItemUnreadyNtc());
                    }
                }

                // Restart the recruitment timer
                data.EntryItem.TimeOut = BoardManager.PARTY_BOARD_TIMEOUT;
                StartRecruitmentTimer(data.EntryItem.Id, BoardManager.PARTY_BOARD_TIMEOUT);
                foreach (var characterId in data.Members)
                {
                    var memberClient = _Server.ClientLookup.GetClientByCharacterId(characterId);
                    if (memberClient != null)
                    {
                        memberClient.Send(new S2CEntryBoardItemTimeoutTimerNtc() { TimeOut = BoardManager.PARTY_BOARD_TIMEOUT });
                    }
                }

                return true;
            }
        }

        public bool CancelReadyUpTimer(uint entryItemId)
        {
            lock (_Boards)
            {
                var data = GetGroupData(entryItemId);
                if (data == null)
                {
                    return false;
                }

                _Server.TimerManager.CancelTimer(data.ReadyUpTimerId);
                data.ReadyUpTimerId = 0;
                return true;
            }
        }

        public bool ExtendReadyUpTimer(uint entryItemId)
        {
            lock (_Boards)
            {
                var data = GetGroupData(entryItemId);
                if (data == null)
                {
                    return false;
                }

                // UI only allows the count down to start from 120
                var ellapsedTime = BoardManager.ENTRY_BOARD_READY_TIMEOUT - _Server.TimerManager.GetTimeLeftInSeconds(data.ReadyUpTimerId);
                _Server.TimerManager.ExtendTimer(data.ReadyUpTimerId, (uint)ellapsedTime);
                return true;
            }
        }

        public ushort GetTimeLeftToReadyUp(uint entryItemId)
        {
            lock (_Boards)
            {
                var data = GetGroupData(entryItemId);
                if (data == null)
                {
                    return 0;
                }
                return (ushort) _Server.TimerManager.GetTimeLeftInSeconds(data.ReadyUpTimerId);
            }
        }

        private static readonly ulong BOARD_CATEGORY_RECRUITMENT = 0x9_00000000;
        private static readonly ulong BOARD_CATEGORY_EXM         = 0x4_00000000;

        public static ulong QuestScheduleIdToExmBoardId(uint questScheduleId)
        {
            return (ulong)(questScheduleId | BOARD_CATEGORY_EXM);
        }

        public static bool BoardIdIsExm(ulong boardId)
        {
            // When then 5th byte is 4, this quest is an extreme mission
            // The bottom 4 bytes are the QuestId/QuestScheduleId
            // 0x4_nnnnnnnn
            return (BOARD_CATEGORY_EXM & boardId) > 0;
        }

        public static bool BoardIdIsRecruitmentCategory(ulong boardId)
        {
            // When then 5th byte is 9, this quest is a board id for general party
            // The bottom 4 bytes are the CategoryId
            // 0x9_nnnnnnnn
            return (BOARD_CATEGORY_RECRUITMENT & boardId) > 0;
        }

        public static ulong BoardIdFromRecruitmentCategory(uint category)
        {
            return (BOARD_CATEGORY_RECRUITMENT | category);
        }

        private static uint GetValueFromBoardId(ulong boardId)
        {
            return unchecked((uint) boardId);
        }

        public static uint RecruitmentCategoryFromBoardId(ulong boardId)
        {
            return GetValueFromBoardId(boardId);
        }

        public static uint GetQuestIdFromBoardId(ulong boardId)
        {
            return GetValueFromBoardId(boardId);
        }
    }
}
