using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GroupChatLeaveCharacterHandler : GameRequestPacketHandler<C2SGroupChatLeaveCharacterReq, S2CGroupChatLeaveCharacterRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GroupChatLeaveCharacterHandler));

        public GroupChatLeaveCharacterHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGroupChatLeaveCharacterRes Handle(GameClient client, C2SGroupChatLeaveCharacterReq request)
        {
            Server.GroupChatManager.LeaveGroupChat(client, out var queue);
            queue.Send();

            return new();
        }
    }
}
