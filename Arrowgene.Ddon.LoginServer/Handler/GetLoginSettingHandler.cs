using Arrowgene.Buffers;
using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class GetLoginSettingHandler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(GetLoginSettingHandler));


        public GetLoginSettingHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GET_LOGIN_SETTING_REQ;

        public override void Handle(LoginClient client, Packet packet)
        {
            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0, Endianness.Big); // error
            buffer.WriteInt32(0, Endianness.Big); // result

            L2CGetLoginSettingsRes entity = new L2CGetLoginSettingsRes
            {
                LoginSetting = {
                    JobLevelMax = 65,
                    ClanMemberMax = 100,
                    CharacterNumMax = 4,
                    EnableVisualEquip = true,
                    FriendListMax = 200,
                    URLInfoList = new List<CDataURLInfo>()
                    {
                        new CDataURLInfo {Type = 1, URL = "https://members.dd-on.jp/manual_nfb/"},
                        new CDataURLInfo {Type = 2, URL = "https://members.dd-on.jp/shop/ingame/stone/detail"},
                        new CDataURLInfo {Type = 3, URL = "https://members.dd-on.jp/shop/ingame/counter?"},
                        new CDataURLInfo {Type = 4, URL = "https://members.dd-on.jp/shop/ingame/attention?"},
                        new CDataURLInfo {Type = 5, URL = "https://members.dd-on.jp/shop/ingame/stone/limit"},
                        new CDataURLInfo {Type = 6, URL = "https://members.dd-on.jp/shop/ingame/counter?"},
                        new CDataURLInfo {Type = 7, URL = "https://startup-server.dd-on.jp/opening/entry/ddo/cog_callback/capcharge"},
                        new CDataURLInfo {Type = 8, URL = "https://members.dd-on.jp/sp_ingame/capcharge/"},
                        new CDataURLInfo {Type = 9, URL = "http://sample09.html"},
                        new CDataURLInfo {Type = 10, URL = "http://sample10.html"},
                        new CDataURLInfo {Type = 11, URL = "http://members.dd-on.jp/sp_ingame/campaign/bnr/bnr01.html?"},
                        new CDataURLInfo {Type = 12, URL = "https://members.dd-on.jp/sp_ingame/support/index.html"},
                        new CDataURLInfo {Type = 13, URL = "https://members.dd-on.jp/api/photoup/authorize"},
                        new CDataURLInfo {Type = 14, URL = "http://frontapi.caplink.jp"},
                        new CDataURLInfo {Type = 15, URL = "https://frontapi.caplink.jp"},
                        new CDataURLInfo {Type = 16, URL = "https://members.dd-on.jp/sp_ingame/caplink/index.html"},
                        new CDataURLInfo {Type = 17, URL = "https://members.dd-on.jp/sp_ingame/campaign/bnr/slide.html"},
                        new CDataURLInfo {Type = 19, URL = "https://members.dd-on.jp/sp_ingame/capcharge/"},
                        new CDataURLInfo {Type = 20, URL = "https://companion-img.dd-on.jp/"},
                    },
                    NoOperationTimeOutTime = 14400,
                },
            };
            EntitySerializer.Get<L2CGetLoginSettingsRes>().Write(buffer, entity);

            Packet response = new Packet(PacketId.L2C_GET_LOGIN_SETTING_RES, buffer.GetAllBytes());
            client.Send(response);

            //client.Send(LoginDump.Dump_20);
        }
    }
}
