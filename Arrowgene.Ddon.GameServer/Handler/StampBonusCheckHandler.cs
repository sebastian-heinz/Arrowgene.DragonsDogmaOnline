using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StampBonusCheckHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StampBonusCheckHandler));

        private readonly DdonGameServer _gameServer;

        public StampBonusCheckHandler(DdonGameServer server) : base(server)
        {
            _gameServer = server;
        }

        public override PacketId Id => PacketId.C2S_STAMP_BONUS_CHECK_REQ;

        public override void Handle(GameClient client, IPacket packet)
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
                client.Send(res);
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
                client.Send(res);
            }
            else
            {
                client.Send(new S2CStampBonusCheckRes()
                {
                    IsRecieveBonusDaily = byte.MaxValue,
                    IsRecieveBonusTotal = byte.MaxValue,
                });
            }
            
        }
    }
}
