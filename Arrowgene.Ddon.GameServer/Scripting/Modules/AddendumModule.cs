using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Microsoft.CodeAnalysis.Scripting;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class AddendumModule : GameServerScriptModule
    {
        public override string ModuleRoot => "addendums";
        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;
        public override bool EnableHotLoad => true;

        public AddendumModule()
        {
        }

        public override bool EvaluateResult(string path, ScriptState<object> result)
        {
            if (result == null)
            {
                return false;
            }

            IAddendum addendum = (IAddendum)result.ReturnValue;
            addendum.Amend();

            return true;
        }
    }
}
