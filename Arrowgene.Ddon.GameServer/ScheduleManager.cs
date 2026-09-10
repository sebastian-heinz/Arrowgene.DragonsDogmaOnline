using Arrowgene.Ddon.GameServer.Tasks;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Ddon.Shared.Model.Rpc;
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
            Tasks = new List<SchedulerTask>();
            Timers = new List<Timer>();
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
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            foreach (var task in Tasks)
            {
                if (!TaskEntries.ContainsKey(task.Type))
                {
                    Logger.Info($"Task '{task.Type}' has no database record. Creating entry.");
                    TaskEntries[task.Type] = new SchedulerTaskEntry { Type = task.Type, Timestamp = 0 };
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
                    Server.RpcManager.AnnounceOthers("internal/command", RpcInternalCommand.ScheduleManagerUpdateTasks, null);
                    Server.Database.UpsertScheduleInfo(task.Type, TaskEntries[task.Type].Timestamp);
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
                            Server.RpcManager.AnnounceOthers("internal/command", RpcInternalCommand.ScheduleManagerUpdateTasks, null);
                            Server.Database.UpsertScheduleInfo(task.Type, TaskEntries[task.Type].Timestamp);
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

        public void PopulateTasks()
        {
            var settings = Server.GameSettings.GameServerSettings;
            Tasks = Server.ScriptManager.SchedulerTaskModule.Tasks;
            foreach (var task in Tasks)
                task.GetOffset = () => settings.GetEffectiveUtcOffset();

            TaskEntries = Server.Database.SelectAllTaskEntries();
        }

        public void UpdateTaskTimestamps()
        {
            foreach (var task in Tasks)
            {
                long next = task.NextTimestamp();
                TaskEntries[task.Type].Timestamp = next;
            }
        }

        public SchedulerTask GetTask(TaskType type)
        {
            return Tasks.Where(x => x.Type == type).FirstOrDefault();
        }
    }
}
