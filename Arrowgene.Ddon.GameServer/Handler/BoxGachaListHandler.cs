using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BoxGachaListHandler : GameRequestPacketHandler<C2SBoxGachaListReq, S2CBoxGachaListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BoxGachaListHandler));

        public BoxGachaListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBoxGachaListRes Handle(GameClient client, C2SBoxGachaListReq request)
        {
            throw new ResponseErrorException(ErrorCode.ERROR_CODE_NOT_IMPLEMENTED);

            S2CBoxGachaListRes res = new S2CBoxGachaListRes();

            string urlBase = Server.GameSettings.GameServerSettings.UrlDomain;

            res.BoxGachaList.Add(new CDataBoxGachaInfo
            {
                Id = 93,
                Begin = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 1000,
                End = DateTimeOffset.UtcNow.AddMonths(12).ToUnixTimeSeconds(),
                Unk1 = true,
                Name = "Consumables box lot",
                Description = "Rare supplies are available!",
                Detail =
                    "The \"Consumables Box Lot\" is a lottery that can be drawn by spending Silver Tickets.\r\n\r\nYou can get consumables such as \"White Dragon Seal Elixir, Top\" and \"Talisman of the Twin Gods\". \r\n\r\nYou can reset the contents of the box to its initial state with the <COL ffff00>About Box Treasure Slot</COL> reset button, so please use it when you have obtained all the items you are looking for.",
                WeightDispType = 1,
                FreeSpaceText = "",
                ListAddr = $"{urlBase}/sp_ingame/campaign/bnr/lotto/lot_icon_170316_03.jpg",
                ImageAddr = $"{urlBase}/sp_ingame/campaign/bnr/lotto/lot_icon_170316_03.jpg",
                SettlementList = new List<CDataBoxGachaSettlementInfo>
                {
                    new()
                    {
                        DrawId = 199,
                        Id = 2,
                        Price = 10,
                        BasePrice = 10,
                        DrawNum = 1,
                        BonusNum = 0,
                        PurchaseNum = 0,
                        PurchaseMaxNum = 0,
                        SpecialPriceNum = 0,
                        SpecialPriceMaxNum = 0,
                        Unk1 = 0
                    }
                },
                BoxGachaSets = new List<CDataBoxGachaItemInfo>
                {
                    new()
                    {
                        ItemId = 13800,
                        ItemNum = 5,
                        ItemStock = 13,
                        Rank = 2,
                        Effect = 0,
                        Probability = 0,
                        DrawNum = 0
                    }
                }
            });

            return res;
        }
    }
}
