using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionGetLoginAnnouncementHandler : GameRequestPacketHandler<C2SConnectionGetLoginAnnouncementReq, S2CConnectionGetLoginAnnouncementRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ConnectionGetLoginAnnouncementHandler));

        public ConnectionGetLoginAnnouncementHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CConnectionGetLoginAnnouncementRes Handle(GameClient client, C2SConnectionGetLoginAnnouncementReq request)
        {
            S2CConnectionGetLoginAnnouncementRes res = EntitySerializer.Get<S2CConnectionGetLoginAnnouncementRes>().Read(GameFull.Dump_153.AsBuffer());

            // This dump just contains the string "お知らせ～", or "Announcement~".
            // I have no idea what the client does with this string, because it doesn't appear to display it anywhere.

            return res;
        }
    }
}
