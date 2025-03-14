using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobGetPlayPointListHandler : GameRequestPacketHandler<C2SJobGetPlayPointListReq, S2CJobGetPlayPointListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobGetPlayPointListHandler));

        public JobGetPlayPointListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CJobGetPlayPointListRes Handle(GameClient client, C2SJobGetPlayPointListReq request)
        {
            return new S2CJobGetPlayPointListRes()
            {
                PlayPointList = client.Character.PlayPointList
            };
        }
    }
}
