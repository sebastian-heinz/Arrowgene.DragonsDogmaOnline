namespace Arrowgene.Ddon.Shared.Model
{
    public class GPValidCourse
    {
        public GPValidCourse()
        {
        }

        public uint Id { get; set; }
        public ulong StartTime { get; set; }
        public ulong EndTime { get; set; }
        public string Comment { get; set; }
    }
}
