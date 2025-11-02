using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GroupChatInviteCharacterHandler : GameRequestPacketHandler<C2SGroupChatInviteCharacterReq, S2CGroupChatInviteCharacterRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GroupChatInviteCharacterHandler));

        public GroupChatInviteCharacterHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGroupChatInviteCharacterRes Handle(GameClient client, C2SGroupChatInviteCharacterReq request)
        {
            throw new ResponseErrorException(ErrorCode.ERROR_CODE_GROUP_CHAT_DISABLED);
        }
    }
}
