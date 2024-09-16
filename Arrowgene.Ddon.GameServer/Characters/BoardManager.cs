using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class BoardManager
    {
        private DdonGameServer _Server;

        private Dictionary<ulong, Dictionary<uint, GroupData>> _Boards;
        private Dictionary<uint, GroupData> _Groups;
        private Dictionary<uint, uint> _CharacterToEntryIdMap;
        private uint EntryItemIdCounter;
        private Stack<uint> _FreeEntryItemIds;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BoardManager));

        public class GroupData
        {
            public ulong BoardId {  get; set; }
            public CDataEntryItem EntryItem {  get; set; }
            public string Password {  get; set; }
            public uint PartyLeaderCharacterId {  get; set; }
            public HashSet<uint> Members {  get; set; }
            public Dictionary<uint, bool> MemberReadyState {  get; set; }
            public bool IsInRecreate {  get; set; }
            public bool ContentInProgress {  get; set; }

            public GroupData()
            {
                EntryItem = new CDataEntryItem();
                Password = string.Empty;
                Members = new HashSet<uint>();
                MemberReadyState = new Dictionary<uint, bool>();
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

            // Entry ID Tracking
            EntryItemIdCounter = 1;
            _FreeEntryItemIds = new Stack<uint>();
            _FreeEntryItemIds.Push(EntryItemIdCounter);
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
            data.EntryItem.Id = GenerateEntryItemId();

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

                ReclaimEntryItemId(data.EntryItem.Id);
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
                data.ContentInProgress = false;

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

        private uint GenerateEntryItemId()
        {
            lock (_FreeEntryItemIds)
            {
                if (_FreeEntryItemIds.Count == 0)
                {
                    EntryItemIdCounter = EntryItemIdCounter + 1;
                    _FreeEntryItemIds.Push(EntryItemIdCounter);
                }

                var entryItemId = _FreeEntryItemIds.Pop();
                Logger.Info($"Allocating EntryId={entryItemId}");
                return entryItemId;
            }
        }

        private void ReclaimEntryItemId(uint entryItemId)
        {
            lock (_FreeEntryItemIds)
            {
                Logger.Info($"Reclaiming EntryId={entryItemId}");
                _FreeEntryItemIds.Push(entryItemId);
            }
        }
    }
}
