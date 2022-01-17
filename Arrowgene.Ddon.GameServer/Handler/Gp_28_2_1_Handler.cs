using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class Gp_28_2_1_Handler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(Gp_28_2_1_Handler));


        public Gp_28_2_1_Handler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_GP_28_2_1_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(InGameDump.Dump_46);
        }
    }
}
