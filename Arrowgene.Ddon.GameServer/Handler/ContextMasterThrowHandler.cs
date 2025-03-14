using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ContextMasterThrowHandler : GameRequestPacketHandler<C2SContextMasterThrowReq, S2CContextMasterThrowRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ContextMasterThrowHandler));
        
        public ContextMasterThrowHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CContextMasterThrowRes Handle(GameClient client, C2SContextMasterThrowReq request)
        {
            client.Party.SendToAll(new S2CContextMasterThrowNtc()
            {
                Info = request.Info
            });

            return new();
        }
    }
}
