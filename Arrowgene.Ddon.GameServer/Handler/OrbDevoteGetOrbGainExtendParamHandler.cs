using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class OrbDevoteGetOrbGainExtendParamHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(OrbDevoteGetOrbGainExtendParamHandler));

        public OrbDevoteGetOrbGainExtendParamHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_ORB_DEVOTE_GET_ORB_GAIN_EXTEND_PARAM_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            // client.Send(InGameDump.Dump_50);
            S2COrbDevoteGetOrbGainExtendParamRes Result = new S2COrbDevoteGetOrbGainExtendParamRes()
            {
                ExtendParam = client.Character.CalculateFullExtendedParams()
            };
            client.Send(Result);
        }
    }
}
