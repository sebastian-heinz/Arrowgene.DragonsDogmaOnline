using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetEnemySetListHandler : StructurePacketHandler<GameClient, C2SInstanceGetEnemySetListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetEnemySetListHandler));

        public InstanceGetEnemySetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetEnemySetListReq> request)
        {   
            S2CInstanceGetEnemySetListRes response = new S2CInstanceGetEnemySetListRes(); //note that the enemy array is inside this and is currently hardcoded
            response.LayoutId.StageID = request.Structure.LayoutId.StageID;
            response.LayoutId.LayerNo = request.Structure.LayoutId.LayerNo;
            response.LayoutId.GroupID = request.Structure.LayoutId.GroupID;
            response.SubGroupId = request.Structure.SubGroupId;

            client.Send(response);
        }
    }
}
