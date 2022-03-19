using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceSetOmInstantKeyValueHandler : StructurePacketHandler<GameClient, C2SInstanceSetOmInstantKeyValueReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceSetOmInstantKeyValueHandler));


        public InstanceSetOmInstantKeyValueHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceSetOmInstantKeyValueReq> stageIdHandler)
        {
            S2CInstanceSetOmInstantKeyValueRes res = new S2CInstanceSetOmInstantKeyValueRes(stageIdHandler.Structure);
            client.Send(res);
        }
    }
}
