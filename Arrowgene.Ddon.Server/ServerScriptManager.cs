using Arrowgene.Ddon.Server.Scripting.modules;
using Arrowgene.Ddon.Shared.Scripting;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Server
{
    public class Globals
    {
        // Currently no script globals are required
        // but leave this class as a way to introduce
        // globals if needed.
    }

    public class ServerScriptManager : ScriptManager<Globals>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ServerScriptManager));

        private Globals Globals { get; set; }

        public GameServerSettingsModule GameServerSettingsModule { get; private set; }
        public ServerScriptManager(string assetsPath) : base(assetsPath, "")
        {
            Globals = new Globals();

            GameServerSettingsModule = new GameServerSettingsModule(ScriptsRoot);

            // Add modules to the list so the generic logic can iterate over all scripting modules
            ScriptModules[GameServerSettingsModule.ModuleRoot] = GameServerSettingsModule;
        }

        public override void Initialize()
        {
            PathsToIgnore.Add(GameServerSettingsModule.TemplatesDirectory);
            base.Initialize(Globals);
        }
    }
}
