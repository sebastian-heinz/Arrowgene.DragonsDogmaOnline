using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartnerPawnSetHandler : GameRequestPacketHandler<C2SPartnerPawnSetReq, S2CPartnerPawnSetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartnerPawnSetHandler));

        public PartnerPawnSetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPartnerPawnSetRes Handle(GameClient client, C2SPartnerPawnSetReq request)
        {
            S2CPartnerPawnSetRes res = new S2CPartnerPawnSetRes();

            Pawn pawn = client.Character.Pawns.Where(p => p.PawnId == request.PawnId).FirstOrDefault() ??
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_NOT_FOUNDED, $"A pawn with the ID {request.PawnId} doesn't exist");

            client.Character.PartnerPawnId = pawn.PawnId;
            Server.Database.ExecuteInTransaction(connection =>
            {
                Server.Database.SetPartnerPawn(client.Character.CharacterId, pawn.PawnId, connection);

                var record = Server.PartnerPawnManager.GetPartnerPawnData(client, connection);
                if (record == null)
                {
                    record = new PartnerPawnData()
                    {
                        PawnId = pawn.PawnId,
                        NumGifts = 0,
                        NumCrafts = 0,
                        NumAdventures = 0,
                    };
                    pawn.PartnerPawnData = record;
                    Server.Database.InsertPartnerPawnRecord(client.Character.CharacterId, record, connection);
                }
                res.PartnerInfo = Server.PartnerPawnManager.GetCDataPartnerPawnData(client, connection) ?? new CDataPartnerPawnData();
            });

            return res;
        }
    }
}
