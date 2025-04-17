using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class JobMasterMigration : IMigrationStrategy
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(JobMasterMigration));

        public uint From => 35;
        public uint To => 36;

        private readonly DatabaseSetting DatabaseSetting;

        public JobMasterMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_job_master.sql");
            db.Execute(conn, adaptedSchema);

            var refundedJp = new Dictionary<uint, Dictionary<JobId, long>>();

            Logger.Info("Collecting information about characters ...");
            var normalCharacters = new HashSet<uint>();
            db.ExecuteReader(conn, "SELECT * FROM ddon_character WHERE game_mode=1;",
               command => { },
               reader =>
               {
                   while (reader.Read())
                   {
                       uint characterCommonId = db.GetUInt32(reader, "character_common_id");
                       normalCharacters.Add(characterCommonId);
                   }
               }
            );

            Logger.Info("Collecting all abilities learned by players ...");
            var abilityMap = new Dictionary<uint, List<Ability>>();
            db.ExecuteReader(conn, "SELECT * FROM ddon_learned_ability;",
               command => { },
               reader =>
               {
                   while (reader.Read())
                   {
                       uint characterCommonId = db.GetUInt32(reader, "character_common_id");
                       if (!normalCharacters.Contains(characterCommonId))
                       {
                           // skip BBM characters
                           continue;
                       }

                       if (!abilityMap.ContainsKey(characterCommonId))
                       {
                           abilityMap[characterCommonId] = new List<Ability>();
                       }

                       var ability = new Ability();
                       ability.AbilityLv = db.GetByte(reader, "ability_lv");
                       ability.Job = (JobId) db.GetByte(reader, "job");
                       ability.AbilityId = db.GetUInt32(reader, "ability_id");
                       abilityMap[characterCommonId].Add(ability);
                   }
               }
            );

            Logger.Info("Collecting all custom skills learned by players ...");
            var customSkillMap = new Dictionary<uint, List<CustomSkill>>();
            db.ExecuteReader(conn, "SELECT * FROM ddon_learned_custom_skill;",
               command => { },
               reader =>
               {
                   while (reader.Read())
                   {
                       uint characterCommonId = db.GetUInt32(reader, "character_common_id");
                       if (!normalCharacters.Contains(characterCommonId))
                       {
                           // skip BBM characters
                           continue;
                       }

                       if (!customSkillMap.ContainsKey(characterCommonId))
                       {
                           customSkillMap[characterCommonId] = new List<CustomSkill>();
                       }

                       var customSkill = new CustomSkill();
                       customSkill.SkillLv = db.GetByte(reader, "skill_lv");
                       customSkill.Job = (JobId)db.GetByte(reader, "job");
                       customSkill.SkillId = db.GetUInt32(reader, "skill_id");
                       customSkillMap[characterCommonId].Add(customSkill);
                   }
               }
            );

            Logger.Info("Calculating refund for all abilities above level 3 ...");
            foreach (var (characterCommonId, abilityList) in abilityMap)
            {
                if (!refundedJp.ContainsKey(characterCommonId))
                {
                    refundedJp[characterCommonId] = new Dictionary<JobId, long>();
                }

                foreach (var ability in abilityList)
                {
                    if (!refundedJp[characterCommonId].ContainsKey(ability.Job))
                    {
                        refundedJp[characterCommonId][ability.Job] = 0;
                    }

                    if (ability.AbilityLv <= 3)
                    {
                        // Leave it alone
                        continue;
                    }

                    refundedJp[characterCommonId][ability.Job] += SkillData.AllAbilities
                        .Where(x => x.AbilityNo == ability.AbilityId)
                        .SelectMany(x => x.Params)
                        .Where(x => x.Lv > 3 && x.Lv <= ability.AbilityLv)
                        .Sum(x => x.RequireJobPoint);

                    ability.AbilityLv = 3;
                }
            }

            Logger.Info("Calculating refund for all custom skills above level 4 ...");
            foreach (var (characterCommonId, customSkillList) in customSkillMap)
            {
                if (!refundedJp.ContainsKey(characterCommonId))
                {
                    refundedJp[characterCommonId] = new Dictionary<JobId, long>();
                }

                foreach (var customSkill in customSkillList)
                {
                    if (!refundedJp[characterCommonId].ContainsKey(customSkill.Job))
                    {
                        refundedJp[characterCommonId][customSkill.Job] = 0;
                    }

                    if (customSkill.SkillLv <= 4)
                    {
                        // Leave it alone
                        continue;
                    }

                    refundedJp[characterCommonId][customSkill.Job] += SkillData.AllSkills
                        .Where(x => x.Job == customSkill.Job)
                        .Where(x => x.SkillNo == customSkill.SkillId)
                        .SelectMany(x => x.Params)
                        .Where(x => x.Lv > 4 && x.Lv <= customSkill.SkillId)
                        .Sum(x => x.RequireJobPoint);

                    customSkill.SkillLv = 4;
                }
            }

            Logger.Info("Collecting existing character job data ...");
            var characterJobData = new Dictionary<uint, Dictionary<JobId, long>>();
            db.ExecuteReader(conn, "SELECT * FROM ddon_character_job_data;",
               command => { },
               reader =>
               {
                   while (reader.Read())
                   {
                       uint characterCommonId = db.GetUInt32(reader, "character_common_id");
                       if (!normalCharacters.Contains(characterCommonId))
                       {
                           // skip BBM characters
                           continue;
                       }

                       if (!characterJobData.ContainsKey(characterCommonId))
                       {
                           characterJobData[characterCommonId] = new Dictionary<JobId, long>();
                       }

                       JobId jobId = (JobId)db.GetByte(reader, "job");
                       characterJobData[characterCommonId][jobId] = db.GetInt32(reader, "job_point");
                   }
               }
            );

            // Drop the tables
            Logger.Info("Dropping equipped skills and abilities ...");
            db.Execute(conn, "DELETE FROM ddon_equipped_ability;");
            db.Execute(conn, "DELETE FROM ddon_equipped_custom_skill;");

            Logger.Info("Updating abilities ...");
            foreach (var (characterCommonId, abilityList) in abilityMap)
            {
                foreach (var ability in abilityList)
                {
                    db.ExecuteNonQuery(conn, "UPDATE ddon_learned_ability SET ability_lv=@ability_lv WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"ability_id\"=@ability_id;",
                    command =>
                    {
                        db.AddParameter(command, "character_common_id", characterCommonId);
                        db.AddParameter(command, "job", (byte)ability.Job);
                        db.AddParameter(command, "ability_id", ability.AbilityId);
                        db.AddParameter(command, "ability_lv", ability.AbilityLv);
                    });
                }
            }

            Logger.Info("Updating custom skills ...");
            foreach (var (characterCommonId, customSkillList) in customSkillMap)
            {
                foreach (var customSkill in customSkillList)
                {
                    db.ExecuteNonQuery(conn, "UPDATE ddon_learned_custom_skill SET skill_lv=@skill_lv WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"skill_id\"=@skill_id;",
                    command =>
                    {
                        db.AddParameter(command, "character_common_id", characterCommonId);
                        db.AddParameter(command, "job", (byte)customSkill.Job);
                        db.AddParameter(command, "skill_id", customSkill.SkillId);
                        db.AddParameter(command, "skill_lv", customSkill.SkillLv);
                    });
                }
            }

            Logger.Info("Refunding job points for each character ...");
            foreach (var (characterCommonId, jobMap) in characterJobData)
            {
                foreach (var (jobId, jobPoints) in jobMap)
                {
                    long totalJobPoints = jobPoints;
                    if (refundedJp.ContainsKey(characterCommonId) && refundedJp[characterCommonId].ContainsKey(jobId))
                    {
                        totalJobPoints += refundedJp[characterCommonId][jobId];
                    }

                    db.ExecuteNonQuery(conn, "UPDATE ddon_character_job_data SET \"job_point\"=@job_point WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job;",
                    command =>
                    {
                        db.AddParameter(command, "character_common_id", characterCommonId);
                        db.AddParameter(command, "job", (byte)jobId);
                        db.AddParameter(command, "job_point", (int) totalJobPoints);
                    });
                }
            }

            return true;
        }
    }
}
