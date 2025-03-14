using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceEnemyBadStatusStartHandler : GameStructurePacketHandler<C2SInstanceEnemyBadStatusStartNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceEnemyBadStatusStartHandler));

        private DdonGameServer _Server;

        public InstanceEnemyBadStatusStartHandler(DdonGameServer server) : base(server)
        {
            _Server = server;
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceEnemyBadStatusStartNtc> packet)
        {
            Logger.Debug($"{packet.Structure.LayoutId}\t{packet.Structure.PosId}.{packet.Structure.Unk4}.{packet.Structure.Unk5}");

            if (_Server.EpitaphRoadManager.TrialInProgress(client.Party))
            {
                _Server.EpitaphRoadManager.EvaluateEnemyAbnormalStatusEffectStart(client.Party, packet.Structure.LayoutId.AsStageLayoutId(), packet.Structure.PosId);
            }
        }
    }
}
