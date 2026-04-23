using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Logging;
using System.Linq;

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

            foreach (var character in server.ClientLookup.GetAllCharacter())
            {
                foreach (var key in character.CompletedQuests.Keys
                    .Where(QuestManager.IsBoardQuest)
                    .ToList())
                {
                    character.CompletedQuests.Remove(key);
                }
            }

            server.LightQuestManager.InsertRecordsFromAsset();

            var questRecords = server.Database.SelectLightQuestRecords();
            var extantQuests = QuestManager.GetQuestsByType(QuestType.Light);

            var quests = questRecords
                .Where(x => !extantQuests.Contains(x.QuestScheduleId))
                .Select(x => server.LightQuestManager.GenerateQuestFromRecord(x));

            QuestManager.AddQuests(server, quests);

            server.RpcManager.AnnounceOthers("internal/command", RpcInternalCommand.BoardQuestDailyRotation, null);
        }
    }
}
