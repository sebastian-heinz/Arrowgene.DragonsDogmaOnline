using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

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
            bool canTotal = _gameServer.StampManager.CanTotalStamp(client.Character.StampBonus);
            bool canDaily = _gameServer.StampManager.CanDailyStamp(client.Character.StampBonus);

            if (canDaily)
            {
                client.Send(new S2CStampBonusCheckRes()
                {
                    SuppressTotal = !canTotal,
                    SuppressDaily = !canDaily,
                });
            }
            else 
            {
                //For whatever reason, suppresses the icon over Ophelia's head.
                client.Send(new S2CStampBonusCheckRes() 
                {
                    Unk0 = 0,
                    Unk1 = ushort.MaxValue,
                    SuppressDaily = true,
                    SuppressTotal = true
                });
            }  
        }
    }
}
