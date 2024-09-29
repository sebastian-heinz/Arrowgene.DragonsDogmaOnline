using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class BonusDungeonManager
    {
        private Dictionary<uint, Dictionary<uint, bool>> _ReadyStatus;
        private Dictionary<uint, uint> _ContentId;
        private BonusDungeonAsset _BonusDungeonAsset;
        private DdonGameServer _Server;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyQuestContentManager));

        public BonusDungeonManager(DdonGameServer server)
        {
            _Server = server;
            _ReadyStatus = new Dictionary<uint, Dictionary<uint, bool>>();
            _ContentId = new Dictionary<uint, uint>();
            _BonusDungeonAsset = server.AssetRepository.BonusDungeonAsset;
        }

        public void StartDungeon(PartyGroup party)
        {
            lock (_ReadyStatus)
            {
                if (!_ContentId.ContainsKey(party.Id))
                {
                    return;
                }

                var dungeonId = _ContentId[party.Id];
                var dungeonInfo = _BonusDungeonAsset.DungeonInfo[dungeonId];

                foreach (var memberClient in party.Clients)
                {
                    var itemUpdateList = new List<CDataItemUpdateResult>();
                    foreach (var item in dungeonInfo.EntryCostList)
                    {
                        itemUpdateList.Add(_Server.ItemManager.ConsumeItemByIdFromMultipleStorages(_Server, memberClient.Character, ItemManager.BothStorageTypes, item.ItemId, item.Num));
                    }

                    memberClient.Send(new S2CItemUpdateCharacterItemNtc()
                    {
                        UpdateType = 0,
                        UpdateItemList = itemUpdateList
                    });
                }

                EndPartyReadyCheck(party);

                var ntc = new S2CStageTicketDungeonStartNtc()
                {
                    Unk0 = dungeonId,
                    StageId = dungeonInfo.StageId,
                    StartPos = dungeonInfo.StartingPos,
                    Unk4 = true,
                };
                party.SendToAll(ntc);
            }
        }

        public uint GetPartyDungeonId(PartyGroup party)
        {
            lock (_ReadyStatus)
            {
                if (!_ContentId.ContainsKey(party.Id))
                {
                    return 0;
                }
                return _ContentId[party.Id];
            }
        }

        public bool InitiatePartyReadyCheck(PartyGroup party, uint dungeonId)
        {
            lock (_ReadyStatus)
            {
                if (_ReadyStatus.ContainsKey(party.Id))
                {
                    Logger.Error($"(PartyReadyCheck) Party ready check is already in progress for PartyId={party.Id}.");
                    return false;
                }

                _ContentId[party.Id] = dungeonId;

                _ReadyStatus[party.Id] = new Dictionary<uint, bool>();
                foreach (var memberClient in party.Clients)
                {
                    _ReadyStatus[party.Id][memberClient.Character.CharacterId] = false;
                }
                Logger.Info($"(PartyReadyCheck) Started party ready check for PartyId={party.Id}");
            }

            return true;
        }

        public void MarkReady(PartyGroup party, Character character, uint dungeonId)
        {
            lock (_ReadyStatus)
            {
                if (!_ReadyStatus.ContainsKey(party.Id) && !InitiatePartyReadyCheck(party, dungeonId))
                {
                    return;
                }

                _ReadyStatus[party.Id][character.CharacterId] = true;
            }
        }

        public void MarkNotReady(PartyGroup party, Character character)
        {
            lock (_ReadyStatus)
            {
                if (_ReadyStatus.ContainsKey(party.Id))
                {
                    return;
                }

                _ReadyStatus[party.Id][character.CharacterId] = false;
            }
        }

        public bool PartyIsReady(PartyGroup party)
        {
            lock (_ReadyStatus)
            {
                if (!_ReadyStatus.ContainsKey(party.Id))
                {
                    return false;
                }

                bool allMembersReady = true;
                foreach (var (characterId, answer) in _ReadyStatus[party.Id])
                {
                    allMembersReady &= answer;
                }
                return allMembersReady;
            }
        }

        public void EndPartyReadyCheck(PartyGroup party)
        {
            lock (_ReadyStatus)
            {
                if (_ReadyStatus.ContainsKey(party.Id))
                {
                    _ReadyStatus.Remove(party.Id);
                }

                if (_ContentId.ContainsKey(party.Id))
                {
                    _ContentId.Remove(party.Id);
                }
            }
        }
    }
}
