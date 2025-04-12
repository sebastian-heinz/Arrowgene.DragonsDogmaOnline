namespace Arrowgene.Ddon.Shared.Model
{
    public enum AchievementType : uint
    {
        Unknown,

        Appraisal,
        ChangeColor,
        ClearBBM, // Param = AchievementBBMParam
        ClearQuest, // Param = QuestId
        ClearQuestType, // Param = AchievementQuestTypeParam
        ClearSubstory, // Param = SubstoryId ???
        CollectType, // Param = AchievementCollectParam
        CraftType, // Param = AchievementCraftTypeParam
        CraftTypeUnique, // Param = AchievementCraftTypeParam
        EmblemStone,
        EnhanceItem,
        EpitaphRoad,
        HirePawn,
        KillEnemyType, // Param = AchievementEnemyParam
        KillTotalEnemy,
        LearnAugments,
        LearnSkills,
        MainLevel, // Param = JobId
        MainLevelGroup, // Param = AchievementLevelGroupParam
        MandragoraSpecies,
        MountCrest,
        OrbDevote, // Param = OrbGainParamType
        PawnAffection,
        PawnCrafting,
        PawnCraftingExam,
        PawnExpedition,
        PawnLevel, // Param = JobId
        PawnTraining,
        SparkleCollect, // Param = QuestAreaId, with None/0 for "Total"
        TakePhoto,
    }

    public enum AchievementEnemyParam : uint
    {
        AlchemizedGoblin,
        AlchemizedGriffin,
        AlchemizedHarpy,
        AlchemizedSkeleton,
        AlchemizedWolf,
        AlchemyEye,
        AlteredZuhl,
        Angules,
        Ape,
        Banded,
        BeastMaster,
        Behemoth,
        Bifrest,
        BlackGriffin,
        BlackKnight,
        BlueNewt,
        BoltGrimwarg,
        Catoblepas,
        Chicken,
        Chimera,
        Cockatrice,
        Colossus,
        Corpse,
        CursedDragon,
        Cyclops,
        Death,
        DeathKnight,
        Deer,
        Drake,
        ElderDragon,
        Eliminator,
        EmpressGhost,
        Ent,
        EvilDragon,
        EvilEye,
        Footbiter,
        ForestGoblin,
        Frog,
        FrostMachina,
        Gargoyle,
        GeoGolem,
        Ghost,
        GhostMail,
        Ghoul,
        GiantWarrior,
        GigantMachina,
        Goat,
        Goblin,
        Golem,
        Gorechimera,
        Gorecyclops,
        GrandEnt,
        GreenGuardian,
        Griffin,
        Grigori,
        Grimwarg,
        GroundInsect,
        Harpy,
        Hobgoblin,
        Ifrit,
        InfectedDirewolf,
        InfectedGriffin,
        InfectedHobgoblin,
        InfectedOrc,
        InfectedSnowHarpy,
        JewelEye,
        KillerBee,
        Lindwurm,
        LivingArmor,
        LostPawn,
        Mandragora,
        Maneater,
        Manticore,
        Medusa,
        Mergan,
        MistDrake,
        MistMan,
        MistWyrm,
        Moth,
        Mudmen,
        NecroMaster,
        Nightmare,
        Ogre,
        Orc,
        OrcLeader,
        Ox,
        PhantasmicGreatDragon,
        Phindymian,
        PhyndymianEnt,
        Pig,
        Pixie,
        Rabbit,
        Rat,
        Redcap,
        RockSaurian,
        Rogue,
        Saurian,
        Scarlet,
        Scourge,
        SeverelyInfectedDemon,
        SeverelyInfectedGriffin,
        ShadowChimera,
        ShadowGoblin,
        ShadowHarpy,
        ShadowMaster,
        ShadowWolf,
        SilverRoar,
        Siren,
        Skeleton,
        SkeletonBrute,
        SkullLord,
        Slime,
        Sphinx,
        Spider,
        Spine,
        SpiritDragonWilmia,
        Strix,
        Stymphalides,
        SulfurSaurian,
        Tarasque,
        Troll,
        Undead,
        Ushumgal,
        Warg,
        WarMaster,
        WhiteChimera,
        WhiteGriffin,
        Wight,
        WildBoar,
        Witch,
        Wolf,
        Wyrm,
        Zuhl
    }

    public enum AchievementQuestTypeParam : uint
    {
        None,
        Bounty,
        Delivery,
        Hidden,
        World,
        Extreme,
        War,
        WildHunt
    }

    public enum AchievementCraftTypeParam : uint
    {
        Other,
        Weapon,
        Armor,
        Furniture
    }

    public enum AchievementLevelGroupParam : uint
    {
        GroupFirst9,
        GroupFirst10,
        GroupAll,
        GroupBlue,
        GroupGreen,
        GroupArcher,
        GroupMelee,
        GroupMage
    }

    public enum AchievementCollectParam : uint
    {
        Chest,
        Herb,
        Ore,
        Wood
    }

    public enum AchievementBBMParam : uint
    {
        Normal,
        Abyss
    }
}
