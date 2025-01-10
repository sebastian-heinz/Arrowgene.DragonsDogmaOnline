using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanPartnerPawnDataGetHandler : GameRequestPacketHandler<C2SClanClanPartnerPawnDataGetReq, S2CClanClanPartnerPawnDataGetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanPartnerPawnDataGetHandler));

        public ClanClanPartnerPawnDataGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanPartnerPawnDataGetRes Handle(GameClient client, C2SClanClanPartnerPawnDataGetReq request)
        {
            S2CClanClanPartnerPawnDataGetRes res = new S2CClanClanPartnerPawnDataGetRes();

            res.PawnId = request.PawnId;

            Pawn pawn = null;
            Server.Database.ExecuteInTransaction(connection =>
            {
                uint ownerCharacterId = Server.Database.GetPawnOwnerCharacterId((uint)request.PawnId, connection);
                if (ownerCharacterId == 0)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_PAWN_PARAM_NOT_FOUND);
                }

                var ownerCharacter = Server.CharacterManager.SelectCharacter(ownerCharacterId, connectionIn:connection);
                pawn = ownerCharacter.Pawns.Find(x => x.PawnId == request.PawnId)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_INVALID);
            });
            GameStructure.CDataNoraPawnInfo(res.PawnInfo, pawn, Server);

            return res;
        }
    }
}
