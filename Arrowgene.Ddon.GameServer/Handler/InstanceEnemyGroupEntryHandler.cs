using Arrowgene.Ddon.GameServer.Context;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceEnemyGroupEntryHandler : StructurePacketHandler<GameClient, C2SInstanceEnemyGroupEntryNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceEnemyGroupEntryHandler));

        public InstanceEnemyGroupEntryHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceEnemyGroupEntryNtc> packet)
        {
            CDataStageLayoutId layout = packet.Structure.LayoutId;

            ContextManager.HandleEntry(client, layout);
        }
    }
}
