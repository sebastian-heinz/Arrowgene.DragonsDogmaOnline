using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class MyRoomFurnitureListGetHandler : GameRequestPacketHandler<C2SMyRoomFurnitureListGetReq, S2CMyRoomFurnitureListGetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(MyRoomFurnitureListGetHandler));

        // TODO: Placed furniture probably needs to be saved per character in a table whenever the furniture placement feature is used via the partner pawn
        private static readonly List<CDataFurnitureLayout> FurnitureList = new List<CDataFurnitureLayout>
        {
            new CDataFurnitureLayout
            {
                ItemID = ItemId.ArisensDesk, 
                OmID = 502930,
                LayoutID = 1
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.MiniTable, 
                OmID = 502931,
                LayoutID = 2
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.MusicPlayer, 
                OmID = 503051,
                LayoutID = 3
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.WallPaintLestania, 
                OmID = 525066,
                LayoutID = 39
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.WhiteDayCarpet, 
                OmID = 525384,
                LayoutID = 4
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.AChair, 
                OmID = 502934,
                LayoutID = 5
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.Bookshelves, 
                OmID = 502935,
                LayoutID = 6
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.DiningTable0, 
                OmID = 502936,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.SingleBed, 
                OmID = 502941,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.Table, 
                OmID = 502948,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.Chest, 
                OmID = 502950,
                LayoutID = 20
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.Bath, 
                OmID = 502953,
                LayoutID = 0
            },
            //new CDataFurnitureLayout
            //{
            //    ItemID = ItemId.ServantsBathingClothesType1, 
            //    OmID = 999999,
            //    LayoutID = 31
            //},
            new CDataFurnitureLayout
            {
                ItemID = ItemId.SheetMusicProtectionOfTheFiveDragons, 
                OmID = 999998,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.SheetMusicTheGirlWhoLostHerMemories, 
                OmID = 999998,
                LayoutID = 61
            },
            //new CDataFurnitureLayout
            //{
            //    ItemID = ItemId.ServantsSleepwearType2,
            //    OmID = 999999,
            //    LayoutID = 32
            //},
            new CDataFurnitureLayout
            {
                ItemID = ItemId.RoomLightChandelier, 
                OmID = 503104,
                LayoutID = 35
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.DiningFlowerPot, 
                OmID = 503107,
                LayoutID = 36
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.LestaniaWeatherForecast, 
                OmID = 522980,
                LayoutID = 38
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.ValentineCarpet0, 
                OmID = 503173,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.DiningTableBitter, 
                OmID = 503174,
                LayoutID = 7
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.PuppetLeviCleaningVer, 
                OmID = 525231,
                LayoutID = 11
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.FoodstuffStorageRack, 
                OmID = 502938,
                LayoutID = 12
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.BrickCookingStation, 
                OmID = 502939,
                LayoutID = 8
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.ValentineCushion, 
                OmID = 503172,
                LayoutID = 15
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.MoodyLampValentine, 
                OmID = 503171,
                LayoutID = 17
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.ChocolateFountain, 
                OmID = 503170,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.AntiqueDeskSnow, 
                OmID = 525051,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.AristocratBathManticore, 
                OmID = 525111,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.AristocratBathWyrm, 
                OmID = 525117,
                LayoutID = 40
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.AristocratBathDrake, 
                OmID = 525118,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.WoolenCarpetSnow, 
                OmID = 525190,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.TapestryDiscord, 
                OmID = 525156,
                LayoutID = 42
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.BasicLivingRoomCarpet, 
                OmID = 525195,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.ChattingBell, 
                OmID = 525220,
                LayoutID = 46
            },
            //new CDataFurnitureLayout
            //{
            //    ItemID = ItemId.MandragoraPottedPlant1Normal, 
            //    OmID = 525000,
            //    LayoutID = 47
            //},
            //new CDataFurnitureLayout
            //{
            //    ItemID = ItemId.MandragoraPottedPlant2Passport, 
            //    OmID = 525001,
            //    LayoutID = 48
            //},
            //new CDataFurnitureLayout
            //{
            //    ItemID = ItemId.MandragoraPottedPlant3Passport, 
            //    OmID = 525002,
            //    LayoutID = 49
            //},
            new CDataFurnitureLayout
            {
                ItemID = ItemId.HeartsLivingRoomCarpetValentine, 
                OmID = 525086,
                LayoutID = 44
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.Closet, 
                OmID = 502951,
                LayoutID = 45
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.SheetMusicAgentOfCorruption, 
                OmID = 999998,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.ValentinePuppet, // Valentine Puppet
                OmID = 525340,
                LayoutID = 10
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.BathValentineHotTub, // Bath - Valentine Hot Tub
                OmID = 525342,
                LayoutID = 25
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.ThirdAnniversaryDecoration, // 3rd Anniversary Decoration
                OmID = 525353,
                LayoutID = 26
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.CookieTableBitter, // Cookie Table - Bitter
                OmID = 525343,
                LayoutID = 18
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.SweetsSetValentine, // Sweets Set - Valentine
                OmID = 525348,
                LayoutID = 9
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.ValentineBed, // Valentine Bed
                OmID = 525407,
                LayoutID = 14
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.ValentineTable, // Valentine Table
                OmID = 525399,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.ValentineCookware, // Valentine Cookware
                OmID = 525382,
                LayoutID = 13
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.WhiteDayClock, // White Day Clock
                OmID = 525404,
                LayoutID = 41
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.TapestryFourthAnniversary, // Tapestry - 4th Anniversary
                OmID = 525395,
                LayoutID = 43
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.MoodyLampFourthAnniversary, // Moody Lamp - 4th Anniversary
                OmID = 525390,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.FourthAnniversaryCake, // 4th Anniversary Cake
                OmID = 525391,
                LayoutID = 19
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.FourthAnniversaryPuppet, // 4th Anniversary Puppet
                OmID = 525393,
                LayoutID = 21
            },
            
            new CDataFurnitureLayout
            {
                ItemID = ItemId.LightVisionLeo, // Light Vision: Leo
                OmID = 999997,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.LightVisionMysial, // Light Vision: Mysial
                OmID = 999997,
                LayoutID = 0
            },
            
            new CDataFurnitureLayout
            {
                ItemID = ItemId.MemoryReflectingCrystalBall, // Memory Reflecting Crystal Ball
                OmID = 525411,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = ItemId.StereoscopicProjector, // Stereoscopic Projector
                OmID = 525313,
                LayoutID = 27
            },
            
            new CDataFurnitureLayout
            {
                ItemID = ItemId.GardenOrnamentChristmas, // Garden Ornament - Christmas
                OmID = 525333,
                LayoutID = 28
            },

            //new CDataFurnitureLayout
            //{
            //    ItemID = ItemId.ArisensLoungewearType1, // Arisen's Loungewear (Type 1)
            //    OmID = 999999,
            //    LayoutID = 29
            //},
            //new CDataFurnitureLayout
            //{
            //    ItemID = ItemId.ServantsLoungewearType1, // Servant's Loungewear (Type 1)
            //    OmID = 999999,
            //    LayoutID = 30
            //},
            new CDataFurnitureLayout
            {
                ItemID = ItemId.LestanianPuppetTower, // Lestanian Puppet - Tower
                OmID = 502957,
                LayoutID = 34
            },
        };

        public MyRoomFurnitureListGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMyRoomFurnitureListGetRes Handle(GameClient client, C2SMyRoomFurnitureListGetReq request)
        {
            S2CMyRoomFurnitureListGetRes res = new S2CMyRoomFurnitureListGetRes();

            // TODO: A list of acquired BGMs needs to be stored per character
            res.MyRoomOption = new CDataMyRoomOption
            {
                BgmAcquirementNoList = new List<CDataCommonU32>
                {
                    new ((int)ItemId.SheetMusicProtectionOfTheFiveDragons)
                },
                BgmAcquirementNo = ItemId.SheetMusicTheGirlWhoLostHerMemories,
                ActivePlanetariumNo = 0
            };

            res.FurnitureList = FurnitureList;

            return res;
        }
    }
}
