using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BlackListGetBlackListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BlackListGetBlackListHandler));


        public BlackListGetBlackListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_BLACK_LIST_GET_BLACK_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.Send(InGameDump.Dump_73);
        }
    }
}
