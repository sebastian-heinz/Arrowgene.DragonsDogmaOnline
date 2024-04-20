using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Asset
{
    public class GPCourseInfoAsset
    {
        public GPCourseInfoAsset()
        {
            Courses = new Dictionary<uint, GPCourse>();
            Effects = new Dictionary<uint, GpCourseEffect>();
            ValidCourses = new Dictionary<uint, GPValidCourse>();
        }
        public Dictionary<uint, GPCourse> Courses { get; set; }
        public Dictionary<uint, GpCourseEffect> Effects { get; set; }
        public Dictionary<uint, GPValidCourse> ValidCourses { get; set; }
    }
}
