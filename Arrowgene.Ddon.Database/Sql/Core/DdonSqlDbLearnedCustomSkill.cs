using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        private static readonly string[] LearnedCustomSkillFields = new string[]
        {
            "character_common_id", "job", "skill_id", "skill_lv"
        };

        private readonly string SqlInsertLearnedCustomSkill = $"INSERT INTO \"ddon_learned_custom_skill\" ({BuildQueryField(LearnedCustomSkillFields)}) VALUES ({BuildQueryInsert(LearnedCustomSkillFields)});";
        private readonly string SqlUpdateLearnedCustomSkill = $"UPDATE \"ddon_learned_custom_skill\" SET {BuildQueryUpdate(LearnedCustomSkillFields)} WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"skill_id\"=@skill_id;";
        private static readonly string SqlSelectLearnedCustomSkills = $"SELECT {BuildQueryField(LearnedCustomSkillFields)} FROM \"ddon_learned_custom_skill\" WHERE \"character_common_id\"=@character_common_id;";

        public bool InsertLearnedCustomSkill(uint commonId, CustomSkill skill, DbConnection? connectionIn = null)
        {
            ExecuteQuerySafe(connectionIn, connection =>
            {
                ExecuteNonQuery(connection, SqlInsertLearnedCustomSkill, command =>
                {
                    AddParameter(command, commonId, skill);
                });
            });
            
            return true;
        }

        public bool UpdateLearnedCustomSkill(uint commonId, CustomSkill updatedSkill, DbConnection? connectionIn = null)
        {
            ExecuteQuerySafe(connectionIn, connection =>
            {
                ExecuteNonQuery(connection, SqlUpdateLearnedCustomSkill, command =>
                {
                    AddParameter(command, commonId, updatedSkill);
                });
            });
            
            return true;
        }

        private CustomSkill ReadLearnedCustomSkill(DbDataReader reader)
        {
            CustomSkill skill = new CustomSkill();
            skill.Job = (JobId) GetByte(reader, "job");
            skill.SkillId = GetUInt32(reader, "skill_id");
            skill.SkillLv = GetByte(reader, "skill_lv");
            return skill;
        }

        private void AddParameter(TCom command, uint commonId, CustomSkill skill)
        {
            AddParameter(command, "character_common_id", commonId);
            AddParameter(command, "job", (byte) skill.Job);
            AddParameter(command, "skill_id", skill.SkillId);
            AddParameter(command, "skill_lv", skill.SkillLv);
        }
    }
}
