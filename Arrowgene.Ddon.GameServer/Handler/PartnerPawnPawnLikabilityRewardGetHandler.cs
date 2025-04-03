using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartnerPawnPawnLikabilityRewardGetHandler : GameRequestPacketHandler<C2SPartnerPawnPawnLikabilityRewardGetReq, S2CPartnerPawnPawnLikabilityRewardGetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartnerPawnPawnLikabilityRewardGetHandler));

        public PartnerPawnPawnLikabilityRewardGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPartnerPawnPawnLikabilityRewardGetRes Handle(GameClient client, C2SPartnerPawnPawnLikabilityRewardGetReq request)
        {
            // Returns pending rewards when player clicks on the pawn
            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (var reward in request.RewardUidList)
                {
                    switch (reward.Type)
                    {
                        case PawnLikabilityRewardType.Ability:
                            Server.Database.InsertSecretAbilityUnlock(client.Character.CommonId, (AbilityId)reward.Value.UID, connection);
                            break;
                    }

                    uint level = Server.PartnerPawnManager.GetLevelForReward(reward);
                    if (level > 0)
                    {
                        Server.Database.DeletePartnerPawnPendingReward(client.Character.CharacterId, client.Character.PartnerPawnId, level, connection);
                    }
                }
            });

            // What is the client expecting to be sent back to it?
            // Seems strange we would send back the same list?
            return new S2CPartnerPawnPawnLikabilityRewardGetRes()
            {
                RewardList = request.RewardUidList
            };
        }
    }
}
