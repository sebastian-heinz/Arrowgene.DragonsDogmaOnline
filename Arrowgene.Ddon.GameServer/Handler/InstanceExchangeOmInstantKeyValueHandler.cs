using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceExchangeOmInstantKeyValueHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceExchangeOmInstantKeyValueHandler));


        public InstanceExchangeOmInstantKeyValueHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_INSTANCE_EXCHANGE_OM_INSTANT_KEY_VALUE_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(SelectedDump.AntiDC_Test2);
        }
    }
}
