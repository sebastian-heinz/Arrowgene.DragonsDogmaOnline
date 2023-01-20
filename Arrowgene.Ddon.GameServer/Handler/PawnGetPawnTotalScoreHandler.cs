using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnGetPawnTotalScoreHandler : GameStructurePacketHandler<C2SPawnGetPawnTotalScoreReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnGetPawnTotalScoreHandler));

        public PawnGetPawnTotalScoreHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnGetPawnTotalScoreReq> packet)
        {
            client.Send(new S2CPawnGetPawnTotalScoreRes());
        }
    }
}