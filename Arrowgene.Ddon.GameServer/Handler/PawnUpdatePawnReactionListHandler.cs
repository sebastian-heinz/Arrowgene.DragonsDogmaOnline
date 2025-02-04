using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnUpdatePawnReactionListHandler : GameRequestPacketHandler<C2SPawnUpdatePawnReactionListReq, S2CPawnUpdatePawnReactionListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnDeleteMyPawnHandler));

        public PawnUpdatePawnReactionListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnUpdatePawnReactionListRes Handle(GameClient client, C2SPawnUpdatePawnReactionListReq request)
        {
            Pawn pawn = client.Character.Pawns.Find(x => x.PawnId == request.PawnId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_INVALID);

            pawn.PawnReactionList = request.PawnReactionList;
            Server.Database.ExecuteInTransaction(conn =>
            {
                foreach (var reaction in request.PawnReactionList)
                {
                    Server.Database.ReplacePawnReaction(request.PawnId, reaction, conn);
                }
            });
                
            var res = new S2CPawnUpdatePawnReactionListRes()
            {
                PawnId = request.PawnId,
                PawnReactionList = pawn.PawnReactionList
            };
            var ntc = new S2CPawnUpdatePawnReactionListNtc()
            {
                PawnId = request.PawnId,
                PawnReactionList = pawn.PawnReactionList
            };

            client.Party.SendToAllExcept(ntc, client);
            return res;
        }
    }
}
