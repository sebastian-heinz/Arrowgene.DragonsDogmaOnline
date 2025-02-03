using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Shared.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class PointModifierModule : GameServerScriptModule
    {
        public override string ModuleRoot => "point_modifiers";
        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;
        public override bool EnableHotLoad => true;

        private Dictionary<string, IPointModifier> Modifiers { get; set; } = new Dictionary<string, IPointModifier>();

        public List<IPointModifier> GetModifiersByType(PointModifierType type)
        {
            return Modifiers.Select(x => x.Value).Where(x => x.ModifierType == type).ToList();
        }

        public List<IPointModifier> GetModifiersByType(List<PointModifierType> types)
        {
            var results = new List<IPointModifier>();
            foreach (var type in types)
            {
                results.AddRange(GetModifiersByType(type));
            }
            return results;
        }

        public PointModifierModule()
        {
        }

        public override bool EvaluateResult(string path, ScriptState<object> result)
        {
            if (result == null)
            {
                return false;
            }

            string scriptName = Path.GetFileNameWithoutExtension(path);
            Modifiers[scriptName] = (IPointModifier)result.ReturnValue;

            return true;
        }
    }
}
