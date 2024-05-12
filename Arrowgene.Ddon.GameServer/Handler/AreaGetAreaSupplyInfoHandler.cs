using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaGetAreaSupplyInfoHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaGetAreaSupplyInfoHandler));

        private static readonly byte[] PcapData = new byte[] { 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0x00,0x00,0x00,0x01,0x00,0x00,0x00,0x01,0x00,0x00,0x2C,0xF6,0x02,0x01 };

        public AreaGetAreaSupplyInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_AREA_GET_AREA_SUPPLY_INFO_REQ;

        public override void Handle(GameClient client, IPacket request)
        {
            Packet response = new Packet(PacketId.S2C_AREA_GET_AREA_SUPPLY_INFO_RES, PcapData);
            client.Send(response);
        }
    }
}
