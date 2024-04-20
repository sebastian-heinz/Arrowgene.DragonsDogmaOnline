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
        public bool Target { get; set; }
        public uint PriorityGroup { get; set; }
        public uint PrioritySameTime { get; set; }
        public uint AnnounceType { get; set; }
        public List<uint> Effects { get; set; }

        public string Comment { get; set; }
    }
}
