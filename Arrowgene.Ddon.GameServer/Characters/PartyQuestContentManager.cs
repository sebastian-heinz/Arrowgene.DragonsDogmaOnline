using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class PartyQuestContentManager
    {
        private DdonGameServer _Server;
        private Dictionary<uint, TimerState> _ContentTimers;

        private Dictionary<uint, TimerState> _VoteTimers;
        private Dictionary<uint, Dictionary<uint, bool>> _VoteStatus;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyQuestContentManager));

        internal class TimerState
        {
            public DateTime TimeStart {  get; set; }
            public TimeSpan Duration {  get; set; }
            public Timer Timer { get; set; }
        }

        public PartyQuestContentManager(DdonGameServer server)
        {
            _Server = server;
            _ContentTimers = new Dictionary<uint, TimerState>();
            _VoteTimers = new Dictionary<uint, TimerState>();
            _VoteStatus = new Dictionary<uint, Dictionary<uint, bool>>();
        }

        public void InitatePartyAbandonVote(PartyGroup party, uint timeoutInSeconds)
        {
            lock (_VoteStatus)
            {
                if (_VoteStatus.ContainsKey(party.Id))
                {
                    Logger.Error($"(VoteToAbandon) Vote is already in progress for PartyId={party.Id}.");
                    return;
                }

                _VoteStatus[party.Id] = new Dictionary<uint, bool>();
                foreach (var client in party.Clients)
                {
                    _VoteStatus[party.Id][client.Character.CharacterId] = false;
                }
                _VoteTimers[party.Id] = new TimerState();

                var timerState = _VoteTimers[party.Id];
                timerState.Duration = TimeSpan.FromSeconds(timeoutInSeconds);
                timerState.TimeStart = DateTime.Now;
                timerState.Timer = new Timer(task =>
                {
                    TimeSpan alreadyElapsed = DateTime.Now.Subtract(timerState.TimeStart);
                    if (alreadyElapsed > timerState.Duration)
                    {
                        Logger.Info($"(VoteToAbandon) Timer expired for PartyId={party.Id}");
                        CancelVoteToAbandonTimer(party.Id);
                    }
                }, null, 0, 1000);
            }
            Logger.Info($"(VoteToAbandon) Started {timeoutInSeconds} seconds timer for PartyId={party.Id}");
        }

        public void CancelVoteToAbandonTimer(uint partyId)
        {
            lock (_VoteStatus)
            {
                _VoteTimers[partyId].Timer.Dispose();
                _VoteStatus.Remove(partyId);
                _VoteTimers.Remove(partyId);
                Logger.Info($"(VoteToAbandon) Canceling timer for PartyId={partyId}");
            }
        }

        public void VoteToAbandon(uint characterId, uint partyId, bool shouldAbandon)
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

                _VoteStatus[partyId][characterId] = shouldAbandon;
            }
        }

        public void VoteToAbandon(Character character, uint partyId, bool shouldAbandon)
        {
            VoteToAbandon(character.CharacterId, partyId, shouldAbandon);
        }

        public bool VoteToAbandonPassed(uint partyId)
        {
            lock (_VoteStatus)
            {
                if (!_VoteStatus.ContainsKey(partyId))
                {
                    return false;
                }

                bool shouldAbandon = true;
                foreach (var (characterId, result) in _VoteStatus[partyId])
                {
                    shouldAbandon &= result;
                }
                return shouldAbandon;
            }
        }

        public void StartTimer(uint partyId, GameClient client, uint playtimeInSeconds)
        {
            lock (_ContentTimers)
            {
                _ContentTimers[partyId] = new TimerState();

                var timerState = _ContentTimers[partyId];
                timerState.Duration = TimeSpan.FromSeconds(playtimeInSeconds);
                timerState.TimeStart = DateTime.Now;
                timerState.Timer = new Timer(task =>
                {
                    TimeSpan alreadyElapsed = DateTime.Now.Subtract(timerState.TimeStart);
                    if (alreadyElapsed > timerState.Duration)
                    {
                        Logger.Info($"(Content) Timer expired for Id={partyId}");
                        client.Party.SendToAll(new S2CQuestPlayTimeupNtc());
                        CancelTimer(partyId);
                    }
                }, null, 0, 1000);
                Logger.Info($"Starting {playtimeInSeconds} second timer for PartyId={partyId}");
            }
        }

        public ulong ExtendTimer(uint partyId, uint amountInSeconds)
        {
            lock (_ContentTimers)
            {
                Logger.Info($"(Content) Extending time by {amountInSeconds} seconds for PartyId={partyId}");
                _ContentTimers[partyId].Duration += TimeSpan.FromSeconds(amountInSeconds);
                return (ulong) ((DateTimeOffset)(_ContentTimers[partyId].TimeStart + _ContentTimers[partyId].Duration)).ToUnixTimeSeconds();
            }
        }

        public (TimeSpan Elapsed, TimeSpan MaximumDuration) CancelTimer(uint partyId)
        {
            lock (_ContentTimers)
            {
                if (_ContentTimers.ContainsKey(partyId))
                {
                    Logger.Info($"(Content) Canceling timer for PartyId={partyId}");
                    _ContentTimers[partyId].Timer.Dispose();

                    var timerState = _ContentTimers[partyId];

                    TimeSpan elapsed = DateTime.Now.Subtract(timerState.TimeStart);
                    var results = (elapsed, timerState.Duration);
                    _ContentTimers.Remove(partyId);
                    return results;
                }
            }
            return (TimeSpan.Zero, TimeSpan.Zero);
        }
    }
}
