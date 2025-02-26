using Arrowgene.Ddon.GameServer.Utils;
using Arrowgene.Ddon.Server;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class TimerManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(TimerManager));

        private DdonGameServer _Server;
        private Dictionary<uint, TimerState> _Timers;
        private UniqueIdPool _IdPool;

        public TimerManager(DdonGameServer server)
        {
            _Server = server;
            _Timers = new Dictionary<uint, TimerState>();
            _IdPool = new UniqueIdPool(1);
        }

        internal class TimerState
        {
            public DateTime TimeStart { get; set; }
            public TimeSpan Duration { get; set; }
            public Timer Timer { get; set; }
            public Action Action {  get; set; }
            public bool TimerStarted {  get; set; }
        }

        public uint CreateTimer(uint timeoutInSeconds, Action action)
        {
            uint timerId = _IdPool.GenerateId();
            lock (_Timers)
            {
                if (_Timers.ContainsKey(timerId))
                {
                    throw new Exception($"TimerId={timerId} already has state associated with it. Unable to allocate additional state.");
                }

                _Timers[timerId] = new TimerState()
                {
                    Action = action,
                    Duration = TimeSpan.FromSeconds(timeoutInSeconds)
                };
            }

            return timerId;
        }

        public bool StartTimer(uint timerId)
        {
            lock (_Timers)
            {
                if (!_Timers.ContainsKey(timerId))
                {
                    return false;
                }

                var timerState = _Timers[timerId];
                if (timerState.TimerStarted)
                {
                    return false;
                }

                timerState.TimeStart = DateTime.Now;
                timerState.Timer = new Timer(task =>
                {
                    TimeSpan alreadyElapsed = DateTime.Now.Subtract(timerState.TimeStart);
                    if (alreadyElapsed > timerState.Duration)
                    {
                        Logger.Info($"TimerId={timerId} expired.");
                        if (timerState.Action != null)
                        {
                            timerState.Action.Invoke();
                        }
                        CancelTimer(timerId);
                    }
                }, null, 0, 1000);
                Logger.Info($"Starting {timerState.Duration.TotalSeconds} second timer for TimerId={timerId}");
            }

            return true;
        }

        public ulong ExtendTimer(uint timerId, uint amountInSeconds)
        {
            lock (_Timers)
            {
                Logger.Info($"Extending timer by {amountInSeconds} seconds for TimerId={timerId}");
                _Timers[timerId].Duration += TimeSpan.FromSeconds(amountInSeconds);
                return (ulong)((DateTimeOffset)(_Timers[timerId].TimeStart + _Timers[timerId].Duration)).ToUnixTimeSeconds();
            }
        }

        public ulong GetTimeLeftInSeconds(uint timerId)
        {
            lock (_Timers)
            {
                if (!_Timers.ContainsKey(timerId))
                {
                    return 0;
                }

                var timeLeft = (_Timers[timerId].Duration - (DateTime.Now.Subtract(_Timers[timerId].TimeStart))).TotalSeconds;

                return (ulong) ((timeLeft < 0) ? 0 : timeLeft);
            }
        }

        public (TimeSpan Elapsed, TimeSpan MaximumDuration) CancelTimer(uint timerId)
        {
            lock (_Timers)
            {
                if (_Timers.ContainsKey(timerId))
                {
                    Logger.Info($"Canceling timer for TimerId={timerId}");
                    _Timers[timerId].Timer.Dispose();

                    var timerState = _Timers[timerId];

                    TimeSpan elapsed = DateTime.Now.Subtract(timerState.TimeStart);
                    var results = (elapsed, timerState.Duration);
                    _Timers.Remove(timerId);
                    _IdPool.ReclaimId(timerId);
                    return results;
                }
            }
            return (TimeSpan.Zero, TimeSpan.Zero);
        }
    }
}
