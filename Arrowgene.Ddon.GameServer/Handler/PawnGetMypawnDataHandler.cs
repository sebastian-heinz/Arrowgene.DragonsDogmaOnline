using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.GameServer.Dump;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetMypawnDataHandler : StructurePacketHandler<GameClient, C2SPawnGetMypawnDataReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetMypawnDataHandler));


        public PawnGetMypawnDataHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnGetMypawnDataReq> req)
        {
                S2CPawnGetMypawnDataRes res = new S2CPawnGetMypawnDataRes(Server.AssetRepository.MyPawnAsset, req.Structure);
                client.Send(res);
        }
    }
}
