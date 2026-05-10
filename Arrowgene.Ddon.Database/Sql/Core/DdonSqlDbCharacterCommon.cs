#nullable enable
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string SqlDeleteEditInfo = "DELETE FROM \"ddon_edit_info\" WHERE \"character_common_id\"=@character_common_id;";

    private const string SqlDeleteStatusInfo = "DELETE FROM \"ddon_status_info\" WHERE \"character_common_id\"=@character_common_id;";

    private static readonly string[] CharacterCommonFields = new[]
    {
        "job", "hide_equip_head", "hide_equip_lantern", "saved_online_status"
    };

    private static readonly string[] CDataEditInfoFields = new[]
    {
        "character_common_id", "sex", "voice", "voice_pitch", "personality", "speech_freq", "body_type", "hair", "beard", "makeup", "scar",
        "eye_preset_no", "nose_preset_no", "mouth_preset_no", "eyebrow_tex_no", "color_skin", "color_hair",
        "color_beard", "color_eyebrow", "color_r_eye", "color_l_eye", "color_makeup", "sokutobu", "hitai",
        "mimi_jyouge", "kannkaku", "mabisasi_jyouge", "hanakuchi_jyouge", "ago_saki_haba", "ago_zengo",
        "ago_saki_jyouge", "hitomi_ookisa", "me_ookisa", "me_kaiten", "mayu_kaiten", "mimi_ookisa", "mimi_muki",
        "elf_mimi", "miken_takasa", "miken_haba", "hohobone_ryou", "hohobone_jyouge", "hohoniku", "erahone_jyouge",
        "erahone_haba", "hana_jyouge", "hana_haba", "hana_takasa", "hana_kakudo", "kuchi_haba", "kuchi_atsusa",
        "eyebrow_uv_offset_x", "eyebrow_uv_offset_y", "wrinkle", "wrinkle_albedo_blend_rate",
        "wrinkle_detail_normal_power", "muscle_albedo_blend_rate", "muscle_detail_normal_power", "height",
        "head_size", "neck_offset", "neck_scale", "upper_body_scale_x", "belly_size", "teat_scale", "tekubi_size",
        "koshi_offset", "koshi_size", "ankle_offset", "fat", "muscle", "motion_filter"
    };

    // Im not convinced most of these fields have to be stored in DB
    private static readonly string[] CDataStatusInfoFields = new[]
    {
        "character_common_id", "revive_point", "hp", "white_hp"
    };

    private static readonly string[] CDataProfileFields = new[]
    {
        "character_common_id", "background_id", "title_uid", "title_index", "motion_id", "motion_frame_no", "comment"
    };

    private static readonly string SqlUpdateEditInfo =
        $"UPDATE \"ddon_edit_info\" SET {BuildQueryUpdate(CDataEditInfoFields)} WHERE \"character_common_id\" = @character_common_id;";

    private static readonly string SqlSelectEditInfo =
        $"SELECT {BuildQueryField(CDataEditInfoFields)} FROM \"ddon_edit_info\" WHERE \"character_common_id\" = @character_common_id;";

    private static readonly string SqlUpdateStatusInfo =
        $"UPDATE \"ddon_status_info\" SET {BuildQueryUpdate(CDataStatusInfoFields)} WHERE \"character_common_id\" = @character_common_id;";

    private static readonly string SqlSelectStatusInfo =
        $"SELECT {BuildQueryField(CDataStatusInfoFields)} FROM \"ddon_status_info\" WHERE \"character_common_id\" = @character_common_id;";

    private readonly string SqlInsertCharacterCommon =
        $"INSERT INTO \"ddon_character_common\" ({BuildQueryField(CharacterCommonFields)}) VALUES ({BuildQueryInsert(CharacterCommonFields)});";

    private readonly string SqlInsertEditInfo = $"INSERT INTO \"ddon_edit_info\" ({BuildQueryField(CDataEditInfoFields)}) VALUES ({BuildQueryInsert(CDataEditInfoFields)});";

    private readonly string SqlInsertStatusInfo =
        $"INSERT INTO \"ddon_status_info\" ({BuildQueryField(CDataStatusInfoFields)}) VALUES ({BuildQueryInsert(CDataStatusInfoFields)});";

    private readonly string SqlUpdateCharacterCommon =
        $"UPDATE \"ddon_character_common\" SET {BuildQueryUpdate(CharacterCommonFields)} WHERE \"character_common_id\" = @character_common_id;";

    private static readonly string SqlSelectCharacterProfile =
        $"SELECT {BuildQueryField(CDataProfileFields)} FROM \"ddon_character_profile\" WHERE \"character_common_id\" = @character_common_id;";

    private readonly string SqlInsertCharacterProfile =
        $"INSERT INTO \"ddon_character_profile\" ({BuildQueryField(CDataProfileFields)}) VALUES ({BuildQueryInsert(CDataProfileFields)});";

    private static readonly string SqlUpdateCharacterProfile =
        $"UPDATE \"ddon_character_profile\" SET {BuildQueryUpdate(CDataProfileFields)} WHERE \"character_common_id\" = @character_common_id;";

    // Local copies of field arrays from other partial files.
    // We cannot reference those static fields directly here because C# does not
    // guarantee static initializer order across partial class files, which causes
    // a type initializer exception at startup if this file initializes first.
    private static readonly string[] StorageItemFieldsLocal = new[]
    {
        "item_uid", "item_id", "safety", "color", "plus_value", "equip_points"
    };

    private static readonly string[] CrestFieldsLocal = new[]
    {
        "character_common_id", "item_uid", "slot", "crest_id", "crest_amount"
    };

    private static readonly string[] EquipmentLimitBreakFieldsLocal = new[]
    {
        "item_uid", "effect_id", "unk1", "effect_type", "unk0"
    };

    private static readonly string SqlSelectStorageItemsByUIdsBase =
        $"SELECT {BuildQueryField(StorageItemFieldsLocal)} FROM \"ddon_storage_item\" WHERE \"item_uid\"";

    private static readonly string SqlSelectCrestDataByCharacter =
        $"SELECT {BuildQueryField(CrestFieldsLocal)} FROM \"ddon_crests\" WHERE \"character_common_id\" = @character_common_id;";

    private static readonly string SqlSelectEquipmentLimitBreakByCharacter =
        $"SELECT {BuildQueryField(EquipmentLimitBreakFieldsLocal)} FROM \"ddon_equipment_limit_break\" WHERE \"character_id\" = @character_id;";


    public override bool UpdateCharacterCommonBaseInfo(CharacterCommon common, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            int commonUpdateRowsAffected = ExecuteNonQuery(connection, SqlUpdateCharacterCommon, command => { AddParameter(command, common); });
            return commonUpdateRowsAffected > NoRowsAffected;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public override bool UpdateEditInfo(CharacterCommon common)
    {
        using DbConnection connection = OpenNewConnection();
        return UpdateEditInfo(connection, common);
    }

    public bool UpdateEditInfo(DbConnection conn, CharacterCommon common)
    {
        int commonUpdateRowsAffected = ExecuteNonQuery(conn, SqlUpdateEditInfo, command => { AddParameter(command, common); });
        return commonUpdateRowsAffected > NoRowsAffected;
    }

    public override bool UpdateStatusInfo(CharacterCommon common)
    {
        using DbConnection connection = OpenNewConnection();
        return UpdateStatusInfo(connection, common);
    }

    public bool UpdateStatusInfo(DbConnection conn, CharacterCommon common)
    {
        int commonUpdateRowsAffected = ExecuteNonQuery(conn, SqlUpdateStatusInfo, command => { AddParameter(command, common); });
        return commonUpdateRowsAffected > NoRowsAffected;
    }

    public override bool UpdateCharacterProfile(CharacterCommon characterCommon, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection,
                SqlUpdateCharacterProfile,
                command => { AddParameter(command, characterCommon); }
            ) == 1;
        });
    }

    private void QueryCharacterCommonData(DbConnection conn, CharacterCommon common, uint characterId)
    {
        // Job data
        ExecuteReader(conn, SqlSelectCharacterJobDataByCharacter,
            command => { AddParameter(command, "@character_common_id", common.CommonId); },
            reader =>
            {
                while (reader.Read()) common.CharacterJobDataList.Add(ReadCharacterJobData(reader));
            });

        // Materialize equip slot records before running batch item/crest/limit-break queries.
        var equipSlots = new List<(string? UId, JobId Job, EquipType EquipType, byte EquipSlot)>();
        ExecuteReader(conn, SqlSelectEquipItemByCharacter,
            command => { AddParameter(command, "@character_common_id", common.CommonId); },
            reader =>
            {
                while (reader.Read())
                    equipSlots.Add((
                        GetStringNullable(reader, "item_uid"),
                        (JobId)GetByte(reader, "job"),
                        (EquipType)GetByte(reader, "equip_type"),
                        GetByte(reader, "equip_slot")
                    ));
            });

        if (equipSlots.Count > 0)
        {
            string[] equipUids = equipSlots.Where(e => e.UId != null).Select(e => e.UId!).Distinct().ToArray();

            var (equipUidsClause, bindEquipUids) = PrepareArrayParameter("uids", equipUids);

            var equipItemMap = new Dictionary<string, Item>();
            using DbConnection equipItemConn = OpenNewConnection();
            ExecuteReader(equipItemConn, $"{SqlSelectStorageItemsByUIdsBase} {equipUidsClause}",
                bindEquipUids,
                reader =>
                {
                    while (reader.Read())
                    {
                        var item = new Item
                        {
                            UId           = GetString(reader, "item_uid"),
                            ItemId        = GetUInt32(reader, "item_id"),
                            SafetySetting = GetByte(reader, "safety"),
                            Color         = GetByte(reader, "color"),
                            PlusValue     = GetByte(reader, "plus_value"),
                            EquipPoints   = GetUInt32(reader, "equip_points")
                        };
                        equipItemMap[item.UId] = item;
                    }
                });

            var crestMap = new Dictionary<string, List<CDataEquipElementParam>>();
            using DbConnection crestConn = OpenNewConnection();
            ExecuteReader(crestConn, SqlSelectCrestDataByCharacter,
                command => { AddParameter(command, "@character_common_id", common.CommonId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        string uid = GetString(reader, "item_uid");
                        if (!crestMap.TryGetValue(uid, out var list))
                            crestMap[uid] = list = new List<CDataEquipElementParam>();
                        list.Add(ReadCrestData(reader).ToCDataEquipElementParam());
                    }
                });

            var limitBreakMap = new Dictionary<string, List<CDataAddStatusParam>>();
            using DbConnection limitBreakConn = OpenNewConnection();
            ExecuteReader(limitBreakConn, SqlSelectEquipmentLimitBreakByCharacter,
                command => { AddParameter(command, "@character_id", characterId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        string uid = GetString(reader, "item_uid");
                        if (!limitBreakMap.TryGetValue(uid, out var list))
                            limitBreakMap[uid] = list = new List<CDataAddStatusParam>();
                        list.Add(new CDataAddStatusParam
                        {
                            EnhanceId   = GetUInt16(reader, "effect_id"),
                            Unk1        = GetUInt16(reader, "unk1"),
                            EnhanceType = (EquipEnhanceType)GetByte(reader, "effect_type"),
                            Unk0        = GetByte(reader, "unk0")
                        });
                    }
                });

            foreach (var (uid, job, equipType, equipSlot) in equipSlots)
            {
                if (string.IsNullOrEmpty(uid))
                {
                    common.EquipmentTemplate.SetEquipItem(null, job, equipType, equipSlot);
                    continue;
                }
                if (!equipItemMap.TryGetValue(uid, out Item? item))
                {
                    // No storage row found - leave slot as null, same as vanilla silent failure
                    continue;
                }
                if (crestMap.TryGetValue(uid, out var crests))
                    item.EquipElementParamList.AddRange(crests);
                item.AddStatusParamList = limitBreakMap.GetValueOrDefault(uid, new List<CDataAddStatusParam>());
                common.EquipmentTemplate.SetEquipItem(item, job, equipType, equipSlot);
            }
        }

        var jobItemSlots = new List<(string? UId, JobId Job, byte EquipSlot)>();
        ExecuteReader(conn, SqlSelectEquipJobItemsByCharacter,
            command => { AddParameter(command, "@character_common_id", common.CommonId); },
            reader =>
            {
                while (reader.Read())
                    jobItemSlots.Add((
                        GetStringNullable(reader, "item_uid"),
                        (JobId)GetByte(reader, "job"),
                        GetByte(reader, "equip_slot")
                    ));
            });

        if (jobItemSlots.Count > 0)
        {
            string[] jobUids = jobItemSlots.Where(e => e.UId != null).Select(e => e.UId!).Distinct().ToArray();
            var (jobUidsClause, bindJobUids) = PrepareArrayParameter("uids", jobUids);

            var jobItemMap = new Dictionary<string, Item>();
            using DbConnection jobItemConn = OpenNewConnection();
            ExecuteReader(jobItemConn, $"{SqlSelectStorageItemsByUIdsBase} {jobUidsClause}",
                command => { bindJobUids(command); },
                reader =>
                {
                    while (reader.Read())
                    {
                        var item = new Item
                        {
                            UId           = GetString(reader, "item_uid"),
                            ItemId        = GetUInt32(reader, "item_id"),
                            SafetySetting = GetByte(reader, "safety"),
                            Color         = GetByte(reader, "color"),
                            PlusValue     = GetByte(reader, "plus_value"),
                            EquipPoints   = GetUInt32(reader, "equip_points")
                        };
                        jobItemMap[item.UId] = item;
                    }
                });

            foreach (var (uid, job, equipSlot) in jobItemSlots)
            {
                if (string.IsNullOrEmpty(uid))
                {
                    common.EquipmentTemplate.SetJobItem(null, job, equipSlot);
                    continue;
                }
                if (!jobItemMap.TryGetValue(uid, out Item? item))
                {
                    // No storage row found - leave slot as null, same as vanilla silent failure
                    continue;
                }
                common.EquipmentTemplate.SetJobItem(item, job, equipSlot);
            }
        }

        ExecuteReader(conn, SqlSelectNormalSkillParam,
            command => { AddParameter(command, "@character_common_id", common.CommonId); },
            reader =>
            {
                while (reader.Read()) common.LearnedNormalSkills.Add(ReadNormalSkillParam(reader));
            });

        ExecuteReader(conn, SqlSelectLearnedCustomSkills,
            command => { AddParameter(command, "@character_common_id", common.CommonId); },
            reader =>
            {
                while (reader.Read()) common.LearnedCustomSkills.Add(ReadLearnedCustomSkill(reader));
            });

        ExecuteReader(conn, SqlSelectEquippedCustomSkills,
            command => { AddParameter(command, "@character_common_id", common.CommonId); },
            reader =>
            {
                while (reader.Read())
                {
                    uint skillId = GetUInt32(reader, "skill_id");
                    JobId job    = (JobId)GetByte(reader, "job");
                    byte slotNo  = GetByte(reader, "slot_no");

                    CustomSkill? skill = common.LearnedCustomSkills
                        .Where(x => x.Job == job && x.SkillId == skillId)
                        .SingleOrDefault();

                    if (skill is null)
                    {
                        Logger.Error($"Invalid equipped custom skill {skillId} in slot {slotNo} for character {common.CommonId}; skipping.");
                        continue;
                    }

                    common.EquippedCustomSkillsDictionary[job][slotNo - 1] = skill;
                }
            });

        ExecuteReader(conn, SqlSelectLearnedAbilities,
            command => { AddParameter(command, "@character_common_id", common.CommonId); },
            reader =>
            {
                while (reader.Read()) common.LearnedAbilities.Add(ReadLearnedAbility(reader));
            });

        ExecuteReader(conn, SqlSelectEquippedAbilities,
            command => { AddParameter(command, "@character_common_id", common.CommonId); },
            reader =>
            {
                while (reader.Read())
                {
                    AbilityId abilityId  = (AbilityId)GetUInt32(reader, "ability_id");
                    JobId job            = (JobId)GetByte(reader, "job");
                    JobId equippedToJob  = (JobId)GetByte(reader, "equipped_to_job");
                    byte slotNo          = GetByte(reader, "slot_no");

                    Ability? aug = common.LearnedAbilities
                        .Where(x => x.Job == job && x.AbilityId == abilityId)
                        .SingleOrDefault();

                    if (aug is null)
                    {
                        Logger.Error($"Invalid equipped ability {abilityId} in slot {slotNo} for character {common.CommonId}; skipping.");
                        continue;
                    }

                    common.EquippedAbilitiesDictionary[equippedToJob][slotNo - 1] = aug;
                }
            });

        common.OrbRelease = SelectOrbReleaseElementFromDragonForceAugmentation(common.CommonId, conn);
    }

    private void StoreCharacterCommonData(DbConnection conn, CharacterCommon common)
    {
        foreach (CDataCharacterJobData characterJobData in common.CharacterJobDataList)
            ReplaceCharacterJobData(common.CommonId, characterJobData, conn);

        foreach (CDataNormalSkillParam normalSkillParam in common.LearnedNormalSkills)
            ReplaceNormalSkillParam(conn, common.CommonId, normalSkillParam);

        foreach (CustomSkill learnedSkills in common.LearnedCustomSkills)
            ExecuteNonQuery(conn, SqlInsertLearnedCustomSkill, command => { AddParameter(command, common.CommonId, learnedSkills); });

        foreach (KeyValuePair<JobId, List<CustomSkill>> jobAndSkills in common.EquippedCustomSkillsDictionary)
            for (int i = 0; i < jobAndSkills.Value.Count; i++)
            {
                CustomSkill? skill = jobAndSkills.Value[i];
                byte slotNo = (byte)(i + 1);
                if (skill != null) ReplaceEquippedCustomSkill(conn, common.CommonId, slotNo, skill);
            }

        foreach (Ability ability in common.LearnedAbilities)
            ExecuteNonQuery(conn, SqlInsertLearnedAbility, command => { AddParameter(command, common.CommonId, ability); });

        foreach (KeyValuePair<JobId, List<Ability>> jobAndAugs in common.EquippedAbilitiesDictionary)
        {
            JobId equippedToJob = jobAndAugs.Key;
            for (int i = 0; i < jobAndAugs.Value.Count; i++)
            {
                Ability? ability = jobAndAugs.Value[i];
                byte slotNo = (byte)(i + 1);
                if (ability != null) ReplaceEquippedAbility(conn, common.CommonId, equippedToJob, slotNo, ability);
            }
        }
    }

    private void ReadAllCharacterCommonData(DbDataReader reader, CharacterCommon common)
    {
        common.CommonId = GetUInt32(reader, "character_common_id");
        common.Job = (JobId)GetByte(reader, "job");
        common.HideEquipHead = GetBoolean(reader, "hide_equip_head");
        common.HideEquipLantern = GetBoolean(reader, "hide_equip_lantern");
        common.JewelrySlotNum = 0;
        common.SavedOnlineStatus = (OnlineStatus)GetByte(reader, "saved_online_status");

        common.EditInfo.Sex = GetByte(reader, "sex");
        common.EditInfo.Voice = GetByte(reader, "voice");
        common.EditInfo.VoicePitch = GetUInt16(reader, "voice_pitch");
        common.EditInfo.Personality = (PawnPersonality)GetByte(reader, "personality");
        common.EditInfo.SpeechFreq = GetByte(reader, "speech_freq");
        common.EditInfo.BodyType = GetByte(reader, "body_type");
        common.EditInfo.Hair = GetByte(reader, "hair");
        common.EditInfo.Beard = GetByte(reader, "beard");
        common.EditInfo.Makeup = GetByte(reader, "makeup");
        common.EditInfo.Scar = GetByte(reader, "scar");
        common.EditInfo.EyePresetNo = GetByte(reader, "eye_preset_no");
        common.EditInfo.NosePresetNo = GetByte(reader, "nose_preset_no");
        common.EditInfo.MouthPresetNo = GetByte(reader, "mouth_preset_no");
        common.EditInfo.EyebrowTexNo = GetByte(reader, "eyebrow_tex_no");
        common.EditInfo.ColorSkin = GetByte(reader, "color_skin");
        common.EditInfo.ColorHair = GetByte(reader, "color_hair");
        common.EditInfo.ColorBeard = GetByte(reader, "color_beard");
        common.EditInfo.ColorEyebrow = GetByte(reader, "color_eyebrow");
        common.EditInfo.ColorREye = GetByte(reader, "color_r_eye");
        common.EditInfo.ColorLEye = GetByte(reader, "color_l_eye");
        common.EditInfo.ColorMakeup = GetByte(reader, "color_makeup");
        common.EditInfo.Sokutobu = GetUInt16(reader, "sokutobu");
        common.EditInfo.Hitai = GetUInt16(reader, "hitai");
        common.EditInfo.MimiJyouge = GetUInt16(reader, "mimi_jyouge");
        common.EditInfo.Kannkaku = GetUInt16(reader, "kannkaku");
        common.EditInfo.MabisasiJyouge = GetUInt16(reader, "mabisasi_jyouge");
        common.EditInfo.HanakuchiJyouge = GetUInt16(reader, "hanakuchi_jyouge");
        common.EditInfo.AgoSakiHaba = GetUInt16(reader, "ago_saki_haba");
        common.EditInfo.AgoZengo = GetUInt16(reader, "ago_zengo");
        common.EditInfo.AgoSakiJyouge = GetUInt16(reader, "ago_saki_jyouge");
        common.EditInfo.HitomiOokisa = GetUInt16(reader, "hitomi_ookisa");
        common.EditInfo.MeOokisa = GetUInt16(reader, "me_ookisa");
        common.EditInfo.MeKaiten = GetUInt16(reader, "me_kaiten");
        common.EditInfo.MayuKaiten = GetUInt16(reader, "mayu_kaiten");
        common.EditInfo.MimiOokisa = GetUInt16(reader, "mimi_ookisa");
        common.EditInfo.MimiMuki = GetUInt16(reader, "mimi_muki");
        common.EditInfo.ElfMimi = GetUInt16(reader, "elf_mimi");
        common.EditInfo.MikenTakasa = GetUInt16(reader, "miken_takasa");
        common.EditInfo.MikenHaba = GetUInt16(reader, "miken_haba");
        common.EditInfo.HohoboneRyou = GetUInt16(reader, "hohobone_ryou");
        common.EditInfo.HohoboneJyouge = GetUInt16(reader, "hohobone_jyouge");
        common.EditInfo.Hohoniku = GetUInt16(reader, "hohoniku");
        common.EditInfo.ErahoneJyouge = GetUInt16(reader, "erahone_jyouge");
        common.EditInfo.ErahoneHaba = GetUInt16(reader, "erahone_haba");
        common.EditInfo.HanaJyouge = GetUInt16(reader, "hana_jyouge");
        common.EditInfo.HanaHaba = GetUInt16(reader, "hana_haba");
        common.EditInfo.HanaTakasa = GetUInt16(reader, "hana_takasa");
        common.EditInfo.HanaKakudo = GetUInt16(reader, "hana_kakudo");
        common.EditInfo.KuchiHaba = GetUInt16(reader, "kuchi_haba");
        common.EditInfo.KuchiAtsusa = GetUInt16(reader, "kuchi_atsusa");
        common.EditInfo.EyebrowUVOffsetX = GetUInt16(reader, "eyebrow_uv_offset_x");
        common.EditInfo.EyebrowUVOffsetY = GetUInt16(reader, "eyebrow_uv_offset_y");
        common.EditInfo.Wrinkle = GetUInt16(reader, "wrinkle");
        common.EditInfo.WrinkleAlbedoBlendRate = GetUInt16(reader, "wrinkle_albedo_blend_rate");
        common.EditInfo.WrinkleDetailNormalPower = GetUInt16(reader, "wrinkle_detail_normal_power");
        common.EditInfo.MuscleAlbedoBlendRate = GetUInt16(reader, "muscle_albedo_blend_rate");
        common.EditInfo.MuscleDetailNormalPower = GetUInt16(reader, "muscle_detail_normal_power");
        common.EditInfo.Height = GetUInt16(reader, "height");
        common.EditInfo.HeadSize = GetUInt16(reader, "head_size");
        common.EditInfo.NeckOffset = GetUInt16(reader, "neck_offset");
        common.EditInfo.NeckScale = GetUInt16(reader, "neck_scale");
        common.EditInfo.UpperBodyScaleX = GetUInt16(reader, "upper_body_scale_x");
        common.EditInfo.BellySize = GetUInt16(reader, "belly_size");
        common.EditInfo.TeatScale = GetUInt16(reader, "teat_scale");
        common.EditInfo.TekubiSize = GetUInt16(reader, "tekubi_size");
        common.EditInfo.KoshiOffset = GetUInt16(reader, "koshi_offset");
        common.EditInfo.KoshiSize = GetUInt16(reader, "koshi_size");
        common.EditInfo.AnkleOffset = GetUInt16(reader, "ankle_offset");
        common.EditInfo.Fat = GetUInt16(reader, "fat");
        common.EditInfo.Muscle = GetUInt16(reader, "muscle");
        common.EditInfo.MotionFilter = GetUInt16(reader, "motion_filter");

        // CDataStatusInfoFields
        common.StatusInfo.RevivePoint = GetByte(reader, "revive_point");
        common.StatusInfo.HP = GetUInt32(reader, "hp");
        common.StatusInfo.WhiteHP = GetUInt32(reader, "white_hp");

        common.CharacterProfile.BackgroundId = GetByte(reader, "background_id");
        common.CharacterProfile.Title.UId = GetUInt32(reader, "title_uid");
        common.CharacterProfile.Title.Index = GetUInt32(reader, "title_index");
        common.CharacterProfile.MotionId = GetUInt16(reader, "motion_id");
        common.CharacterProfile.MotionFrameNo = GetUInt32(reader, "motion_frame_no");
        common.CharacterProfile.Comment = GetString(reader, "comment");

        if (common.StatusInfo.HP == 0 || common.StatusInfo.WhiteHP == 0)
        {
            // TODO: Figure out why this is happening
            Logger.Error($"Unexpected: Character CommonId={common.CommonId} has a health value of 0 stored in the DB");
            common.StatusInfo.HP = 760;
            common.StatusInfo.WhiteHP = 760;
        }
    }

    private void AddParameter(DbCommand command, CharacterCommon common)
    {
        // CharacterCommonFields
        AddParameter(command, "@character_common_id", common.CommonId);
        AddParameter(command, "@job", (byte)common.Job);
        AddParameter(command, "@hide_equip_head", common.HideEquipHead);
        AddParameter(command, "@hide_equip_lantern", common.HideEquipLantern);
        AddParameter(command, "@saved_online_status", (byte)common.SavedOnlineStatus);
        // CDataEditInfoFields
        AddParameter(command, "@sex", common.EditInfo.Sex);
        AddParameter(command, "@voice", common.EditInfo.Voice);
        AddParameter(command, "@voice_pitch", common.EditInfo.VoicePitch);
        AddParameter(command, "@personality", (byte)common.EditInfo.Personality);
        AddParameter(command, "@speech_freq", common.EditInfo.SpeechFreq);
        AddParameter(command, "@body_type", common.EditInfo.BodyType);
        AddParameter(command, "@hair", common.EditInfo.Hair);
        AddParameter(command, "@beard", common.EditInfo.Beard);
        AddParameter(command, "@makeup", common.EditInfo.Makeup);
        AddParameter(command, "@scar", common.EditInfo.Scar);
        AddParameter(command, "@eye_preset_no", common.EditInfo.EyePresetNo);
        AddParameter(command, "@nose_preset_no", common.EditInfo.NosePresetNo);
        AddParameter(command, "@mouth_preset_no", common.EditInfo.MouthPresetNo);
        AddParameter(command, "@eyebrow_tex_no", common.EditInfo.EyebrowTexNo);
        AddParameter(command, "@color_skin", common.EditInfo.ColorSkin);
        AddParameter(command, "@color_hair", common.EditInfo.ColorHair);
        AddParameter(command, "@color_beard", common.EditInfo.ColorBeard);
        AddParameter(command, "@color_eyebrow", common.EditInfo.ColorEyebrow);
        AddParameter(command, "@color_r_eye", common.EditInfo.ColorREye);
        AddParameter(command, "@color_l_eye", common.EditInfo.ColorLEye);
        AddParameter(command, "@color_makeup", common.EditInfo.ColorMakeup);
        AddParameter(command, "@sokutobu", common.EditInfo.Sokutobu);
        AddParameter(command, "@hitai", common.EditInfo.Hitai);
        AddParameter(command, "@mimi_jyouge", common.EditInfo.MimiJyouge);
        AddParameter(command, "@kannkaku", common.EditInfo.Kannkaku);
        AddParameter(command, "@mabisasi_jyouge", common.EditInfo.MabisasiJyouge);
        AddParameter(command, "@hanakuchi_jyouge", common.EditInfo.HanakuchiJyouge);
        AddParameter(command, "@ago_saki_haba", common.EditInfo.AgoSakiHaba);
        AddParameter(command, "@ago_zengo", common.EditInfo.AgoZengo);
        AddParameter(command, "@ago_saki_jyouge", common.EditInfo.AgoSakiJyouge);
        AddParameter(command, "@hitomi_ookisa", common.EditInfo.HitomiOokisa);
        AddParameter(command, "@me_ookisa", common.EditInfo.MeOokisa);
        AddParameter(command, "@me_kaiten", common.EditInfo.MeKaiten);
        AddParameter(command, "@mayu_kaiten", common.EditInfo.MayuKaiten);
        AddParameter(command, "@mimi_ookisa", common.EditInfo.MimiOokisa);
        AddParameter(command, "@mimi_muki", common.EditInfo.MimiMuki);
        AddParameter(command, "@elf_mimi", common.EditInfo.ElfMimi);
        AddParameter(command, "@miken_takasa", common.EditInfo.MikenTakasa);
        AddParameter(command, "@miken_haba", common.EditInfo.MikenHaba);
        AddParameter(command, "@hohobone_ryou", common.EditInfo.HohoboneRyou);
        AddParameter(command, "@hohobone_jyouge", common.EditInfo.HohoboneJyouge);
        AddParameter(command, "@hohoniku", common.EditInfo.Hohoniku);
        AddParameter(command, "@erahone_jyouge", common.EditInfo.ErahoneJyouge);
        AddParameter(command, "@erahone_haba", common.EditInfo.ErahoneHaba);
        AddParameter(command, "@hana_jyouge", common.EditInfo.HanaJyouge);
        AddParameter(command, "@hana_haba", common.EditInfo.HanaHaba);
        AddParameter(command, "@hana_takasa", common.EditInfo.HanaTakasa);
        AddParameter(command, "@hana_kakudo", common.EditInfo.HanaKakudo);
        AddParameter(command, "@kuchi_haba", common.EditInfo.KuchiHaba);
        AddParameter(command, "@kuchi_atsusa", common.EditInfo.KuchiAtsusa);
        AddParameter(command, "@eyebrow_uv_offset_x", common.EditInfo.EyebrowUVOffsetX);
        AddParameter(command, "@eyebrow_uv_offset_y", common.EditInfo.EyebrowUVOffsetY);
        AddParameter(command, "@wrinkle", common.EditInfo.Wrinkle);
        AddParameter(command, "@wrinkle_albedo_blend_rate", common.EditInfo.WrinkleAlbedoBlendRate);
        AddParameter(command, "@wrinkle_detail_normal_power", common.EditInfo.WrinkleDetailNormalPower);
        AddParameter(command, "@muscle_albedo_blend_rate", common.EditInfo.MuscleAlbedoBlendRate);
        AddParameter(command, "@muscle_detail_normal_power", common.EditInfo.MuscleDetailNormalPower);
        AddParameter(command, "@height", common.EditInfo.Height);
        AddParameter(command, "@head_size", common.EditInfo.HeadSize);
        AddParameter(command, "@neck_offset", common.EditInfo.NeckOffset);
        AddParameter(command, "@neck_scale", common.EditInfo.NeckScale);
        AddParameter(command, "@upper_body_scale_x", common.EditInfo.UpperBodyScaleX);
        AddParameter(command, "@belly_size", common.EditInfo.BellySize);
        AddParameter(command, "@teat_scale", common.EditInfo.TeatScale);
        AddParameter(command, "@tekubi_size", common.EditInfo.TekubiSize);
        AddParameter(command, "@koshi_offset", common.EditInfo.KoshiOffset);
        AddParameter(command, "@koshi_size", common.EditInfo.KoshiSize);
        AddParameter(command, "@ankle_offset", common.EditInfo.AnkleOffset);
        AddParameter(command, "@fat", common.EditInfo.Fat);
        AddParameter(command, "@muscle", common.EditInfo.Muscle);
        AddParameter(command, "@motion_filter", common.EditInfo.MotionFilter);
        // CDataStatusInfoFields
        AddParameter(command, "@revive_point", common.StatusInfo.RevivePoint);
        AddParameter(command, "@hp", common.GreenHp);
        AddParameter(command, "@white_hp", common.WhiteHp);
        // CDataArisenProfile
        AddParameter(command, "@background_id", common.CharacterProfile.BackgroundId);
        AddParameter(command, "@title_uid", common.CharacterProfile.Title.UId);
        AddParameter(command, "@title_index", common.CharacterProfile.Title.Index);
        AddParameter(command, "@motion_id", common.CharacterProfile.MotionId);
        AddParameter(command, "@motion_frame_no", common.CharacterProfile.MotionFrameNo);
        AddParameter(command, "@comment", common.CharacterProfile.Comment);
    }
}
