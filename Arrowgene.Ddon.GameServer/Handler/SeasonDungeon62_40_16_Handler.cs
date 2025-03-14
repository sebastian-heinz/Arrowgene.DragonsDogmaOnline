using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeon62_40_16_Handler : GameStructurePacketHandler<C2S_SEASON_62_40_16_NTC>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeon62_40_16_Handler));

        public SeasonDungeon62_40_16_Handler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2S_SEASON_62_40_16_NTC> packet)
        {
            StageLayoutId stageId = packet.Structure.LayoutId.AsStageLayoutId();
            Logger.Info($"Other packet: {stageId}");

            if (StageManager.IsLegacyEpitaphHubArea(stageId))
            {
                var dungeonInfo = Server.EpitaphRoadManager.GetDungeonInfoByHubStageId(stageId);

                bool finalTrialUnlocked = true;
                foreach (var statue in dungeonInfo.Statues)
                {
                    finalTrialUnlocked &= client.Character.EpitaphRoadState.UnlockedContent.Contains(statue.EpitaphId);
                    if (!finalTrialUnlocked)
                    {
                        break;
                    }
                }

                if (finalTrialUnlocked)
                {
                    client.Send(new S2CSeasonDungeonSetOmStateNtc()
                    {
                        LayoutId = packet.Structure.LayoutId,
                        PosId = packet.Structure.PosId,
                        State = SoulOrdealOmState.AreaUnlocked
                    });
                }
            }
            else
            {
                // TODO: Query conditions for newer epitaph (big trials?)
                client.Send(new S2CSeasonDungeonSetOmStateNtc()
                {
                    LayoutId = packet.Structure.LayoutId,
                    PosId = packet.Structure.PosId,
                    State = SoulOrdealOmState.Locked
                });
            }
        }
    }
}
