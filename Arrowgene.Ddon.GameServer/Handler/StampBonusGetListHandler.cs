using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StampBonusGetListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StampBonusGetListHandler));


        public StampBonusGetListHandler(DdonGameServer server) : base(server)
        {
        }


     public override PacketId Id => PacketId.C2S_STAMP_BONUS_GET_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {

            client.Send(GameFull.Dump_700);
        }
    }
} 
