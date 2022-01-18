using System.Runtime.Serialization;
using Arrowgene.Ddon.Server;

namespace Arrowgene.Ddon.LoginServer
{
    [DataContract]
    public class LoginServerSetting
    {
        [DataMember(Order = 1)] public ServerSetting ServerSetting { get; set; }
        [DataMember(Order = 10)] public bool AccountRequired { get; set; }
        [DataMember(Order = 100)] public uint JobLevelMax { get; set; }
        [DataMember(Order = 101)] public uint ClanMemberMax { get; set; }
        [DataMember(Order = 102)] public byte CharacterNumMax { get; set; }
        [DataMember(Order = 103)] public bool EnableVisualEquip { get; set; }
        [DataMember(Order = 104)] public uint FriendListMax { get; set; }
        [DataMember(Order = 105)] public uint NoOperationTimeOutTime { get; set; }
        [DataMember(Order = 106)] public string UrlManual { get; set; }
        [DataMember(Order = 106)] public string UrlShopDetail { get; set; }
        [DataMember(Order = 106)] public string UrlShopCounterA { get; set; }
        [DataMember(Order = 106)] public string UrlShopAttention { get; set; }
        [DataMember(Order = 106)] public string UrlShopStoneLimit { get; set; }
        [DataMember(Order = 106)] public string UrlShopCounterB { get; set; }
        [DataMember(Order = 106)] public string UrlChargeCallback { get; set; }
        [DataMember(Order = 106)] public string UrlChargeA { get; set; }
        [DataMember(Order = 106)] public string UrlSample9 { get; set; }
        [DataMember(Order = 106)] public string UrlSample10 { get; set; }
        [DataMember(Order = 106)] public string UrlCampaignBanner { get; set; }
        [DataMember(Order = 106)] public string UrlSupportIndex { get; set; }
        [DataMember(Order = 106)] public string UrlPhotoupAuthorize { get; set; }
        [DataMember(Order = 106)] public string UrlApiA { get; set; }
        [DataMember(Order = 106)] public string UrlApiB { get; set; }
        [DataMember(Order = 106)] public string UrlIndex { get; set; }
        [DataMember(Order = 106)] public string UrlCampaign { get; set; }
        [DataMember(Order = 106)] public string UrlChargeB { get; set; }
        [DataMember(Order = 106)] public string UrlCompanionImage { get; set; }

        public LoginServerSetting()
        {
            ServerSetting = new ServerSetting();
            ServerSetting.ServerPort = 52100;
            ServerSetting.Name = "Login";
            
            AccountRequired = false;
            
            JobLevelMax = 65;
            ClanMemberMax = 100;
            CharacterNumMax = 4;
            EnableVisualEquip = true;
            FriendListMax = 200;
            NoOperationTimeOutTime = 14400;

            string urlDomain = "http://localhost";
            UrlManual = $"{urlDomain}/manual_nfb/";
            UrlShopDetail = $"{urlDomain}/shop/ingame/stone/detail";
            UrlShopCounterA = $"{urlDomain}/shop/ingame/counter?";
            UrlShopAttention = $"{urlDomain}/shop/ingame/attention?";
            UrlShopStoneLimit = $"{urlDomain}/shop/ingame/stone/limit";
            UrlShopCounterB = $"{urlDomain}/shop/ingame/counter?";
            UrlChargeCallback = $"{urlDomain}/opening/entry/ddo/cog_callback/charge";
            UrlChargeA = $"{urlDomain}/sp_ingame/charge/";
            UrlSample9 = "http://sample09.html";
            UrlSample10 = "http://sample10.html";
            UrlCampaignBanner = $"{urlDomain}/sp_ingame/campaign/bnr/bnr01.html?";
            UrlSupportIndex = $"{urlDomain}/sp_ingame/support/index.html";
            UrlPhotoupAuthorize = $"{urlDomain}/api/photoup/authorize";
            UrlApiA = $"{urlDomain}/link/api";
            UrlApiB = $"{urlDomain}/link/api";
            UrlIndex = $"{urlDomain}/sp_ingame/link/index.html";
            UrlCampaign = $"{urlDomain}/sp_ingame/campaign/bnr/slide.html";
            UrlChargeB = $"{urlDomain}/sp_ingame/charge/";
            UrlCompanionImage = $"{urlDomain}/";
        }

        public LoginServerSetting(LoginServerSetting setting)
        {
            ServerSetting = new ServerSetting(setting.ServerSetting);
            
            AccountRequired = setting.AccountRequired;
            
            JobLevelMax = setting.JobLevelMax;
            ClanMemberMax = setting.ClanMemberMax;
            CharacterNumMax = setting.CharacterNumMax;
            EnableVisualEquip = setting.EnableVisualEquip;
            FriendListMax = setting.FriendListMax;
            NoOperationTimeOutTime = setting.NoOperationTimeOutTime;
            UrlManual = setting.UrlManual;
            UrlShopDetail = setting.UrlShopDetail;
            UrlShopCounterA = setting.UrlShopCounterA;
            UrlShopAttention = setting.UrlShopAttention;
            UrlShopStoneLimit = setting.UrlShopStoneLimit;
            UrlShopCounterB = setting.UrlShopCounterB;
            UrlChargeCallback = setting.UrlChargeCallback;
            UrlChargeA = setting.UrlChargeA;
            UrlSample9 = setting.UrlSample9;
            UrlSample10 = setting.UrlSample10;
            UrlCampaignBanner = setting.UrlCampaignBanner;
            UrlSupportIndex = setting.UrlSupportIndex;
            UrlPhotoupAuthorize = setting.UrlPhotoupAuthorize;
            UrlApiA = setting.UrlApiA;
            UrlApiB = setting.UrlApiB;
            UrlIndex = setting.UrlIndex;
            UrlCampaign = setting.UrlCampaign;
            UrlChargeB = setting.UrlChargeB;
            UrlCompanionImage = setting.UrlCompanionImage;
        }
    }
}
