using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ProfileSetArisenProfileHandler : GameRequestPacketHandler<C2SProfileSetArisenProfileReq, S2CProfileSetArisenProfileRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ProfileSetArisenProfileHandler));

        public ProfileSetArisenProfileHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CProfileSetArisenProfileRes Handle(GameClient client, C2SProfileSetArisenProfileReq request)
        {
            client.Character.ArisenProfile = request.ArisenProfile;
            Server.Database.UpdateCharacterArisenProfile(client.Character);
            return new();
        }
    }
}
