using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Tasks.Implementations
{
    internal class StampResetTask : DailyTask
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StampResetTask));

        public StampResetTask(uint hour, uint minute) : base(TaskType.LoginStamps, hour, minute)
        {
        }

        public override bool IsEnabled(DdonGameServer server)
        {
            return true;
        }

        public override void RunTask(DdonGameServer server)
        {
            Logger.Info("Performing daily stamp rollover");

            foreach(var character in server.ClientLookup.GetAllCharacter())
            {
                server.StampManager.RefreshStamp(character);
            }

            server.RpcManager.AnnounceAll("internal/command", RpcInternalCommand.StampReset, null);

            server.Database.ResetCharacterStamps();
        }
    }
}
