using Arrowgene.Ddon.Server.Scripting.interfaces;
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

        public GameServerSettingsModule GameServerSettings { get; private set; } = new GameServerSettingsModule();

        public ServerScriptManager(string assetsPath) : base(assetsPath, "")
        {
            Globals = new Globals();

            // Add modules to the list so the generic logic can iterate over all scripting modules
            ScriptModules[GameServerSettings.ModuleRoot] = GameServerSettings;
        }

        public override void Initialize()
        {
            base.Initialize(Globals);
        }
    }
}
