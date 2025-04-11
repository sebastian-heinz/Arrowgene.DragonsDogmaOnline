#load "libs.csx"

private static class Settings
{
    public static double DefaultGatherDropsRandomBias
    {
        get
        {
            return LibDdon.GetSetting<double>("GameServerSettings", "DefaultGatherDropsRandomBias");
        }
    }

    public static int DefaultGatherDropMaxSlots
    {
        get
        {
            return LibDdon.GetSetting<int>("GameServerSettings", "DefaultGatherDropMaxSlots");
        }
    }

    public static int MaximumDropsPerDefaultGatherRoll
    {
        get
        {
            return LibDdon.GetSetting<int>("GameServerSettings", "MaximumDropsPerDefaultGatherRoll");
        }
    }
}

public class Mixin : IDefaultGatherMixin
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(Mixin));

    public override List<InstancedGatheringItem> GenerateGatheringDrops(GameClient client, StageLayoutId stageLayoutId, uint index)
    {
        if (StageManager.IsBitterBlackMazeStageId(stageLayoutId) || StageManager.IsEpitaphRoadStageId(stageLayoutId))
        {
            return new();
        }

        List<InstancedGatheringItem> results = new();
        if (LibDdon.Assets.DefaultGatheringDropsAsset.SpotDefaultDrops.ContainsKey((stageLayoutId, index)))
        {
            results = HandleSpotDrops(client, stageLayoutId, index);
        }
        else
        {
            results = HandleAreaDrops(client, stageLayoutId, index);
        }

        return results;
    }

    private List<InstancedGatheringItem> HandleSpotDrops(GameClient client, StageLayoutId stageLayoutId, uint index)
    {
        // TODO: Fill in spot info before using it
        return new();
        /*
        var drops = LibDdon.Assets.DefaultGatheringDropsAsset.SpotDefaultDrops[(stageLayoutId, index)];

        // Create a list which golds all the items we can roll
        var rolls = drops.Select(x => x.ItemId).ToList();

        // Determine how many items to generate
        var slots = Random.Shared.WeightedNext(1, drops.Count, Settings.DefaultGatherDropsRandomBias);

        var dropTable = drops.ToDictionary(x => x.ItemId, x => x);
        return RollDrops(slots, rolls, dropTable, Settings.DefaultGatherDropsRandomBias);
        */
    }

    private List<InstancedGatheringItem> HandleAreaDrops(GameClient client, StageLayoutId stageLayoutId, uint index)
    {
        var stage = Stage.StageInfoFromStageLayoutId(stageLayoutId);
        if (!LibDdon.Assets.GatheringSpotInfoAsset.GatheringInfoMap.ContainsKey(stage.StageNo))
        {
            return new();
        }

        var stageSpots = LibDdon.Assets.GatheringSpotInfoAsset.GatheringInfoMap[stage.StageNo];
        if (!stageSpots.ContainsKey((stageLayoutId.GroupId, index)))
        {
            return new();
        }

        if (!LibDdon.Assets.DefaultGatheringDropsAsset.AreaDefaultDrops.ContainsKey(client.Character.AreaId))
        {
            return new();
        }

        var spotInfo = stageSpots[(stageLayoutId.GroupId, index)];

        Logger.Debug($"{stageLayoutId}.{index} {spotInfo.GatheringType}");

        var dropTable = GetDropCategoriesForType(spotInfo.GatheringType)
            .Select(x => LibDdon.Assets.DefaultGatheringDropsAsset.AreaDefaultDrops[client.Character.AreaId][x])
            .Where(x => x.Count > 0)
            .SelectMany(x => x)
            .Where(x => x.StageId == stage.StageId)
            .OrderBy(x => x.ItemLevel)
            .ToDictionary(x => x.ItemId, x => x);
        if (dropTable.Count == 0)
        {
            return new();
        }

        var potentialSlots = Settings.DefaultGatherDropMaxSlots;

        var gatherPointRank = GatherTypeExtension.MIN_TYPE_RANK;
        if (spotInfo.GatheringType.IsChest())
        {
            gatherPointRank = spotInfo.GatheringType.GetChestRank();
            potentialSlots += (int)(gatherPointRank - GatherTypeExtension.MIN_TYPE_RANK);
        }

        // Create a list which holds all the items we can roll
        var rolls = dropTable.Keys.ToList();

        var bias = FindRollBiasForSpot(gatherPointRank);

        // Determine how many items to generate
        var slots = Random.Shared.WeightedNext(1, potentialSlots + 1, Settings.DefaultGatherDropsRandomBias);
        return RollDrops(slots, rolls, dropTable, bias);
    }

    private double FindRollBiasForSpot(int rank)
    {
        var preferenceScore = GatherTypeExtension.MAX_TYPE_RANK - rank + 1;

        double minBias = Settings.DefaultGatherDropsRandomBias; // Higher makes more skewed to common items
        double maxBias = 0.3; // lower makes it more skewed to rarer items

        double t = (rank - 1.0) / (GatherTypeExtension.MAX_TYPE_RANK - 1.0); // Normalized rank [0, 1]
        return minBias * Math.Pow(maxBias / minBias, t);
    }

    private List<InstancedGatheringItem> RollDrops(int slots, List<ItemId> rolls, Dictionary<ItemId, DefaultGatheringDrop> dropTable, double rollBias)
    {
        Logger.Debug($"GatheringBias={rollBias}");

        var results = new List<InstancedGatheringItem>();
        for (int i = 0; i < slots && rolls.Count > 0; i++)
        {
            var itemId = rolls[Random.Shared.WeightedNext(rolls.Count, rollBias)];

            var item = dropTable[itemId];

            var drop = new InstancedGatheringItem()
            {
                ItemId = item.ItemId,
                Quality = item.Quality
            };

            if (item.MaxAmount == 0)
            {
                drop.ItemNum = (uint)Random.Shared.WeightedNext((int)item.MinAmount, Settings.MaximumDropsPerDefaultGatherRoll + 1, Settings.DefaultGatherDropsRandomBias);
            }
            else
            {
                drop.ItemNum = (uint)Random.Shared.WeightedNext((int)item.MinAmount, (int)item.MaxAmount + 1, Settings.DefaultGatherDropsRandomBias);
            }

            if (drop.ItemNum == 0)
            {
                // Skip item since none got generated
                // TODO: Is this a configuration issue?
                continue;
            }

            rolls.Remove(itemId);
            results.Add(drop);
        }
        return results;
    }

    private static List<DropCategory> SimpleTreasureChestCategories = new()
    {
        DropCategory.Currency,
        DropCategory.Consumable,
        DropCategory.Ore,
        DropCategory.Sand,
        DropCategory.Thread,
        DropCategory.Plants,
        DropCategory.Meat,
        DropCategory.Scrolls,
    };

    private static Dictionary<GatheringType, List<DropCategory>> DropCategories = new Dictionary<GatheringType, List<DropCategory>>()
    {
        [GatheringType.OM_GATHER_NONE] = SimpleTreasureChestCategories, // Some spots are assigned as none? Looks like plain treasure chests
        [GatheringType.OM_GATHER_TREE_LV1] = new() { DropCategory.Lumber },
        [GatheringType.OM_GATHER_TREE_LV2] = new() { DropCategory.Lumber },
        [GatheringType.OM_GATHER_TREE_LV3] = new() { DropCategory.Lumber },
        [GatheringType.OM_GATHER_TREE_LV4] = new() { DropCategory.Lumber },
        [GatheringType.OM_GATHER_JWL_LV1] = new() { DropCategory.Gemstones },
        [GatheringType.OM_GATHER_JWL_LV2] = new() { DropCategory.Gemstones },
        [GatheringType.OM_GATHER_JWL_LV3] = new() { DropCategory.Gemstones },
        [GatheringType.OM_GATHER_CRST_LV1] = new() { DropCategory.Ore },
        [GatheringType.OM_GATHER_CRST_LV2] = new() { DropCategory.Ore },
        [GatheringType.OM_GATHER_CRST_LV3] = new() { DropCategory.Ore },
        [GatheringType.OM_GATHER_CRST_LV4] = new() { DropCategory.Ore },
        [GatheringType.OM_GATHER_KEY_LV1] = Enum.GetValues(typeof(DropCategory)).Cast<DropCategory>().ToList(),
        [GatheringType.OM_GATHER_KEY_LV2] = Enum.GetValues(typeof(DropCategory)).Cast<DropCategory>().ToList(),
        [GatheringType.OM_GATHER_KEY_LV3] = Enum.GetValues(typeof(DropCategory)).Cast<DropCategory>().ToList(),
        [GatheringType.OM_GATHER_TREA_OLD] = SimpleTreasureChestCategories,
        [GatheringType.OM_GATHER_KEY_LV4] = Enum.GetValues(typeof(DropCategory)).Cast<DropCategory>().ToList(),
        [GatheringType.OM_GATHER_TREA_IRON] = Enum.GetValues(typeof(DropCategory)).Cast<DropCategory>().ToList(),
        [GatheringType.OM_GATHER_TREA_TREE] = SimpleTreasureChestCategories,
        [GatheringType.OM_GATHER_TREA_SILVER] = Enum.GetValues(typeof(DropCategory)).Cast<DropCategory>().ToList(),
        [GatheringType.OM_GATHER_TREA_GOLD] = Enum.GetValues(typeof(DropCategory)).Cast<DropCategory>().ToList(),
        [GatheringType.OM_GATHER_BOX] = SimpleTreasureChestCategories,
        [GatheringType.OM_GATHER_SHIP] = SimpleTreasureChestCategories,
        [GatheringType.OM_GATHER_DRAGON] = new() { DropCategory.Meat, DropCategory.Claws, DropCategory.Bones, DropCategory.Hides, DropCategory.Horns },
        [GatheringType.OM_GATHER_CORPSE] = new() { DropCategory.Meat, DropCategory.Claws, DropCategory.Bones, DropCategory.Hides, DropCategory.Horns, DropCategory.Furs, DropCategory.Feathers },
        [GatheringType.OM_GATHER_GRASS] = new() { DropCategory.Plants },
        [GatheringType.OM_GATHER_FLOWER] = new() { DropCategory.Plants },
        [GatheringType.OM_GATHER_MUSHROOM] = new() { DropCategory.Mushrooms },
        [GatheringType.OM_GATHER_CLOTH] = new() { DropCategory.Fabric, DropCategory.Thread },
        [GatheringType.OM_GATHER_BOOK] = new() { DropCategory.Scrolls },
        [GatheringType.OM_GATHER_SAND] = new() { DropCategory.Sand },
        [GatheringType.OM_GATHER_ALCHEMY] = new() { DropCategory.Liquids, DropCategory.Lumber, DropCategory.Other },
        [GatheringType.OM_GATHER_WATER] = new() { DropCategory.Liquids },
        [GatheringType.OM_GATHER_SHELL] = new() { DropCategory.Sand, DropCategory.Bones, DropCategory.Gemstones, DropCategory.Currency, DropCategory.Other },
        [GatheringType.OM_GATHER_ANTIQUE] = new() { DropCategory.Plants },
        [GatheringType.OM_GATHER_ONE_OFF] = new(),  // Red Light's in S2 areas
        [GatheringType.OM_GATHER_TWINKLE] = new() { DropCategory.Sand }
    };

    private List<DropCategory> GetDropCategoriesForType(GatheringType gatheringType)
    {
        if (!DropCategories.ContainsKey(gatheringType))
        {
            return new();
        }
        return DropCategories[gatheringType];
    }
}

return new Mixin();
