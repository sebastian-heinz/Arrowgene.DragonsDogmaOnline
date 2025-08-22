using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetRegisteredPawnDataHandler : GameRequestPacketQueueHandler<C2SPawnGetRegisteredPawnDataReq, S2CPawnGetRegisteredPawnDataRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetRegisteredPawnDataHandler));

        public PawnGetRegisteredPawnDataHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SPawnGetRegisteredPawnDataReq request)
        {
            PacketQueue queue = new();

            var mixin = Server.ScriptManager.MixinModule.Get<IRentalCostMixin>("rental_cost");

            Server.Database.ExecuteInTransaction(connection =>
            {
                uint ownerCharacterId = Server.Database.GetPawnOwnerCharacterId((uint)request.PawnId, connection);
                if (ownerCharacterId == 0)
                {
                    throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_PAWN_PARAM_NOT_FOUND);
                }

                var ownerCharacter = Server.CharacterManager.SelectCharacter(ownerCharacterId, true, connection);
                Pawn pawn = ownerCharacter.Pawns.Where(x => x.PawnId == request.PawnId).FirstOrDefault()
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_NOT_FOUNDED);

                HashSet<uint> clanPawns = [.. Server.Database.SelectClanPawns(client.Character.ClanId, limit: 1000, connectionIn: connection)];

                var res = new S2CPawnGetRegisteredPawnDataRes
                {
                    PawnId = (uint)request.PawnId,
                    PawnInfo = pawn.CDataPawnInfo
                };
                res.PawnInfo.AdventureCount = Server.GameSettings.GameServerSettings.RentalPawnAdventureCount;
                res.PawnInfo.MaxAdventureCount = Server.GameSettings.GameServerSettings.RentalPawnAdventureCount;
                res.PawnInfo.CraftCount = Server.GameSettings.GameServerSettings.RentalPawnCraftCount;
                res.PawnInfo.MaxCraftCount = Server.GameSettings.GameServerSettings.RentalPawnCraftCount;

                client.Enqueue(res, queue);

                //S2C_PAWN_GET_PAWN_PROFILE_NTC
                var profileNtc = new S2CPawnGetPawnProfileNtc()
                {
                    CharacterId = ownerCharacterId,
                    PawnId = pawn.PawnId,
                    OwnerBaseInfo = Server.Database.SelectCommunityCharacterBaseInfo(ownerCharacterId, connection),
                    PawnProfile = pawn.CharacterProfile.CDataArisenProfile,
                    Comment = pawn.CharacterProfile.Comment,
                    RentalCost = mixin.GetRentalCost(client, pawn.CDataRegisterdPawnList, clanPawns.Contains(pawn.PawnId))
                };
                client.Enqueue(profileNtc, queue);

                //S2C_PAWN_GET_PAWN_HISTORY_INFO_NTC
                var historyNtc = new S2CPawnGetPawnHistoryInfoNtc()
                {
                    CharacterId = ownerCharacterId,
                    PawnId = pawn.PawnId,
                    PawnHistoryList = Server.Database.SelectPawnHistory(pawn.PawnId, connection)
                };
                client.Enqueue(historyNtc, queue);

                //S2C_PAWN_GET_PAWN_TOTAL_SCORE_INFO_NTC
                var scoreNtc = new S2CPawnGetPawnTotalScoreInfoNtc()
                {
                    CharacterId = ownerCharacterId,
                    PawnId = pawn.PawnId,
                    PawnTotalScore = Server.Database.SelectPawnTotalScore(pawn.PawnId, connection)
                };
                client.Enqueue(scoreNtc, queue);
            });

            return queue;
        }
    }
}
