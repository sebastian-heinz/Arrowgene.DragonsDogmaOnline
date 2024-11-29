using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageUnisonAreaChangeBeginRecruitmentHandler : GameRequestPacketHandler<C2SStageUnisonAreaChangeBeginRecruitmentReq, S2CStageUnisonAreaChangeBeginRecruitmentRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageUnisonAreaChangeBeginRecruitmentHandler));

        public StageUnisonAreaChangeBeginRecruitmentHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CStageUnisonAreaChangeBeginRecruitmentRes Handle(GameClient client, C2SStageUnisonAreaChangeBeginRecruitmentReq request)
        {
            Server.DungeonManager.MarkReady(client.Party, client.Character, request.ContentId);

            if (Server.DungeonManager.PartyIsReady(client.Party))
            {
                Server.DungeonManager.StartActivity(client.Party, DungeonManager.StartDungeon);
            }

            return new S2CStageUnisonAreaChangeBeginRecruitmentRes();
        }
    }
}
