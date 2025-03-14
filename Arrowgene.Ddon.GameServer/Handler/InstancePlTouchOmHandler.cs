using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstancePlTouchOmHandler : GameStructurePacketHandler<C2SInstancePlTouchOmNtc>
    {
        private DdonGameServer _Server;
        public InstancePlTouchOmHandler(DdonGameServer server) : base(server)
        {
            _Server = server;
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstancePlTouchOmNtc> packet)
        {
            StageLayoutId stageId = packet.Structure.LayoutId.AsStageLayoutId();
            if (StageManager.IsEpitaphRoadStageId(stageId))
            {
                _Server.EpitaphRoadManager.HandleStatueUnlock(client, stageId, packet.Structure.SubGroupId);
            }
        }
    }
}
