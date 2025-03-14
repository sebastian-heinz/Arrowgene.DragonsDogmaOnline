using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterSetOnlineStatusHandler : GameRequestPacketHandler<C2SCharacterSetOnlineStatusReq, S2CCharacterSetOnlineStatusRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterSetOnlineStatusHandler));

        public CharacterSetOnlineStatusHandler(DdonGameServer server) : base(server)
        {
        }


        public override S2CCharacterSetOnlineStatusRes Handle(GameClient client, C2SCharacterSetOnlineStatusReq request)
        {
            client.Character.OnlineStatus = request.OnlineStatus;

            // TODO: Figure out request.IsSaveSetting

            return new S2CCharacterSetOnlineStatusRes()
            {
                OnlineStatus = request.OnlineStatus
            };
        }
    }
}
