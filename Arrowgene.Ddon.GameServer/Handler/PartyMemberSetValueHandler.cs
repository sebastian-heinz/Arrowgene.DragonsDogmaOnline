using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyMemberSetValueHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyMemberSetValueHandler));


        public PartyMemberSetValueHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_PARTY_PARTY_MEMBER_SET_VALUE_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.Send(GameFull.Dump_900);
        }
    }
}