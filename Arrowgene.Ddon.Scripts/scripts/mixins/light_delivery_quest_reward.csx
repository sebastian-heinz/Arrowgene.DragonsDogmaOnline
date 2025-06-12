#load "libs.csx"

public class Mixin : ILightQuestRewardMixin
{
    private static readonly double BULK_FACTOR = 1.2;

    public override uint CalculateRewardXP(LightQuestRecord record, double difficulty = 0.0)
    {
        ushort level = record.Level;

        // Small reward for doing things in bulk.
        double adjustedCount = Math.Pow(record.Count, BULK_FACTOR);

        // Based on curve from the Default XP mixin
        // More generous at most levels, less generous at the jumps.
        double baseXP = 1.150 * Math.Pow(level, 1.90) + 100;

        return (uint)(baseXP * adjustedCount);
    }

    public override uint CalculateRewardR(LightQuestRecord record, double difficulty = 0.0)
    {
        ushort level = record.Level;

        // Small reward for delivering things in bulk.
        double adjustedCount = Math.Pow(record.Count, BULK_FACTOR);

        // Totally arbitrary, will need to be reworked when pawn hiring gets adjusted.
        return (uint)(3.3 * adjustedCount * Math.Pow(level, 1.3));
    }

    public override uint CalculateRewardG(LightQuestRecord record, double difficulty = 0.0)
    {
        ushort level = record.Level;

        // Small reward for doing things in bulk.
        double adjustedCount = Math.Pow(record.Count, BULK_FACTOR);

        // Based on a curve fit of the price vs level for a subset of equipment, with some tweaks
        double fitGold = Math.Pow(level, 1.90) + 50;

        return (uint)(fitGold * adjustedCount);
    }

    public uint CalculateRewardGAlt(LightQuestRecord record, double difficulty = 0.0)
    {
        var itemInfo = LibDdon.Assets.ClientItemInfos[(uint)record.Target];
        var price = itemInfo.Price;

        // Small reward for doing things in bulk.
        double adjustedCount = Math.Pow(record.Count, BULK_FACTOR);

        return (uint)(price * adjustedCount * 3);
    }

    public override uint CalculateRewardAP(LightQuestRecord record, double difficulty = 0.0)
    {
        QuestAreaId areaId = record.QuestInfo.AreaId;
        uint level = record.Level;

        uint amount;

        if (areaId >= QuestAreaId.BloodbaneIsle)
        {
            amount = 250;
        }
        else
        {
            uint tier = level / 5;
            amount = 5 * tier * tier + 5 * tier + 30;
            amount /= 2;
        }

        if (record.QuestInfo.IsAreaOrder)
        {
            amount *= 5;
        }
 
        return amount;
    }
}

return new Mixin();
