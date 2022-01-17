using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class OrbDevoteGetOrbGainExtendParamHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(OrbDevoteGetOrbGainExtendParamHandler));


        public OrbDevoteGetOrbGainExtendParamHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_ORB_DEVOTE_GET_ORB_GAIN_EXTEND_PARAM_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(InGameDump.Dump_50);
        }
    }
}
