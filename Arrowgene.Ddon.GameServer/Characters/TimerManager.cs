using Arrowgene.Ddon.GameServer.Utils;
using Arrowgene.Ddon.Server;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class TimerManager(DdonGameServer server)
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(TimerManager));

        private readonly DdonGameServer _Server = server;
        private readonly Dictionary<uint, TimerState> _Timers = [];
        private readonly UniqueIdPool _IdPool = new(1);

        internal class TimerState
        {
            public DateTime TimeStart { get; set; }
            public TimeSpan Duration { get; set; }
            public TimeSpan CumulativeElapsed { get; set; } = TimeSpan.Zero;
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
                    Duration = TimeSpan.FromSeconds(timeoutInSeconds),
                };
            }

            return timerId;
        }

        public bool StartTimer(uint timerId)
        {
            lock (_Timers)
            {
                if (!_Timers.TryGetValue(timerId, out TimerState timerState))
                {
                    return false;
                }

                if (timerState.TimerStarted)
                {
                    return false;
                }

                timerState.TimeStart = DateTime.Now;
                Logger.Info($"Starting {timerState.Duration.TotalSeconds} second timer for TimerId={timerId}");

                timerState.Timer = new Timer(task =>
                {
                    TimeSpan alreadyElapsed = DateTime.Now.Subtract(timerState.TimeStart);
                    if (alreadyElapsed > timerState.Duration)
                    {
                        Logger.Info($"TimerId={timerId} expired.");
                        timerState.Action?.Invoke();
                        CancelTimer(timerId);
                    }
                }, null, 0, 1000);
                timerState.TimerStarted = true;
            }

            return true;
        }

        public ulong ExtendTimer(uint timerId, uint amountInSeconds)
        {
            lock (_Timers)
            {
                // TODO: This needs some guards to check if the timer exists.
                Logger.Info($"Extending timer by {amountInSeconds} seconds for TimerId={timerId}");
                _Timers[timerId].Duration += TimeSpan.FromSeconds(amountInSeconds);
                return (ulong)((DateTimeOffset)(_Timers[timerId].TimeStart + _Timers[timerId].Duration)).ToUnixTimeSeconds();
            }
        }

        public ulong RestartTimer(uint timerId)
        {
            lock (_Timers)
            {
                // TODO: This needs some guards to check if the timer exists.
                Logger.Info($"Restarting timer for TimerId={timerId}");
                _Timers[timerId].TimeStart = DateTime.Now;
                return (ulong)((DateTimeOffset)(_Timers[timerId].TimeStart + _Timers[timerId].Duration)).ToUnixTimeSeconds();
            }
        }

        public void PauseTimer(uint timerId)
        {
            lock (_Timers)
            {
                if (!_Timers.TryGetValue(timerId, out TimerState timerState))
                {
                    throw new Exception($"TimerId={timerId} does not exist.");
                }

                Logger.Info($"Pausing timer for TimerId={timerId}");
                if (timerState.TimerStarted)
                {
                    timerState.Timer.Dispose();
                    timerState.CumulativeElapsed += DateTime.Now.Subtract(timerState.TimeStart);
                    timerState.Duration = GetTimeLeft(timerId);
                    timerState.TimerStarted = false;
                }
            }
        }

        public TimeSpan GetTimeLeft(uint timerId)
        {
            lock (_Timers)
            {
                if (!_Timers.TryGetValue(timerId, out TimerState timer))
                {
                    return TimeSpan.Zero;
                }

                if (!timer.TimerStarted)
                {
                    return timer.Duration;
                }

                var timeLeft = timer.Duration - DateTime.Now.Subtract(timer.TimeStart);
                timeLeft = timeLeft >= TimeSpan.Zero ? timeLeft : TimeSpan.Zero;

                return timeLeft;
            }
        }

        public ulong GetTimeLeftInSeconds(uint timerId)
        {
            return (ulong) GetTimeLeft(timerId).TotalSeconds;            
        }

        public bool IsTimerStarted(uint timerId)
        {
            lock (_Timers)
            {
                return _Timers.TryGetValue(timerId, out TimerState value) && value.TimerStarted;
            }
        }

        public (TimeSpan Elapsed, TimeSpan MaximumDuration) CancelTimer(uint timerId)
        {
            lock (_Timers)
            {
                if (_Timers.TryGetValue(timerId, out TimerState timerState))
                {
                    Logger.Info($"Canceling timer for TimerId={timerId}");
                    TimeSpan elapsed = timerState.CumulativeElapsed;
                    if (_Timers[timerId].TimerStarted)
                    {
                        _Timers[timerId].Timer.Dispose();
                        elapsed += DateTime.Now.Subtract(timerState.TimeStart);
                    }

                    var results = (elapsed, timerState.Duration + timerState.CumulativeElapsed);
                    _Timers.Remove(timerId);
                    _IdPool.ReclaimId(timerId);
                    return results;
                }
            }
            return (TimeSpan.Zero, TimeSpan.Zero);
        }
    }
}
