using System.Diagnostics;

namespace Arrowgene.Ddon.Shared.Model.EpitaphRoad
{
    public enum EpitaphIdKind : byte
    {
        Path = 0,
        Trial = 1,
        Barrier = 2,
        Statue = 3,
        WeeklyReward = 4,
        Door = 5,
        GatheringPoint = 6
    }

    public class EpitaphId
    {
        private static readonly Bitfield Kind = new Bitfield(3, 0, "Kind");
        // If input is NPC
        private static readonly Bitfield NpcId = new Bitfield(27, 4, "NpcId");
        // If input is OM
        private static readonly Bitfield StageId    = new Bitfield(15, 4, "StageId");
        private static readonly Bitfield GroupId    = new Bitfield(24, 16, "GroupId");
        private static readonly Bitfield PosId = new Bitfield(29, 25, "PosId");

        // Index common for both
        private static readonly Bitfield Index = new Bitfield(31, 30, "Index");

        public static uint GenerateTrialId(StageLayoutId location, uint posId, uint index)
        {
            return (uint)(Kind.Value((ulong)EpitaphIdKind.Trial) |
                          StageId.Value(location.Id) |
                          GroupId.Value(location.GroupId) |
                          PosId.Value(posId) |
                          Index.Value(index));
        }

        public static uint GenerateBarrierId(StageLayoutId location, uint posId, uint index)
        {
            return (uint)(Kind.Value((ulong)EpitaphIdKind.Barrier) |
                          StageId.Value(location.Id) |
                          GroupId.Value(location.GroupId) |
                          PosId.Value(posId) |
                          Index.Value(index));
        }

        public static uint GenerateStatueId(StageLayoutId location, uint posId, uint index)
        {
            return (uint)(Kind.Value((ulong)EpitaphIdKind.Statue) |
                          StageId.Value(location.Id) |
                          GroupId.Value(location.GroupId) |
                          PosId.Value(posId) |
                          Index.Value(index));
        }

        public static uint GeneratePathId(NpcId npcId, uint index)
        {
            return (uint)(Kind.Value((ulong)EpitaphIdKind.Path) |
                          NpcId.Value((ulong)npcId) |
                          Index.Value(index));
        }

        public static uint GenerateWeeklyRewardId(StageLayoutId location, uint posId, uint index)
        {
            return (uint)(Kind.Value((ulong)EpitaphIdKind.WeeklyReward) |
              StageId.Value(location.Id) |
              GroupId.Value(location.GroupId) |
              PosId.Value(posId) |
              Index.Value(index));
        }

        public static uint GenerateDoorId(StageLayoutId location, uint posId, uint index)
        {
            return (uint)(Kind.Value((ulong)EpitaphIdKind.Door) |
              StageId.Value(location.Id) |
              GroupId.Value(location.GroupId) |
              PosId.Value(posId) |
              Index.Value(index));
        }

        public static uint GenerateGatheringPointId(StageLayoutId location, uint posId, uint index)
        {
            return (uint)(Kind.Value((ulong)EpitaphIdKind.GatheringPoint) |
              StageId.Value(location.Id) |
              GroupId.Value(location.GroupId) |
              PosId.Value(posId) |
              Index.Value(index));
        }

        public static EpitaphIdKind GetKind(uint epitaphId)
        {
            return (EpitaphIdKind) Kind.Get(epitaphId);
        }

        public static uint GetIndex(uint epitaphId)
        {
            return (uint) Index.Get(epitaphId);
        }

        public static uint GetIndexWidth()
        {
            return (uint) Index.Width();
        }
    }
}
