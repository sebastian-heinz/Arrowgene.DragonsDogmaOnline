using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model
{
    public class GPCourse
    {
        public GPCourse()
        {
            Effects = new List<uint>();
        }

        public uint Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string IconPath { get; set; }
        public bool Target { get; set; }
        public uint PriorityGroup { get; set; }
        public uint PrioritySameTime { get; set; }
        public uint AnnounceType { get; set; }
        public List<uint> Effects { get; set; }
    }
}
