using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Scripting;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Scripting
{
    public class GlobalVariables
    {
        public GlobalVariables()
        {
        }
    };

    public class GameServerScriptManager : ScriptManager<GlobalVariables>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GameServerScriptManager));

        private DdonGameServer Server { get; }
        private GlobalVariables Globals { get; }
        public NpcExtendedFacilityModule NpcExtendedFacilityModule { get; private set; } = new NpcExtendedFacilityModule();
        public GameItemModule GameItemModule { get; private set; } = new GameItemModule();
        public MixinModule MixinModule { get; private set; } = new MixinModule();
        public QuestModule QuestModule { get; private set; } = new QuestModule();

        public GameServerScriptManager(DdonGameServer server) : base(server.AssetRepository.AssetsPath, "libs")
        {
            Server = server;
            Globals = new GlobalVariables();

            // Initialize LibDdon Singleton
            LibDdon.SetServer(server);

            // Add modules to the list so the generic logic can iterate over all scripting modules
            ScriptModules[NpcExtendedFacilityModule.ModuleRoot] = NpcExtendedFacilityModule;
            ScriptModules[GameItemModule.ModuleRoot] = GameItemModule;
            ScriptModules[MixinModule.ModuleRoot] = MixinModule;
            ScriptModules[QuestModule.ModuleRoot] = QuestModule;
        }

        public override void Initialize()
        {
            base.Initialize(Globals);
        }
    }
}
