namespace Arrowgene.Ddon.Shared.Model.EpitaphRoad
{
    public enum SoulOrdealObjective : byte
    {
        DefeatEnemyCount                  = 1, // Param1 = Amount
        DefeatEnemyWithAbnormalStatusCount = 2, // Param1 = Amount
        InflictAbnormalStatusCount        = 3, // Param1 = Amount
        CannotDieMoreThanOnce             = 4,
        CannotBeAffectedByAbnormalStatus  = 5, // Param1 = Amount
        ItemNoteUsedMoreThanOnce          = 7,
        EliminateTheEnemy                 = 8,
        CompleteConditionsWithinTimeLimit = 10, // Param1 = TimeInSeconds
    }
}
