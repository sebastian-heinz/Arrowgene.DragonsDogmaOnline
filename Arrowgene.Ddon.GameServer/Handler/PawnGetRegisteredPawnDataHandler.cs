using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetRegisteredPawnDataHandler : StructurePacketHandler<GameClient, C2SPawnGetRegisteredPawnDataReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetRegisteredPawnDataHandler));

        public PawnGetRegisteredPawnDataHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnGetRegisteredPawnDataReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.Id == packet.Structure.PawnId).Single();
            var res = new S2CPawnGetRegisteredPawnDataRes();
            GameStructure.CDataPawnInfo(res.PawnInfo, pawn);
            client.Send(res);
        }
    }
}