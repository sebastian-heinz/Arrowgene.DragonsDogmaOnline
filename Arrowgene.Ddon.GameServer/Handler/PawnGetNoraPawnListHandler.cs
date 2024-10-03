using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetNoraPawnListHandler : GameRequestPacketHandler<C2SPawnGetNoraPawnListReq,S2CPawnGetNoraPawnListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetNoraPawnListHandler));


        public PawnGetNoraPawnListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_PAWN_GET_NORA_PAWN_LIST_REQ;

        public override S2CPawnGetNoraPawnListRes Handle(GameClient client, C2SPawnGetNoraPawnListReq request)
        {
            // client.Send(GameFull.Dump_118);
            var result = new S2CPawnGetNoraPawnListRes();

            List<uint> pawnIds = Server.Database.SelectAllPlayerPawns(uint.MaxValue);
            result.NoraPawnList = pawnIds.OrderBy(x => Random.Shared.Next()).Take(100).Select(x => new CDataRegisterdPawnList() { PawnId = x }).ToList();
            return result;
        }
    }
}
