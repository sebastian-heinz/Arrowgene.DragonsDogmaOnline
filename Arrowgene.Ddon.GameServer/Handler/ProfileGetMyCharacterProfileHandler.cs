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
    public class ProfileGetMyCharacterProfileHandler : StructurePacketHandler<GameClient, C2SProfileGetMyCharacterProfileReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ProfileGetMyCharacterProfileHandler));
        
        public ProfileGetMyCharacterProfileHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SProfileGetMyCharacterProfileReq> packet)
        {
            // TODO: Proper implementation
            client.Send(new S2CProfileGetMyCharacterProfileRes());
        }
    }
}