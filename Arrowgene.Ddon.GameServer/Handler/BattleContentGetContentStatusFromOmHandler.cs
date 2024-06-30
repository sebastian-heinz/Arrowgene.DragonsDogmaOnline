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
                Unk0 = request.Unk0, // Shows up as first argument of C2S_BATTLE_CONTENT_CONTENT_ENTRY_REQ (stage?)
                Unk1 = 1,
                Unk2 = 1,
                Unk3 = 1,
                Unk4 = 1,
            };

            // 0, 0, 0, 0 says can't do content
            // 1, 1, 1, 1 bring up chaarcter slect board
            // 1, 1, 0, 0 introduces phase change req packet???
            // 1, 1, 1, 0
            // 0, 0, 1, 0 asks if player wants to procede
        }
    }
}
