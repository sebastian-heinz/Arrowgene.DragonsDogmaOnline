using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StampBonusCheckHandler : GameRequestPacketHandler<C2SStampBonusCheckReq, S2CStampBonusCheckRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StampBonusCheckHandler));

        private readonly DdonGameServer _gameServer;

        public StampBonusCheckHandler(DdonGameServer server) : base(server)
        {
            _gameServer = server;
        }

        public override S2CStampBonusCheckRes Handle(GameClient client, C2SStampBonusCheckReq request)
        {
            bool canDaily = _gameServer.StampManager.CanDailyStamp(client.Character.StampBonus);
            bool canTotal = _gameServer.StampManager.CanTotalStamp(client.Character.StampBonus);

            var stampBonusList = _gameServer.StampManager.GetDailyStampAssets().Select(x => x.StampBonus.First()).ToList();

            if (canTotal)
            {
                var res = new S2CStampBonusCheckRes()
                {
                    IsRecieveBonusDaily = 0,
                    IsRecieveBonusTotal = 0,
                };
                res.StampCheck.Add(new CDataStampCheck()
                {
                    Unk0 = 1,
                    Unk1 = 0,
                });
                return res;
            }
            else if (canDaily)
            {
                var res = new S2CStampBonusCheckRes()
                {
                    IsRecieveBonusDaily = 1,
                    IsRecieveBonusTotal = 0,
                };
                res.StampCheck.Add(new CDataStampCheck()
                {
                    Unk0 = 1,
                    Unk1 = 0,
                });
                return res;
            }
            else
            {
                return new S2CStampBonusCheckRes()
                {
                    IsRecieveBonusDaily = byte.MaxValue,
                    IsRecieveBonusTotal = byte.MaxValue,
                };
            }
        }
    }
}
