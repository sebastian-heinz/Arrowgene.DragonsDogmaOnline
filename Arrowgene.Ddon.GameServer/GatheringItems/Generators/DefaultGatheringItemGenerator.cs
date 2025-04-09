using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.GatheringItems.Generators
{
    public class DefaultGatheringItemGenerator : IGatheringGenerator
    {
        private readonly DdonGameServer Server;

        public DefaultGatheringItemGenerator(DdonGameServer server)
        {
            Server = server;
        }

        public override bool IsEnabled()
        {
            return Server.GameSettings.GameServerSettings.EnableDefaultGatheringDrops;
        }

        public override List<InstancedGatheringItem> Generate(GameClient client, StageLayoutId stageLayoutId, uint index)
        {
            var mixin = Server.ScriptManager.MixinModule.Get<IDefaultGatherMixin>("default_gathering");
            return mixin.GenerateGatheringDrops(client, stageLayoutId, index);
        }
    }
}

