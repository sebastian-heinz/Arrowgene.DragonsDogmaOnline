using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BlackListGetBlackListHandler : GameRequestPacketHandler<C2SBlackListGetBlackListReq, S2CBlackListGetBlackListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BlackListGetBlackListHandler));


        public BlackListGetBlackListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBlackListGetBlackListRes Handle(GameClient client, C2SBlackListGetBlackListReq request)
        {
            return new()
            {
                BlackList = Server.Database.SelectBlackListFull(client.Character.CharacterId)
            };
        }
    }
}
