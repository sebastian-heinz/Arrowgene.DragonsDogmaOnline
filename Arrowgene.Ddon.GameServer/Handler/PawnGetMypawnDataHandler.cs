using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetMypawnDataHandler : StructurePacketHandler<GameClient, C2SPawnGetMypawnDataReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetMypawnDataHandler));


        public PawnGetMypawnDataHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnGetMypawnDataReq> req)
        {
                Pawn pawn = client.Character.PawnBySlotNo(req.Structure.SlotNo);
                var res = new S2CPawnGetMypawnDataRes();
                res.PawnId = pawn.PawnId;
                GameStructure.CDataPawnInfo(res.PawnInfo, pawn);
                client.Send(res);
        }
    }
}
