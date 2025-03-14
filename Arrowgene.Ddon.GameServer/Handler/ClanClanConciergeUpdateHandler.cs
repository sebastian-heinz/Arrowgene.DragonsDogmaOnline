using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanConciergeUpdateHandler : GameRequestPacketHandler<C2SClanClanConciergeUpdateReq, S2CClanClanConciergeUpdateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanConciergeUpdateHandler));

        public ClanClanConciergeUpdateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanConciergeUpdateRes Handle(GameClient client, C2SClanClanConciergeUpdateReq request)
        {
            // TODO: Deduct clan points.
            S2CClanClanConciergeUpdateRes res = new S2CClanClanConciergeUpdateRes();
            var clan = Server.ClanManager.GetClan(client.Character.ClanId);

            Server.Database.InsertOrUpdateClanBaseCustomization(client.Character.ClanId, Shared.Model.ClanFurnitureType.Concierge, request.ConciergeId);
            res.NpcId = request.ConciergeId;
            return res;
        }
    }
}
