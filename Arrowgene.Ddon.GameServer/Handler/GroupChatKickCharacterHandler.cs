using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GroupChatKickCharacterHandler : GameRequestPacketHandler<C2SGroupChatKickCharacterReq, S2CGroupChatKickCharacterRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GroupChatKickCharacterHandler));

        public GroupChatKickCharacterHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGroupChatKickCharacterRes Handle(GameClient client, C2SGroupChatKickCharacterReq request)
        {
            throw new ResponseErrorException(ErrorCode.ERROR_CODE_GROUP_CHAT_DISABLED);
        }
    }
}
