using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

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

            Pawn pawn = client.Character.Pawns.Find(p => p.PawnId == request.PawnId);

            client.Character.PartnerPawnId = pawn.PawnId;
            Server.Database.ExecuteInTransaction(connection =>
            {
                Server.Database.SetPartnerPawn(client.Character.CharacterId, pawn.PawnId, connection);

                var record = Server.Database.GetPartnerPawnRecord(client.Character.CharacterId, pawn.PawnId, connection);
                if (record == null)
                {
                    record = new PartnerPawnData()
                    {
                        PawnId = pawn.PawnId,
                        Personality = PawnPersonality.Normal,
                        NumGifts = 0,
                        NumCrafts = 0,
                        NumAdventures = 0,
                    };
                    Server.Database.InsertPartnerPawnRecord(client.Character.CharacterId, record, connection);
                }
                res.PartnerInfo = record.ToCDataPartnerPawnData();
            });

            return res;
        }
    }
}
