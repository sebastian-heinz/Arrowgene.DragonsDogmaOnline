using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StampBonusReceiveTotalHandler : GameRequestPacketHandler<C2SStampBonusRecieveTotalReq, S2CStampBonusRecieveTotalRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StampBonusReceiveTotalHandler));

        private readonly DdonGameServer _gameServer;

        public StampBonusReceiveTotalHandler(DdonGameServer server) : base(server)
        {
            _gameServer = server;
        }

        public override S2CStampBonusRecieveTotalRes Handle(GameClient client, C2SStampBonusRecieveTotalReq request)
        {
            var totalStamps = _gameServer.StampManager.GetTotalStampAssets().Where(x => x.StampNum == client.Character.StampBonus.TotalStamp);

            _gameServer.StampManager.HandleStampBonuses(client, totalStamps);

            return new S2CStampBonusRecieveTotalRes();
        }
    }
} 
