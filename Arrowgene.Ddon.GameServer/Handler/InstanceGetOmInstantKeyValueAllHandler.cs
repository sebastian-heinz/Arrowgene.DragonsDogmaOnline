using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetOmInstantKeyValueAllHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(InstanceGetOmInstantKeyValueAllHandler));


        public InstanceGetOmInstantKeyValueAllHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_INSTANCE_GET_OM_INSTANT_KEY_VALUE_ALL_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(GameFull.Dump_112);
        }
    }
}
