#nullable enable
using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpShopDisplayTypeHandler : GameRequestPacketHandler<C2SGpShopDisplayGetTypeReq, S2CGpShopDisplayGetTypeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpShopDisplayTypeHandler));

        public GpShopDisplayTypeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGpShopDisplayGetTypeRes Handle(GameClient client, C2SGpShopDisplayGetTypeReq request)
        {
            // TODO: implement S2C_GP_GP_SHOP_DISPLAY_GET_TYPE_RES
            S2CGpShopDisplayGetTypeRes res = new S2CGpShopDisplayGetTypeRes();
            
            List<CDataGPShopDisplayType> items = new List<CDataGPShopDisplayType>
            {
                new() { ID = 1, Name = "ピックアップ", InGameUrlID = 11 },
                new() { ID = 3, Name = "パスポート", InGameUrlID = 11 },
                new() { ID = 6, Name = "オプションコース", InGameUrlID = 11 },
                new() { ID = 7, Name = "ポーンボイス", InGameUrlID = 11 },
                new() { ID = 8, Name = "クラフト関連", InGameUrlID = 11 },
                new() { ID = 9, Name = "クレスト", InGameUrlID = 11 },
                new() { ID = 10, Name = "消耗品＆装備品", InGameUrlID = 11 },
                new() { ID = 11, Name = "ボイス定型文＆ショートカット枠", InGameUrlID = 11 },
                new() { ID = 12, Name = "LV100＆ジョブ修練セット", InGameUrlID = 11 },
                new() { ID = 13, Name = "LV.100になる宝珠", InGameUrlID = 11 },
                new() { ID = 14, Name = "シーズン1～2戦技・ジョブ修練全解放", InGameUrlID = 11 },
                new() { ID = 15, Name = "！　期間限定　！", InGameUrlID = 11 }
            };
            res.Items = items;

            return res;
        }
    }
}
