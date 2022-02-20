using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class FriendGetRecentCharacterListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FriendGetRecentCharacterListHandler));


        public FriendGetRecentCharacterListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_FRIEND_GET_RECENT_CHARACTER_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.Send(InGameDump.Dump_71);
        }
    }
}
