using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class DungeonManager
    {
        private Dictionary<uint, Dictionary<uint, bool>> _ReadyStatus;
        private Dictionary<uint, uint> _ContentId;
        private DdonGameServer _Server;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DungeonManager));

        public DungeonManager(DdonGameServer server)
        {
            _Server = server;
            _ReadyStatus = new Dictionary<uint, Dictionary<uint, bool>>();
            _ContentId = new Dictionary<uint, uint>();
        }

        public void StartActivity(PartyGroup party, Action<DdonGameServer, PartyGroup, uint> action)
        {
            lock (_ReadyStatus)
            {
                if (!_ContentId.ContainsKey(party.Id))
                {
                    return;
                }

                uint contentId = GetPartyContentId(party);

                EndPartyReadyCheck(party);

                action.Invoke(_Server, party, contentId);
            }
        }

        public uint GetPartyContentId(PartyGroup party)
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

        public bool InitiatePartyReadyCheck(PartyGroup party, uint contentId)
        {
            lock (_ReadyStatus)
            {
                if (_ReadyStatus.ContainsKey(party.Id))
                {
                    Logger.Error($"(PartyReadyCheck) Party ready check is already in progress for PartyId={party.Id}.");
                    return false;
                }

                _ContentId[party.Id] = contentId;

                _ReadyStatus[party.Id] = new Dictionary<uint, bool>();
                foreach (var memberClient in party.Clients)
                {
                    _ReadyStatus[party.Id][memberClient.Character.CharacterId] = false;
                }
                Logger.Info($"(PartyReadyCheck) Started party ready check for PartyId={party.Id}");
            }

            return true;
        }

        public void MarkReady(PartyGroup party, Character character, uint contentId)
        {
            lock (_ReadyStatus)
            {
                if (!_ReadyStatus.ContainsKey(party.Id) && !InitiatePartyReadyCheck(party, contentId))
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
                if (!_ReadyStatus.ContainsKey(party.Id))
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

        public bool IsReadyCheckInProgress(PartyGroup party)
        {
            lock (_ReadyStatus)
            {
                return _ReadyStatus.ContainsKey(party.Id);
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

        public static void StartDungeon(DdonGameServer server, PartyGroup party, uint contentId)
        {
            if (server.EpitaphRoadManager.IsEpitaphId(contentId))
            {
                StartEpitaphRoad(server, party, contentId);
            }
            else
            {
                StartBonusDungeon(server, party, contentId);
            }
        }

        private static void StartBonusDungeon(DdonGameServer server, PartyGroup party, uint contentId)
        {
            var BonusDungeonAsset = server.AssetRepository.BonusDungeonAsset;
            
            if (!BonusDungeonAsset.DungeonInfo.ContainsKey(contentId))
            {
                return;
            }

            var dungeonInfo = BonusDungeonAsset.DungeonInfo[contentId];
            foreach (var memberClient in party.Clients)
            {
                var itemUpdateList = new List<CDataItemUpdateResult>();
                foreach (var item in dungeonInfo.EntryCostList)
                {
                    itemUpdateList.AddRange(server.ItemManager.ConsumeItemByIdFromMultipleStorages(server, memberClient.Character, ItemManager.BothStorageTypes, item.ItemId, item.Num));
                }

                memberClient.Send(new S2CItemUpdateCharacterItemNtc()
                {
                    UpdateType = 0,
                    UpdateItemList = itemUpdateList
                });
            }

            var ntc = new S2CStageDungeonStartNtc()
            {
                Unk0 = contentId,
                StageId = dungeonInfo.StageId,
                StartPos = dungeonInfo.StartingPos,
                Unk4 = true,
            };
            party.SendToAll(ntc);
        }

        private static void StartEpitaphRoad(DdonGameServer server, PartyGroup party, uint epitaphId)
        {
            var section = server.EpitaphRoadManager.GetSectionById(epitaphId);
            var ntc = new S2CStageDungeonStartNtc()
            {
                StageId = section.StageId,
                StartPos = section.StartingPos,
                Unk4 = true,
            };
            party.SendToAll(ntc);
        }
    }
}
