using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arrowgene.Ddon.GameServer.Quests.LightQuests;
using Arrowgene.Ddon.GameServer.Characters;

namespace Arrowgene.Ddon.GameServer.Tasks.Implementations
{
    internal class BoardQuestRotationTask : DailyTask
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BoardQuestRotationTask));

        public BoardQuestRotationTask(uint hour, uint minute) : base(TaskType.BoardQuestRotation, hour, minute)
        {
        }

        public override bool IsEnabled(DdonGameServer server)
        {
            return true;
        }

        public override void RunTask(DdonGameServer server)
        {
            Logger.Info("Performing daily light quest rotation");

            server.LightQuestManager.InsertRecordsFromAsset();

            server.RpcManager.AnnounceAll("internal/command", RpcInternalCommand.BoardQuestDailyRotation, null);
        }
    }
}
