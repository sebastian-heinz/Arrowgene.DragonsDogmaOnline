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
        private readonly GameLogicSetting _gameSetting;

        public GetLoginSettingHandler(DdonLoginServer server) : base(server)
        {
            _setting = server.Setting;
            _gameSetting = server.GameSetting;
        }

        public override PacketId Id => PacketId.C2L_GET_LOGIN_SETTING_REQ;

        public override void Handle(LoginClient client, IPacket packet)
        {
            L2CGetLoginSettingsRes entity = new L2CGetLoginSettingsRes
            {
                LoginSetting =
                {
                    JobLevelMax = _gameSetting.JobLevelMax,
                    ClanMemberMax = _gameSetting.ClanMemberMax,
                    CharacterNumMax = _gameSetting.CharacterNumMax,
                    EnableVisualEquip = _gameSetting.EnableVisualEquip,
                    FriendListMax = _gameSetting.FriendListMax,
                    URLInfoList = new List<CDataURLInfo>()
                    {
                        new CDataURLInfo {Type = 1, URL = _gameSetting.UrlManual},
                        new CDataURLInfo {Type = 2, URL = _gameSetting.UrlShopDetail},
                        new CDataURLInfo {Type = 3, URL = _gameSetting.UrlShopCounterA},
                        new CDataURLInfo {Type = 4, URL = _gameSetting.UrlShopAttention},
                        new CDataURLInfo {Type = 5, URL = _gameSetting.UrlShopStoneLimit},
                        new CDataURLInfo {Type = 6, URL = _gameSetting.UrlShopCounterB},
                        new CDataURLInfo {Type = 7, URL = _gameSetting.UrlChargeCallback},
                        new CDataURLInfo {Type = 8, URL = _gameSetting.UrlChargeA},
                        new CDataURLInfo {Type = 9, URL = _gameSetting.UrlSample9},
                        new CDataURLInfo {Type = 10, URL = _gameSetting.UrlSample10},
                        new CDataURLInfo {Type = 11, URL = _gameSetting.UrlCampaignBanner},
                        new CDataURLInfo {Type = 12, URL = _gameSetting.UrlSupportIndex},
                        new CDataURLInfo {Type = 13, URL = _gameSetting.UrlPhotoupAuthorize},
                        new CDataURLInfo {Type = 14, URL = _gameSetting.UrlApiA},
                        new CDataURLInfo {Type = 15, URL = _gameSetting.UrlApiB},
                        new CDataURLInfo {Type = 16, URL = _gameSetting.UrlIndex},
                        new CDataURLInfo {Type = 17, URL = _gameSetting.UrlCampaign},
                        new CDataURLInfo {Type = 19, URL = _gameSetting.UrlChargeB},
                        new CDataURLInfo {Type = 20, URL = _gameSetting.UrlCompanionImage},
                    },
                    NoOperationTimeOutTime = _setting.NoOperationTimeOutTime,
                },
            };
            client.Send(entity);
        }
    }
}
