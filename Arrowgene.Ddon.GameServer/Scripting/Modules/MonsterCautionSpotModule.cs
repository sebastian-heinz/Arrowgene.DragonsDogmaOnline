using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Shared.Model.Quest;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;
using System.IO;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class MonsterCautionSpotModule : GameServerScriptModule
    {
        public override string ModuleRoot => Path.Combine("area_rank", "monster_caution_spots");
        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;
        public override bool EnableHotLoad => true;

        public Dictionary<QuestAreaId, List<IMonsterSpotInfo>> EnemyGroups { get; private set; }

        public MonsterCautionSpotModule()
        {
            EnemyGroups = new Dictionary<QuestAreaId, List<IMonsterSpotInfo>>();
        }

        public override bool EvaluateResult(string path, ScriptState<object> result)
        {
            if (result == null)
            {
                return false;
            }

            IMonsterSpotInfo spotInfo = (IMonsterSpotInfo)result.ReturnValue;
            if (spotInfo == null)
            {
                return false;
            }

            spotInfo.Initialize();

            if (!EnemyGroups.ContainsKey(spotInfo.AreaId))
            {
                EnemyGroups[spotInfo.AreaId] = new List<IMonsterSpotInfo>();
            }

            // Remove if there are overlaps
            EnemyGroups[spotInfo.AreaId].RemoveAll(x => spotInfo.GetLocation().Equals(x.GetLocation()));
            // Add the new entry to the list
            EnemyGroups[spotInfo.AreaId].Add(spotInfo);

            var quest = QuestManager.GetQuestByQuestId(QuestId.WorldManageMonsterCaution);
            if (quest != null)
            {
                LibDdon.RegenerateQuest(quest);
            }

            return true;
        }
    }
}

