using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    internal class StampBonusReceiveTotalHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StampBonusReceiveTotalHandler));

        private readonly DdonGameServer _gameServer;

        public StampBonusReceiveTotalHandler(DdonGameServer server) : base(server)
        {
            _gameServer = server;
        }

        public override PacketId Id => PacketId.C2S_STAMP_BONUS_RECIEVE_TOTAL_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            var totalStamps = _gameServer.StampManager.GetTotalStampAssets().Where(x => x.StampNum == client.Character.StampBonus.TotalStamp);

            _gameServer.StampManager.HandleStampBonuses(client, totalStamps);

            //This is misusing the dumped data but the client accepts it.
            client.Send(new Packet(PacketId.S2C_STAMP_BONUS_RECIEVE_TOTAL_RES, GameFull.data_Dump_701));
        }
    }
} 
