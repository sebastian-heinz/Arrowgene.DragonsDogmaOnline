using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GetLoginSettingHandler : LoginRequestPacketHandler<C2LGetLoginSettingReq, L2CGetLoginSettingRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GetLoginSettingHandler));

        public GetLoginSettingHandler(DdonLoginServer server) : base(server)
        {
        }

        public override L2CGetLoginSettingRes Handle(LoginClient client, C2LGetLoginSettingReq request)
        {
            L2CGetLoginSettingRes response = new L2CGetLoginSettingRes
            {
                LoginSetting =
                {
                    JobLevelMax = Server.GameSetting.GameServerSettings.JobLevelMax,
                    ClanMemberMax = Server.GameSetting.GameServerSettings.ClanMemberMax,
                    CharacterNumMax = Server.GameSetting.GameServerSettings.CharacterNumMax,
                    EnableVisualEquip = Server.GameSetting.GameServerSettings.EnableVisualEquip,
                    FriendListMax = Server.GameSetting.GameServerSettings.FriendListMax,
                    URLInfoList = new List<CDataURLInfo>()
                    {
                        new CDataURLInfo {Type = 1, URL = Server.GameSetting.GameServerSettings.UrlManual},
                        new CDataURLInfo {Type = 2, URL = Server.GameSetting.GameServerSettings.UrlShopDetail},
                        new CDataURLInfo {Type = 3, URL = Server.GameSetting.GameServerSettings.UrlShopCounterA},
                        new CDataURLInfo {Type = 4, URL = Server.GameSetting.GameServerSettings.UrlShopAttention},
                        new CDataURLInfo {Type = 5, URL = Server.GameSetting.GameServerSettings.UrlShopStoneLimit},
                        new CDataURLInfo {Type = 6, URL = Server.GameSetting.GameServerSettings.UrlShopCounterB},
                        new CDataURLInfo {Type = 7, URL = Server.GameSetting.GameServerSettings.UrlChargeCallback},
                        new CDataURLInfo {Type = 8, URL = Server.GameSetting.GameServerSettings.UrlChargeA},
                        new CDataURLInfo {Type = 9, URL = Server.GameSetting.GameServerSettings.UrlSample9},
                        new CDataURLInfo {Type = 10, URL = Server.GameSetting.GameServerSettings.UrlSample10},
                        new CDataURLInfo {Type = 11, URL = Server.GameSetting.GameServerSettings.UrlCampaignBanner},
                        new CDataURLInfo {Type = 12, URL = Server.GameSetting.GameServerSettings.UrlSupportIndex},
                        new CDataURLInfo {Type = 13, URL = Server.GameSetting.GameServerSettings.UrlPhotoupAuthorize},
                        new CDataURLInfo {Type = 14, URL = Server.GameSetting.GameServerSettings.UrlApiA},
                        new CDataURLInfo {Type = 15, URL = Server.GameSetting.GameServerSettings.UrlApiB},
                        new CDataURLInfo {Type = 16, URL = Server.GameSetting.GameServerSettings.UrlIndex},
                        new CDataURLInfo {Type = 17, URL = Server.GameSetting.GameServerSettings.UrlCampaign},
                        new CDataURLInfo {Type = 19, URL = Server.GameSetting.GameServerSettings.UrlChargeB},
                        new CDataURLInfo {Type = 20, URL = Server.GameSetting.GameServerSettings.UrlCompanionImage},
                    },
                    NoOperationTimeOutTime = Server.Setting.NoOperationTimeOutTime,
                },
            };
            return response;
        }
    }
}
