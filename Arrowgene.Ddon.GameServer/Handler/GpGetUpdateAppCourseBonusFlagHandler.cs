using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGetUpdateAppCourseBonusFlagHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGetUpdateAppCourseBonusFlagHandler));


        public GpGetUpdateAppCourseBonusFlagHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_GP_GET_UPDATE_APP_COURSE_BONUS_FLAG_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.Send(InGameDump.Dump_97);
        }
    }
}
