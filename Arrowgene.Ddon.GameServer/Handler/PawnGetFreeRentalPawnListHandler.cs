using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetFreeRentalPawnListHandler : GameRequestPacketHandler<C2SPawnGetFreeRentalPawnListReq, S2CPawnGetFreeRentalPawnListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetFreeRentalPawnListHandler));

        public PawnGetFreeRentalPawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnGetFreeRentalPawnListRes Handle(GameClient client, C2SPawnGetFreeRentalPawnListReq request)
        {
            S2CPawnGetFreeRentalPawnListRes res = new S2CPawnGetFreeRentalPawnListRes();

            // TODO: track free rental pawns courses in DB
            res.FreeRentalPawnList.Add(
                new CDataFreeRentalPawnList
                {
                    PawnId = 1,
                    Name = "FreeRentalPawn1 (active)",
                    ImageAddr = "http://localhost:52099/shop/img/payment/icon_pawn1.png",
                    LineupId = 1,
                    ExpireDateTime = (ulong)DateTimeOffset.UtcNow.AddMonths(12).ToUnixTimeSeconds()
                });
            res.FreeRentalPawnList.Add(
                new CDataFreeRentalPawnList
                {
                    PawnId = 2,
                    Name = "FreeRentalPawn2 (expired)",
                    ImageAddr = "http://localhost:52099/shop/img/payment/icon_pawn1.png",
                    LineupId = 2,
                    ExpireDateTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 1000
                });

            return res;
        }
    }
}
