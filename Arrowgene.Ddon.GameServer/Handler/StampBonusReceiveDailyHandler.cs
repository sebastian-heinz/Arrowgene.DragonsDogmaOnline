using Arrowgene.Ddon.Database.Deferred;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StampBonusReceiveDailyHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StampBonusReceiveDailyHandler));

        private readonly DdonGameServer _gameServer;
        public StampBonusReceiveDailyHandler(DdonGameServer server) : base(server)
        {
            _gameServer = server;
        }

        public override PacketId Id => PacketId.C2S_STAMP_BONUS_RECIEVE_DAILY_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            //Update stamp bonus data.
            client.Character.StampBonus.LastStamp = DateTime.Now;
            client.Character.StampBonus.ConsecutiveStamp += 1;
            client.Character.StampBonus.TotalStamp += 1;

            _gameServer.Database.UpdateCharacterStampData(client.Character.CharacterId, client.Character.StampBonus);

            var dailyStamps = _gameServer.StampManager.GetDailyStampAssets().Where(x => x.StampNum == client.Character.StampBonus.ConsecutiveStamp);

            _gameServer.StampManager.HandleStampBonuses(client, dailyStamps);

            client.Send(GameFull.Dump_701);
        }
    }
} 
