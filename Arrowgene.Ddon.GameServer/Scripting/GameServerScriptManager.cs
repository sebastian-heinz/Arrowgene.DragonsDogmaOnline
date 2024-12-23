using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Scripting;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class GlobalVariables
    {
        public GlobalVariables(DdonGameServer server)
        {
            Server = server;
        }

        public DdonGameServer Server { get; }
    };

    public class GameServerScriptManager : ScriptManager<GlobalVariables>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GameServerScriptManager));

        private DdonGameServer Server { get; }
        private GlobalVariables Globals { get; }
        public NpcExtendedFacilityModule NpcExtendedFacilityModule { get; private set; } = new NpcExtendedFacilityModule();

        public GameServerScriptManager(DdonGameServer server) : base(server.AssetRepository.AssetsPath)
        {
            Server = server;
            Globals = new GlobalVariables(Server);

            // Add modules to the list so the generic logic can iterate over all scripting modules
            ScriptModules[NpcExtendedFacilityModule.ModuleRoot] = NpcExtendedFacilityModule;
        }

        public override void Initialize()
        {
            base.Initialize(Globals);
        }
    }
}
