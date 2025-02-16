using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeon62_12_16_Handler : GameStructurePacketHandler<C2S_SEASON_DUNGEON_62_12_16_NTC>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeon62_12_16_Handler));

        public SeasonDungeon62_12_16_Handler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2S_SEASON_DUNGEON_62_12_16_NTC> packet)
        {
            Logger.Debug($"{packet.Structure.LayoutId.AsStageLayoutId()}, SubGroupId={packet.Structure.PosId}");

            var stageId = packet.Structure.LayoutId.AsStageLayoutId();

            var state = Server.EpitaphRoadManager.GetEpitaphState(client, stageId, packet.Structure.PosId);

            // Renders tombstones with white/red/blue marker
            client.Send(new S2CSeasonDungeonSetOmStateNtc()
            {
                LayoutId = packet.Structure.LayoutId,
                PosId = packet.Structure.PosId,
                State = state,
            });
        }
    }
}
