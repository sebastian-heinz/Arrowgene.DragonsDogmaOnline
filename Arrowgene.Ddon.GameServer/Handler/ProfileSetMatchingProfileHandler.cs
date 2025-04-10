using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ProfileSetMatchingProfileHandler : GameRequestPacketHandler<C2SProfileSetMatchingProfileReq, S2CProfileSetMatchingProfileRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ProfileSetMatchingProfileHandler));

        public ProfileSetMatchingProfileHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CProfileSetMatchingProfileRes Handle(GameClient client, C2SProfileSetMatchingProfileReq request)
        {
            client.Character.MatchingProfile = request.MatchingProfile;
            Server.Database.UpdateCharacterMatchingProfile(client.Character);
            return new();
        }
    }
}
