using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonGetStatueStateHandler : GameStructurePacketHandler<C2SSeasonDungeonGetStatueStateNtc>
    {
        private DdonGameServer _Server;

        public SeasonDungeonGetStatueStateHandler(DdonGameServer server) : base(server)
        {
            _Server = server;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSeasonDungeonGetStatueStateNtc> packet)
        {
            StageLayoutId stageId = packet.Structure.LayoutId.AsStageLayoutId();
            if (_Server.EpitaphRoadManager.IsStatueUnlocked(client, stageId, packet.Structure.PosId))
            {
                client.Send(new S2CSeasonDungeonSetOmStateNtc()
                {
                    LayoutId = packet.Structure.LayoutId,
                    PosId = packet.Structure.PosId,
                    State = SoulOrdealOmState.AreaUnlocked
                });
            }
        }
    }
}
