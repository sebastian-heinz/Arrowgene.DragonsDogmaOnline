using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StampBonusReceiveDailyHandler : GameRequestPacketHandler<C2SStampBonusRecieveDailyReq, S2CStampBonusRecieveDailyRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StampBonusReceiveDailyHandler));

        private readonly DdonGameServer _gameServer;
        public StampBonusReceiveDailyHandler(DdonGameServer server) : base(server)
        {
            _gameServer = server;
        }

        public override S2CStampBonusRecieveDailyRes Handle(GameClient client, C2SStampBonusRecieveDailyReq request)
        {
            _gameServer.StampManager.UpdateStamp(client.Character);

            var dailyStamps = _gameServer.StampManager.GetDailyStampAssets().Where(x => x.StampNum == client.Character.StampBonus.ConsecutiveStamp);

            _gameServer.StampManager.HandleStampBonuses(client, dailyStamps);

            return new S2CStampBonusRecieveDailyRes();
        }
    }
} 
