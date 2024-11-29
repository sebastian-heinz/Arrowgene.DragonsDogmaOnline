#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GachaListHandler : GameRequestPacketHandler<C2SGachaListReq, S2CGachaListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GachaListHandler));

        public GachaListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGachaListRes Handle(GameClient client, C2SGachaListReq request)
        {
            S2CGachaListRes res = new S2CGachaListRes()
            {
                GachaList = Server.AssetRepository.GachaAsset.GachaInfoList.Values.ToList()
            };

#if false
            res.GachaList.Add(new CDataGachaInfo
            {
                Id = 266,
                Begin = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 1000,
                End = DateTimeOffset.UtcNow.AddMonths(12).ToUnixTimeSeconds(),
                Name = "Weapons Lot [TypeA]",
                Description = "Any of the weapons we have sold in the past.\r\nYou will always get one!",
                Detail =
                    "The \"Weapon Lot [Type A]\" is a lottery in which you can win by spending either gold rocks or silver tickets,\r\nSilver Ticket.\r\n\r\nWeapons sold in the past,\r\nYou can get one of the 144 types out of the 290 types of weapons sold in the past.\r\n\r\n<COL ff8c00>[Included lineup]</COL\r\n\r\nEvil mark of the warrior's tool (7 kinds)\r\nTrue Strike of the Demon (8 kinds)\r\nHammer of holy purification(9 kinds)\r\nThe sword flash of black brilliance (9 kinds)\r\nRemembrance of raging flames (10 kinds)\r\nThe bright blade of green jade (12 kinds)\r\nPioneer of blue storm (12 kinds)\r\nConquering instigator(12 kinds)\r\nWise dragon wisdom(13 kinds)\r\nBlade of loyalty(13 kinds)Armor of the water god(13 kinds)\r\nThe armor of protection(13 kinds)\r\nWarrior of iron armor (13 kinds)\r\n\r\n*For details of the lineup, please refer to the \"Product List\".",
                WeightDispType = 2,
                WeightDispTitle = "Percentage Provided",
                WeightDispText = "Class S 100.0%\r\n*Probability of provision is rounded to one decimal place.",
                ListAddr = "http://localhost:52099/shop/img/payment/icon_lot72.jpg",
                ImageAddr = "http://localhost:52099/shop/img/payment/image_lot67_01_campaign2.jpg",
                DrawGroups = new List<CDataGachaDrawGroupInfo>
                {
                    new CDataGachaDrawGroupInfo
                    {
                        GachaSettlementList = new List<CDataGachaSettlementInfo>
                        {
                            new CDataGachaSettlementInfo
                            {
                                DrawGroupId = 351,
                                Id = 1,
                                Price = 1,
                                BasePrice = 1,
                                PurchaseNum = 0,
                                PurchaseMaxNum = 0,
                                SpecialPriceNum = 0,
                                SpecialPriceMaxNum = 0,
                                Unk1 = 0
                            },
                            new CDataGachaSettlementInfo
                            {
                                DrawGroupId = 352,
                                Id = 2,
                                Price = 10,
                                BasePrice = 10,
                                PurchaseNum = 0,
                                PurchaseMaxNum = 0,
                                SpecialPriceNum = 0,
                                SpecialPriceMaxNum = 0,
                                Unk1 = 0
                            }
                        },
                        GachaDrawList = new List<CDataGachaDrawInfo>
                        {
                            new CDataGachaDrawInfo
                            {
                                Num = 1,
                                IsBonus = false,
                                GachaItemInfo = new List<CDataGachaItemInfo>
                                {
                                    new CDataGachaItemInfo
                                    {
                                        ItemId = 10215,
                                        ItemNum = 1,
                                        Rank = 1,
                                        Effect = 0,
                                        Probability = 0.69
                                    }
                                }
                            }
                        }
                    }
                }
            });
#endif

            return res;
        }
    }
}
