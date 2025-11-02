using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BlackListAddBlackListHandler : GameRequestPacketHandler<C2SBlackListAddBlackListReq, S2CBlackListAddBlackListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BlackListAddBlackListHandler));

        public BlackListAddBlackListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBlackListAddBlackListRes Handle(GameClient client, C2SBlackListAddBlackListReq request)
        {
            if (request.CharacterInfo.CharacterId == 0 
                || !Server.Database.InsertBlackList(client.Character.CharacterId, request.CharacterInfo.CharacterId))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_BLACK_LIST_TARGET_INVALID);
            }

            return new() { CharacterBaseInfo = request.CharacterInfo };
        }
    }
}
