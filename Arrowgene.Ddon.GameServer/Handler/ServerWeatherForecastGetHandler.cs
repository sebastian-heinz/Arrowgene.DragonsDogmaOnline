using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerWeatherForecastGetHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ServerWeatherForecastGetHandler));

        private static readonly byte[] PcapData = new byte[] { 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x06,0x00,0x00,0x00,0x5A,0x00,0x00,0x00,0x04,0x00,0x00,0x00,0x01,0x00,0x00,0x00,0x01,0x00,0x00,0x00,0x02,0x00,0x00,0x00,0x02,0x00,0x01,0x01 };

        public ServerWeatherForecastGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_SERVER_WEATHER_FORECAST_GET_REQ;

        public override void Handle(GameClient client, IPacket request)
        {
            Packet response = new Packet(PacketId.S2C_SERVER_WEATHER_FORECAST_GET_RES, PcapData);
            client.Send(response);
        }
    }
}
