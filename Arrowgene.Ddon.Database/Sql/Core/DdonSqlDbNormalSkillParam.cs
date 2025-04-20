using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string SqlDeleteNormalSkillParam =
        "DELETE FROM \"ddon_normal_skill_param\" WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"skill_no\"=@skill_no;";

    protected static readonly string[] CDataNormalSkillParamFields = new[]
    {
        "character_common_id", "job", "skill_no", "index", "pre_skill_no"
    };

    private static readonly string SqlUpdateNormalSkillParam =
        $"UPDATE \"ddon_normal_skill_param\" SET {BuildQueryUpdate(CDataNormalSkillParamFields)} WHERE \"character_common_id\" = @character_common_id AND \"job\" = @job AND \"skill_no\"=@skill_no;";

    private static readonly string SqlSelectNormalSkillParam =
        $"SELECT {BuildQueryField(CDataNormalSkillParamFields)} FROM \"ddon_normal_skill_param\" WHERE \"character_common_id\" = @character_common_id;";

    private static readonly string SqlSelectAllNormalSkillParam =
        $"SELECT {BuildQueryField(CDataNormalSkillParamFields)} FROM \"ddon_normal_skill_param\" WHERE \"character_common_id\" = @character_common_id and \"job\" = @job;";

    private readonly string SqlInsertIfNotExistsNormalSkillParam =
        $"INSERT INTO \"ddon_normal_skill_param\" ({BuildQueryField(CDataNormalSkillParamFields)}) SELECT {BuildQueryInsert(CDataNormalSkillParamFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_normal_skill_param\" WHERE \"character_common_id\" = @character_common_id AND \"job\" = @job AND \"skill_no\"=@skill_no);";

    private readonly string SqlInsertNormalSkillParam =
        $"INSERT INTO \"ddon_normal_skill_param\" ({BuildQueryField(CDataNormalSkillParamFields)}) VALUES ({BuildQueryInsert(CDataNormalSkillParamFields)});";

    public override List<CDataNormalSkillParam> SelectNormalSkillParam(uint commonId, JobId job)
    {
        using DbConnection connection = OpenNewConnection();
        return SelectNormalSkillParam(connection, commonId, job);
    }

    public List<CDataNormalSkillParam> SelectNormalSkillParam(DbConnection conn, uint commonId, JobId job)
    {
        List<CDataNormalSkillParam> LearnedNormalSkills = new();

        ExecuteInTransaction(conn =>
        {
            ExecuteReader(conn, SqlSelectAllNormalSkillParam,
                command =>
                {
                    AddParameter(command, "@character_common_id", commonId);
                    AddParameter(command, "@job", (byte)job);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        CDataNormalSkillParam LearnedNormalSkill = ReadNormalSkillParam(reader);
                        LearnedNormalSkills.Add(LearnedNormalSkill);
                    }
                });
        });

        return LearnedNormalSkills;
    }

    public override bool InsertIfNotExistsNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertIfNotExistsNormalSkillParam(connection, commonId, normalSkillParam);
    }

    public bool InsertIfNotExistsNormalSkillParam(DbConnection conn, uint commonId, CDataNormalSkillParam normalSkillParam)
    {
        return ExecuteNonQuery(conn, SqlInsertIfNotExistsNormalSkillParam, command => { AddParameter(command, commonId, normalSkillParam); }) == 1;
    }

    public override bool InsertNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertNormalSkillParam(connection, commonId, normalSkillParam);
    }

    public bool InsertNormalSkillParam(DbConnection conn, uint commonId, CDataNormalSkillParam normalSkillParam)
    {
        return ExecuteNonQuery(conn, SqlInsertNormalSkillParam, command => { AddParameter(command, commonId, normalSkillParam); }) == 1;
    }

    public override bool ReplaceNormalSkillParam(uint commonId, CDataNormalSkillParam normalSkillParam)
    {
        using DbConnection connection = OpenNewConnection();
        return ReplaceNormalSkillParam(connection, commonId, normalSkillParam);
    }

    public bool ReplaceNormalSkillParam(DbConnection conn, uint commonId, CDataNormalSkillParam normalSkillParam)
    {
        Logger.Debug("Inserting storage item.");
        if (!InsertIfNotExistsNormalSkillParam(conn, commonId, normalSkillParam))
        {
            Logger.Debug("Storage item already exists, replacing.");
            return UpdateNormalSkillParam(conn, commonId, normalSkillParam.Job, normalSkillParam.SkillNo, normalSkillParam);
        }

        return true;
    }

    public override bool UpdateNormalSkillParam(uint commonId, JobId job, uint skillNo, CDataNormalSkillParam normalSkillParam)
    {
        using DbConnection connection = OpenNewConnection();
        return UpdateNormalSkillParam(connection, commonId, job, skillNo, normalSkillParam);
    }

    public bool UpdateNormalSkillParam(DbConnection connection, uint commonId, JobId job, uint skillNo, CDataNormalSkillParam normalSkillParam)
    {
        return ExecuteNonQuery(connection, SqlUpdateNormalSkillParam, command => { AddParameter(command, commonId, normalSkillParam); }) == 1;
    }

    public override bool DeleteNormalSkillParam(uint commonId, JobId job, uint skillNo)
    {
        return ExecuteNonQuery(SqlDeleteNormalSkillParam, command =>
        {
            AddParameter(command, "@character_common_id", commonId);
            AddParameter(command, "@job", (byte)job);
            AddParameter(command, "@skill_no", skillNo);
        }) == 1;
    }

    private CDataNormalSkillParam ReadNormalSkillParam(DbDataReader reader)
    {
        CDataNormalSkillParam normalSkillParam = new();
        normalSkillParam.Job = (JobId)GetByte(reader, "job");
        normalSkillParam.Index = GetUInt32(reader, "index");
        normalSkillParam.SkillNo = GetUInt32(reader, "skill_no");
        normalSkillParam.PreSkillNo = GetUInt32(reader, "pre_skill_no");
        return normalSkillParam;
    }

    private void AddParameter(DbCommand command, uint commonId, CDataNormalSkillParam normalSkillParam)
    {
        AddParameter(command, "character_common_id", commonId);
        AddParameter(command, "job", (byte)normalSkillParam.Job);
        AddParameter(command, "skill_no", normalSkillParam.SkillNo);
        AddParameter(command, "index", normalSkillParam.Index);
        AddParameter(command, "pre_skill_no", normalSkillParam.PreSkillNo);
    }
}
