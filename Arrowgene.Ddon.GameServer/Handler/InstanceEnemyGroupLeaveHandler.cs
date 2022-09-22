using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceEnemyGroupLeaveHandler : StructurePacketHandler<GameClient, C2SInstanceEnemyGroupLeaveNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceEnemyGroupLeaveHandler));

        public InstanceEnemyGroupLeaveHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceEnemyGroupLeaveNtc> packet)
        {
            // I don't know what we're supposed to do with this info            
        }
    }
}