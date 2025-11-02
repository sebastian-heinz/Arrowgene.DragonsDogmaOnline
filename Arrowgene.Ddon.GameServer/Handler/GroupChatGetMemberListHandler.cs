using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GroupChatGetMemberListHandler : GameRequestPacketHandler<C2SGroupChatGetMemberListReq, S2CGroupChatGetMemberListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GroupChatGetMemberListHandler));

        public GroupChatGetMemberListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGroupChatGetMemberListRes Handle(GameClient client, C2SGroupChatGetMemberListReq request)
        {
            S2CGroupChatGetMemberListRes res = new()
            {
                GroupMemberList = Server.GroupChatManager.GetGroupChatMembers(client.Character.GroupChatId)
            };

            return res;
        }
    }
}
