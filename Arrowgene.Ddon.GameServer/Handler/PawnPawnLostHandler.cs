using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnPawnLostHandler : GameStructurePacketHandler<C2SPawnPawnLostReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnPawnLostHandler));
        
        public PawnPawnLostHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnPawnLostReq> packet)
        {
            // TODO: Lost pawns system
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            client.Send(new S2CPawnPawnLostRes()
            {
                PawnId = pawn.PawnId,
                PawnName = pawn.Name,
                IsLost = false
            });
        }
    }
}