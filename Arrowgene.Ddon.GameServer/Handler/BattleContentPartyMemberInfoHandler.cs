using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentPartyMemberInfoHandler : GameRequestPacketHandler<C2SBattleContentPartyMemberInfoReq, S2CBattleContentPartyMemberInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentPartyMemberInfoHandler));

        public BattleContentPartyMemberInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentPartyMemberInfoRes Handle(GameClient client, C2SBattleContentPartyMemberInfoReq request)
        {
            S2C_BATTLE_71_12_16_NTC ntc = new S2C_BATTLE_71_12_16_NTC()
            {
                Unk0 = 0,
                Unk1 = "Yooo",
                StageId = 611,
                StartPos = 0,
                Unk4 = true,
            };
            client.Send(ntc);

            return new S2CBattleContentPartyMemberInfoRes();
        }
    }
}

