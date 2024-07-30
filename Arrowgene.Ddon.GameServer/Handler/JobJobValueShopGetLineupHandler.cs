using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobJobValueShopGetLineupHandler : GameRequestPacketHandler<C2SJobJobValueShopGetLineupReq, S2CJobJobValueShopGetLineupRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobGetPlayPointListHandler));

        public JobJobValueShopGetLineupHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CJobJobValueShopGetLineupRes Handle(GameClient client, C2SJobJobValueShopGetLineupReq packet)
        {
            return new S2CJobJobValueShopGetLineupRes()
            { 
                JobId = packet.JobId,
                JobValueType = packet.JobValueType,
                JobValueShopItemList = Server.AssetRepository.JobValueShopAsset.Where(x => x.Item1 == packet.JobId).Select(x => x.Item2).ToList()
            };
        }
    }
}
