using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public enum QuestId : uint
    {
        None = 0,

        // Main Quests
        // IDs correspond to Dragon's Dogma Online\nativePC\rom\ui\quest\mqxxxxx_ID.arc
        // When passing a mission to the client, the QuestId is used to look up files in
        // the respective directories for different quests. Seems it may be possible to use
        // this to create additional main story quests.

        // Season 1.0
        ResolutionsAndOmens = 1,
        TheSlumberingGod = 2,
        EnvoyOfReconcilliation = 3,
        SolidersOfTheRift = 4,
        AServantsPledge = 26,
        TheCrimsonCrystal = 25,
        TheDullGreyArk = 5,
        TheGirlInTheForest = 6,
        TheGoblinKing = 27,
        TheHouseOfSteam = 7,
        TheAssailedFort = 9,
        TheCastleOfDusk = 10,
        TheGodsAwakening = 11,
        // Season 1.1
        TheGirlCladInDarkness = 12,
        TheStolenHeart = 13,
        TheRoarsOfAThousand = 14,
        ReturnToYore = 15,
        AFriendlyVisit = 16,
        TheCourseOfLife = 17,
        // Season 1.2
        ABriefRespite = 28,
        TheArkOnceMore = 18,
        ThinkingOfAFriend = 19,
        TheBeastsFinalMoments = 29,
        TheFutureEntrustedToUs = 20,
        TheQuandaryOfSoliders = 21,
        TheDwellersOfTheGoldenLand = 22,
        TheGoldenKey = 23,
        TheGreatAlchemist = 24,
        BeForevermoreWhiteDragon = 30,
        // Season 2.0
        TheStormThatBroughtATragedy = 20010,
        TheGirlWhoLostHerMemories = 20020,
        TheCorruptionAndTheKnights = 20030,
        ExploringTheDenOfMonsters = 20040,
        EliminateTheCorrosionInfestation = 20050,
        TheManFromAnotherLand = 20060,
        TheFateOfLestania = 20070,
        // Season 2.1
        AFreshIncident = 20080,
        Negotiations = 20090,
        LeogsIllness = 20100,
        AStrangeLandsLight = 20110,
        TheLoneArisen = 20120,
        StrayingPower = 20130,
        TheEntrustedOne = 20140,
        // Season 2.2
        ANewContinent = 20150,
        TheLostHometown = 20160,
        TheVillageOfSoliders = 20170,
        Homecoming = 20180,
        GallantFootsteps = 20190,
        TheDarknessOfTheHeart = 20200,
        // Season 2.3
        TheSpiritLand = 20210,
        WithinTheTree = 20220,
        RestorationRequirements = 20230,
        ReasonAndBonds = 20240,
        TheNewGeneration = 20250,
        // Season 3.0
        TheLandOfDespair = 30010,
        InSearchOfHope = 30020,
        MeirovaTheVeteranGeneral = 30030,
        ThePrincesWhereabouts = 30040,
        SurvivorsVillage = 30050,
        TheOpposition = 30060,
        PrinceNedo = 30070,
        // Season 3.1
        TheRoyalFamilySacrament = 30080,
        TheApproachingDemonArmy = 30090,
        PortentOfDespair = 30100,
        DiversionaryTactics = 30110,
        TheSecretEntrance = 30120,
        ADesperateInfiltration = 30125,
        TheBattleOfLookoutCastle = 30130,
        // Season 3.2
        TheRoadToTheRoyalCapital = 30140,
        RallyTheTroops = 30150,
        AttackOnTheRoyalCapital = 30160,
        AnOmenOfDestruction = 30170,
        ABriefDragonForce = 30180,
        ThePlightOfLookoutCastle = 30190,
        TheFinalBattleOfTheRoyalCapital = 30200,
        // Season 3.3
        TheMissingPrince = 30210,
        NedosTrail = 30220,
        TheRoyalFamilyMausoleum = 30230,
        TheDreadfulPassage = 30240,
        TheRelicsOfTheFirstKing = 30250,
        HopesBitterEnd = 30260,
        // Season 3.4?
        ThoseWhoFollowTheDragon = 30270,
        Unknown30410 = 30410, // Name is in Japanese
        Unknown30420 = 30420, // Name is in Japanese
        Unknown30430 = 30430, // Name is in Japanese
        Unknown30440 = 30440, // Name is in Japanese

        Unknown5004 = 5004,   // Entry is blank, just picture of a church?

        // World Quests
        ConfrontationWithScouts = 20005001,
        AmbushInTheWellsDepths = 20005002,
        TheKnightsBitterEnemy = 20005010,

        // Personal Quests
        CraftedTokenOfTheHeart = 60000016,

        // Custom World Manage Quest Ids
        WorldManageMonsterCaution = 79000001,
    }
}
