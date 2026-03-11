using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetMyPawnDataHandler : GameRequestPacketHandler<C2SPawnGetMyPawnDataReq, S2CPawnGetMyPawnDataRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetMyPawnDataHandler));

        public PawnGetMyPawnDataHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnGetMyPawnDataRes Handle(GameClient client, C2SPawnGetMyPawnDataReq request)
        {
            Pawn pawn = client.Character.PawnBySlotNo(request.SlotNo);

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
                    OwnerBaseInfo = client.Character.CDataCommunityCharacterBaseInfo,
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

            S2CPawnGetPawnOrbDevoteInfoNtc orbNtc = new S2CPawnGetPawnOrbDevoteInfoNtc()
            {
                CharacterId = client.Character.CharacterId,
                PawnId = pawn.PawnId,
                OrbPageStatusList = Server.OrbUnlockManager.GetOrbPageStatus(pawn),
                JobOrbTreeStatusList = Server.JobOrbUnlockManager.GetJobOrbTreeStatus(client.Character, OrbTreeType.Season2),
                JobOrbHiBOStatusList = Server.JobOrbUnlockManager.GetJobOrbTreeStatus(client.Character, OrbTreeType.Season3),
            };
            client.Send(orbNtc);

            var res = new S2CPawnGetMyPawnDataRes
            {
                PawnId = pawn.PawnId,
                PawnInfo = pawn.CDataPawnInfo
            };

            return res;
        }
    }
}
