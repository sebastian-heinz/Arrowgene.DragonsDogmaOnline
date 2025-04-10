namespace Arrowgene.Ddon.Shared.Model
{
    public class AchievementAsset
    {
        public uint Id { get; set; }
        public uint SortId { get; set; }
        public AchievementCategory Category { get; set; }
        public AchievementType Type { get; set; }
        public uint Param {  get; set; }
        public uint Count { get; set; }
        public uint RewardId { get; set; }
    }
}
