using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class PartyQuestContentManager
    {
        private DdonGameServer _Server;
        private Dictionary<uint, uint> _ContentTimers;

        private Dictionary<uint, uint> _VoteTimers;
        private Dictionary<uint, Dictionary<uint, VoteAnswer>> _VoteStatus;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyQuestContentManager));

        public PartyQuestContentManager(DdonGameServer server)
        {
            _Server = server;
            _VoteStatus = new Dictionary<uint, Dictionary<uint, VoteAnswer>>();
            _VoteTimers = new Dictionary<uint, uint>();
            _ContentTimers = new Dictionary<uint, uint>();
        }

        public void InitatePartyAbandonVote(GameClient client, PartyGroup party, uint timeoutInSeconds)
        {
            lock (_VoteStatus)
            {
                if (_VoteStatus.ContainsKey(party.Id))
                {
                    Logger.Error($"(VoteToAbandon) Vote is already in progress for PartyId={party.Id}.");
                    return;
                }

                _VoteStatus[party.Id] = new Dictionary<uint, VoteAnswer>();
                foreach (var memberClient in party.Clients)
                {
                    _VoteStatus[party.Id][memberClient.Character.CharacterId] = VoteAnswer.Undecided;
                }

                _VoteTimers[party.Id] = _Server.TimerManager.CreateTimer(timeoutInSeconds, () =>
                {
                    Logger.Info($"(VoteToAbandon) Timer expired for PartyId={party.Id}");
                    client.Party.SendToAll(new S2CQuestPlayInterruptAnswerNtc() { IsInterrupt = false });
                    CancelVoteToAbandonTimer(party.Id);
                });
                _Server.TimerManager.StartTimer(_VoteTimers[party.Id]);
            }
            Logger.Info($"(VoteToAbandon) Started {timeoutInSeconds} seconds timer for PartyId={party.Id}");
        }

        public void RemovePartyMember(uint partyId, uint characterId)
        {
            lock (_VoteStatus)
            {
                if (!_VoteStatus.ContainsKey(partyId))
                {
                    return;
                }

                if (_VoteStatus[partyId].ContainsKey(characterId))
                {
                    _VoteStatus[partyId].Remove(characterId);
                }
            }
        }

        public void RemovePartyMember(uint partyId, Character character)
        {
            RemovePartyMember(partyId, character.CharacterId);
        }

        public void CancelVoteToAbandonTimer(uint partyId)
        {
            lock (_VoteStatus)
            {
                _Server.TimerManager.CancelTimer(_VoteTimers[partyId]);
                _VoteStatus.Remove(partyId);
                _VoteTimers.Remove(partyId);
                Logger.Info($"(VoteToAbandon) Canceling timer for PartyId={partyId}");
            }
        }

        public void VoteToAbandon(uint characterId, uint partyId, VoteAnswer answer)
        {
            lock (_VoteStatus)
            {
                if (!_VoteStatus.ContainsKey(partyId))
                {
                    return;
                }

                if (!_VoteStatus[partyId].ContainsKey(characterId))
                {
                    return;
                }

                _VoteStatus[partyId][characterId] = answer;
            }
        }

        public void VoteToAbandon(Character character, uint partyId, VoteAnswer answer)
        {
            VoteToAbandon(character.CharacterId, partyId, answer);
        }

        public bool AllMembersVoted(uint partyId)
        {
            lock (_VoteStatus)
            {
                if (!_VoteStatus.ContainsKey(partyId))
                {
                    return false;
                }

                bool allMembersVoted = true;
                foreach (var (characterId, answer) in _VoteStatus[partyId])
                {
                    allMembersVoted &= answer != VoteAnswer.Undecided;
                }
                return allMembersVoted;
            }
        }

        public bool VotedPassed(uint partyId)
        {
            lock (_VoteStatus)
            {
                if (!_VoteStatus.ContainsKey(partyId))
                {
                    return false;
                }

                bool allMembersVoted = true;
                foreach (var (characterId, answer) in _VoteStatus[partyId])
                {
                    allMembersVoted &= (answer == VoteAnswer.Agree);
                }
                return allMembersVoted;
            }
        }

        public void StartTimer(uint partyId, GameClient client, uint playtimeInSeconds)
        {
            lock (_ContentTimers)
            {
                _ContentTimers[partyId] = _Server.TimerManager.CreateTimer(playtimeInSeconds, () =>
                {
                    Logger.Info($"(Content) Timer expired for Id={partyId}");
                    client.Party.SendToAll(new S2CQuestPlayTimeupNtc());
                    CancelTimer(partyId);
                });
                _Server.TimerManager.StartTimer(_ContentTimers[partyId]);
                Logger.Info($"Starting {playtimeInSeconds} second timer for PartyId={partyId}");
            }
        }

        public ulong ExtendTimer(uint partyId, uint amountInSeconds)
        {
            lock (_ContentTimers)
            {
                Logger.Info($"(Content) Extending time by {amountInSeconds} seconds for PartyId={partyId}");
                return _Server.TimerManager.ExtendTimer(_ContentTimers[partyId], amountInSeconds);
            }
        }

        public (TimeSpan Elapsed, TimeSpan MaximumDuration) CancelTimer(uint partyId)
        {
            lock (_ContentTimers)
            {
                if (_ContentTimers.ContainsKey(partyId))
                {
                    Logger.Info($"(Content) Canceling timer for PartyId={partyId}");
                    var results = _Server.TimerManager.CancelTimer(_ContentTimers[partyId]);
                    _ContentTimers.Remove(partyId);
                    return results;
                }
            }
            return (TimeSpan.Zero, TimeSpan.Zero);
        }

        public ulong CheckTimer(uint partyId)
        {
            lock (_ContentTimers)
            {
                if (_ContentTimers.ContainsKey(partyId))
                {
                    return _Server.TimerManager.GetTimeLeftInSeconds(_ContentTimers[partyId]);
                }
            }

            return 0;
        }
    }
}
