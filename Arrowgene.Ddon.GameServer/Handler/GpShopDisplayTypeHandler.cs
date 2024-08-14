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
            S2CGpShopDisplayGetTypeRes res = new S2CGpShopDisplayGetTypeRes();
            
            // https://h1g.jp/dd-on/?%E3%82%AA%E3%83%B3%E3%83%A9%E3%82%A4%E3%83%B3%E3%82%B7%E3%83%A7%E3%83%83%E3%83%97
            // TODO: add to some asset
            List<CDataGPShopDisplayType> items = new List<CDataGPShopDisplayType>
            {
                new() { ID = 1, Name = "Selection", InGameUrlID = 11 },
                new() { ID = 3, Name = "Passport", InGameUrlID = 11 },
                // new() { ID = 6, Name = "Optional Courses", InGameUrlID = 11 },
                // new() { ID = 7, Name = "Pawn Voices", InGameUrlID = 11 },
                // new() { ID = 8, Name = "Crafting related", InGameUrlID = 11 },
                // new() { ID = 9, Name = "Crests", InGameUrlID = 11 },
                // new() { ID = 10, Name = "Consumables & Accessories", InGameUrlID = 11 },
                // new() { ID = 11, Name = "Voice Templates & Shortcut Box", InGameUrlID = 11 },
                // new() { ID = 12, Name = "Lvl. 100 & Job Training Set", InGameUrlID = 11 },
                // new() { ID = 13, Name = "Pearl to be lvl. 100", InGameUrlID = 11 },
                // new() { ID = 14, Name = "Season 1-2 combat skills and job training all unlocked", InGameUrlID = 11 },
                // new() { ID = 15, Name = "!Limited time only!", InGameUrlID = 11 }
            };
            res.Items = items;

            return res;
        }
    }
}
