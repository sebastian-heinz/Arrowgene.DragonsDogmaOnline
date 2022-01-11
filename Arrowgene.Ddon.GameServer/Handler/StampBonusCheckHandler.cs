using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StampBonusCheckHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(StampBonusCheckHandler));


        public StampBonusCheckHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_STAMP_BONUS_CHECK_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(InGameDump.Dump_95);
        }
    }
}
