using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerGetGameSettingHandler : GameRequestPacketHandler<C2SServerGetGameSettingReq, S2CServerGetGameSettingRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ServerGetGameSettingHandler));

        public ServerGetGameSettingHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CServerGetGameSettingRes Handle(GameClient client, C2SServerGetGameSettingReq request)
        {
            var res = new S2CServerGetGameSettingRes.Serializer().Read(GameDump.Dump_10.AsBuffer());

            res.GameSetting.JobLevelMax = Server.Setting.GameLogicSetting.JobLevelMax;
            res.GameSetting.ExpRequiredPerLevel[0].ExpList = res.GameSetting.ExpRequiredPerLevel[0].ExpList.Take((int)res.GameSetting.JobLevelMax).ToList();

            res.GameSetting.EnableVisualEquip = Server.Setting.GameLogicSetting.EnableVisualEquip;

            res.GameSetting.ClanMemberMax = Server.Setting.GameLogicSetting.ClanMemberMax;
            res.GameSetting.CharacterNumMax = Server.Setting.GameLogicSetting.CharacterNumMax;
            res.GameSetting.FriendListMax = Server.Setting.GameLogicSetting.FriendListMax;

            res.GameSetting.UrlInfoList = new List<CDataURLInfo>()
            {
                new() {Type = 1, URL = Server.Setting.GameLogicSetting.UrlManual},
                new() {Type = 2, URL = Server.Setting.GameLogicSetting.UrlShopDetail},
                new() {Type = 3, URL = Server.Setting.GameLogicSetting.UrlShopCounterA},
                new() {Type = 4, URL = Server.Setting.GameLogicSetting.UrlShopAttention},
                new() {Type = 5, URL = Server.Setting.GameLogicSetting.UrlShopStoneLimit},
                new() {Type = 6, URL = Server.Setting.GameLogicSetting.UrlShopCounterB},
                new() {Type = 7, URL = Server.Setting.GameLogicSetting.UrlChargeCallback},
                new() {Type = 8, URL = Server.Setting.GameLogicSetting.UrlChargeA},
                new() {Type = 9, URL = Server.Setting.GameLogicSetting.UrlSample9},
                new() {Type = 10, URL = Server.Setting.GameLogicSetting.UrlSample10},
                new() {Type = 11, URL = Server.Setting.GameLogicSetting.UrlCampaignBanner},
                new() {Type = 12, URL = Server.Setting.GameLogicSetting.UrlSupportIndex},
                new() {Type = 13, URL = Server.Setting.GameLogicSetting.UrlPhotoupAuthorize},
                new() {Type = 14, URL = Server.Setting.GameLogicSetting.UrlApiA},
                new() {Type = 15, URL = Server.Setting.GameLogicSetting.UrlApiB},
                new() {Type = 16, URL = Server.Setting.GameLogicSetting.UrlIndex},
                new() {Type = 17, URL = Server.Setting.GameLogicSetting.UrlCampaign},
                new() {Type = 19, URL = Server.Setting.GameLogicSetting.UrlChargeB},
                new() {Type = 20, URL = Server.Setting.GameLogicSetting.UrlCompanionImage},
            };

            res.GameSetting.PlayPointMax = Server.Setting.GameLogicSetting.PlayPointMax;
            res.GameSetting.WalletLimits = Server.Setting.GameLogicSetting.WalletLimits;

            return res;
        }
    }
}
