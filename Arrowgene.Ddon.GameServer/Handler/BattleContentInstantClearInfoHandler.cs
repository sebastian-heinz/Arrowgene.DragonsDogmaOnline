using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentInstantClearInfoHandler : GameRequestPacketHandler<C2SBattleContentInstantClearInfoReq, S2CBattleContentInstantClearInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentInstantClearInfoHandler));

        public BattleContentInstantClearInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentInstantClearInfoRes Handle(GameClient client, C2SBattleContentInstantClearInfoReq request)
        {
            // Not sure what this does, can't seem to make anything show up
            // I think it must be related to the ready up select menu, but not sure
            var result = new S2CBattleContentInstantClearInfoRes();
            return result;
        }
    }
}
