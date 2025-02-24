using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataAchievementFurnitureReward
{
    public uint RewardId { get; set; }
    public uint SortId { get; set; }
    public CDataAchievementIdentifier AchieveIdentifier { get; set; } = new();
    public uint FurnitureItemId { get; set; }
    public bool IsReceived { get; set; }

    public class Serializer : EntitySerializer<CDataAchievementFurnitureReward>
    {
        public override void Write(IBuffer buffer, CDataAchievementFurnitureReward obj)
        {
            WriteUInt32(buffer, obj.RewardId);
            WriteUInt32(buffer, obj.SortId);
            WriteEntity(buffer, obj.AchieveIdentifier);
            WriteUInt32(buffer, obj.FurnitureItemId);
            WriteBool(buffer, obj.IsReceived);
        }

        public override CDataAchievementFurnitureReward Read(IBuffer buffer)
        {
            CDataAchievementFurnitureReward obj = new CDataAchievementFurnitureReward();
            obj.RewardId = ReadUInt32(buffer);
            obj.SortId = ReadUInt32(buffer);
            obj.AchieveIdentifier = ReadEntity<CDataAchievementIdentifier>(buffer);
            obj.FurnitureItemId = ReadUInt32(buffer);
            obj.IsReceived = ReadBool(buffer);
            return obj;
        }
    }
}
