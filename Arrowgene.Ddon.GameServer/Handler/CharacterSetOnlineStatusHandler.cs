using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterSetOnlineStatusHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterSetOnlineStatusHandler));


        public CharacterSetOnlineStatusHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CHARACTER_SET_ONLINE_STATUS_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.Send(SelectedDump.AntiDC_NoOpe);
        }
    }
}
