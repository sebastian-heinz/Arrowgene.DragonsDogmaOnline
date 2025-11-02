using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Tasks.Implementations
{
    public class GroupChatPruningTask : DailyTask
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GroupChatPruningTask));

        public GroupChatPruningTask(uint hour, uint minute) : base(TaskType.GroupChatPruning, hour, minute)
        {

        }

        public override bool IsEnabled(DdonGameServer server)
        {
            return true;
        }

        public override void RunTask(DdonGameServer server)
        {
            Logger.Info("Performing daily group chat pruning task.");

            server.Database.PruneGroupChatGroups();
        }
    }
}
