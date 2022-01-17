using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageGetStageListHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(StageGetStageListHandler));


        public StageGetStageListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_STAGE_GET_STAGE_LIST_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(GameDump.Dump_19);
        }
    }
}
