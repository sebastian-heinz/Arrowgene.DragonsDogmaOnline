using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterSetOnlineStatusHandler : GameRequestPacketHandler<C2SCharacterSetOnlineStatusReq, S2CCharacterSetOnlineStatusRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterSetOnlineStatusHandler));

        public CharacterSetOnlineStatusHandler(DdonGameServer server) : base(server)
        {
        }

        private static bool IsPersistentOnlineStatus(OnlineStatus status)
        {
            return status == OnlineStatus.Online
                || status == OnlineStatus.Offline
                || status == OnlineStatus.Leaving
                || status == OnlineStatus.Busy;
        }

        public override S2CCharacterSetOnlineStatusRes Handle(GameClient client, C2SCharacterSetOnlineStatusReq request)
        {
            Server.CharacterManager.UpdateOnlineStatus(client, client.Character, request.OnlineStatus);

            if (request.IsSaveSetting && IsPersistentOnlineStatus(request.OnlineStatus))
            {
                client.Character.SavedOnlineStatus = request.OnlineStatus;
                Server.Database.UpdateCharacterCommonBaseInfo(client.Character);
            }

            return new S2CCharacterSetOnlineStatusRes()
            {
                OnlineStatus = request.OnlineStatus
            };
        }
    }
}
