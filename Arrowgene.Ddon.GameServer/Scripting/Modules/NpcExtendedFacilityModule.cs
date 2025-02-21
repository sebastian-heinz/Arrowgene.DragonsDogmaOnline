using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Shared.Model;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class NpcExtendedFacilityModule : GameServerScriptModule
    {
        public override string ModuleRoot => "extended_facilities";
        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;
        public override bool EnableHotLoad => true;

        public Dictionary<NpcId, INpcExtendedFacility> NpcExtendedFacilities { get; private set; }

        public NpcExtendedFacilityModule()
        {
            NpcExtendedFacilities = new Dictionary<NpcId, INpcExtendedFacility>();
        }

        public override bool EvaluateResult(string path, ScriptState<object> result)
        {
            if (result == null)
            {
                return false;
            }

            INpcExtendedFacility extendedFacility = (INpcExtendedFacility)result.ReturnValue;
            NpcExtendedFacilities[extendedFacility.NpcId] = extendedFacility;

            return true;
        }
    }
}
