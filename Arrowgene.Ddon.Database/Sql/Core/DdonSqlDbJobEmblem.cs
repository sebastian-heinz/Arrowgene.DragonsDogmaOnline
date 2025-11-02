using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    /* ddon_job_emblem */
    protected static readonly string[] JobEmblemFields = new[]
    {
        "character_id", "job_id", "emblem_level", "emblem_points_used",
        "physical_attack", "magick_attack", "physical_defense", "magick_defense",
        "max_hp", "max_stamina", "healing_power", "endurance", "blow_power", "chance_attack", "exhaust_attack",
        "knockout_power", "fire_resist", "ice_resist", "thunder_resist", "holy_resist", "dark_resist"
    };

    private readonly string SqlUpsertJobEmblemData = $"""
        INSERT INTO ddon_job_emblem ({BuildQueryField(JobEmblemFields)})
        VALUES ({BuildQueryInsert(JobEmblemFields)})
        ON CONFLICT (character_id, job_id)
        DO UPDATE SET {BuildQueryUpdate(JobEmblemFields)};
    """;

    private readonly string SqlSelectAllJobEmblemData =
        $"SELECT {BuildQueryField(JobEmblemFields)} FROM \"ddon_job_emblem\" WHERE  \"character_id\"=@character_id;";

    private readonly string SqlSelectJobEmblemData =
        $"SELECT {BuildQueryField(JobEmblemFields)} FROM \"ddon_job_emblem\" WHERE \"character_id\"=@character_id AND \"job_id\"=@job_id;";

    public override bool UpsertJobEmblemData(uint characterId, JobEmblem jobEmblem, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpsertJobEmblemData, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "job_id", (byte)jobEmblem.JobId);
                AddParameter(command, "emblem_level", jobEmblem.EmblemLevel);
                AddParameter(command, "emblem_points_used", jobEmblem.EmblemPointsUsed);
                AddParameter(command, "physical_attack", jobEmblem.StatLevels[EquipStatId.PhysicalAttack]);
                AddParameter(command, "magick_attack", jobEmblem.StatLevels[EquipStatId.MagickAttack]);
                AddParameter(command, "physical_defense", jobEmblem.StatLevels[EquipStatId.PhysicalDefense]);
                AddParameter(command, "magick_defense", jobEmblem.StatLevels[EquipStatId.MagickDefense]);
                AddParameter(command, "max_hp", jobEmblem.StatLevels[EquipStatId.MaxHp]);
                AddParameter(command, "max_stamina", jobEmblem.StatLevels[EquipStatId.MaxStamina]);
                AddParameter(command, "healing_power", jobEmblem.StatLevels[EquipStatId.HealingPower]);
                AddParameter(command, "endurance", jobEmblem.StatLevels[EquipStatId.Endurance]);
                AddParameter(command, "blow_power", jobEmblem.StatLevels[EquipStatId.BlowPower]);
                AddParameter(command, "chance_attack", jobEmblem.StatLevels[EquipStatId.ChanceAttack]);
                AddParameter(command, "exhaust_attack", jobEmblem.StatLevels[EquipStatId.ExhaustAttack]);
                AddParameter(command, "knockout_power", jobEmblem.StatLevels[EquipStatId.KnockoutPower]);
                AddParameter(command, "fire_resist", jobEmblem.StatLevels[EquipStatId.FireResist]);
                AddParameter(command, "ice_resist", jobEmblem.StatLevels[EquipStatId.IceResist]);
                AddParameter(command, "thunder_resist", jobEmblem.StatLevels[EquipStatId.ThunderResist]);
                AddParameter(command, "holy_resist", jobEmblem.StatLevels[EquipStatId.HolyResist]);
                AddParameter(command, "dark_resist", jobEmblem.StatLevels[EquipStatId.DarkResist]);
            }) == 1;
        });
    }

    public override List<JobEmblem> GetAllJobEmblemData(uint characterId, DbConnection? connectionIn = null)
    {
        List<JobEmblem> results = new();
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectAllJobEmblemData, command =>
            {
                AddParameter(command, "character_id", characterId);
            },
            reader =>
            {
                while (reader.Read())
                {
                    results.Add(ReadEmblemData(reader));
                }
            });
        });
        return results;
    }

    public override JobEmblem GetJobEmblemData(uint characterId, JobId jobId, DbConnection? connectionIn = null)
    {
        JobEmblem result = null;
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectJobEmblemData, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "job_id", (byte)jobId);
            }, reader =>
            {
                if (reader.Read())
                {
                    result = ReadEmblemData(reader);
                }
            });
        });
        return result;
    }

    private JobEmblem ReadEmblemData(DbDataReader reader)
    {
        var result = new JobEmblem
        {
            JobId = (JobId)GetByte(reader, "job_id"),
            EmblemLevel = GetByte(reader, "emblem_level"),
            EmblemPointsUsed = GetUInt16(reader, "emblem_points_used"),
        };

        result.StatLevels[EquipStatId.PhysicalAttack] = GetByte(reader, "physical_attack");
        result.StatLevels[EquipStatId.PhysicalDefense] = GetByte(reader, "physical_defense");
        result.StatLevels[EquipStatId.MagickAttack] = GetByte(reader, "magick_attack");
        result.StatLevels[EquipStatId.MagickDefense] = GetByte(reader, "magick_defense");
        result.StatLevels[EquipStatId.MaxHp] = GetByte(reader, "max_hp");
        result.StatLevels[EquipStatId.MaxStamina] = GetByte(reader, "max_stamina");
        result.StatLevels[EquipStatId.HealingPower] = GetByte(reader, "healing_power");
        result.StatLevels[EquipStatId.Endurance] = GetByte(reader, "endurance");
        result.StatLevels[EquipStatId.BlowPower] = GetByte(reader, "blow_power");
        result.StatLevels[EquipStatId.ChanceAttack] = GetByte(reader, "chance_attack");
        result.StatLevels[EquipStatId.ExhaustAttack] = GetByte(reader, "exhaust_attack");
        result.StatLevels[EquipStatId.KnockoutPower] = GetByte(reader, "knockout_power");
        result.StatLevels[EquipStatId.FireResist] = GetByte(reader, "fire_resist");
        result.StatLevels[EquipStatId.IceResist] = GetByte(reader, "ice_resist");
        result.StatLevels[EquipStatId.ThunderResist] = GetByte(reader, "thunder_resist");
        result.StatLevels[EquipStatId.HolyResist] = GetByte(reader, "holy_resist");
        result.StatLevels[EquipStatId.DarkResist] = GetByte(reader, "dark_resist");

        return result;
    }
}
