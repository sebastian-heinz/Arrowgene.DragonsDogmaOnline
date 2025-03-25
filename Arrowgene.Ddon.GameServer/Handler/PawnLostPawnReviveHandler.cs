using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnLostPawnReviveHandler : GameRequestPacketHandler<C2SPawnLostPawnReviveReq, S2CPawnLostPawnReviveRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnLostPawnReviveHandler));
        
        public PawnLostPawnReviveHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnLostPawnReviveRes Handle(GameClient client, C2SPawnLostPawnReviveReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);
            pawn.PawnState = PawnState.None;
            Server.Database.UpdatePawnBaseInfo(pawn);

            return new S2CPawnLostPawnReviveRes()
            {
                PawnId = request.PawnId
            };
        }
    }
}
