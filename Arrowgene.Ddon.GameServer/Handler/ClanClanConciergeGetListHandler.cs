using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanConciergeGetListHandler : GameRequestPacketHandler<C2SClanClanConciergeGetListReq, S2CClanClanConciergeGetListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanConciergeGetListHandler));

        public ClanClanConciergeGetListHandler(DdonGameServer server) : base(server)
        {
        }


        public override S2CClanClanConciergeGetListRes Handle(GameClient client, C2SClanClanConciergeGetListReq request)
        {
            S2CClanClanConciergeGetListRes res = new S2CClanClanConciergeGetListRes();
            var clan = Server.ClanManager.GetClan(client.Character.ClanId);
            res.ClanPoint = clan.ClanServerParam.MoneyClanPoint;
            res.ConciergeItemList.Add(new()
            {
                NpcId = 1043, 
                RequireClanPoint = 0
            });
            res.ConciergeItemList.Add(new()
            {
                NpcId = 1044,
                RequireClanPoint = 0
            });
            res.ConciergeItemList.Add(new()
            {
                NpcId = 674,
                RequireClanPoint = 0 // TODO: Actually 10000, but need to only pay this once?
            });

            return res;
        }

        private readonly byte[] ConciergeData =
        {
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x3,0x0, 0x0, 0x4, 0x14,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x4, 0x13, 0x0, 0x0, 0x0, 0x0,0x0, 0x0, 0x2, 0xA2,
            0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x4, 0x56, 0x0
        };
    }
}
