using System.Collections.Generic;
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
    public class CraftCancelCraftHandler : GameStructurePacketHandler<C2SCraftCancelCraftReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftCancelCraftHandler));

        public CraftCancelCraftHandler(DdonGameServer server) : base(server)
        {
        }
        
        public override void Handle(GameClient client, StructurePacket<C2SCraftCancelCraftReq> packet)
        {
            Server.Database.DeletePawnCraftProgress(client.Character.CharacterId, packet.Structure.CraftMainPawnID);
            
            client.Send(new C2SCraftCancelCraftRes());
        }
    }
}
