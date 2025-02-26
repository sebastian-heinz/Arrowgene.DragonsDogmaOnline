using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Scripting;
using Arrowgene.Logging;
using System.IO;

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
        public ChatCommandModule ChatCommandModule { get; private set; } = new ChatCommandModule();
        public PointModifierModule PointModifierModule { get; private set; } = new PointModifierModule();
        public AddendumModule AddendumModule { get; private set; } = new AddendumModule();

        public GameServerScriptManager(DdonGameServer server) : base(server.AssetRepository.AssetsPath, "libs")
        {
            Server = server;
            Globals = new GlobalVariables();

            // Initialize LibDdon Singleton
            LibDdon.SetServer(server);

            // Add modules to the list so the generic logic can iterate over all scripting modules
            ScriptModules[ChatCommandModule.ModuleRoot] = ChatCommandModule;
            ScriptModules[GameItemModule.ModuleRoot] = GameItemModule;
            ScriptModules[MixinModule.ModuleRoot] = MixinModule;
            ScriptModules[NpcExtendedFacilityModule.ModuleRoot] = NpcExtendedFacilityModule;
            ScriptModules[QuestModule.ModuleRoot] = QuestModule;
            ScriptModules[PointModifierModule.ModuleRoot] = PointModifierModule;

            // This module should run last since it applies fine grained modifications to other modules
            ScriptModules[AddendumModule.ModuleRoot] = AddendumModule;
        }

        public override void Initialize()
        {
            base.Initialize(Globals);
        }

        protected override void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            var module = GetModuleFromFilePath(e.FullPath);
            if (module == null)
            {
                // No module associated with this script
                return;
            }

            base.OnChanged(sender, e);

            if (!module.GetType().Equals(typeof(AddendumModule)))
            {
                if (AddendumModule.Scripts.Count == 0)
                {
                    // No addendums to be reapplied
                    return;
                }

                try
                {
                    Logger.Info("Reapplying addendums");
                    foreach (var watcher in AddendumModule.Watchers)
                    {
                        watcher.EnableRaisingEvents = false;
                    }

                    foreach (var script in AddendumModule.Scripts)
                    {
                        CompileScript(AddendumModule, script);
                    }
                }
                finally
                {
                    foreach (var watcher in AddendumModule.Watchers)
                    {
                        watcher.EnableRaisingEvents = true;
                    }
                }
            }
        }
    }
}
