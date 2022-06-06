using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanBaseGetInfoHandler : StructurePacketHandler<GameClient, C2SClanClanBaseGetInfoReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanBaseGetInfoHandler));


        public ClanClanBaseGetInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SClanClanBaseGetInfoReq> req)
        {
            S2CClanClanBaseGetInfoRes res = new S2CClanClanBaseGetInfoRes();
            client.Send(res);
        }
    }
}
