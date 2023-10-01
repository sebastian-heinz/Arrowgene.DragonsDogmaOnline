using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] CDataNormalSkillParamFields = new string[]
        {
            "character_common_id", "job", "skill_no", "index", "pre_skill_no"
        };

        private readonly string SqlInsertNormalSkillParam = $"INSERT INTO \"ddon_normal_skill_param\" ({BuildQueryField(CDataNormalSkillParamFields)}) VALUES ({BuildQueryInsert(CDataNormalSkillParamFields)});";
        private readonly string SqlInsertIfNotExistsNormalSkillParam = $"INSERT INTO \"ddon_normal_skill_param\" ({BuildQueryField(CDataNormalSkillParamFields)}) SELECT {BuildQueryInsert(CDataNormalSkillParamFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_normal_skill_param\" WHERE \"character_common_id\" = @character_common_id AND \"job\" = @job AND \"skill_no\"=@skill_no);";
        private static readonly string SqlUpdateNormalSkillParam = $"UPDATE \"ddon_normal_skill_param\" SET {BuildQueryUpdate(CDataNormalSkillParamFields)} WHERE \"character_common_id\" = @character_common_id AND \"job\" = @job AND \"skill_no\"=@skill_no;";
        private static readonly string SqlSelectNormalSkillParam = $"SELECT {BuildQueryField(CDataNormalSkillParamFields)} FROM \"ddon_normal_skill_param\" WHERE \"character_common_id\" = @character_common_id;";
        private const string SqlDeleteNormalSkillParam = "DELETE FROM \"ddon_normal_skill_param\" WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"skill_no\"=@skill_no;";

        public bool InsertIfNotExistsNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsNormalSkillParam(connection, commonId, normalSkillParam);
        }

        public bool InsertIfNotExistsNormalSkillParam(TCon conn, uint commonId, CDataNormalSkillParam normalSkillParam)
        {
            return ExecuteNonQuery(conn, SqlInsertIfNotExistsNormalSkillParam, command =>
            {
                AddParameter(command, commonId, normalSkillParam);
            }) == 1;
        }
        
        public bool InsertNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam)
        {
            using TCon connection = OpenNewConnection();
            return InsertNormalSkillParam(connection, commonId, normalSkillParam);
        }

        public bool InsertNormalSkillParam(TCon conn, uint commonId, CDataNormalSkillParam normalSkillParam)
        {
            return ExecuteNonQuery(conn, SqlInsertNormalSkillParam, command =>
            {
                AddParameter(command, commonId, normalSkillParam);
            }) == 1;
        }

        public bool ReplaceNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam)
        {
            using TCon connection = OpenNewConnection();
            return ReplaceNormalSkillParam(connection, commonId, normalSkillParam);
        }

        public bool ReplaceNormalSkillParam(TCon conn, uint commonId, CDataNormalSkillParam normalSkillParam)
        {
            Logger.Debug("Inserting storage item.");
            if (!InsertIfNotExistsNormalSkillParam(conn, commonId, normalSkillParam))
            {
                Logger.Debug("Storage item already exists, replacing.");
                return UpdateNormalSkillParam(conn, commonId, normalSkillParam.Job, normalSkillParam.SkillNo, normalSkillParam);
            }

            return true;
        }

        public bool UpdateNormalSkillParam(uint commonId, JobId job, uint skillNo, CDataNormalSkillParam normalSkillParam)
        {
            using TCon connection = OpenNewConnection();
            return UpdateNormalSkillParam(connection, commonId, job, skillNo, normalSkillParam);
        }

        public bool UpdateNormalSkillParam(TCon connection, uint commonId, JobId job, uint skillNo, CDataNormalSkillParam normalSkillParam)
        {
            return ExecuteNonQuery(connection, SqlUpdateNormalSkillParam, command =>
            {
                AddParameter(command, commonId, normalSkillParam);
            }) == 1;
        }
        
        public bool DeleteNormalSkillParam(uint commonId, JobId job, uint skillNo)
        {
            return ExecuteNonQuery(SqlDeleteNormalSkillParam, command =>
            {
                AddParameter(command, "@character_common_id", commonId);
                AddParameter(command, "@job", (byte)job);
                AddParameter(command, "@skill_no", skillNo);
            }) == 1;
        }

        private CDataNormalSkillParam ReadNormalSkillParam(TReader reader)
        {
            CDataNormalSkillParam normalSkillParam = new CDataNormalSkillParam();
            normalSkillParam.Job = (JobId)GetByte(reader, "job");
            normalSkillParam.SkillNo = GetUInt32(reader, "skill_no");
            normalSkillParam.Index = GetUInt32(reader, "index");
            normalSkillParam.PreSkillNo = GetUInt32(reader, "pre_skill_no");
            return normalSkillParam;
        }

        private void AddParameter(TCom command, uint commonId, CDataNormalSkillParam normalSkillParam)
        {
            AddParameter(command, "character_common_id", commonId);
            AddParameter(command, "job", (byte)normalSkillParam.Job);
            AddParameter(command, "skill_no", normalSkillParam.SkillNo);
            AddParameter(command, "index", normalSkillParam.Index);
            AddParameter(command, "pre_skill_no", normalSkillParam.PreSkillNo);
        }
    }
}
