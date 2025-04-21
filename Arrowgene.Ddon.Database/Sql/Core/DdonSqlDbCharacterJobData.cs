using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    protected static readonly string[] CDataCharacterJobDataFields =
    [
        "character_common_id", "job", "exp", "job_point", "lv", "atk", "def", "m_atk", "m_def", "strength", "down_power", "shake_power",
        "stun_power", "consitution", "guts", "fire_resist", "ice_resist", "thunder_resist", "holy_resist", "dark_resist", "spread_resist",
        "freeze_resist", "shock_resist", "absorb_resist", "dark_elm_resist", "poison_resist", "slow_resist", "sleep_resist", "stun_resist",
        "wet_resist", "oil_resist", "seal_resist", "curse_resist", "soft_resist", "stone_resist", "gold_resist", "fire_reduce_resist",
        "ice_reduce_resist", "thunder_reduce_resist", "holy_reduce_resist", "dark_reduce_resist", "atk_down_resist", "def_down_resist",
        "m_atk_down_resist", "m_def_down_resist"
    ];

    private static readonly string SqlUpdateCharacterJobData =
        $"UPDATE \"ddon_character_job_data\" SET {BuildQueryUpdate(CDataCharacterJobDataFields)} WHERE \"character_common_id\" = @character_common_id AND \"job\" = @job;";

    private static readonly string SqlSelectCharacterJobDataByCharacter =
        $"SELECT {BuildQueryField(CDataCharacterJobDataFields)} FROM \"ddon_character_job_data\" WHERE \"character_common_id\" = @character_common_id;";

    private static readonly string SqlInsertCharacterJobData =
        $"INSERT INTO \"ddon_character_job_data\" ({BuildQueryField(CDataCharacterJobDataFields)}) VALUES ({BuildQueryInsert(CDataCharacterJobDataFields)});";

    private static readonly string SqlUpsertCharacterJobData =
        $@"INSERT INTO ""ddon_character_job_data"" ({BuildQueryField(CDataCharacterJobDataFields)}) VALUES ({BuildQueryInsert(CDataCharacterJobDataFields)}) ON CONFLICT (""character_common_id"", ""job"") DO UPDATE SET {BuildQueryUpdate(CDataCharacterJobDataFields)};";

    public override bool ReplaceCharacterJobData(uint commonId, CDataCharacterJobData data, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
            ExecuteNonQuery(
                connection,
                SqlUpsertCharacterJobData,
                cmd => AddParameter(cmd, commonId, data)
            ) == 1
        );
    }

    public bool InsertCharacterJobData(DbConnection connection, uint commonId, CDataCharacterJobData updatedCharacterJobData, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn,
            connection => { return ExecuteNonQuery(connection, SqlInsertCharacterJobData, command => { AddParameter(command, commonId, updatedCharacterJobData); }) == 1; });
    }

    public override bool UpdateCharacterJobData(uint commonId, CDataCharacterJobData updatedCharacterJobData, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn,
            connection => { return ExecuteNonQuery(connection, SqlUpdateCharacterJobData, command => { AddParameter(command, commonId, updatedCharacterJobData); }) == 1; });
    }

    private CDataCharacterJobData ReadCharacterJobData(DbDataReader reader)
    {
        CDataCharacterJobData characterJobData = new();
        characterJobData.Job = (JobId)GetByte(reader, "job");
        characterJobData.Exp = GetUInt32(reader, "exp");
        characterJobData.JobPoint = GetUInt32(reader, "job_point");
        characterJobData.Lv = GetUInt32(reader, "lv");
        characterJobData.Atk = GetUInt16(reader, "atk");
        characterJobData.Def = GetUInt16(reader, "def");
        characterJobData.MAtk = GetUInt16(reader, "m_atk");
        characterJobData.MDef = GetUInt16(reader, "m_def");
        characterJobData.Strength = GetUInt16(reader, "strength");
        characterJobData.DownPower = GetUInt16(reader, "down_power");
        characterJobData.ShakePower = GetUInt16(reader, "shake_power");
        characterJobData.StunPower = GetUInt16(reader, "stun_power");
        characterJobData.Consitution = GetUInt16(reader, "consitution");
        characterJobData.Guts = GetUInt16(reader, "guts");
        characterJobData.FireResist = GetByte(reader, "fire_resist");
        characterJobData.IceResist = GetByte(reader, "ice_resist");
        characterJobData.ThunderResist = GetByte(reader, "thunder_resist");
        characterJobData.HolyResist = GetByte(reader, "holy_resist");
        characterJobData.DarkResist = GetByte(reader, "dark_resist");
        characterJobData.SpreadResist = GetByte(reader, "spread_resist");
        characterJobData.FreezeResist = GetByte(reader, "freeze_resist");
        characterJobData.ShockResist = GetByte(reader, "shock_resist");
        characterJobData.AbsorbResist = GetByte(reader, "absorb_resist");
        characterJobData.DarkElmResist = GetByte(reader, "dark_elm_resist");
        characterJobData.PoisonResist = GetByte(reader, "poison_resist");
        characterJobData.SlowResist = GetByte(reader, "slow_resist");
        characterJobData.SleepResist = GetByte(reader, "sleep_resist");
        characterJobData.StunResist = GetByte(reader, "stun_resist");
        characterJobData.WetResist = GetByte(reader, "wet_resist");
        characterJobData.OilResist = GetByte(reader, "oil_resist");
        characterJobData.SealResist = GetByte(reader, "seal_resist");
        characterJobData.CurseResist = GetByte(reader, "curse_resist");
        characterJobData.SoftResist = GetByte(reader, "soft_resist");
        characterJobData.StoneResist = GetByte(reader, "stone_resist");
        characterJobData.GoldResist = GetByte(reader, "gold_resist");
        characterJobData.FireReduceResist = GetByte(reader, "fire_reduce_resist");
        characterJobData.IceReduceResist = GetByte(reader, "ice_reduce_resist");
        characterJobData.ThunderReduceResist = GetByte(reader, "thunder_reduce_resist");
        characterJobData.HolyReduceResist = GetByte(reader, "holy_reduce_resist");
        characterJobData.DarkReduceResist = GetByte(reader, "dark_reduce_resist");
        characterJobData.AtkDownResist = GetByte(reader, "atk_down_resist");
        characterJobData.DefDownResist = GetByte(reader, "def_down_resist");
        characterJobData.MAtkDownResist = GetByte(reader, "m_atk_down_resist");
        characterJobData.MDefDownResist = GetByte(reader, "m_def_down_resist");
        return characterJobData;
    }

    private void AddParameter(DbCommand command, uint commonId, CDataCharacterJobData characterJobData)
    {
        AddParameter(command, "character_common_id", commonId);
        AddParameter(command, "job", (byte)characterJobData.Job);
        AddParameter(command, "exp", characterJobData.Exp);
        AddParameter(command, "job_point", characterJobData.JobPoint);
        AddParameter(command, "lv", characterJobData.Lv);
        AddParameter(command, "atk", characterJobData.Atk);
        AddParameter(command, "def", characterJobData.Def);
        AddParameter(command, "m_atk", characterJobData.MAtk);
        AddParameter(command, "m_def", characterJobData.MDef);
        AddParameter(command, "strength", characterJobData.Strength);
        AddParameter(command, "down_power", characterJobData.DownPower);
        AddParameter(command, "shake_power", characterJobData.ShakePower);
        AddParameter(command, "stun_power", characterJobData.StunPower);
        AddParameter(command, "consitution", characterJobData.Consitution);
        AddParameter(command, "guts", characterJobData.Guts);
        AddParameter(command, "fire_resist", characterJobData.FireResist);
        AddParameter(command, "ice_resist", characterJobData.IceResist);
        AddParameter(command, "thunder_resist", characterJobData.ThunderResist);
        AddParameter(command, "holy_resist", characterJobData.HolyResist);
        AddParameter(command, "dark_resist", characterJobData.DarkResist);
        AddParameter(command, "spread_resist", characterJobData.SpreadResist);
        AddParameter(command, "freeze_resist", characterJobData.FreezeResist);
        AddParameter(command, "shock_resist", characterJobData.ShockResist);
        AddParameter(command, "absorb_resist", characterJobData.AbsorbResist);
        AddParameter(command, "dark_elm_resist", characterJobData.DarkElmResist);
        AddParameter(command, "poison_resist", characterJobData.PoisonResist);
        AddParameter(command, "slow_resist", characterJobData.SlowResist);
        AddParameter(command, "sleep_resist", characterJobData.SleepResist);
        AddParameter(command, "stun_resist", characterJobData.StunResist);
        AddParameter(command, "wet_resist", characterJobData.WetResist);
        AddParameter(command, "oil_resist", characterJobData.OilResist);
        AddParameter(command, "seal_resist", characterJobData.SealResist);
        AddParameter(command, "curse_resist", characterJobData.CurseResist);
        AddParameter(command, "soft_resist", characterJobData.SoftResist);
        AddParameter(command, "stone_resist", characterJobData.StoneResist);
        AddParameter(command, "gold_resist", characterJobData.GoldResist);
        AddParameter(command, "fire_reduce_resist", characterJobData.FireReduceResist);
        AddParameter(command, "ice_reduce_resist", characterJobData.IceReduceResist);
        AddParameter(command, "thunder_reduce_resist", characterJobData.ThunderReduceResist);
        AddParameter(command, "holy_reduce_resist", characterJobData.HolyReduceResist);
        AddParameter(command, "dark_reduce_resist", characterJobData.DarkReduceResist);
        AddParameter(command, "atk_down_resist", characterJobData.AtkDownResist);
        AddParameter(command, "def_down_resist", characterJobData.DefDownResist);
        AddParameter(command, "m_atk_down_resist", characterJobData.MAtkDownResist);
        AddParameter(command, "m_def_down_resist", characterJobData.MDefDownResist);
    }
}
