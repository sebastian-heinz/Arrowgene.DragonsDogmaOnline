using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
// using static Arrowgene.Ddon.GameServer.Characters.ExmManager;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class ContentManager
    {
        private DdonGameServer _Server;
        private Dictionary<uint, TimerState> _ContentTimers;

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ContentManager));

        internal class TimerState
        {
            public DateTime TimeStart {  get; set; }
            public TimeSpan Duration {  get; set; }
            public Timer Timer { get; set; }
        }

        public ContentManager(DdonGameServer server)
        {
            _Server = server;
            _ContentTimers = new Dictionary<uint, TimerState>();
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
                        Logger.Info($"Timer expired for Id={partyId}");
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
                Logger.Info($"Extending time by {amountInSeconds} seconds for PartyId={partyId}");
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
                    Logger.Info($"Canceling timer for PartyId={partyId}");
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
