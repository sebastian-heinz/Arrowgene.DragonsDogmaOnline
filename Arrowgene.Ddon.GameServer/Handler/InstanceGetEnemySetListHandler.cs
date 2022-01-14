using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetEnemySetListHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(InstanceGetEnemySetListHandler));


        public InstanceGetEnemySetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_INSTANCE_GET_ENEMY_SET_LIST_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(SelectedDump.Dump_93047);
        }
    }
}
