using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class OrbDevoteGetReleaseOrbElementListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(OrbDevoteGetReleaseOrbElementListHandler));

        private IDatabase _Database;

        public OrbDevoteGetReleaseOrbElementListHandler(DdonGameServer server) : base(server)
        {
            _Database = server.Database;
        }

        public override PacketId Id => PacketId.C2S_ORB_DEVOTE_GET_RELEASE_ORB_ELEMENT_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            S2COrbDevoteGetReleaseOrbElementListRes Response = new S2COrbDevoteGetReleaseOrbElementListRes();
            Response.OrbElementList = _Database.SelectOrbReleaseElementFromDragonForceAugmentation(client.Character.CommonId);
            client.Send(Response);
        }
    }
}
