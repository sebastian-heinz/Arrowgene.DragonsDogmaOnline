namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestScheduleId
    {
        private static readonly Bitfield Variant = new(6, 0, "Variant");
        private static readonly Bitfield Index = new(13, 7, "Index");
        private static readonly Bitfield Subgroup = new(20, 14, "Subgroup");
        private static readonly Bitfield Group = new(27, 21, "Group");
        private static readonly Bitfield Type = new(31, 28, "Type");

        private static readonly Bitfield RotationVariant = new(27, 0, "RotationVariant");

        public static uint GenerateScheduleId(byte type, byte group, byte subgroup, byte index, byte variant)
        {
            return (uint)(Variant.Value(variant) |
                Index.Value(index) |
                Subgroup.Value(subgroup) |
                Group.Value(group) |
                Type.Value(type));
        }

        public static uint GenerateRotatingId(byte type, uint variant)
        {
            return (uint)(RotationVariant.Value(variant) |
                Type.Value(type));
        }

        public static uint BaseScheduleId(uint scheduleId)
        {
            return GenerateScheduleId(
                (byte) Type.Get(scheduleId),
                (byte) Group.Get(scheduleId),
                (byte) Subgroup.Get(scheduleId),
                (byte) Index.Get(scheduleId), 
                0
            );
        }

        public static byte GetVariant(uint scheduleId)
        {
            return (byte)Variant.Value(scheduleId);
        }

        public static uint GetRotatingVariant(uint scheduleId)
        {
            return (uint)RotationVariant.Value(scheduleId);
        }

        public static byte GetType(uint scheduleId)
        {
            return (byte)(Type.Get(scheduleId));
        }
    }
}
