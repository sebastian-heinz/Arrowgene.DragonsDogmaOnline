using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class OrbDevoteGetOrbGainExtendParamHandler : GameRequestPacketHandler<C2SOrbDevoteGetOrbGainExtendParam, S2COrbDevoteGetOrbGainExtendParamRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(OrbDevoteGetOrbGainExtendParamHandler));

        public OrbDevoteGetOrbGainExtendParamHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2COrbDevoteGetOrbGainExtendParamRes Handle(GameClient client, C2SOrbDevoteGetOrbGainExtendParam request)
        {
            // client.Send(InGameDump.Dump_50);
            S2COrbDevoteGetOrbGainExtendParamRes res = new()
            {
                ExtendParam = client.Character.CalculateFullExtendedParams()
            };
            return res;
        }
    }
}
