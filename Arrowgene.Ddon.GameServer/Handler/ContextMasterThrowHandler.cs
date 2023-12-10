using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ContextMasterThrowHandler : GameStructurePacketHandler<C2SContextMasterThrowReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ContextMasterThrowHandler));
        
        public ContextMasterThrowHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SContextMasterThrowReq> packet)
        {
            client.Send(new S2CContextMasterThrowRes());

            client.Party.SendToAll(new S2CContextMasterThrowNtc()
            {
                Info = packet.Structure.Info
            });
        }
    }
}