using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Tasks
{
    public class EpitaphSchedulerTask : WeeklyTask
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EpitaphSchedulerTask));

        public EpitaphSchedulerTask(DayOfWeek day, uint hour, uint minute) : base(TaskType.EpitaphRoadRewardsReset, day, hour, minute)
        {

        }

        public override bool IsEnabled(DdonGameServer server)
        {
            return server.Setting.GameLogicSetting.EnableEpitaphWeeklyRewards;
        }

        public override void RunTask(DdonGameServer server)
        {
            Logger.Info("Performing weekly epitaph reset");
            server.ChatManager.SendMessage("Performing weekly epitaph reset", string.Empty, string.Empty, LobbyChatMsgType.ManagementAlertN, server.ClientLookup.GetAll());

            server.Database.DeleteWeeklyEpitaphClaimedRewards();

            foreach (var client in server.ClientLookup.GetAll())
            {
                client.Character.EpitaphRoadState.WeeklyRewardsClaimed.Clear();
            }
        }
    }
}
