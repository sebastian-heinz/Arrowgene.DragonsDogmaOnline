using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class NpcGetExtendedFacilityHandler : GameRequestPacketHandler<C2SNpcGetNpcExtendedFacilityReq, S2CNpcGetNpcExtendedFacilityRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(NpcGetExtendedFacilityHandler));

        public NpcGetExtendedFacilityHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CNpcGetNpcExtendedFacilityRes Handle(GameClient client, C2SNpcGetNpcExtendedFacilityReq request)
        {
            // client.Send(GameFull.Dump_202);
            // var pcap = new S2CNpcGetNpcExtendedFacilityRes.Serializer().Read(pcap_data);

            return new S2CNpcGetNpcExtendedFacilityRes();
        }

        private readonly byte[] pcap_data = new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0xC2, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x45, 0x00, 0x00, 0x00, 0x43, 0x00, 0x00, 0x11, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x43, 0x20, 0xFB, 0xE8, 0xC0, 0xA0, 0xEC};
    }
}
