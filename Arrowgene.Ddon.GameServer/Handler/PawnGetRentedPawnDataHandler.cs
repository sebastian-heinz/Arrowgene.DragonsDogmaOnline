using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetRentedPawnDataHandler : GameRequestPacketHandler<C2SPawnGetRentedPawnDataReq, S2CPawnGetRentedPawnDataRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetRentedPawnDataHandler));

        public PawnGetRentedPawnDataHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnGetRentedPawnDataRes Handle(GameClient client, C2SPawnGetRentedPawnDataReq request)
        {
            if (request.SlotNo == 0)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_INVALID_SLOT_NO);
            }

            var pawn = client.Character.RentedPawns[request.SlotNo - 1];
            var result = new S2CPawnGetRentedPawnDataRes()
            {
                PawnId = pawn.PawnId,
                PawnInfo = pawn.CDataPawnInfo,
            };

            var mixin = Server.ScriptManager.MixinModule.Get<IRentalCostMixin>("rental_cost");
            PacketQueue queue = new();
            Server.Database.ExecuteInTransaction(connection =>
            {
                HashSet<uint> clanPawns = [.. Server.Database.SelectClanPawns(client.Character.ClanId, limit: 1000, connectionIn: connection)];

                //S2C_PAWN_GET_PAWN_PROFILE_NTC
                var profileNtc = new S2CPawnGetPawnProfileNtc()
                {
                    CharacterId = client.Character.CharacterId,
                    PawnId = pawn.PawnId,
                    OwnerBaseInfo = Server.Database.SelectCommunityCharacterBaseInfo(pawn.OwningCharacterId, connection),
                    PawnProfile = pawn.CharacterProfile.CDataArisenProfile,
                    Comment = pawn.CharacterProfile.Comment,
                    RentalCost = mixin.GetRentalCost(client, pawn.CDataRegisterdPawnList, clanPawns.Contains(pawn.PawnId))
                };
                client.Enqueue(profileNtc, queue);

                //S2C_PAWN_GET_PAWN_HISTORY_INFO_NTC
                var historyNtc = new S2CPawnGetPawnHistoryInfoNtc()
                {
                    CharacterId = client.Character.CharacterId,
                    PawnId = pawn.PawnId,
                    PawnHistoryList = Server.Database.SelectPawnHistory(pawn.PawnId, connection)
                };
                client.Enqueue(historyNtc, queue);

                //S2C_PAWN_GET_PAWN_TOTAL_SCORE_INFO_NTC
                var scoreNtc = new S2CPawnGetPawnTotalScoreInfoNtc()
                {
                    CharacterId = client.Character.CharacterId,
                    PawnId = pawn.PawnId,
                    PawnTotalScore = Server.Database.SelectPawnTotalScore(pawn.PawnId, connection)
                };
                client.Enqueue(scoreNtc, queue);
            });

            queue.Send();

            return result;
        }
    }
}
