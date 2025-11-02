using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetRentedPawnListHandler : GameRequestPacketHandler<C2SPawnGetRentedPawnListReq, S2CPawnGetRentedPawnListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetRentedPawnListHandler));

        public PawnGetRentedPawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnGetRentedPawnListRes Handle(GameClient client, C2SPawnGetRentedPawnListReq request)
        {
            var response = new S2CPawnGetRentedPawnListRes();
            for (int i = 0; i < client.Character.RentedPawns.Count; i++)
            {
                var pawn = client.Character.RentedPawns[i];
                var cdata = pawn.CDataRentedPawnList;
                cdata.SlotNo = (uint)(i + 1);
                response.RentedPawnList.Add(cdata);
            }

            return response;
        }
    }
}
