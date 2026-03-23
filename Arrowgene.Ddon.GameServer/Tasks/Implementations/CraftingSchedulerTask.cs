using System;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model.Rpc;

namespace Arrowgene.Ddon.GameServer.Tasks.Implementations
{
    public class CraftingSchedulerTask : SecondlyTask
    {
        public CraftingSchedulerTask() : base(TaskType.Crafting, 1)
        {
        }

        public override void RunTask(DdonGameServer server)
        {
            server.RpcManager.AnnounceAll("internal/command", RpcInternalCommand.UpdateCrafting, null);
        }

        public override string TaskTypeName() => "Crafting";
    }
}