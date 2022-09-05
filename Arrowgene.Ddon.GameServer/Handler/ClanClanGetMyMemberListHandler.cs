using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanGetMyMemberListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanGetMyMemberListHandler));


        public ClanClanGetMyMemberListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CLAN_CLAN_GET_MY_MEMBER_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            S2CClanClanGetMyMemberListRes res = new S2CClanClanGetMyMemberListRes();
            res.CharacterId = client.Character.Id;
            res.FirstName = client.Character.CharacterInfo.FirstName;
            res.LastName = client.Character.CharacterInfo.LastName;
            client.Send(res);
            
           // client.Send(InGameDump.Dump_67);
        }
    }
}
