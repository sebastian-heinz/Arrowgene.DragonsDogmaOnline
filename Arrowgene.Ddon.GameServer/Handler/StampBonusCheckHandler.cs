using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Diagnostics;
using System.Transactions;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StampBonusCheckHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StampBonusCheckHandler));

        public StampBonusCheckHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_STAMP_BONUS_CHECK_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            S2CStampBonusCheckRes res = new S2CStampBonusCheckRes()
            {
                SuppressTotal = false,
                SuppressDaily = false
            };

            //client.Send(InGameDump.Dump_95);
            client.Send(res);
        }
    }
}
