using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
                ItemID = 11760, // Arisen's Desk
                OmID = 502930,
                LayoutID = 1
            },
            new CDataFurnitureLayout
            {
                ItemID = 13225, // Mini Table
                OmID = 502931,
                LayoutID = 2
            },
            new CDataFurnitureLayout
            {
                ItemID = 16134, // Music Player
                OmID = 503051,
                LayoutID = 3
            },
            new CDataFurnitureLayout
            {
                ItemID = 17103, // Wall Paint - Lestania
                OmID = 525066,
                LayoutID = 39
            },
            new CDataFurnitureLayout
            {
                ItemID = 23486, // White Day Carpet
                OmID = 525384,
                LayoutID = 4
            },
            new CDataFurnitureLayout
            {
                ItemID = 13228, // A Chair
                OmID = 502934,
                LayoutID = 5
            },
            new CDataFurnitureLayout
            {
                ItemID = 13229, // Bookshelves
                OmID = 502935,
                LayoutID = 6
            },
            new CDataFurnitureLayout
            {
                ItemID = 13230, // Dining Table
                OmID = 502936,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 13237, // Single Bed
                OmID = 502941,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 13244, // Table
                OmID = 502948,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 13246, // Chest
                OmID = 502950,
                LayoutID = 20
            },
            new CDataFurnitureLayout
            {
                ItemID = 13251, // Bath
                OmID = 502953,
                LayoutID = 0
            },
            //new CDataFurnitureLayout
            //{
            //    ItemID = 16128, // Servant's Bathing Clothes (Type 1)
            //    OmID = 999999,
            //    LayoutID = 31
            //},
            new CDataFurnitureLayout
            {
                ItemID = 21470, // Sheet Music - Protection of the Five Dragons
                OmID = 999998,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 21476, // Sheet Music - The Girl Who Lost Her Memories
                OmID = 999998,
                LayoutID = 61
            },
            //new CDataFurnitureLayout
            //{
            //    ItemID = 16132, // Servant's Sleepwear (Type 2)
            //    OmID = 999999,
            //    LayoutID = 32
            //},
            new CDataFurnitureLayout
            {
                ItemID = 16167, // Room Light - Chandelier
                OmID = 503104,
                LayoutID = 35
            },
            new CDataFurnitureLayout
            {
                ItemID = 16177, // Dining - Flower Pot
                OmID = 503107,
                LayoutID = 36
            },
            new CDataFurnitureLayout
            {
                ItemID = 16727, // Lestania Weather Forecast
                OmID = 522980,
                LayoutID = 38
            },
            new CDataFurnitureLayout
            {
                ItemID = 16745, // Valentine Carpet
                OmID = 503173,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 16749, // Dining Table - Bitter
                OmID = 503174,
                LayoutID = 7
            },
            new CDataFurnitureLayout
            {
                ItemID = 17932, // Puppet - Levi cleaning ver.
                OmID = 525231,
                LayoutID = 11
            },
            new CDataFurnitureLayout
            {
                ItemID = 13235, // Foodstuff Storage Rack
                OmID = 502938,
                LayoutID = 12
            },
            new CDataFurnitureLayout
            {
                ItemID = 13231, // Brick Cooking Station
                OmID = 502939,
                LayoutID = 8
            },
            new CDataFurnitureLayout
            {
                ItemID = 16751, // Valentine Cushion
                OmID = 503172,
                LayoutID = 15
            },
            new CDataFurnitureLayout
            {
                ItemID = 16753, // Moody Lamp - Valentine
                OmID = 503171,
                LayoutID = 17
            },
            new CDataFurnitureLayout
            {
                ItemID = 16755, // Chocolate Fountain
                OmID = 503170,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 17070, // Antique Desk - Snow
                OmID = 525051,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 17087, // Aristocrat Bath - Manticore
                OmID = 525111,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 17093, // Aristocrat Bath - Wyrm
                OmID = 525117,
                LayoutID = 40
            },
            new CDataFurnitureLayout
            {
                ItemID = 17094, // Aristocrat Bath - Drake
                OmID = 525118,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 17115, // Woolen Carpet - Snow
                OmID = 525190,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 17138, // Tapestry - Discord
                OmID = 525156,
                LayoutID = 42
            },
            new CDataFurnitureLayout
            {
                ItemID = 17151, // Basic Living Room Carpet
                OmID = 525195,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 17152, // Chatting Bell
                OmID = 525220,
                LayoutID = 46
            },
            //new CDataFurnitureLayout
            //{
            //    ItemID = 17370, // Mandragora Potted Plant 1 (Normal)
            //    OmID = 525000,
            //    LayoutID = 47
            //},
            //new CDataFurnitureLayout
            //{
            //    ItemID = 17371, // Mandragora Potted Plant 2 (Passport)
            //    OmID = 525001,
            //    LayoutID = 48
            //},
            //new CDataFurnitureLayout
            //{
            //    ItemID = 17372, // Mandragora Potted Plant 3 (Passport)
            //    OmID = 525002,
            //    LayoutID = 49
            //},
            new CDataFurnitureLayout
            {
                ItemID = 19642, // Heart's Living Room Carpet - Valentine
                OmID = 525086,
                LayoutID = 44
            },
            new CDataFurnitureLayout
            {
                ItemID = 19642, // Closet
                OmID = 502951,
                LayoutID = 45
            },
            new CDataFurnitureLayout
            {
                ItemID = 21476, // Sheet Music - The Girl Who Lost Her Memories
                OmID = 999998,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 21546, // Valentine Puppet
                OmID = 525340,
                LayoutID = 10
            },
            new CDataFurnitureLayout
            {
                ItemID = 21548, // Bath - Valentine Hot Tub
                OmID = 525342,
                LayoutID = 25
            },
            new CDataFurnitureLayout
            {
                ItemID = 21660, // 3rd Anniversary Decoration
                OmID = 525353,
                LayoutID = 26
            },
            new CDataFurnitureLayout
            {
                ItemID = 21549, // Cookie Table - Bitter
                OmID = 525343,
                LayoutID = 18
            },
            new CDataFurnitureLayout
            {
                ItemID = 21554, // Sweets Set - Valentine
                OmID = 525348,
                LayoutID = 9
            },
            new CDataFurnitureLayout
            {
                ItemID = 23477, // Valentine Bed
                OmID = 525407,
                LayoutID = 14
            },
            new CDataFurnitureLayout
            {
                ItemID = 23479, // Valentine Table
                OmID = 525399,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 23481, // Valentine Cookware
                OmID = 525382,
                LayoutID = 13
            },
            new CDataFurnitureLayout
            {
                ItemID = 23484, // White Day Clock
                OmID = 525404,
                LayoutID = 41
            },
            new CDataFurnitureLayout
            {
                ItemID = 23502, // Tapestry - 4th Anniversary
                OmID = 525395,
                LayoutID = 43
            },
            new CDataFurnitureLayout
            {
                ItemID = 23503, // Moody Lamp - 4th Anniversary
                OmID = 525390,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 23504, // 4th Anniversary Cake
                OmID = 525391,
                LayoutID = 19
            },
            new CDataFurnitureLayout
            {
                ItemID = 23505, // 4th Anniversary Puppet
                OmID = 525393,
                LayoutID = 21
            },
            
            new CDataFurnitureLayout
            {
                ItemID = 23525, // Light Vision: Leo
                OmID = 999997,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 23526, // Light Vision: Mysial
                OmID = 999997,
                LayoutID = 0
            },
            
            new CDataFurnitureLayout
            {
                ItemID = 25028, // Memory Reflecting Crystal Ball
                OmID = 525411,
                LayoutID = 0
            },
            new CDataFurnitureLayout
            {
                ItemID = 19563, // Stereoscopic Projector
                OmID = 525313,
                LayoutID = 27
            },
            
            new CDataFurnitureLayout
            {
                ItemID = 21185, // Garden Ornament - Christmas
                OmID = 525333,
                LayoutID = 28
            },
            
            //new CDataFurnitureLayout
            //{
            //    ItemID = 16122, // Arisen's Loungewear (Type 1)
            //    OmID = 999999,
            //    LayoutID = 29
            //},
            //new CDataFurnitureLayout
            //{
            //    ItemID = 16125, // Servant's Loungewear (Type 1)
            //    OmID = 999999,
            //    LayoutID = 30
            //},
            new CDataFurnitureLayout
            {
                ItemID = 13240, // Lestanian Puppet - Tower
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
                    new (21470)
                },
                BgmAcquirementNo = 21476,
                ActivePlanetariumNo = 0
            };

            res.FurnitureList = FurnitureList;

            return res;
        }
    }
}
