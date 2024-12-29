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
                    JobLevelMax = Server.GameSetting.JobLevelMax,
                    ClanMemberMax = Server.GameSetting.ClanMemberMax,
                    CharacterNumMax = Server.GameSetting.CharacterNumMax,
                    EnableVisualEquip = Server.GameSetting.EnableVisualEquip,
                    FriendListMax = Server.GameSetting.FriendListMax,
                    URLInfoList = new List<CDataURLInfo>()
                    {
                        new CDataURLInfo {Type = 1, URL = Server.GameSetting.UrlManual},
                        new CDataURLInfo {Type = 2, URL = Server.GameSetting.UrlShopDetail},
                        new CDataURLInfo {Type = 3, URL = Server.GameSetting.UrlShopCounterA},
                        new CDataURLInfo {Type = 4, URL = Server.GameSetting.UrlShopAttention},
                        new CDataURLInfo {Type = 5, URL = Server.GameSetting.UrlShopStoneLimit},
                        new CDataURLInfo {Type = 6, URL = Server.GameSetting.UrlShopCounterB},
                        new CDataURLInfo {Type = 7, URL = Server.GameSetting.UrlChargeCallback},
                        new CDataURLInfo {Type = 8, URL = Server.GameSetting.UrlChargeA},
                        new CDataURLInfo {Type = 9, URL = Server.GameSetting.UrlSample9},
                        new CDataURLInfo {Type = 10, URL = Server.GameSetting.UrlSample10},
                        new CDataURLInfo {Type = 11, URL = Server.GameSetting.UrlCampaignBanner},
                        new CDataURLInfo {Type = 12, URL = Server.GameSetting.UrlSupportIndex},
                        new CDataURLInfo {Type = 13, URL = Server.GameSetting.UrlPhotoupAuthorize},
                        new CDataURLInfo {Type = 14, URL = Server.GameSetting.UrlApiA},
                        new CDataURLInfo {Type = 15, URL = Server.GameSetting.UrlApiB},
                        new CDataURLInfo {Type = 16, URL = Server.GameSetting.UrlIndex},
                        new CDataURLInfo {Type = 17, URL = Server.GameSetting.UrlCampaign},
                        new CDataURLInfo {Type = 19, URL = Server.GameSetting.UrlChargeB},
                        new CDataURLInfo {Type = 20, URL = Server.GameSetting.UrlCompanionImage},
                    },
                    NoOperationTimeOutTime = Server.Setting.NoOperationTimeOutTime,
                },
            };
            return response;
        }
    }
}
