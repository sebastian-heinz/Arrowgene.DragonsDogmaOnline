using Arrowgene.Ddon.GameServer.Tasks;
using Arrowgene.Ddon.GameServer.Tasks.Implementations;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Arrowgene.Ddon.GameServer
{
    public class ScheduleManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ScheduleManager));

        private List<SchedulerTask> Tasks;
        private DdonGameServer Server;
        private List<Timer> Timers;

        private static readonly int TIMER_TICK_HOURLY = 1 * 1000; // 1 second
        private static readonly int TIMER_TICK_DAILY = 10 * 1000; // 10 seconds
        private static readonly int TIMER_TICK_WEEKLY = 30 * 1000; // 30 seconds

        public ScheduleManager(DdonGameServer server)
        {
            Server = server;
            Timers = new List<Timer>();

            // TODO: Load from server config
            Tasks = new List<SchedulerTask>()
            {
                new EpitaphSchedulerTask(DayOfWeek.Monday, 5, 0),
                new AreaPointResetTask(DayOfWeek.Monday, 5, 0),
                new RankingBoardResetTask(DayOfWeek.Monday, 5, 0),
                new PawnLikabilityIncreaseResetTask(5, 0),
                new EquipmentRecycleResetTask(5, 0),
            };
        }

        private int GetTimerTick(ScheduleInterval interval)
        {
            switch (interval)
            {
                case ScheduleInterval.Hourly:
                    return TIMER_TICK_HOURLY;
                case ScheduleInterval.Daily:
                    return TIMER_TICK_DAILY;
                case ScheduleInterval.Weekly:
                    return TIMER_TICK_WEEKLY;
                default:
                    return TIMER_TICK_HOURLY;
            }
        }

        public void StartServerTasks()
        {
            Dictionary<TaskType, SchedulerTaskEntry> entries = Server.Database.SelectAllTaskEntries();

            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            foreach (var task in Tasks)
            {
                if (!entries.ContainsKey(task.Type))
                {
                    Logger.Error($"Task '{task.Type}' has no record in the database. Skipping.");
                    continue;
                }

                if (!task.IsEnabled(Server))
                {
                    // This task is not enabled so skip it
                    continue;
                }

                long nextAction = entries[task.Type].Timestamp;
                if (now >= nextAction)
                {
                    task.RunTask(Server);
                    entries[task.Type].Timestamp = task.NextTimestamp();
                    Server.Database.UpdateScheduleInfo(task.Type, entries[task.Type].Timestamp);
                }

                var timerTick = GetTimerTick(task.Interval);
                var timer = new Timer(state =>
                {
                    long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    if (now >= entries[task.Type].Timestamp)
                    {
                        task.RunTask(Server);
                        entries[task.Type].Timestamp = task.NextTimestamp();
                        Server.Database.UpdateScheduleInfo(task.Type, entries[task.Type].Timestamp);
                    }
                }, null, timerTick, timerTick);

                Timers.Add(timer);
            }
        }

        public long TimeToNextTaskUpdate(TaskType taskType)
        {
            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var task = Tasks.Where(x => x.Type == taskType).FirstOrDefault();
            if (task == null)
            {
                return 0;
            }

            long next = task.NextTimestamp();

            return (next > now) ? (next - now) : 0;
        }

        public List<SchedulerTask> GetTasks()
        {
            return Tasks;
        }

        public SchedulerTask GetTask(TaskType type)
        {
            return Tasks.Where(x => x.Type == type).FirstOrDefault();
        }
    }
}
