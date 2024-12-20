using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class NpcExtendedFacilityModule : ScriptModule
    {
        public override string ModuleRoot => "extended_facilities";
        public override string Filter => "*.csx";
        public override bool ScanSubdirectories => true;

        public Dictionary<NpcId, INpcExtendedFacility> NpcExtendedFacilities { get; private set; }

        public NpcExtendedFacilityModule()
        {
            NpcExtendedFacilities = new Dictionary<NpcId, INpcExtendedFacility>();
        }

        public override ScriptOptions Options()
        {
            return ScriptOptions.Default
                .AddReferences(MetadataReference.CreateFromFile(typeof(DdonGameServer).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(AssetRepository).Assembly.Location))
                .AddImports("System", "System.Collections", "System.Collections.Generic")
                .AddImports("Arrowgene.Ddon.Shared")
                .AddImports("Arrowgene.Ddon.Shared.Model")
                .AddImports("Arrowgene.Ddon.GameServer")
                .AddImports("Arrowgene.Ddon.GameServer.Characters")
                .AddImports("Arrowgene.Ddon.GameServer.Scripting")
                .AddImports("Arrowgene.Ddon.GameServer.Scripting.Interfaces")
                .AddImports("Arrowgene.Ddon.Shared.Entity.PacketStructure")
                .AddImports("Arrowgene.Ddon.Shared.Entity.Structure")
                .AddImports("Arrowgene.Ddon.Shared.Model.Quest");
        }

        public override bool EvaluateResult(ScriptState<object> result)
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
