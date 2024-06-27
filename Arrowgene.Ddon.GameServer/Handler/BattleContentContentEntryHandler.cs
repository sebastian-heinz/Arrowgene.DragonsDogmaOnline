using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentContentEntryHandler : GameRequestPacketHandler<C2SBattleContentContentEntryReq, S2CBattleContentContentEntryRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentContentEntryHandler));

        public BattleContentContentEntryHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentContentEntryRes Handle(GameClient client, C2SBattleContentContentEntryReq request)
        {
            S2CBattleContentContentEntryNtc ntc = new S2CBattleContentContentEntryNtc()
            {
                StageId = 2001,
                Unk0 = new CDataBattleContentUnk0()
                {
                    
                }
            };
            client.Send(ntc);

            return new S2CBattleContentContentEntryRes();
        }
    }
}
