using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceExchangeOmInstantKeyValueHandler : StructurePacketHandler<GameClient, C2SInstanceExchangeOmInstantKeyValueReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceExchangeOmInstantKeyValueHandler));


        public InstanceExchangeOmInstantKeyValueHandler(DdonGameServer server) : base(server)
        {
        }


        public override void Handle(GameClient client, StructurePacket<C2SInstanceExchangeOmInstantKeyValueReq> stageIdHandler)
        {
            S2CInstanceExchangeOmInstantKeyValueRes res = new S2CInstanceExchangeOmInstantKeyValueRes(stageIdHandler.Structure);
            client.Send(res);
        }
    }
}
