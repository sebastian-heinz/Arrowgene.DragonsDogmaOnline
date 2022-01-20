using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionLoginHandler : StructurePacketHandler<GameClient, C2SConnectionLoginReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ConnectionLoginHandler));


        public ConnectionLoginHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SConnectionLoginReq> packet)
        {
            Logger.Debug(client, $"Received SessionKey:{packet.Structure.SessionKey} for platform:{packet.Structure.PlatformType}");

            //TODO update key for user, for next login
            
            S2CConnectionLoginRes res = new S2CConnectionLoginRes();
            res.OneTimeToken = packet.Structure.SessionKey; // TODO set new token for next login
            client.Send(res);

            //client.Send(GameDump.Dump_4);
        }
    }
}
