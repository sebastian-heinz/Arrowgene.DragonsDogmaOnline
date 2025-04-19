using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string SqlDeleteEquippedAbility =
        "DELETE FROM \"ddon_equipped_ability\" WHERE \"character_common_id\"=@character_common_id AND \"equipped_to_job\"=@equipped_to_job AND \"slot_no\"=@slot_no;";

    private const string SqlDeleteEquippedAbilities =
        "DELETE FROM \"ddon_equipped_ability\" WHERE \"character_common_id\"=@character_common_id AND \"equipped_to_job\"=@equipped_to_job;";

    protected static readonly string[] EquippedAbilityFields = new[]
    {
        "character_common_id", "equipped_to_job", "job", "slot_no", "ability_id"
    };

    private static readonly string SqlUpdateEquippedAbility =
        $"UPDATE \"ddon_equipped_ability\" SET {BuildQueryUpdate(EquippedAbilityFields)} WHERE \"character_common_id\"=@old_character_common_id AND \"equipped_to_job\"=@old_equipped_to_job AND \"slot_no\"=@old_slot_no;";

    private static readonly string SqlSelectEquippedAbilities =
        $"SELECT {BuildQueryField(EquippedAbilityFields)} FROM \"ddon_equipped_ability\" WHERE \"character_common_id\"=@character_common_id ORDER BY equipped_to_job, slot_no;";

    protected static readonly string[] PresetAbilityFields = new[]
    {
        "character_id", "preset_no", "preset_name",
        "ability_1", "ability_2", "ability_3", "ability_4", "ability_5",
        "ability_6", "ability_7", "ability_8", "ability_9", "ability_10"
    };

    private static readonly string SqlUpdateAbilityPreset =
        $"UPDATE \"ddon_preset_ability\" SET {BuildQueryUpdate(PresetAbilityFields)} WHERE \"character_id\"=@character_id AND \"preset_no\"=@preset_no;";

    private static readonly string SqlSelectAbilityPresets =
        $"SELECT {BuildQueryField(PresetAbilityFields)} FROM \"ddon_preset_ability\" WHERE \"character_id\"=@character_id ORDER BY preset_no;";

    private readonly string SqlInsertEquippedAbility =
        $"INSERT INTO \"ddon_equipped_ability\" ({BuildQueryField(EquippedAbilityFields)}) VALUES ({BuildQueryInsert(EquippedAbilityFields)});";

    private readonly string SqlInsertIfNotExistsAbilityPreset =
        $"INSERT INTO \"ddon_preset_ability\" ({BuildQueryField(PresetAbilityFields)}) SELECT {BuildQueryInsert(PresetAbilityFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_preset_ability\" WHERE \"character_id\"=@character_id AND \"preset_no\"=@preset_no);";

    private readonly string SqlInsertIfNotExistsEquippedAbility =
        $"INSERT INTO \"ddon_equipped_ability\" ({BuildQueryField(EquippedAbilityFields)}) SELECT {BuildQueryInsert(EquippedAbilityFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_equipped_ability\" WHERE \"character_common_id\"=@character_common_id AND \"equipped_to_job\"=@equipped_to_job AND \"slot_no\"=@slot_no);";

    public bool InsertIfNotExistsEquippedAbility(uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertIfNotExistsEquippedAbility(connection, commonId, equippedToJob, slotNo, ability);
    }

    public bool InsertIfNotExistsEquippedAbility(DbConnection connection, uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
    {
        return ExecuteNonQuery(connection, SqlInsertIfNotExistsEquippedAbility, command => { AddParameter(command, commonId, equippedToJob, slotNo, ability); }) == 1;
    }

    public override bool InsertEquippedAbility(uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertEquippedAbility(connection, commonId, equippedToJob, slotNo, ability);
    }

    public bool InsertEquippedAbility(DbConnection connection, uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
    {
        return ExecuteNonQuery(connection, SqlInsertEquippedAbility, command => { AddParameter(command, commonId, equippedToJob, slotNo, ability); }) == 1;
    }

    public override bool ReplaceEquippedAbility(uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
    {
        using DbConnection connection = OpenNewConnection();
        return ReplaceEquippedAbility(connection, commonId, equippedToJob, slotNo, ability);
    }

    public bool ReplaceEquippedAbility(DbConnection connection, uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
    {
        Logger.Debug("Inserting equipped ability.");
        if (!InsertIfNotExistsEquippedAbility(connection, commonId, equippedToJob, slotNo, ability))
        {
            Logger.Debug("Equipped ability already exists, replacing.");
            return UpdateEquippedAbility(connection, commonId, equippedToJob, slotNo, equippedToJob, slotNo, ability);
        }

        return true;
    }

    public override bool ReplaceEquippedAbilities(uint commonId, JobId equippedToJob, List<Ability> abilities)
    {
        return ExecuteInTransaction(connection =>
        {
            // Remove previously equipped abilities
            DeleteEquippedAbilities(connection, commonId, equippedToJob);
            // Insert new ones
            for (byte i = 0; i < abilities.Count; i++)
            {
                Ability ability = abilities[i];
                if (ability == null) continue;
                byte slotNo = (byte)(i + 1);
                InsertEquippedAbility(connection, commonId, equippedToJob, slotNo, ability);
            }
        });
    }

    public override bool UpdateEquippedAbility(uint commonId, JobId oldEquippedToJob, byte oldSlotNo, JobId equippedToJob, byte slotNo, Ability updatedAbility)
    {
        using DbConnection connection = OpenNewConnection();
        return UpdateEquippedAbility(connection, commonId, oldEquippedToJob, oldSlotNo, equippedToJob, slotNo, updatedAbility);
    }

    public bool UpdateEquippedAbility(DbConnection connection, uint commonId, JobId oldEquippedToJob, byte oldSlotNo, JobId equippedToJob, byte slotNo, Ability updatedAbility)
    {
        return ExecuteNonQuery(connection, SqlUpdateEquippedAbility, command =>
        {
            AddParameter(command, commonId, equippedToJob, slotNo, updatedAbility);
            AddParameter(command, "@old_character_common_id", commonId);
            AddParameter(command, "@old_equipped_to_job", (byte)oldEquippedToJob);
            AddParameter(command, "@old_slot_no", oldSlotNo);
        }) == 1;
    }

    public override bool DeleteEquippedAbility(uint commonId, JobId equippedToJob, byte slotNo)
    {
        using DbConnection connection = OpenNewConnection();
        return DeleteEquippedAbility(connection, commonId, equippedToJob, slotNo);
    }

    public bool DeleteEquippedAbility(DbConnection connection, uint commonId, JobId equippedToJob, byte slotNo)
    {
        return ExecuteNonQuery(connection, SqlDeleteEquippedAbility, command =>
        {
            AddParameter(command, "@character_common_id", commonId);
            AddParameter(command, "@equipped_to_job", (byte)equippedToJob);
            AddParameter(command, "@slot_no", slotNo);
        }) == 1;
    }

    public override bool DeleteEquippedAbilities(uint commonId, JobId equippedToJob)
    {
        using DbConnection connection = OpenNewConnection();
        return DeleteEquippedAbilities(connection, commonId, equippedToJob);
    }

    public bool DeleteEquippedAbilities(DbConnection connection, uint commonId, JobId equippedToJob)
    {
        return ExecuteNonQuery(connection, SqlDeleteEquippedAbilities, command =>
        {
            AddParameter(command, "@character_common_id", commonId);
            AddParameter(command, "@equipped_to_job", (byte)equippedToJob);
        }) == 1;
    }

    private void AddParameter(DbCommand command, uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
    {
        AddParameter(command, "character_common_id", commonId);
        AddParameter(command, "equipped_to_job", (byte)equippedToJob);
        AddParameter(command, "job", (byte)ability.Job);
        AddParameter(command, "slot_no", slotNo);
        AddParameter(command, "ability_id", ability.AbilityId);
        AddParameter(command, "ability_lv", ability.AbilityLv);
    }

    public bool InsertIfNotExistsAbilityPreset(DbConnection connection, uint characterId, CDataPresetAbilityParam preset)
    {
        return ExecuteNonQuery(connection, SqlInsertIfNotExistsAbilityPreset, command => { AddParameter(command, characterId, preset); }) == 1;
    }

    public override bool ReplaceAbilityPreset(uint characterId, CDataPresetAbilityParam preset)
    {
        using DbConnection connection = OpenNewConnection();
        return ReplaceAbilityPreset(connection, characterId, preset);
    }

    public bool ReplaceAbilityPreset(DbConnection connection, uint characterId, CDataPresetAbilityParam preset)
    {
        if (!InsertIfNotExistsAbilityPreset(connection, characterId, preset)) return UpdateAbilityPreset(connection, characterId, preset);

        return true;
    }

    public override bool UpdateAbilityPreset(uint characterId, CDataPresetAbilityParam preset)
    {
        using DbConnection connection = OpenNewConnection();
        return UpdateAbilityPreset(connection, characterId, preset);
    }

    public bool UpdateAbilityPreset(DbConnection connection, uint characterId, CDataPresetAbilityParam preset)
    {
        return ExecuteNonQuery(connection, SqlUpdateAbilityPreset, command => { AddParameter(command, characterId, preset); }) == 1;
    }

    private void AddParameter(DbCommand command, uint characterId, CDataPresetAbilityParam preset)
    {
        AddParameter(command, "character_id", characterId);
        AddParameter(command, "preset_no", preset.PresetNo);
        AddParameter(command, "preset_name", preset.PresetName);
        for (int i = 0; i < 10; i++)
        {
            string key = $"ability_{i + 1}";
            if (i < preset.AbilityList.Count)
                AddParameter(command, key, preset.AbilityList[i].AcquirementNo);
            else
                AddParameter(command, key, -1);
        }
    }

    private CDataPresetAbilityParam ReadAbilityPreset(DbDataReader reader)
    {
        CDataPresetAbilityParam preset = new();

        preset.PresetNo = GetByte(reader, "preset_no");
        preset.PresetName = GetString(reader, "preset_name");
        for (int i = 0; i < 10; i++)
        {
            string key = $"ability_{i + 1}";
            int value = GetInt32(reader, key);
            if (value == -1) continue;
            preset.AbilityList.Add(new CDataSetAcquirementParam
            {
                AcquirementNo = (uint)value
            });
        }

        return preset;
    }
}
