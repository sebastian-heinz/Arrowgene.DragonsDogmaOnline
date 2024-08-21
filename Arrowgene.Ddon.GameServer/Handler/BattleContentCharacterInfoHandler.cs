using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.BattleContent;
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
            S2CBattleContentInfoListRes pcap = EntitySerializer.Get<S2CBattleContentInfoListRes>().Read(InGameDump.Dump_93.AsBuffer());
            pcap.BattleContentStatusList[0].BattleContentSituationData.StartTime = 4875391515;
            pcap.BattleContentStatusList[0].BattleContentSituationData.ContentId = 2;
            pcap.BattleContentStatusList[0].BattleContentSituationData.Unk8 = 0;
            pcap.BattleContentStatusList[0].BattleContentSituationData.Unk11 = 0;
            pcap.BattleContentStatusList[0].BattleContentSituationData.RewardReceived = true;
            pcap.BattleContentStatusList[0].BattleContentSituationData.RewardBonus = BattleContentRewardBonus.Normal;

            pcap.BattleContentStatusList[0].BattleContentSituationData.Unk3 = true;
            pcap.BattleContentStatusList[0].BattleContentSituationData.ReportReset = 1;
            pcap.BattleContentStatusList[0].BattleContentSituationData.ReportSearchResults = 0;

            var result = new S2CBattleContentCharacterInfoRes()
            {
                SituationData = pcap.BattleContentStatusList[0].BattleContentSituationData,
                Unk2List = pcap.BattleContentStatusList[0].BattleContentAvailableRewardsList
            };

            return result;
        }
    }
}
