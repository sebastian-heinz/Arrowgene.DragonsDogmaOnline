using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    internal class BlackListRemoveBlackListHandler : GameRequestPacketHandler<C2SBlackListRemoveBlackListReq, S2CBlackListRemoveBlackListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BlackListRemoveBlackListHandler));

        public BlackListRemoveBlackListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBlackListRemoveBlackListRes Handle(GameClient client, C2SBlackListRemoveBlackListReq request)
        {
            Server.Database.DeleteBlackList(client.Character.CharacterId, request.CharacterId);

            return new() { RemoveCharacterId = request.CharacterId };
        }
    }
}
