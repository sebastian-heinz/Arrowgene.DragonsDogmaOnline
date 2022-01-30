using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageGetStageListHandler : StructurePacketHandler<GameClient, C2SStageGetStageListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageGetStageListHandler));

        public StageGetStageListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SStageGetStageListReq> packet)
        {
            S2CStageGetStageListRes obj = new S2CStageGetStageListRes();
            obj.StageList = (Server as DdonGameServer).StageList;

            client.Send(obj);
        }
    }
}
