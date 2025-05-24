using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class JobEmblemStatModule : GameServerScriptModule
    {
        public override string ModuleRoot => Path.Combine("emblems", "stats");
        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;
        public override bool EnableHotLoad => true;

        private Dictionary<EquipStatId, List<CDataJobEmblemStatUpgradeData>> StatTables { get; set; }

        public JobEmblemStatModule()
        {
            StatTables = new Dictionary<EquipStatId, List<CDataJobEmblemStatUpgradeData>>();
        }

        public List<CDataJobEmblemStatUpgradeData> GetData()
        {
            return StatTables.SelectMany(x => x.Value).ToList();
        }

        public ushort GetStatAmount(EquipStatId equipStatId, byte level)
        {
            return (ushort) StatTables[equipStatId].Where(x => x.Level <= level).Sum(x => x.Amount);
        }

        public override bool EvaluateResult(string path, ScriptState<object> result)
        {
            if (result == null)
            {
                return false;
            }

            var statTable = (IJobEmblemStatTable)result.ReturnValue;
            if (statTable != null)
            {
                StatTables[statTable.StatId] = statTable.StatUpgradeData;
            }

            return true;
        }
    }
}
