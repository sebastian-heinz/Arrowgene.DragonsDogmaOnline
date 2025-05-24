using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class StageManager
    {
        private readonly static List<CDataStageInfo> StageList = EntitySerializer.Get<S2CStageGetStageListRes>().Read(GameDump.data_Dump_19).StageList;

        public static uint ConvertIdToStageNo(uint stageId)
        {
            foreach (CDataStageInfo stageInfo in StageList)
            {
                if (stageInfo.Id == stageId)
                    return stageInfo.StageNo;
            }

            return 0;
        }

        public static uint ConvertIdToStageNo(StageLayoutId stageId)
        {
            return ConvertIdToStageNo(stageId.Id);
        }

        // List of "safe" areas, where the context reset NTC will be sent.
        // TODO: Complete with all the safe areas. Maybe move it to DB or config?
        private static readonly HashSet<uint> SafeStageIds = new HashSet<StageInfo>()
            {
                //WDT
                Stage.TheWhiteDragonTemple0,
                Stage.CraftRoom,
                Stage.CaveHarbor,
                Stage.ClanHall,
                Stage.ArisensRoom,

                //Lestania
                Stage.WhiteDeerInn,
                Stage.BlackGrapeInn,
                Stage.SeaDragonInn,
                Stage.SingingWindsInn,
                Stage.RedCrystalInn,
                Stage.SleepingWolfInn,
                Stage.GoldenTankardInn,
                Stage.GrittenFort0,
                Stage.PawnCathedral,
                Stage.HobolicCave,
                Stage.MysreeGroveShrine,
                Stage.ZandoraWastelandsShrine,
                Stage.MergodaResidentialArea,
                Stage.SecretBowmakersHome,
                Stage.KinozaMineralSprings,

                //BBI
                Stage.ExpeditionGarrison,

                //Phindym
                Stage.ProtectorsRetreat,
                Stage.MorfaulCentrum,
                Stage.DanaCentrum,
                Stage.GlyndwrCentrum,
                Stage.HollowofBeginningsGatheringArea,
                Stage.TowerofIvanos,
                Stage.SpiritArtsHut,
                Stage.ManunVillage,

                //Acre Selund
                Stage.FortressCityMegadoResidentialLevel0,
                Stage.FortressCityMegadoResidentialLevel1,
                Stage.FortressCityMegadoResidentialLevel2,
                Stage.FortressCityMegadoResidentialLevel3,
                Stage.FortThines1, // TODO: The other ones?
                Stage.FortThinesGreatDiningHall,
                Stage.LookoutCastle0, // TODO: The other ones?
                Stage.BerthasBanditGroupHideout,
                Stage.PiremothTravelersInn,
                Stage.RothgillTravelersInn,
                Stage.MephiteTravelersInn,
                Stage.HeroicSpiritSleepingPathRathniteFoothills,
                Stage.HeroicSpiritSleepingPathFeryanaWilderness,
                Stage.OldHeroicSpiritShrine,
                Stage.EliGuardTower,
                Stage.NorthernBanditHideout,

                //Other
                Stage.BonusDungeonLobby,
                Stage.BitterblackMazeCove,
                Stage.BreyaCoast, // Summer Event Hub Area
            }.Select(x => x.StageId).ToHashSet();
        public static bool IsSafeArea(uint stageId)
        {
            return SafeStageIds.Contains(stageId);
        }

        public static bool IsSafeArea(StageLayoutId stageId)
        {
            return IsSafeArea(stageId.Id);
        }

        // List of lobby areas, where you're supposed to see all other players.
        // TODO: Complete with all the safe areas. Maybe move it to DB or config?
        public static readonly HashSet<uint> HubStageIds = new HashSet<StageInfo>(){
            Stage.TheWhiteDragonTemple0,
            Stage.BreyaCoast, // (Summer Event Hub Area)
            Stage.DanaCentrum, 
            Stage.ClanHall, //  (has special handling)
            Stage.FortressCityMegadoResidentialLevel0, 
            Stage.FortressCityMegadoResidentialLevel1, 
            Stage.FortressCityMegadoResidentialLevel2, 
            Stage.FortressCityMegadoResidentialLevel3,
            Stage.FortressCityMegadoRoyalPalaceLevel,
            Stage.BitterblackMazeCove
        }.Select(x => x.StageId).ToHashSet();

        public static bool IsHubArea(uint stageId)
        {
            return HubStageIds.Contains(stageId);
        }

        public static bool IsHubArea(StageLayoutId stageId)
        {
            return IsHubArea(stageId.Id);
        }

        public static readonly uint BitterblackCove = Stage.BitterblackMazeCove.StageId;

        private static readonly HashSet<uint> BitterBlackStageIds = new HashSet<StageInfo>()
        {
            Stage.BitterblackMazeCove,
            Stage.BitterblackMazeNetherworld1AbyssA,
            Stage.BitterblackMazeNetherworld1AbyssB,
            Stage.BitterblackMazeNetherworld1AbyssC,
            Stage.BitterblackMazeNetherworld1RoutundaA,
            Stage.BitterblackMazeNetherworld1RoutundaB,
            Stage.BitterblackMazeNetherworld1RoutundaC,
            Stage.BitterblackMazeNetherworld2AbyssA,
            Stage.BitterblackMazeNetherworld2AbyssB,
            Stage.BitterblackMazeNetherworld2AbyssC,
            Stage.BitterblackMazeNetherworld2RoutundaA,
            Stage.BitterblackMazeNetherworld2RoutundaB,
            Stage.BitterblackMazeNetherworld2RoutundaC,
            Stage.BitterblackMazeNetherworld3AbyssA,
            Stage.BitterblackMazeNetherworld3AbyssB,
            Stage.BitterblackMazeNetherworld3AbyssC,
            Stage.BitterblackMazeNetherworld3RoutundaA,
            Stage.BitterblackMazeNetherworld3RoutundaB,
            Stage.BitterblackMazeNetherworld3RoutundaC,
            Stage.BitterblackMazeNetherworld3RoutundaADeath,
            Stage.BitterblackMazeNetherworld3RoutundaBDeath,
            Stage.BitterblackMazeNetherworld3RoutundaCDeath,
            Stage.BitterblackMazeNetherworld4AbyssA,
            Stage.BitterblackMazeNetherworld4AbyssB,
            Stage.BitterblackMazeNetherworld4AbyssC,
            Stage.BitterblackMazeNetherworld4AbyssADeath,
            Stage.BitterblackMazeNetherworld4AbyssBDeath,
            Stage.BitterblackMazeNetherworld4AbyssCDeath,
            Stage.BitterblackMazeRift0,
            Stage.BitterblackMazeRift1,
            Stage.BitterblackMazeRift2,
            Stage.BitterblackMazeRift3,
            Stage.BitterblackMazeRift4,
            Stage.BitterblackMazeGardenofIgnominy,
            Stage.BitterblackMazeDuskmoonTower,
            Stage.BitterblackMazeRotundaofDread,
            Stage.BitterblackMazeNoxiousCathedral,
            Stage.BitterblackMazeTraitorsTower,
            Stage.BitterblackMazeFallenCity,
            Stage.BitterblackMazeAltaroftheBlackCurse
        }.Select(x => x.StageId).ToHashSet();

        public static bool IsBitterBlackMazeStageId(uint stageId)
        {
            return BitterBlackStageIds.Contains(stageId);
        }

        public static bool IsBitterBlackMazeStageId(StageLayoutId stageId)
        {
            return IsBitterBlackMazeStageId(stageId.Id);
        }

        private static readonly HashSet<uint> BitterBlackNormalBossStageIds = new HashSet<StageInfo>()
        {
            Stage.BitterblackMazeGardenofIgnominy, 
            Stage.BitterblackMazeDuskmoonTower, 
            Stage.BitterblackMazeRotundaofDread, 
        }.Select(x => x.StageId).ToHashSet();

        public static bool IsBitterBlackMazeBossStageId(uint stageId)
        {
            return BitterBlackNormalBossStageIds.Contains(stageId);
        }

        public static bool IsBitterBlackMazeBossStageId(StageLayoutId stageId)
        {
            return IsBitterBlackMazeBossStageId(stageId.Id);
        }

        private static readonly HashSet<uint> BitterBlackAbyssBossStageIds = new HashSet<StageInfo>()
        {
            Stage.BitterblackMazeNoxiousCathedral,
            Stage.BitterblackMazeTraitorsTower, 
            Stage.BitterblackMazeFallenCity, 
            Stage.BitterblackMazeAltaroftheBlackCurse, 
        }.Select(x => x.StageId).ToHashSet();

        public static bool IsBitterBlackMazeAbyssBossStageId(uint stageId)
        {
            return BitterBlackAbyssBossStageIds.Contains(stageId);
        }

        public static bool IsBitterBlackMazeAbyssBossStageId(StageLayoutId stageId)
        {
            return IsBitterBlackMazeAbyssBossStageId(stageId.Id);
        }

        // Hubs
        // 549 Sleeping Path : Rathnite Foothills (Pehr)
        // 557 Sleeping Path : Feryana (Ira)
        // 558 Old Heroic Spirit Shrine (Morgan -> Memory of Megadosys, Selim -> Memory of Urteca)

        private static readonly HashSet<uint> EpitaphRoadStageIds = new HashSet<StageInfo>()
        {
            // 3.0
            Stage.HeroicSpiritSleepingPathShrine, 
            Stage.HeroicSpiritSleepingPathCave,
            // Cave Depth??
            Stage.HeroicSpiritSleepingPathWaterway, 
            // 3.1
            Stage.HeroicSpiritSleepingPathRuins, 
            Stage.HeroicSpiritSleepingPathWell, 
            Stage.HeroicSpiritSleepingPathTomb, 
            Stage.HeroicSpiritSleepingPathRuinsDeepestLevel, 
            // 3.2
            Stage.MemoryofMegadosys, 
            Stage.MemoryofMegadosysOldRoad,
            Stage.MemoryofMegadosysWarGodSpace, 
            // 3.3
            Stage.MemoryofUrteca, 
            Stage.MemoriesoftheEarthSacredFlamePath, 
            Stage.MemoryofUrtecaWarGodSpace, 
            Stage.MemoryofRoyalFamilyMausoleum, 
            Stage.MemoryofFirefallMountainCampsite,
        }.Select(x => x.StageId).ToHashSet();

        public static bool IsEpitaphRoadStageId(uint stageId)
        {
            return EpitaphRoadStageIds.Contains(stageId);
        }

        public static bool IsEpitaphRoadStageId(StageLayoutId stageId)
        {
            return IsEpitaphRoadStageId(stageId.Id);
        }

        private static readonly HashSet<uint> LegacyEpitaphRoadStageIds = new HashSet<StageInfo>
        {
            // 3.0
            Stage.HeroicSpiritSleepingPathShrine,
            Stage.HeroicSpiritSleepingPathCave, 
            // Cave Depth??
            Stage.HeroicSpiritSleepingPathWaterway, 
            // 3.1
            Stage.HeroicSpiritSleepingPathRuins, 
            Stage.HeroicSpiritSleepingPathWell, 
            Stage.HeroicSpiritSleepingPathTomb, 
            Stage.HeroicSpiritSleepingPathRuinsDeepestLevel,
        }.Select(x => x.StageId).ToHashSet();

        public static bool IsLegacyEpitaphRoadStageId(uint stageId)
        {
            return LegacyEpitaphRoadStageIds.Contains(stageId);
        }

        public static bool IsLegacyEpitaphRoadStageId(StageLayoutId stageId)
        {
            return IsLegacyEpitaphRoadStageId(stageId.Id);
        }

        private static readonly HashSet<uint> EpitaphHubArea = new HashSet<StageInfo>
        {
            Stage.HeroicSpiritSleepingPathRathniteFoothills,
            Stage.HeroicSpiritSleepingPathFeryanaWilderness, 
            Stage.MemoryofUrteca, 
        }.Select(x => x.StageId).ToHashSet();

        public static bool IsEpitaphHubArea(uint stageId)
        {
            return EpitaphHubArea.Contains(stageId);
        }

        public static bool IsEpitaphHubArea(StageLayoutId stageId)
        {
            return IsEpitaphHubArea(stageId.Id);
        }

        private static readonly HashSet<uint> LegacyEpitaphHubArea = new HashSet<StageInfo>
        {
            Stage.HeroicSpiritSleepingPathRathniteFoothills,
            Stage.HeroicSpiritSleepingPathFeryanaWilderness,
        }.Select(x => x.StageId).ToHashSet();

        public static bool IsLegacyEpitaphHubArea(uint stageId)
        {
            return LegacyEpitaphHubArea.Contains(stageId);
        }

        public static bool IsLegacyEpitaphHubArea(StageLayoutId stageId)
        {
            return IsLegacyEpitaphHubArea(stageId.Id);
        }

        private static readonly HashSet<uint> OverworldAreas = new HashSet<StageInfo>
        {
            Stage.Lestania,
            Stage.BloodbaneIsle0,
            Stage.BloodbaneIsle1,
            Stage.FaranaPlains0,
            Stage.FaranaPlains1,
            Stage.ElanWaterGrove,
            Stage.KingalCanyon,
            Stage.MorrowForest,
            Stage.RathniteFoothills,
            Stage.FeryanaWilderness,
            Stage.MegadosysPlateau,
            Stage.UrtecaMountains
        }.Select(x => x.StageId).ToHashSet();

        public static bool IsDungeon(uint stageId)
        {
            if (SafeStageIds.Contains(stageId) || OverworldAreas.Contains(stageId))
            {
                return false;
            }
            return true;
        }

        public static bool IsDungeon(StageLayoutId stageId)
        {
            return IsDungeon(stageId.Id);
        }
    }
}
