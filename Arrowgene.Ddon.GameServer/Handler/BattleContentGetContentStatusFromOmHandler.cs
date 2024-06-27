using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentGetContentStatusFromOmHandler : GameRequestPacketHandler<C2SBattleContentGetContentStatusFromOmReq, S2CBattleContentGetContentStatusFromOmRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentGetContentStatusFromOmHandler));

        public BattleContentGetContentStatusFromOmHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentGetContentStatusFromOmRes Handle(GameClient client, C2SBattleContentGetContentStatusFromOmReq request)
        {
            return new S2CBattleContentGetContentStatusFromOmRes()
            {
                Unk0 = 603, // Shows up as first argument of C2S_BATTLE_CONTENT_CONTENT_ENTRY_REQ (stage?)
                Unk1 = 0, // Setting this to 0 allowed me to enter
                Unk2 = 3,
                Unk3 = 4, // ???
                Unk4 = 5, // ???
            };
        }
    }
}
