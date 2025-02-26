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

            res.GameSetting.JobLevelMax = Server.GameSettings.GameServerSettings.JobLevelMax;
            res.GameSetting.ExpRequiredPerLevel[0].ExpList = res.GameSetting.ExpRequiredPerLevel[0].ExpList.Take((int)res.GameSetting.JobLevelMax).ToList();

            res.GameSetting.EnableVisualEquip = Server.GameSettings.GameServerSettings.EnableVisualEquip;

            res.GameSetting.ClanMemberMax = Server.GameSettings.GameServerSettings.ClanMemberMax;
            res.GameSetting.CharacterNumMax = Server.GameSettings.GameServerSettings.CharacterNumMax;
            res.GameSetting.FriendListMax = Server.GameSettings.GameServerSettings.FriendListMax;

            res.GameSetting.UrlInfoList = new List<CDataURLInfo>()
            {
                new() {Type = 1, URL = Server.GameSettings.GameServerSettings.UrlManual},
                new() {Type = 2, URL = Server.GameSettings.GameServerSettings.UrlShopDetail},
                new() {Type = 3, URL = Server.GameSettings.GameServerSettings.UrlShopCounterA},
                new() {Type = 4, URL = Server.GameSettings.GameServerSettings.UrlShopAttention},
                new() {Type = 5, URL = Server.GameSettings.GameServerSettings.UrlShopStoneLimit},
                new() {Type = 6, URL = Server.GameSettings.GameServerSettings.UrlShopCounterB},
                new() {Type = 7, URL = Server.GameSettings.GameServerSettings.UrlChargeCallback},
                new() {Type = 8, URL = Server.GameSettings.GameServerSettings.UrlChargeA},
                new() {Type = 9, URL = Server.GameSettings.GameServerSettings.UrlSample9},
                new() {Type = 10, URL = Server.GameSettings.GameServerSettings.UrlSample10},
                new() {Type = 11, URL = Server.GameSettings.GameServerSettings.UrlCampaignBanner},
                new() {Type = 12, URL = Server.GameSettings.GameServerSettings.UrlSupportIndex},
                new() {Type = 13, URL = Server.GameSettings.GameServerSettings.UrlPhotoupAuthorize},
                new() {Type = 14, URL = Server.GameSettings.GameServerSettings.UrlApiA},
                new() {Type = 15, URL = Server.GameSettings.GameServerSettings.UrlApiB},
                new() {Type = 16, URL = Server.GameSettings.GameServerSettings.UrlIndex},
                new() {Type = 17, URL = Server.GameSettings.GameServerSettings.UrlCampaign},
                new() {Type = 19, URL = Server.GameSettings.GameServerSettings.UrlChargeB},
                new() {Type = 20, URL = Server.GameSettings.GameServerSettings.UrlCompanionImage},
            };

            res.GameSetting.PlayPointMax = Server.GameSettings.GameServerSettings.PlayPointMax;
            res.GameSetting.WalletLimits = Server.GameSettings.GameServerSettings.WalletLimits.Select(x => new CDataWalletLimit()
            {
                WalletType = x.Key,
                MaxValue = x.Value,
            }).ToList();

            return res;
        }
    }
}
