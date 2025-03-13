using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Scheduler;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartnerPawnNextPresentTimeGetHandler : GameRequestPacketHandler<C2SPartnerPawnNextPresentTimeGetReq,S2CPartnerPawnNextPresentTimeGetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartnerPawnPawnLikabilityReleasedRewardListGetHandler));

        public PartnerPawnNextPresentTimeGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPartnerPawnNextPresentTimeGetRes Handle(GameClient client, C2SPartnerPawnNextPresentTimeGetReq request)
        {
            long timeToNext = 0;
            bool isMax = false;

            Server.Database.ExecuteInTransaction(connection =>
            {
                var partnerPawnData = Server.Database.GetPartnerPawnRecord(client.Character.CharacterId, client.Character.PartnerPawnId, connection) ?? new PartnerPawnData();

                isMax = partnerPawnData.CalculateLikability() >= PartnerPawnManager.MAX_PARTNER_PAWN_LIKABILITY_RATING;
                if (!isMax && Server.Database.HasPartnerPawnLastAffectionIncreaseRecord(client.Character.CharacterId, client.Character.PartnerPawnId, PartnerPawnAffectionAction.Gift, connection))
                {
                    timeToNext = Server.ScheduleManager.TimeToNextTaskUpdate(TaskType.PawnAffectionIncreaseInteractionReset);
                }
            });

            return new S2CPartnerPawnNextPresentTimeGetRes()
            {
                RemainSec = (uint) timeToNext,
                IsMax = isMax
            };
        }
    }
}
