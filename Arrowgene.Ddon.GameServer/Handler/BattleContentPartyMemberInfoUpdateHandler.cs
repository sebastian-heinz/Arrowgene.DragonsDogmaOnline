using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentPartyMemberInfoUpdateHandler : GameRequestPacketHandler<C2SBattleContentPartyMemberInfoUpdateReq, S2CBattleContentPartyMemberInfoUpdateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentPartyMemberInfoUpdateHandler));

        public BattleContentPartyMemberInfoUpdateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentPartyMemberInfoUpdateRes Handle(GameClient client, C2SBattleContentPartyMemberInfoUpdateReq request)
        {
            // TODO: Probably need to send an NTC for each character?
            // S2CBattleContentPartyMemberInfoUpdateNtc

            S2CBattleContentPartyMemberInfoUpdateNtc ntc = new S2CBattleContentPartyMemberInfoUpdateNtc()
            {
                CharacterId = 1,
                JobLevel = 8
            };
            client.Send(ntc);

            return new S2CBattleContentPartyMemberInfoUpdateRes();
        }
    }
}

