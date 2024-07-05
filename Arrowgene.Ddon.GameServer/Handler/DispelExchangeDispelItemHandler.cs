using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class DispelExchangeDispelItemHandler : GameRequestPacketHandler<C2SDispelExchangeDispelItemReq, S2CDispelExchangeDispelItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(DispelExchangeDispelItemHandler));

        public DispelExchangeDispelItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CDispelExchangeDispelItemRes Handle(GameClient client, C2SDispelExchangeDispelItemReq request)
        {
            var res = new S2CDispelExchangeDispelItemRes();

            return res;
        }
    }
}

