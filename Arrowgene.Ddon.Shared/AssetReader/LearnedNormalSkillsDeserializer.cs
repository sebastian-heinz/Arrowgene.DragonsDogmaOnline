using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Asset;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class LearnedNormalSkillsDeserializer : IAssetDeserializer<LearnedNormalSkillsAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(LearnedNormalSkillsDeserializer));

        public LearnedNormalSkillsAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            LearnedNormalSkillsAsset asset = new LearnedNormalSkillsAsset();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            // List<uint> Keys = document.RootElement.EnumerateObject().ToList
            var Results = document.RootElement.EnumerateObject().ToList();

            foreach (JsonProperty Property in Results)
            {
                JobId job = (JobId) uint.Parse(Property.Name);

                var Entries = Property.Value.EnumerateArray().ToList();

                List<LearnedNormalSkill> LearnedNormalSkills = new List<LearnedNormalSkill>();
                foreach (var Entry in Entries)
                {
                    var LearnedNormalSkill = new LearnedNormalSkill();
                    LearnedNormalSkill.JpCost = Entry.GetProperty("jp").GetUInt32();
                    LearnedNormalSkill.RequiredLevel = Entry.GetProperty("level").GetUInt32();
                    LearnedNormalSkill.Name = Entry.GetProperty("name").GetString();
                    LearnedNormalSkill.Type = Entry.GetProperty("type").GetString();

                    foreach (var SkillNo in Entry.GetProperty("skillno").EnumerateArray().ToList())
                    {
                        LearnedNormalSkill.SkillNo.Add(SkillNo.GetUInt32());
                    }

                    LearnedNormalSkills.Add(LearnedNormalSkill);
                }

                asset.LearnedNormalSkills.Add(job, LearnedNormalSkills);
            }

            return asset;
        }
    }
}
