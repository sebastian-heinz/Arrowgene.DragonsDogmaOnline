using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ProfileGetMyCharacterProfileHandler : GameRequestPacketHandler<C2SProfileGetMyCharacterProfileReq, S2CProfileGetMyCharacterProfileRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ProfileGetMyCharacterProfileHandler));

        public ProfileGetMyCharacterProfileHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CProfileGetMyCharacterProfileRes Handle(GameClient client, C2SProfileGetMyCharacterProfileReq request)
        {
            return new S2CProfileGetMyCharacterProfileRes
            {
                HistoryElementList = Server.AchievementManager.GetArisenAchievementHistory(client),
                AchieveCategoryStatusList = Server.AchievementManager.GetCategoryStatus(client),
                OrbStatusList = Server.OrbUnlockManager.GetOrbPageStatus(client.Character),
                JobOrbTreeStatusList = Server.JobOrbUnlockManager.GetJobOrbTreeStatus(client.Character),
                AbilityCostMax = Server.CharacterManager.GetMaxAugmentAllocation(client.Character)
            };
        }
    }
}
