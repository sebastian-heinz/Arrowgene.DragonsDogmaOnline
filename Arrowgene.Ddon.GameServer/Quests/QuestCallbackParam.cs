using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Quests
{
    public class QuestCallbackParam
    {
        public GameClient Client { get; }
        public QuestState QuestState { get; }
        public List<CDataQuestCommand> ResultCommands;
        public List<CDataQuestCommand> CheckCommands;

        public QuestCallbackParam(GameClient client, QuestState questState)
        {
            Client = client;
            QuestState = questState;
            ResultCommands = new List<CDataQuestCommand>();
            CheckCommands = new List<CDataQuestCommand>();
        }
    }
}
