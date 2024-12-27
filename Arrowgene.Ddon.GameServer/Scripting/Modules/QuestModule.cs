using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server.Scripting;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model.Quest;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class QuestModule : ScriptModule
    {
        public override string ModuleRoot => "quests";
        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;
        public override bool EnableHotLoad => true;
        public Dictionary<QuestId, List<IQuest>> Quests { get; private set; }
        public Dictionary<uint, IQuest> QuestsByScheduleId { get; private set; }

        public QuestModule()
        {
            Quests = new Dictionary<QuestId, List<IQuest>>();
            QuestsByScheduleId = new Dictionary<uint, IQuest>();
        }

        public override ScriptOptions Options()
        {
            return ScriptOptions.Default
                .AddReferences(MetadataReference.CreateFromFile(typeof(DdonGameServer).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(AssetRepository).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(LibUtils).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(LibDdon).Assembly.Location))
                .AddImports("System", "System.Collections", "System.Collections.Generic")
                .AddImports("Arrowgene.Ddon.Shared")
                .AddImports("Arrowgene.Ddon.Shared.Model")
                .AddImports("Arrowgene.Ddon.GameServer")
                .AddImports("Arrowgene.Ddon.GameServer.Characters")
                .AddImports("Arrowgene.Ddon.GameServer.Scripting")
                .AddImports("Arrowgene.Ddon.GameServer.Scripting.Interfaces")
                .AddImports("Arrowgene.Ddon.Shared.Entity.PacketStructure")
                .AddImports("Arrowgene.Ddon.Shared.Entity.Structure")
                .AddImports("Arrowgene.Ddon.Shared.Model.Quest")
                .AddImports("Arrowgene.Ddon.Server.Scripting");
        }

        public override bool EvaluateResult(string path, ScriptState<object> result)
        {
            if (result == null)
            {
                return false;
            }

            IQuest quest = (IQuest)result.ReturnValue;
            if (quest == null)
            {
                return false;
            }

            if (!Quests.ContainsKey(quest.QuestId))
            {
                Quests[quest.QuestId] = new List<IQuest>();
            }

            if (QuestsByScheduleId.ContainsKey(quest.QuestScheduleId))
            {
                Quests[quest.QuestId].RemoveAll(x => x.QuestScheduleId == quest.QuestScheduleId);
            }

            // Initialize any state required
            quest.Initialize(path);

            Quests[quest.QuestId].Add(quest);
            QuestsByScheduleId[quest.QuestScheduleId] = quest;

            return true;
        }
    }
}
