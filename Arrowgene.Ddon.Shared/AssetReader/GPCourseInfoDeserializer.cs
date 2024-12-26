using System.IO;
using System.Linq;
using System.Text.Json;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Asset;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class GPCourseInfoDeserializer : IAssetDeserializer<GPCourseInfoAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(GPCourseInfoDeserializer));

        public GPCourseInfoAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            GPCourseInfoAsset asset = new GPCourseInfoAsset();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            var ValidCourses = document.RootElement.GetProperty("valid_courses").EnumerateArray().ToList();
            foreach (var ValidCourse in ValidCourses)
            {
                GPValidCourse obj = new GPValidCourse();
                obj.Id = ValidCourse.GetProperty("course_id").GetUInt32();
                obj.StartTime = ValidCourse.GetProperty("start_time").GetUInt64();
                obj.EndTime = ValidCourse.GetProperty("end_time").GetUInt64();
                obj.Comment = ValidCourse.GetProperty("comment").GetString();

                asset.ValidCourses.Add(obj.Id, obj);
            }

            var Courses = document.RootElement.GetProperty("courses").EnumerateArray().ToList();
            foreach (var Course in Courses)
            {
                GPCourse obj = new GPCourse();
                obj.Id = Course.GetProperty("id").GetUInt32();
                obj.Name = Course.GetProperty("name").GetString();
                obj.Target = Course.GetProperty("target").GetUInt32() == 1;
                obj.PriorityGroup = Course.GetProperty("priority_grp").GetUInt32();
                obj.PrioritySameTime = Course.GetProperty("priority_same_time").GetUInt32();
                obj.AnnounceType = Course.GetProperty("announce_type").GetUInt32();
                obj.Comment = Course.GetProperty("comment").GetString();
                obj.Url = Course.GetProperty("url").GetString();
                obj.IconPath = Course.GetProperty("icon_path").GetString();
                obj.Description = Course.GetProperty("description").GetString();

                var EffectUIDs = Course.GetProperty("effects").EnumerateArray().ToList();
                foreach (var EffectUID in EffectUIDs)
                {
                    obj.Effects.Add(EffectUID.GetUInt32());
                }

                asset.Courses.Add(obj.Id, obj);
            }

            var Effects = document.RootElement.GetProperty("effects").EnumerateArray().ToList();
            foreach (var Effect in Effects)
            {
                GpCourseEffect obj = new GpCourseEffect();
                obj.Uid = Effect.GetProperty("uid").GetUInt32();
                obj.Id = Effect.GetProperty("id").GetUInt32();
                obj.Param0 = Effect.GetProperty("param0").GetUInt32();
                obj.Param1 = Effect.GetProperty("param1").GetUInt32();
                obj.Comment = Effect.GetProperty("comment").GetString();

                asset.Effects.Add(obj.Uid, obj);
            }

            return asset;
        }
    }
}
