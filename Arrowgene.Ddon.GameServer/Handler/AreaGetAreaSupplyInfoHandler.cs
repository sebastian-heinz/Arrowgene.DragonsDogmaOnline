using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    /// <summary>
    /// Client requests what supply items it is currently eligible for, not the whole list. 
    /// </summary>
    public class AreaGetAreaSupplyInfoHandler : GameRequestPacketHandler<C2SAreaGetAreaSupplyInfoReq, S2CAreaGetAreaSupplyInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaGetAreaSupplyInfoHandler));

        private static readonly byte[] PcapData = new byte[] { 0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0x00,0x00,0x00,0x01,0x00,0x00,0x00,0x01,0x00,0x00,0x2C,0xF6,0x02,0x01 };

        public AreaGetAreaSupplyInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CAreaGetAreaSupplyInfoRes Handle(GameClient client, C2SAreaGetAreaSupplyInfoReq request)
        {
            var pcap = EntitySerializer.Get<S2CAreaGetAreaSupplyInfoRes>().Read(PcapData);

            return pcap;
        }
    }
}
