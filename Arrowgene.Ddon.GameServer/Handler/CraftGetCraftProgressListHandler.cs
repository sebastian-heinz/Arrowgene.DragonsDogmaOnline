using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftGetCraftProgressListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftGetCraftProgressListHandler));

        public CraftGetCraftProgressListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CRAFT_GET_CRAFT_PROGRESS_LIST_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            // TODO: Fetch values from DB

            S2CCraftGetCraftProgressListRes res = new S2CCraftGetCraftProgressListRes();
            // TODO: CraftProgressList, CreatedRecipeList
            res.CraftMyPawnList = client.Character.Pawns.Select(pawn => new CDataCraftPawnList()
            {
                PawnId = pawn.PawnId,
                CraftExp = 2500,
                CraftPoint = 10,
                CraftRankLimit = 10
            }).ToList();
            client.Send(res);
        }
    }
}