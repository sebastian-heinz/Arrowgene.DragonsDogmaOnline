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
    public class PawnGetPartyPawnDataHandler : GameRequestPacketHandler<C2SPawnGetPartyPawnDataReq, S2CPawnGetPartyPawnDataRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetPartyPawnDataHandler));

        private readonly DdonGameServer Server;

        public PawnGetPartyPawnDataHandler(DdonGameServer server) : base(server)
        {
            Server = server;
        }

        public override S2CPawnGetPartyPawnDataRes Handle(GameClient client, C2SPawnGetPartyPawnDataReq packet)
        {
            GameClient owner = Server.ClientLookup.GetClientByCharacterId(packet.CharacterId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_PARAM_NOT_FOUND);

            List<Pawn> pawns = [.. owner.Character.Pawns, .. owner.Character.RentedPawns];
            Pawn pawn = pawns
                .Find(pawn => pawn.PawnId == packet.PawnId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_NOT_FOUNDED,
                $"Couldn't find pawn ID {packet.PawnId}.");

            var res = new S2CPawnGetPartyPawnDataRes
            {
                CharacterId = pawn.CharacterId,
                PawnId = pawn.PawnId,
                PawnInfo = pawn.CDataPawnInfo
            };

            var mixin = Server.ScriptManager.MixinModule.Get<IRentalCostMixin>("rental_cost");
            PacketQueue queue = new();

            Server.Database.ExecuteInTransaction(connection =>
            {
                HashSet<uint> clanPawns = [.. Server.Database.SelectClanPawns(client.Character.ClanId, limit: 1000, connectionIn: connection)];

                if (pawn is RentalPawn rentalPawn)
                {
                    // This is a rented pawn, the real owner may not be online.
                    //S2C_PAWN_GET_PAWN_PROFILE_NTC
                    var profileNtc = new S2CPawnGetPawnProfileNtc()
                    {
                        CharacterId = client.Character.CharacterId,
                        PawnId = rentalPawn.PawnId,
                        OwnerBaseInfo = Server.Database.SelectCommunityCharacterBaseInfo(rentalPawn.OwningCharacterId, connection),
                        PawnProfile = rentalPawn.CharacterProfile.CDataArisenProfile,
                        Comment = rentalPawn.CharacterProfile.Comment,
                        RentalCost = mixin.GetRentalCost(client, rentalPawn.CDataRegisterdPawnList, clanPawns.Contains(rentalPawn.PawnId))
                    };
                    client.Enqueue(profileNtc, queue);

                    //S2C_PAWN_GET_PAWN_HISTORY_INFO_NTC
                    var historyNtc = new S2CPawnGetPawnHistoryInfoNtc()
                    {
                        CharacterId = client.Character.CharacterId,
                        PawnId = rentalPawn.PawnId,
                        PawnHistoryList = Server.Database.SelectPawnHistory(rentalPawn.PawnId, connection)
                    };
                    client.Enqueue(historyNtc, queue);

                    //S2C_PAWN_GET_PAWN_TOTAL_SCORE_INFO_NTC
                    var scoreNtc = new S2CPawnGetPawnTotalScoreInfoNtc()
                    {
                        CharacterId = client.Character.CharacterId,
                        PawnId = rentalPawn.PawnId,
                        PawnTotalScore = Server.Database.SelectPawnTotalScore(rentalPawn.PawnId, connection)
                    };
                    client.Enqueue(scoreNtc, queue);

                    res.PawnInfo = rentalPawn.CDataPawnInfo;
                }
                else
                {
                    // This is a main pawn, belonging to either the querying client or someone in their party.
                
                    //S2C_PAWN_GET_PAWN_PROFILE_NTC
                    var profileNtc = new S2CPawnGetPawnProfileNtc()
                    {
                        CharacterId = pawn.CharacterId,
                        PawnId = pawn.PawnId,
                        OwnerBaseInfo = owner.Character.CDataCommunityCharacterBaseInfo,
                        PawnProfile = pawn.CharacterProfile.CDataArisenProfile,
                        Comment = pawn.CharacterProfile.Comment,
                        RentalCost = mixin.GetRentalCost(client, pawn.CDataRegisterdPawnList, clanPawns.Contains(pawn.PawnId))
                    };
                    client.Enqueue(profileNtc, queue);

                    //S2C_PAWN_GET_PAWN_HISTORY_INFO_NTC
                    var historyNtc = new S2CPawnGetPawnHistoryInfoNtc()
                    {
                        CharacterId = pawn.CharacterId,
                        PawnId = pawn.PawnId,
                        PawnHistoryList = Server.Database.SelectPawnHistory(pawn.PawnId, connection)
                    };
                    client.Enqueue(historyNtc, queue);

                    //S2C_PAWN_GET_PAWN_TOTAL_SCORE_INFO_NTC
                    var scoreNtc = new S2CPawnGetPawnTotalScoreInfoNtc()
                    {
                        CharacterId = pawn.CharacterId,
                        PawnId = pawn.PawnId,
                        PawnTotalScore = Server.Database.SelectPawnTotalScore(pawn.PawnId, connection)
                    };
                    client.Enqueue(scoreNtc, queue);

                    //S2C_PAWN_GET_PAWN_ORB_DEVOTE_INFO_NTC
                    S2CPawnGetPawnOrbDevoteInfoNtc ntc = new S2CPawnGetPawnOrbDevoteInfoNtc()
                    {
                        CharacterId = pawn.CharacterId,
                        PawnId = pawn.PawnId,
                        OrbPageStatusList = Server.OrbUnlockManager.GetOrbPageStatus(pawn),
                        JobOrbTreeStatusList = Server.JobOrbUnlockManager.GetJobOrbTreeStatus(owner.Character, OrbTreeType.Season2),
                        JobOrbHiBOStatusList = Server.JobOrbUnlockManager.GetJobOrbTreeStatus(owner.Character, OrbTreeType.Season3),
                    };
                    client.Enqueue(ntc, queue);
                }
            });
            queue.Send();

            return res;
        }
    }
}
