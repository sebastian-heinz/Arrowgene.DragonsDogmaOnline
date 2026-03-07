using Arrowgene.Ddon.Server.Settings;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.LoginServer.Handler;

public sealed class GetLoginSettingHandler(DdonLoginServer server) : LoginRequestPacketHandler<C2LGetLoginSettingReq, L2CGetLoginSettingRes>(server)
{
    private readonly L2CGetLoginSettingRes _response = BuildResponse(server);

    public override L2CGetLoginSettingRes Handle(
        LoginClient client,
        C2LGetLoginSettingReq request)
    {
        return _response;
    }

    private static L2CGetLoginSettingRes BuildResponse(DdonLoginServer server)
    {
        GameServerSettings settings = server.GameSetting.GameServerSettings;

        L2CGetLoginSettingRes response = new()
        {
            LoginSetting =
            {
                JobLevelMax = settings.JobLevelMax,
                ClanMemberMax = settings.ClanMemberMax,
                CharacterNumMax = settings.CharacterNumMax,
                EnableVisualEquip = settings.EnableVisualEquip,
                FriendListMax = settings.FriendListMax,
                NoOperationTimeOutTime = server.Setting.NoOperationTimeOutTime
            }
        };

        (uint, string)[] urlInfos =
        [
            (1u, settings.UrlManual),
            (2u, settings.UrlShopDetail),
            (3u, settings.UrlShopCounterA),
            (4u, settings.UrlShopAttention),
            (5u, settings.UrlShopStoneLimit),
            (6u, settings.UrlShopCounterB),
            (7u, settings.UrlChargeCallback),
            (8u, settings.UrlChargeA),
            (9u, settings.UrlSample9),
            (10u, settings.UrlSample10),
            (11u, settings.UrlCampaignBanner),
            (12u, settings.UrlSupportIndex),
            (13u, settings.UrlPhotoupAuthorize),
            (14u, settings.UrlApiA),
            (15u, settings.UrlApiB),
            (16u, settings.UrlIndex),
            (17u, settings.UrlCampaign),
            (19u, settings.UrlChargeB),
            (20u, settings.UrlCompanionImage)
        ];

        foreach ((uint type, string url) in urlInfos)
            response.LoginSetting.URLInfoList.Add(
                new CDataURLInfo { Type = type, URL = url }
            );

        return response;
    }
}
