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

        ResolutionsAndOmens = 1,
        TheSlumberingGod = 2,
        EnvoyOfReconcilliation = 3,
        SolidersOfTheRift = 4,
        TheDullGreyArk = 5,
        TheGirlInTheForest = 6,
        TheHouseOfSteam = 7,
        TheAssailedFort = 9,
        TheCastleOfDusk = 10,
        TheGodsAwakening = 11,
        TheGirlCladInDarkness = 12,
        TheStolenHeart = 13,
        TheRoarsOfAThousand = 14,
        ReturnToYore = 15,
        AFriendlyVisit = 16,
        TheCourseOfLife = 17,
        TheArkOnceMore = 18,
        ThinkingOfAFriend = 19,
        TheFutureEntrustedToUs = 20,
        TheQuandaryOfSoliders = 21,
        TheDwellersOfTheGoldenLand = 22,
        TheGoldenKey = 23,
        TheGreatAlchemist = 24,
        TheCrimsonCrystal = 25,
        AServantsPledge = 26,
        TheGoblinKing = 27,
        ABriefRespite = 28,
        TheBeastsFinalMoments = 29,
        BeForevermoreWhiteDragon = 30,

        // Seems like quest names and IDs got out of sync when this list was made
        TheStormThatBroughtATragedy = 20010,
        TheStormThatBoughtATragedy = 20020,
        TheGirlWhoLostHerMemories = 20030,
        TheCorruptionAndTheKnights = 20040,
        ExploringTheDenOfMonsters = 20050,
        EliminateTheCorrosionInfestation = 20060,
        TheManFromAnotherLand = 20070,
        TheFateOfLestania = 20080,
        AFreshIncident = 20090,
        Negotiations = 20100,
        LeogsIllness = 20110,
        AStrangeLandsLight = 20120,
        TheLoneArisen = 20130,
        StrayingPower = 20140,
        TheEntrustedOne = 20150,
        ANewContinent = 20160,
        TheLostHometown = 20170,
        TheVillageOfSoliders = 20180,
        Homecoming = 20190,
        GallantFootsteps = 20200,
        TheDarknessOfTheHeart = 20210,
        TheSpiritLand = 20220,
        WithinTheTree = 20230,
        RestorationRequirements = 20240,
        ReasonAndBonds = 20250,

        TheNewGeneration = 30010,
        TheLandOfDespair = 30020,
        InSearchOfHope = 30030,
        MeirovaTheVeteranGeneral = 30040,
        ThePrincesWhereabouts = 30050,
        SurvivorsVillage = 30060,
        TheOpposition = 30070,
        PrinceNedo = 30080,
        TheRoyalFamilySacrament = 30090,
        TheApproachingDemonArmy = 30100,
        PortentOfDespair = 30110,
        TheSecretEntrance = 30120,
        ADesperateInfiltration = 30125,
        TheBattleOfLookoutCastle = 30130,
        TheRoadToTheRoyalCapital = 30140,
        RallyTheTroops = 30150,
        AttackOnTheRoyalCapital = 30160,
        AnOmenOfDestruction = 30170,
        ABriefDragonForce = 30180,
        ThePlightOfLookoutCastle = 30190,
        TheFinalBattleOfTheRoyalCapital = 30200,
        TheMissingPrince = 30210,
        NedosTrail = 30220,
        TheRoyalFamilyMausoleum = 30230,
        TheDreadfulPassage = 30240,
        TheRelicsOfTheFirstKing = 30250,
        HopesBitterEnd = 30260,
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
    }
}
