using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGetUpdateAppCourseBonusFlagHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(GpGetUpdateAppCourseBonusFlagHandler));


        public GpGetUpdateAppCourseBonusFlagHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_GP_GET_UPDATE_APP_COURSE_BONUS_FLAG_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(InGameDump.Dump_97);
        }
    }
}
