using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class DailyMissionListGetHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(DailyMissionListGetHandler));


        public DailyMissionListGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_DAILY_MISSION_LIST_GET_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(GameFull.Dump_119);
        }
    }
}
