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
        Dictionary<TaskType, SchedulerTaskEntry> TaskEntries = new();

        private static readonly int TIMER_TICK_HOURLY = 1 * 1000; // 1 second
        private static readonly int TIMER_TICK_DAILY = 10 * 1000; // 10 seconds
        private static readonly int TIMER_TICK_WEEKLY = 30 * 1000; // 30 seconds
        private static readonly int TIMER_TICK_SECONDLY = 1 * 1000; // 1 second

        public ScheduleManager(DdonGameServer server)
        {
            Server = server;
            Timers = new List<Timer>();

            // TODO: Load from server config
            Tasks =
            [
                new EpitaphSchedulerTask(DayOfWeek.Monday, 5, 0),
                new AreaPointResetTask(DayOfWeek.Monday, 5, 0),
                new RankingBoardResetTask(DayOfWeek.Monday, 5, 0),
                new BBMResetTicketTask(DayOfWeek.Monday, 5, 0),
                new CraftingSchedulerTask(),
                new PawnLikabilityIncreaseResetTask(5, 0),
                new EquipmentRecycleResetTask(5, 0),
                new BoardQuestRotationTask(5, 0),
                new WorldQuestResetTask(
                    server.GameSettings.GameServerSettings.WorldQuestResetDay,
                    server.GameSettings.GameServerSettings.WorldQuestResetHour,
                    server.GameSettings.GameServerSettings.WorldQuestResetMinute),
                new GroupChatPruningTask(0, 0),
                new StampResetTask(5, 0),
            ];
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
                case ScheduleInterval.Secondly:
                    return TIMER_TICK_SECONDLY;
                default:
                    return TIMER_TICK_HOURLY;
            }
        }

        public void StartServerTasks()
        {
            TaskEntries = Server.Database.SelectAllTaskEntries();

            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            foreach (var task in Tasks)
            {
                if (!TaskEntries.ContainsKey(task.Type))
                {
                    Logger.Error($"Task '{task.Type}' has no record in the database. Skipping.");
                    continue;
                }

                if (!task.IsEnabled(Server))
                {
                    // This task is not enabled so skip it
                    continue;
                }

                long nextAction = TaskEntries[task.Type].Timestamp;
                if (now >= nextAction)
                {
                    task.RunTask(Server);
                    TaskEntries[task.Type].Timestamp = task.NextTimestamp();
                    Server.Database.UpdateScheduleInfo(task.Type, TaskEntries[task.Type].Timestamp);
                }

                var timerTick = GetTimerTick(task.Interval);
                var timer = new Timer(state =>
                {
                    long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    if (now >= TaskEntries[task.Type].Timestamp)
                    {
                        task.RunTask(Server);
                        TaskEntries[task.Type].Timestamp = task.NextTimestamp();
                        if (task.Interval != ScheduleInterval.Secondly)
                        {
                            Server.Database.UpdateScheduleInfo(task.Type, TaskEntries[task.Type].Timestamp);
                        }
                    }
                }, null, timerTick, timerTick);

                Timers.Add(timer);
            }
        }

        public long TimeToNextTaskUpdate(TaskType taskType)
        {
            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (!TaskEntries.ContainsKey(taskType))
            {
                return 0;
            }

            long next = TaskEntries[taskType].Timestamp;

            return (now > next) ? 0 : (next - now);
        }

        public long TaskExpiry(TaskType taskType)
        {
            if (!TaskEntries.ContainsKey(taskType))
            {
                return 0;
            }
            return TaskEntries[taskType].Timestamp;
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
