using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ContextGetSetContextHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ContextGetSetContextHandler));


        public ContextGetSetContextHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CONTEXT_GET_SET_CONTEXT_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(SelectedDump.Dump_1715);
        }
    }
}
