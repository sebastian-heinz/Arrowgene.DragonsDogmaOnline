using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class MyRoomCsvReader : CsvReaderWriter<MyRoomCsv>
    {
        protected override int NumExpectedItems => 5;

        protected override MyRoomCsv CreateInstance(string[] properties)
        {
            string mandragoraName1 = properties[0];
            if (!uint.TryParse(properties[1], out uint mandragoraId1)) return null;
            string mandragoraName2 = properties[2];
            if (!uint.TryParse(properties[3], out uint mandragoraId2)) return null;
            string mandragoraName3 = properties[4];
            if (!uint.TryParse(properties[5], out uint mandragoraId3)) return null;
            if (!bool.TryParse(properties[6], out bool hideMandragora1)) return null;
            if (!bool.TryParse(properties[7], out bool hideMandragora2)) return null;
            if (!bool.TryParse(properties[8], out bool hideMandragora3)) return null;
            if (!uint.TryParse(properties[9], out uint tableType)) return null;
            if (!uint.TryParse(properties[10], out uint sideTable)) return null;
            if (!uint.TryParse(properties[11], out uint cabinetType)) return null;
            if (!uint.TryParse(properties[12], out uint carpetTypeReadingRoom)) return null;
            if (!uint.TryParse(properties[13], out uint singleChair)) return null;
            if (!uint.TryParse(properties[14], out uint wallBookshelf)) return null;
            if (!uint.TryParse(properties[15], out uint kitchenTableType)) return null;
            if (!uint.TryParse(properties[16], out uint fullKettleSet)) return null;
            if (!uint.TryParse(properties[17], out uint cuisine)) return null;
            if (!uint.TryParse(properties[18], out uint plushDollA)) return null;
            if (!uint.TryParse(properties[19], out uint plushDollB)) return null;
            if (!uint.TryParse(properties[20], out uint leftKitchenShelfFoodstuffs)) return null;
            if (!uint.TryParse(properties[21], out uint cookwareSet)) return null;
            if (!uint.TryParse(properties[22], out uint bedType)) return null;
            if (!uint.TryParse(properties[23], out uint puppetType)) return null;
            if (!uint.TryParse(properties[24], out uint lampType)) return null;
            if (!uint.TryParse(properties[25], out uint livingTableType)) return null;
            if (!uint.TryParse(properties[26], out uint cuisineType)) return null;
            if (!uint.TryParse(properties[27], out uint chest)) return null;
            if (!uint.TryParse(properties[28], out uint chestFigurine)) return null;
            if (!uint.TryParse(properties[29], out uint bath)) return null;
            if (!uint.TryParse(properties[30], out uint arisensRoomDecor)) return null;
            if (!uint.TryParse(properties[31], out uint object0)) return null;
            if (!uint.TryParse(properties[32], out uint objectB)) return null;
            if (!uint.TryParse(properties[33], out uint arisensLoungewear)) return null;
            if (!uint.TryParse(properties[34], out uint servantsLoungewear)) return null;
            if (!uint.TryParse(properties[35], out uint servantsBathingClothes)) return null;
            if (!uint.TryParse(properties[36], out uint servantsSleepwear)) return null;
            if (!uint.TryParse(properties[37], out uint objectA)) return null;
            if (!uint.TryParse(properties[38], out uint arisensRoomLighting)) return null;
            if (!uint.TryParse(properties[39], out uint tableFlowerA)) return null;
            if (!uint.TryParse(properties[40], out uint tableFlowerB)) return null;
            if (!uint.TryParse(properties[41], out uint weatherForecast)) return null;
            if (!uint.TryParse(properties[42], out uint wallpaper)) return null;
            if (!uint.TryParse(properties[43], out uint spout)) return null;
            if (!uint.TryParse(properties[44], out uint cuckooClock)) return null;
            if (!uint.TryParse(properties[45], out uint leftWallFurniture)) return null;
            if (!uint.TryParse(properties[46], out uint rightWallFurniture)) return null;
            if (!uint.TryParse(properties[47], out uint carpetTypeLivingRoom)) return null;
            if (!uint.TryParse(properties[48], out uint furniture)) return null;
            if (!uint.TryParse(properties[49], out uint summoningBell)) return null;
            if (!uint.TryParse(properties[50], out uint music)) return null;

            return new MyRoomCsv
            {
                MandragoraName1 = mandragoraName1,
                MandragoraId1 = mandragoraId1,
                MandragoraName2 = mandragoraName2,
                MandragoraId2 = mandragoraId2,
                MandragoraName3 = mandragoraName3,
                MandragoraId3 = mandragoraId3,
                HideMandragora1 = hideMandragora1,
                HideMandragora2 = hideMandragora2,
                HideMandragora3 = hideMandragora3,
                TableType = tableType,
                SideTable = sideTable,
                CabinetType = cabinetType,
                CarpetTypeReadingRoom = carpetTypeReadingRoom,
                SingleChair = singleChair,
                WallBookshelf = wallBookshelf,
                KitchenTableType = kitchenTableType,
                FullKettleSet = fullKettleSet,
                Cuisine = cuisine,
                PlushDollA = plushDollA,
                PlushDollB = plushDollB,
                LeftKitchenShelfFoodstuffs = leftKitchenShelfFoodstuffs,
                CookwareSet = cookwareSet,
                BedType = bedType,
                PuppetType = puppetType,
                LampType = lampType,
                LivingTableType = livingTableType,
                CuisineType = cuisineType,
                Chest = chest,
                ChestFigurine = chestFigurine,
                Bath = bath,
                ArisensRoomDecor = arisensRoomDecor,
                Object0 = object0,
                ObjectB = objectB,
                ArisensLoungewear = arisensLoungewear,
                ServantsLoungewear = servantsLoungewear,
                ServantsBathingClothes = servantsBathingClothes,
                ServantsSleepwear = servantsSleepwear,
                ObjectA = objectA,
                ArisensRoomLighting = arisensRoomLighting,
                TableFlowerA = tableFlowerA,
                TableFlowerB = tableFlowerB,
                WeatherForecast = weatherForecast,
                Wallpaper = wallpaper,
                Spout = spout,
                CuckooClock = cuckooClock,
                LeftWallFurniture = leftWallFurniture,
                RightWallFurniture = rightWallFurniture,
                CarpetTypeLivingRoom = carpetTypeLivingRoom,
                Furniture = furniture,
                SummoningBell = summoningBell,
                Music = music,
            };
        }
    }
}
