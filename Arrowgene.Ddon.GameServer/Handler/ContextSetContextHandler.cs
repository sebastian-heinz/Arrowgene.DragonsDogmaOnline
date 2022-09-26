using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ContextSetContextHandler : StructurePacketHandler<GameClient, C2SContextSetContextNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ContextSetContextHandler));


        public ContextSetContextHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SContextSetContextNtc> packet)
        {
            // Should this be stored and later be sent in the GetSetContextHandler?
            // Or should it be sent immediately?
            // To the client or to all party?
            Tuple<CDataContextSetBase, CDataContextSetAdditional> context = new Tuple<CDataContextSetBase, CDataContextSetAdditional>(packet.Structure.Base, packet.Structure.Additional);
            client.Party.Contexts[packet.Structure.Base.UniqueId] = context;
        }        
    }
}