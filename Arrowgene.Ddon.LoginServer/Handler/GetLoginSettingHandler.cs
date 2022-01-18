using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GetLoginSettingHandler : PacketHandler<LoginClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GetLoginSettingHandler));

        private readonly LoginServerSetting _setting;

        public GetLoginSettingHandler(DdonLoginServer server) : base(server)
        {
            _setting = server.Setting;
        }

        public override PacketId Id => PacketId.C2L_GET_LOGIN_SETTING_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
            L2CGetLoginSettingsRes entity = new L2CGetLoginSettingsRes
            {
                LoginSetting =
                {
                    JobLevelMax = _setting.JobLevelMax,
                    ClanMemberMax = _setting.ClanMemberMax,
                    CharacterNumMax = _setting.CharacterNumMax,
                    EnableVisualEquip = _setting.EnableVisualEquip,
                    FriendListMax = _setting.FriendListMax,
                    URLInfoList = new List<CDataURLInfo>()
                    {
                        new CDataURLInfo {Type = 1, URL = _setting.UrlManual},
                        new CDataURLInfo {Type = 2, URL = _setting.UrlShopDetail},
                        new CDataURLInfo {Type = 3, URL = _setting.UrlShopCounterA},
                        new CDataURLInfo {Type = 4, URL = _setting.UrlShopAttention},
                        new CDataURLInfo {Type = 5, URL = _setting.UrlShopStoneLimit},
                        new CDataURLInfo {Type = 6, URL = _setting.UrlShopCounterB},
                        new CDataURLInfo {Type = 7, URL = _setting.UrlChargeCallback},
                        new CDataURLInfo {Type = 8, URL = _setting.UrlChargeA},
                        new CDataURLInfo {Type = 9, URL = _setting.UrlSample9},
                        new CDataURLInfo {Type = 10, URL = _setting.UrlSample10},
                        new CDataURLInfo {Type = 11, URL = _setting.UrlCampaignBanner},
                        new CDataURLInfo {Type = 12, URL = _setting.UrlSupportIndex},
                        new CDataURLInfo {Type = 13, URL = _setting.UrlPhotoupAuthorize},
                        new CDataURLInfo {Type = 14, URL = _setting.UrlApiA},
                        new CDataURLInfo {Type = 15, URL = _setting.UrlApiB},
                        new CDataURLInfo {Type = 16, URL = _setting.UrlIndex},
                        new CDataURLInfo {Type = 17, URL = _setting.UrlCampaign},
                        new CDataURLInfo {Type = 19, URL = _setting.UrlChargeB},
                        new CDataURLInfo {Type = 20, URL = _setting.UrlCompanionImage},
                    },
                    NoOperationTimeOutTime = 14400,
                },
            };
            client.Send(entity);
        }
    }
}
