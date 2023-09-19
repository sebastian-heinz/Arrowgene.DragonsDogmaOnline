using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        protected static readonly string[] CDataNormalSkillParamFields = new string[]
        {
            "character_common_id", "job", "skill_no", "index", "pre_skill_no"
        };

        private readonly string SqlInsertNormalSkillParam = $"INSERT INTO \"ddon_normal_skill_param\" ({BuildQueryField(CDataNormalSkillParamFields)}) VALUES ({BuildQueryInsert(CDataNormalSkillParamFields)});";

        protected virtual string SqlReplaceNormalSkillParam { get; } =
            $"INSERT OR REPLACE INTO \"ddon_normal_skill_param\" ({BuildQueryField(CDataNormalSkillParamFields)}) VALUES ({BuildQueryInsert(CDataNormalSkillParamFields)});";

        private static readonly string SqlUpdateNormalSkillParam = $"UPDATE \"ddon_normal_skill_param\" SET {BuildQueryUpdate(CDataNormalSkillParamFields)} WHERE \"character_common_id\" = @character_common_id AND \"job\" = @job AND \"skill_no\"=@skill_no;";
        private static readonly string SqlSelectNormalSkillParam = $"SELECT {BuildQueryField(CDataNormalSkillParamFields)} FROM \"ddon_normal_skill_param\" WHERE \"character_common_id\" = @character_common_id;";
        private const string SqlDeleteNormalSkillParam = "DELETE FROM \"ddon_normal_skill_param\" WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"skill_no\"=@skill_no;";

        private CDataNormalSkillParam ReadNormalSkillParam(DbDataReader reader)
        {
            CDataNormalSkillParam normalSkillParam = new CDataNormalSkillParam();
            normalSkillParam.Job = (JobId) GetByte(reader, "job");
            normalSkillParam.SkillNo = GetUInt32(reader, "skill_no");
            normalSkillParam.Index = GetUInt32(reader, "index");
            normalSkillParam.PreSkillNo = GetUInt32(reader, "pre_skill_no");
            return normalSkillParam;
        }

        private void AddParameter(TCom command, uint commonId, CDataNormalSkillParam normalSkillParam)
        {
            AddParameter(command, "character_common_id", commonId);
            AddParameter(command, "job", (byte) normalSkillParam.Job);
            AddParameter(command, "skill_no", normalSkillParam.SkillNo);
            AddParameter(command, "index", normalSkillParam.Index);
            AddParameter(command, "pre_skill_no", normalSkillParam.PreSkillNo);
        }
    }
}
