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

            res.GameSetting.JobLevelMax = Server.GameLogicSettings.JobLevelMax;
            res.GameSetting.ExpRequiredPerLevel[0].ExpList = res.GameSetting.ExpRequiredPerLevel[0].ExpList.Take((int)res.GameSetting.JobLevelMax).ToList();

            res.GameSetting.EnableVisualEquip = Server.GameLogicSettings.EnableVisualEquip;

            res.GameSetting.ClanMemberMax = Server.GameLogicSettings.ClanMemberMax;
            res.GameSetting.CharacterNumMax = Server.GameLogicSettings.CharacterNumMax;
            res.GameSetting.FriendListMax = Server.GameLogicSettings.FriendListMax;

            res.GameSetting.UrlInfoList = new List<CDataURLInfo>()
            {
                new() {Type = 1, URL = Server.GameLogicSettings.UrlManual},
                new() {Type = 2, URL = Server.GameLogicSettings.UrlShopDetail},
                new() {Type = 3, URL = Server.GameLogicSettings.UrlShopCounterA},
                new() {Type = 4, URL = Server.GameLogicSettings.UrlShopAttention},
                new() {Type = 5, URL = Server.GameLogicSettings.UrlShopStoneLimit},
                new() {Type = 6, URL = Server.GameLogicSettings.UrlShopCounterB},
                new() {Type = 7, URL = Server.GameLogicSettings.UrlChargeCallback},
                new() {Type = 8, URL = Server.GameLogicSettings.UrlChargeA},
                new() {Type = 9, URL = Server.GameLogicSettings.UrlSample9},
                new() {Type = 10, URL = Server.GameLogicSettings.UrlSample10},
                new() {Type = 11, URL = Server.GameLogicSettings.UrlCampaignBanner},
                new() {Type = 12, URL = Server.GameLogicSettings.UrlSupportIndex},
                new() {Type = 13, URL = Server.GameLogicSettings.UrlPhotoupAuthorize},
                new() {Type = 14, URL = Server.GameLogicSettings.UrlApiA},
                new() {Type = 15, URL = Server.GameLogicSettings.UrlApiB},
                new() {Type = 16, URL = Server.GameLogicSettings.UrlIndex},
                new() {Type = 17, URL = Server.GameLogicSettings.UrlCampaign},
                new() {Type = 19, URL = Server.GameLogicSettings.UrlChargeB},
                new() {Type = 20, URL = Server.GameLogicSettings.UrlCompanionImage},
            };

            res.GameSetting.PlayPointMax = Server.GameLogicSettings.PlayPointMax;
            res.GameSetting.WalletLimits = Server.GameLogicSettings.WalletLimits.Select(x => new CDataWalletLimit()
            {
                WalletType = x.Key,
                MaxValue = x.Value,
            }).ToList();

            return res;
        }
    }
}
