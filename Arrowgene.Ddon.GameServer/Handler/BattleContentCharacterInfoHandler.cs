using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentCharacterInfoHandler : GameRequestPacketHandler<C2SBattleContentCharacterInfoReq, S2CBattleContentCharacterInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentCharacterInfoHandler));

        public BattleContentCharacterInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentCharacterInfoRes Handle(GameClient client, C2SBattleContentCharacterInfoReq request)
        {
            var contentStatus = BitterblackMazeManager.GetUpdatedContentStatus(Server, client.Character);

            var result = new S2CBattleContentCharacterInfoRes()
            {
                SituationData = contentStatus.BattleContentSituationData,
                Unk2List = contentStatus.BattleContentAvailableRewardsList
            };

            result.SituationData.Unk8 = 5;
            result.SituationData.Unk11 = 2;

            return result;
        }
    }
}
