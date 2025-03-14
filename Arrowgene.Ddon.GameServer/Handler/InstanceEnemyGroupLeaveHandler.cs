using Arrowgene.Ddon.GameServer.Context;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceEnemyGroupLeaveHandler : GameStructurePacketHandler<C2SInstanceEnemyGroupLeaveNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceEnemyGroupLeaveHandler));

        public InstanceEnemyGroupLeaveHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceEnemyGroupLeaveNtc> packet)
        {
            ContextManager.DelegateMaster(client, packet.Structure.LayoutId);
        }
    }
}
