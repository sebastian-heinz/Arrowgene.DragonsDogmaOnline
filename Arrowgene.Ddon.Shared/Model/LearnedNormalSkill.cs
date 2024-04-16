using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model
{
    public class LearnedNormalSkill
    {
        public LearnedNormalSkill()
        {
            SkillNo = new List<uint>();
        }

        public List<uint> SkillNo { get; set; }
        public uint JpCost { get; set; }
        public uint RequiredLevel { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
