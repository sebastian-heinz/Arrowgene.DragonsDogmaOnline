using System.Collections.Generic;
using System.Linq;
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

        public MyRoomFurnitureListGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CMyRoomFurnitureListGetRes Handle(GameClient client, C2SMyRoomFurnitureListGetReq request)
        {
            var customizations = Server.Database.SelectMyRoomCustomization(client.Character.CharacterId);
            foreach (var requiredCustom in RequiredFurniturePlacements)
            {
                if (!customizations.ContainsValue(requiredCustom.Value))
                {
                    customizations[requiredCustom.Key] = requiredCustom.Value;
                }
            }

            var unlockedItems = client.Character.UnlockableItems
                .Where(x => x.Category == UnlockableItemCategory.FurnitureItem)
                .Select(x => (ItemId)x.Id)
                .Union(DefaultFurnitureIds);

            S2CMyRoomFurnitureListGetRes res = new();

            res.MyRoomOption.BgmAcquirementNoList = unlockedItems
                .Intersect(SheetMusicIds)
                .Select(x => new CDataCommonU32((uint)x))
                .ToList();
            res.MyRoomOption.BgmAcquirementNo = customizations.FirstOrDefault(x => x.Value == MyRoomMyRoomBgmUpdateHandler.MYROOM_BGM_LAYOUTNO).Key;
            res.MyRoomOption.ActivePlanetariumNo = customizations.FirstOrDefault(x => x.Value == MyRoomUpdatePlanetariumHandler.MYROOM_PLANETARIUM_LAYOUTNO).Key;

            res.FurnitureList = unlockedItems
                .Select(x => new CDataFurnitureLayout()
                {
                    ItemID = x,
                    OmID = 0,
                    LayoutID = SheetMusicIds.Contains(x) || LightVisionIds.Contains(x) ? (byte)0 : customizations.GetValueOrDefault(x),
                })
                .ToList();

            return res;
        }

        /// <summary>
        /// Furniture that comes with the room by default. Several of these occupy unique slots and cannot be replaced. 
        /// </summary>
        public static readonly HashSet<ItemId> DefaultFurnitureIds = new HashSet<ItemId>()
        {
            ItemId.AChair,
            ItemId.ArisensDesk,
            ItemId.BasicLivingRoomCarpet,
            ItemId.Bath,
            ItemId.Chest,
            ItemId.DiningTable0,
            ItemId.ChattingBell, // Is this granted by a personal quest?
            ItemId.RoomLightChandelier,
            ItemId.SingleBed,
            ItemId.Table,
            ItemId.MemoryReflectingCrystalBall, // This is technically a craft/event reward but is too cool to gate off.
        };

        /// <summary>
        /// Furniture that either fills spots that do not permit being empty, or fill special slots that cannot be overwritten by the player.
        /// Will fill their LayoutId if no other customization entry exists for that slot.
        /// </summary>
        public static readonly Dictionary<ItemId, byte> RequiredFurniturePlacements = new()
        {
            { ItemId.ArisensDesk, 1 },                          // Default
            { ItemId.AChair, 5 },                               // Special Slot
            { ItemId.DiningTable0, 7 },                         // Default
            { ItemId.SingleBed, 14 },                           // Default
            { ItemId.Table, 18 },                               // Default
            { ItemId.Chest, 20 },                               // Special Slot
            { ItemId.Bath, 25 },                                // Default
            { ItemId.RoomLightChandelier, 35 },                 // Default
            { ItemId.ChattingBell, 46 },                        // Special Slot
            //{ ItemId.MandragoraPottedPlant1Normal, 47 },      // Special Slot
            //{ ItemId.MandragoraPottedPlant2Passport, 48 },    // Special Slot
            //{ ItemId.MandragoraPottedPlant3Passport, 49 }     // Special Slot
        };

        /// <summary>
        /// "Furniture" items for the various sheet music used by the Music Player.
        /// </summary>
        public static readonly HashSet<ItemId> SheetMusicIds = new HashSet<ItemId>()
        {
            ItemId.SheetMusicProtectionOfTheFiveDragons,
            ItemId.SheetMusicTwinFangsOfTheEarthDragon,
            ItemId.SheetMusicImminentTriumph,
            ItemId.SheetMusicTheStolenHeart,
            ItemId.SheetMusicMostSimpleAndBeautifulWorld,
            ItemId.SheetMusicHollowOfBeginnings,
            ItemId.SheetMusicTheGirlWhoLostHerMemories,
            ItemId.SheetMusicAgentOfCorruption,
            ItemId.SheetMusicWarriorOfEvil,
            ItemId.SheetMusicSpiritDragonKing,
            ItemId.SheetMusicImminentTriumphSpiritDragonKing
        };

        /// <summary>
        /// "Furniture" items for the various light vision displays used by the Stereoscopic Projector.
        /// </summary>
        public static readonly HashSet<ItemId> LightVisionIds = new HashSet<ItemId>()
        {
            ItemId.LightVision,
            ItemId.LightVisionAdairDonnchadh,
            ItemId.LightVisionBertha,
            ItemId.LightVisionCameron,
            ItemId.LightVisionCamus,
            ItemId.LightVisionCarrie,
            ItemId.LightVisionCecily,
            ItemId.LightVisionChester,
            ItemId.LightVisionCornelia,
            ItemId.LightVisionCraig,
            ItemId.LightVisionDiamantes,
            ItemId.LightVisionElliot,
            ItemId.LightVisionEmerada,
            ItemId.LightVisionFabio,
            ItemId.LightVisionGerd,
            ItemId.LightVisionGillian,
            ItemId.LightVisionGurdolin,
            ItemId.LightVisionHeinz,
            ItemId.LightVisionIris,
            ItemId.LightVisionIvan,
            ItemId.LightVisionJoseph,
            ItemId.LightVisionKieshildt,
            ItemId.LightVisionKirsty,
            ItemId.LightVisionKlaus,
            ItemId.LightVisionLeo,
            ItemId.LightVisionLisa,
            ItemId.LightVisionLise,
            ItemId.LightVisionLoeg,
            ItemId.LightVisionMayleaf,
            ItemId.LightVisionMeirova,
            ItemId.LightVisionMephis,
            ItemId.LightVisionMysial,
            ItemId.LightVisionNedo,
            ItemId.LightVisionOliver,
            ItemId.LightVisionPlum,
            ItemId.LightVisionRingdeel,
            ItemId.LightVisionRudolfo,
            ItemId.LightVisionSeneka,
            ItemId.LightVisionSonel,
            ItemId.LightVisionSonia,
            ItemId.LightVisionTheodor,
            ItemId.LightVisionVanessa
        };
    }
}
