using System;
using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] CharacterFields = new string[]
        {
            "version", "account_id", "first_name", "last_name", "created", "job", "jewelry_slot_num", "my_pawn_slot_num", "rental_pawn_slot_num", "hide_equip_head", "hide_equip_lantern", "hide_equip_head_pawn", "hide_equip_lantern_pawn", "arisen_profile_share_range"
        };

        private static readonly string[] CDataEditInfoFields = new string[]
        {
            "character_id", "sex", "voice", "voice_pitch", "personality", "speech_freq", "body_type", "hair", "beard", "makeup", "scar",
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

        private static readonly string[] CDataStatusInfoFields = new string[]
        {
            "character_id", "hp", "stamina", "revive_point", "max_hp", "max_stamina", "white_hp", "gain_hp", "gain_stamina",
            "gain_attack", "gain_defense", "gain_magic_attack", "gain_magic_defense"
        };

        private static readonly string[] CDataMatchingProfileFields = new string[]
        {
            "character_id", "entry_job", "entry_job_level", "current_job", "current_job_level", "objective_type1", "objective_type2", 
            "play_style", "comment", "is_join_party"
        };

        private static readonly string[]  CDataArisenProfileFields = new string[]
        {
            "character_id", "background_id", "title_uid", "title_index", "motion_id", "motion_frame_no"
        };

        private readonly string SqlInsertCharacter = $"INSERT INTO `ddon_character` ({BuildQueryField(CharacterFields)}) VALUES ({BuildQueryInsert(CharacterFields)});";
        private static readonly string SqlUpdateCharacter = $"UPDATE `ddon_character` SET {BuildQueryUpdate(CharacterFields)} WHERE `id` = @id;";
        private static readonly string SqlSelectCharacter = $"SELECT `id`, {BuildQueryField(CharacterFields)} FROM `ddon_character` WHERE `id` = @id;";
        private static readonly string SqlSelectCharactersByAccountId = $"SELECT `id`, {BuildQueryField(CharacterFields)} FROM `ddon_character` WHERE `account_id` = @account_id;";
        private static readonly string SqlSelectAllCharacterData = $"SELECT `id`, {BuildQueryField("ddon_character", CharacterFields)}, {BuildQueryField("ddon_character_edit_info", CDataEditInfoFields)}, {BuildQueryField("ddon_character_status_info", CDataStatusInfoFields)}, {BuildQueryField("ddon_character_matching_profile", CDataMatchingProfileFields)}, {BuildQueryField("ddon_character_arisen_profile", CDataArisenProfileFields)} "
            + "FROM `ddon_character` "
            + "LEFT JOIN `ddon_character_edit_info` ON `ddon_character_edit_info`.`character_id` = `ddon_character`.`id` "
            + "LEFT JOIN `ddon_character_status_info` ON `ddon_character_status_info`.`character_id` = `ddon_character`.`id` "
            + "LEFT JOIN `ddon_character_matching_profile` ON `ddon_character_matching_profile`.`character_id` = `ddon_character`.`id` "
            + "LEFT JOIN `ddon_character_arisen_profile` ON `ddon_character_arisen_profile`.`character_id` = `ddon_character`.`id` "
            + "WHERE `id` = @id";
        private static readonly string SqlSelectAllCharactersDataByAccountId = $"SELECT `id`, {BuildQueryField("ddon_character", CharacterFields)}, {BuildQueryField("ddon_character_edit_info", CDataEditInfoFields)}, {BuildQueryField("ddon_character_status_info", CDataStatusInfoFields)}, {BuildQueryField("ddon_character_matching_profile", CDataMatchingProfileFields)}, {BuildQueryField("ddon_character_arisen_profile", CDataArisenProfileFields)} "
            + "FROM `ddon_character` "
            + "LEFT JOIN `ddon_character_edit_info` ON `ddon_character_edit_info`.`character_id` = `ddon_character`.`id` "
            + "LEFT JOIN `ddon_character_status_info` ON `ddon_character_status_info`.`character_id` = `ddon_character`.`id` "
            + "LEFT JOIN `ddon_character_matching_profile` ON `ddon_character_matching_profile`.`character_id` = `ddon_character`.`id` "
            + "LEFT JOIN `ddon_character_arisen_profile` ON `ddon_character_arisen_profile`.`character_id` = `ddon_character`.`id` "
            + "WHERE `account_id` = @account_id";
        private const string SqlDeleteCharacter = "DELETE FROM `ddon_character` WHERE `id`=@id;";


        private readonly string SqlInsertCharacterEditInfo = $"INSERT INTO `ddon_character_edit_info` ({BuildQueryField(CDataEditInfoFields)}) VALUES ({BuildQueryInsert(CDataEditInfoFields)});";
        private static readonly string SqlUpdateCharacterEditInfo = $"UPDATE `ddon_character_edit_info` SET {BuildQueryUpdate(CDataEditInfoFields)} WHERE `character_id` = @character_id;";
        private static readonly string SqlSelectCharacterEditInfo = $"SELECT {BuildQueryField(CDataEditInfoFields)} FROM `ddon_character_edit_info` WHERE `character_id` = @character_id;";
        private const string SqlDeleteCharacterEditInfo = "DELETE FROM `ddon_character_edit_info` WHERE `character_id`=@character_id;";


        private readonly string SqlInsertCharacterStatusInfo = $"INSERT INTO `ddon_character_status_info` ({BuildQueryField(CDataStatusInfoFields)}) VALUES ({BuildQueryInsert(CDataStatusInfoFields)});";
        private static readonly string SqlUpdateCharacterStatusInfo = $"UPDATE `ddon_character_status_info` SET {BuildQueryUpdate(CDataStatusInfoFields)} WHERE `character_id` = @character_id;";
        private static readonly string SqlSelectCharacterStatusInfo = $"SELECT {BuildQueryField(CDataStatusInfoFields)} FROM `ddon_character_status_info` WHERE `character_id` = @character_id;";
        private const string SqlDeleteCharacterStatusInfo = "DELETE FROM `ddon_character_status_info` WHERE `character_id`=@character_id;";


        private readonly string SqlInsertCharacterMatchingProfile = $"INSERT INTO `ddon_character_matching_profile` ({BuildQueryField(CDataMatchingProfileFields)}) VALUES ({BuildQueryInsert(CDataMatchingProfileFields)});";
        private static readonly string SqlUpdateCharacterMatchingProfile = $"UPDATE `ddon_character_matching_profile` SET {BuildQueryUpdate(CDataMatchingProfileFields)} WHERE `character_id` = @character_id;";
        private static readonly string SqlSelectCharacterMatchingProfile = $"SELECT {BuildQueryField(CDataMatchingProfileFields)} FROM `ddon_character_matching_profile` WHERE `character_id` = @character_id;";
        private const string SqlDeleteCharacterMatchingProfile = "DELETE FROM `ddon_character_matching_profile` WHERE `character_id`=@character_id;";


        private readonly string SqlInsertCharacterArisenProfile = $"INSERT INTO `ddon_character_arisen_profile` ({BuildQueryField(CDataArisenProfileFields)}) VALUES ({BuildQueryInsert(CDataArisenProfileFields)});";
        private static readonly string SqlUpdateCharacterArisenProfile = $"UPDATE `ddon_character_arisen_profile` SET {BuildQueryUpdate(CDataArisenProfileFields)} WHERE `character_id` = @character_id;";
        private static readonly string SqlSelectCharacterArisenProfile = $"SELECT {BuildQueryField(CDataArisenProfileFields)} FROM `ddon_character_arisen_profile` WHERE `character_id` = @character_id;";
        private const string SqlDeleteCharacterArisenProfile = "DELETE FROM `ddon_character_arisen_profile` WHERE `character_id`=@character_id;";


        public bool CreateCharacter(Character character)
        {
            character.Created = DateTime.Now;
            int rowsAffected = ExecuteNonQuery(SqlInsertCharacter, command => { AddParameter(command, character); }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }
            character.Id = (uint) autoIncrement;

            rowsAffected = ExecuteNonQuery(SqlInsertCharacterEditInfo, command => { AddParameter(command, character); });
            if (rowsAffected <= NoRowsAffected)
            {
                return false;
            }

            rowsAffected = ExecuteNonQuery(SqlInsertCharacterStatusInfo, command => { AddParameter(command, character); });
            if (rowsAffected <= NoRowsAffected)
            {
                return false;
            }

            rowsAffected = ExecuteNonQuery(SqlInsertCharacterMatchingProfile, command => { AddParameter(command, character); });
            if (rowsAffected <= NoRowsAffected)
            {
                return false;
            }

            rowsAffected = ExecuteNonQuery(SqlInsertCharacterArisenProfile, command => { AddParameter(command, character); });
            if (rowsAffected <= NoRowsAffected)
            {
                return false;
            }

            StoreCharacterData(character);

            return true;
        }

        public bool UpdateCharacter(Character character)
        {
            int characterUpdateRowsAffected = ExecuteNonQuery(SqlUpdateCharacter, command =>
            {
                AddParameter(command, "@id", character.Id);
                AddParameter(command, character);
            });

            characterUpdateRowsAffected += ExecuteNonQuery(SqlUpdateCharacterEditInfo, command =>
            {
                AddParameter(command, "@id", character.Id);
                AddParameter(command, character);
            });

            characterUpdateRowsAffected += ExecuteNonQuery(SqlUpdateCharacterStatusInfo, command =>
            {
                AddParameter(command, "@id", character.Id);
                AddParameter(command, character);
            });

            characterUpdateRowsAffected += ExecuteNonQuery(SqlUpdateCharacterMatchingProfile, command =>
            {
                AddParameter(command, "@id", character.Id);
                AddParameter(command, character);
            });

            characterUpdateRowsAffected += ExecuteNonQuery(SqlUpdateCharacterArisenProfile, command =>
            {
                AddParameter(command, "@id", character.Id);
                AddParameter(command, character);
            });

            StoreCharacterData(character);

            return characterUpdateRowsAffected > NoRowsAffected;
        }

        public Character SelectCharacter(uint characterId)
        {
            Character character = null;
            ExecuteReader(SqlSelectAllCharacterData,
                command => { AddParameter(command, "@id", characterId); }, reader =>
                {
                    if (reader.Read())
                    {
                        character = ReadAllCharacterData(reader);
                    }
                });

            QueryCharacterData(character);

            return character;
        }

        public List<Character> SelectCharactersByAccountId(int accountId)
        {
            List<Character> characters = new List<Character>();
            ExecuteReader(SqlSelectAllCharactersDataByAccountId,
                command => { AddParameter(command, "@account_id", accountId); }, reader =>
                {
                    while (reader.Read())
                    {
                        Character character = ReadAllCharacterData(reader);
                        characters.Add(character);

                        QueryCharacterData(character);
                    }
                });

            return characters;
        }

        public bool DeleteCharacter(uint characterId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteCharacter,
                command => { AddParameter(command, "@id", characterId); });
            return rowsAffected > NoRowsAffected;
        }

        private void QueryCharacterData(Character character)
        {
            // Job data
            ExecuteReader(SqlSelectCharacterJobDataByCharacter,
                command => { AddParameter(command, "@character_id", character.Id); }, reader =>
                {
                    while (reader.Read())
                    {
                        character.CharacterInfo.CharacterJobDataList.Add(ReadCharacterJobData(reader));
                    }
                });

            // Equips
            CDataCharacterEquipData characterEquipData = new CDataCharacterEquipData();
            ExecuteReader(SqlSelectEquipItemInfo,
                command => 
                { 
                    AddParameter(command, "@character_id", character.Id); 
                    AddParameter(command, "@job", (byte) character.CharacterInfo.Job); 
                }, 
                reader =>
                {
                    while (reader.Read())
                    {
                        characterEquipData.Equips.Add(ReadEquipItemInfo(reader));
                    }
                });
            character.CharacterInfo.CharacterEquipDataList.Add(characterEquipData);

            CDataCharacterEquipData characterVisualEquipData = new CDataCharacterEquipData();
            ExecuteReader(SqlSelectVisualEquipItemInfo,
                command => 
                { 
                    AddParameter(command, "@character_id", character.Id); 
                    AddParameter(command, "@job", (byte) character.CharacterInfo.Job); 
                }, 
                reader =>
                {
                    while (reader.Read())
                    {
                        characterVisualEquipData.Equips.Add(ReadEquipItemInfo(reader));
                    }
                });
            character.CharacterInfo.CharacterEquipViewDataList.Add(characterVisualEquipData);

            // Job Items
            ExecuteReader(SqlSelectEquipJobItem,
                command => 
                { 
                    AddParameter(command, "@character_id", character.Id); 
                    AddParameter(command, "@job", (byte) character.CharacterInfo.Job); 
                }, 
                reader =>
                {
                    while (reader.Read())
                    {
                        character.CharacterInfo.CharacterEquipJobItemList.Add(ReadEquipJobItem(reader));
                    }
                });

            // Normal Skills
            ExecuteReader(SqlSelectNormalSkillParam,
                command => { AddParameter(command, "@character_id", character.Id); },
                reader =>
                {
                    while (reader.Read())
                    {
                        character.NormalSkills.Add(ReadNormalSkillParam(reader));
                    }
                });

            // Custom Skills
            ExecuteReader(SqlSelectCustomSkillsSetAcquirementParam,
                command => { AddParameter(command, "@character_id", character.Id); },
                reader =>
                {
                    while (reader.Read())
                    {
                        character.CustomSkills.Add(ReadSetAcquirementParam(reader));
                    }
                });

            // Abilities
            ExecuteReader(SqlSelectAbilitiesSetAcquirementParam,
                command => { AddParameter(command, "@character_id", character.Id); },
                reader =>
                {
                    while (reader.Read())
                    {
                        character.Abilities.Add(ReadSetAcquirementParam(reader));
                    }
                });
        }

        private void StoreCharacterData(Character character)
        {
            foreach(CDataCharacterJobData characterJobData in character.CharacterInfo.CharacterJobDataList)
            {
                ExecuteNonQuery(SqlReplaceCharacterJobData, command =>
                {
                    AddParameter(command, character.Id, characterJobData);
                });
            }

            foreach(CDataCharacterEquipData characterEquipData in character.CharacterInfo.CharacterEquipDataList)
            {
                foreach(CDataEquipItemInfo equipItemInfo in characterEquipData.Equips)
                {
                    ExecuteNonQuery(SqlReplaceEquipItemInfo, command =>
                    {
                        AddParameter(command, character.Id, character.CharacterInfo.Job, equipItemInfo);
                    });
                }
            }

            foreach(CDataCharacterEquipData characterEquipData in character.CharacterInfo.CharacterEquipViewDataList)
            {
                foreach(CDataEquipItemInfo equipItemInfo in characterEquipData.Equips)
                {
                    ExecuteNonQuery(SqlReplaceEquipItemInfo, command =>
                    {
                        AddParameter(command, character.Id, character.CharacterInfo.Job, equipItemInfo);
                    });
                }
            }

            foreach(CDataEquipJobItem equipJobItem in character.CharacterInfo.CharacterEquipJobItemList)
            {
                ExecuteNonQuery(SqlReplaceEquipJobItem, command =>
                {
                    AddParameter(command, character.Id, character.CharacterInfo.Job, equipJobItem);
                });
            }

            foreach(CDataNormalSkillParam normalSkillParam in character.NormalSkills)
            {
                ExecuteNonQuery(SqlReplaceNormalSkillParam, command =>
                {
                    AddParameter(command, character.Id, normalSkillParam);
                });
            }

            foreach(CDataSetAcquirementParam setAcquirementParam in character.CustomSkills)
            {
                ExecuteNonQuery(SqlReplaceSetAcquirementParam, command =>
                {
                    AddParameter(command, character.Id, setAcquirementParam);
                });
            }

            foreach(CDataSetAcquirementParam setAcquirementParam in character.Abilities)
            {
                ExecuteNonQuery(SqlReplaceSetAcquirementParam, command =>
                {
                    AddParameter(command, character.Id, setAcquirementParam);
                });
            }
        }

        private Character ReadAllCharacterData(DbDataReader reader)
        {
            Character character = new Character();
            character.Id = GetUInt32(reader, "id");

            character.AccountId = GetInt32(reader, "account_id");
            character.CharacterInfo.Version = GetUInt32(reader, "version");
            character.CharacterInfo.FirstName = GetString(reader, "first_name");
            character.CharacterInfo.LastName = GetString(reader, "last_name");
            character.Created = GetDateTime(reader, "created");
            character.CharacterInfo.Job = (JobId) GetByte(reader, "job");
            character.CharacterInfo.JewelrySlotNum = GetByte(reader, "jewelry_slot_num");
            character.CharacterInfo.MyPawnSlotNum = GetByte(reader, "my_pawn_slot_num");
            character.CharacterInfo.RentalPawnSlotNum = GetByte(reader, "rental_pawn_slot_num");
            character.CharacterInfo.HideEquipHead = GetBoolean(reader, "hide_equip_head");
            character.CharacterInfo.HideEquipLantern = GetBoolean(reader, "hide_equip_lantern");
            character.CharacterInfo.HideEquipHeadPawn = GetBoolean(reader, "hide_equip_head_pawn");
            character.CharacterInfo.HideEquipLanternPawn = GetBoolean(reader, "hide_equip_lantern_pawn");
            character.CharacterInfo.ArisenProfileShareRange = GetByte(reader, "arisen_profile_share_range");

            character.CharacterInfo.EditInfo.Sex = GetByte(reader, "sex");
            character.CharacterInfo.EditInfo.Voice = GetByte(reader, "voice");
            character.CharacterInfo.EditInfo.VoicePitch = GetUInt16(reader, "voice_pitch");
            character.CharacterInfo.EditInfo.Personality = GetByte(reader, "personality");
            character.CharacterInfo.EditInfo.SpeechFreq = GetByte(reader, "speech_freq");
            character.CharacterInfo.EditInfo.BodyType = GetByte(reader, "body_type");
            character.CharacterInfo.EditInfo.Hair = GetByte(reader, "hair");
            character.CharacterInfo.EditInfo.Beard = GetByte(reader, "beard");
            character.CharacterInfo.EditInfo.Makeup = GetByte(reader, "makeup");
            character.CharacterInfo.EditInfo.Scar = GetByte(reader, "scar");
            character.CharacterInfo.EditInfo.EyePresetNo = GetByte(reader, "eye_preset_no");
            character.CharacterInfo.EditInfo.NosePresetNo = GetByte(reader, "nose_preset_no");
            character.CharacterInfo.EditInfo.MouthPresetNo = GetByte(reader, "mouth_preset_no");
            character.CharacterInfo.EditInfo.EyebrowTexNo = GetByte(reader, "eyebrow_tex_no");
            character.CharacterInfo.EditInfo.ColorSkin = GetByte(reader, "color_skin");
            character.CharacterInfo.EditInfo.ColorHair = GetByte(reader, "color_hair");
            character.CharacterInfo.EditInfo.ColorBeard = GetByte(reader, "color_beard");
            character.CharacterInfo.EditInfo.ColorEyebrow = GetByte(reader, "color_eyebrow");
            character.CharacterInfo.EditInfo.ColorREye = GetByte(reader, "color_r_eye");
            character.CharacterInfo.EditInfo.ColorLEye = GetByte(reader, "color_l_eye");
            character.CharacterInfo.EditInfo.ColorMakeup = GetByte(reader, "color_makeup");
            character.CharacterInfo.EditInfo.Sokutobu = GetUInt16(reader, "sokutobu");
            character.CharacterInfo.EditInfo.Hitai = GetUInt16(reader, "hitai");
            character.CharacterInfo.EditInfo.MimiJyouge = GetUInt16(reader, "mimi_jyouge");
            character.CharacterInfo.EditInfo.Kannkaku = GetUInt16(reader, "kannkaku");
            character.CharacterInfo.EditInfo.MabisasiJyouge = GetUInt16(reader, "mabisasi_jyouge");
            character.CharacterInfo.EditInfo.HanakuchiJyouge = GetUInt16(reader, "hanakuchi_jyouge");
            character.CharacterInfo.EditInfo.AgoSakiHaba = GetUInt16(reader, "ago_saki_haba");
            character.CharacterInfo.EditInfo.AgoZengo = GetUInt16(reader, "ago_zengo");
            character.CharacterInfo.EditInfo.AgoSakiJyouge = GetUInt16(reader, "ago_saki_jyouge");
            character.CharacterInfo.EditInfo.HitomiOokisa = GetUInt16(reader, "hitomi_ookisa");
            character.CharacterInfo.EditInfo.MeOokisa = GetUInt16(reader, "me_ookisa");
            character.CharacterInfo.EditInfo.MeKaiten = GetUInt16(reader, "me_kaiten");
            character.CharacterInfo.EditInfo.MayuKaiten = GetUInt16(reader, "mayu_kaiten");
            character.CharacterInfo.EditInfo.MimiOokisa = GetUInt16(reader, "mimi_ookisa");
            character.CharacterInfo.EditInfo.MimiMuki = GetUInt16(reader, "mimi_muki");
            character.CharacterInfo.EditInfo.ElfMimi = GetUInt16(reader, "elf_mimi");
            character.CharacterInfo.EditInfo.MikenTakasa = GetUInt16(reader, "miken_takasa");
            character.CharacterInfo.EditInfo.MikenHaba = GetUInt16(reader, "miken_haba");
            character.CharacterInfo.EditInfo.HohoboneRyou = GetUInt16(reader, "hohobone_ryou");
            character.CharacterInfo.EditInfo.HohoboneJyouge = GetUInt16(reader, "hohobone_jyouge");
            character.CharacterInfo.EditInfo.Hohoniku = GetUInt16(reader, "hohoniku");
            character.CharacterInfo.EditInfo.ErahoneJyouge = GetUInt16(reader, "erahone_jyouge");
            character.CharacterInfo.EditInfo.ErahoneHaba = GetUInt16(reader, "erahone_haba");
            character.CharacterInfo.EditInfo.HanaJyouge = GetUInt16(reader, "hana_jyouge");
            character.CharacterInfo.EditInfo.HanaHaba = GetUInt16(reader, "hana_haba");
            character.CharacterInfo.EditInfo.HanaTakasa = GetUInt16(reader, "hana_takasa");
            character.CharacterInfo.EditInfo.HanaKakudo = GetUInt16(reader, "hana_kakudo");
            character.CharacterInfo.EditInfo.KuchiHaba = GetUInt16(reader, "kuchi_haba");
            character.CharacterInfo.EditInfo.KuchiAtsusa = GetUInt16(reader, "kuchi_atsusa");
            character.CharacterInfo.EditInfo.EyebrowUVOffsetX = GetUInt16(reader, "eyebrow_uv_offset_x");
            character.CharacterInfo.EditInfo.EyebrowUVOffsetY = GetUInt16(reader, "eyebrow_uv_offset_y");
            character.CharacterInfo.EditInfo.Wrinkle = GetUInt16(reader, "wrinkle");
            character.CharacterInfo.EditInfo.WrinkleAlbedoBlendRate = GetUInt16(reader, "wrinkle_albedo_blend_rate");
            character.CharacterInfo.EditInfo.WrinkleDetailNormalPower = GetUInt16(reader, "wrinkle_detail_normal_power");
            character.CharacterInfo.EditInfo.MuscleAlbedoBlendRate = GetUInt16(reader, "muscle_albedo_blend_rate");
            character.CharacterInfo.EditInfo.MuscleDetailNormalPower = GetUInt16(reader, "muscle_detail_normal_power");
            character.CharacterInfo.EditInfo.Height = GetUInt16(reader, "height");
            character.CharacterInfo.EditInfo.HeadSize = GetUInt16(reader, "head_size");
            character.CharacterInfo.EditInfo.NeckOffset = GetUInt16(reader, "neck_offset");
            character.CharacterInfo.EditInfo.NeckScale = GetUInt16(reader, "neck_scale");
            character.CharacterInfo.EditInfo.UpperBodyScaleX = GetUInt16(reader, "upper_body_scale_x");
            character.CharacterInfo.EditInfo.BellySize = GetUInt16(reader, "belly_size");
            character.CharacterInfo.EditInfo.TeatScale = GetUInt16(reader, "teat_scale");
            character.CharacterInfo.EditInfo.TekubiSize = GetUInt16(reader, "tekubi_size");
            character.CharacterInfo.EditInfo.KoshiOffset = GetUInt16(reader, "koshi_offset");
            character.CharacterInfo.EditInfo.KoshiSize = GetUInt16(reader, "koshi_size");
            character.CharacterInfo.EditInfo.AnkleOffset = GetUInt16(reader, "ankle_offset");
            character.CharacterInfo.EditInfo.Fat = GetUInt16(reader, "fat");
            character.CharacterInfo.EditInfo.Muscle = GetUInt16(reader, "muscle");
            character.CharacterInfo.EditInfo.MotionFilter = GetUInt16(reader, "motion_filter");

            character.CharacterInfo.StatusInfo.HP = GetUInt32(reader, "hp");
            character.CharacterInfo.StatusInfo.Stamina = GetUInt32(reader, "stamina");
            character.CharacterInfo.StatusInfo.RevivePoint = GetByte(reader, "revive_point");
            character.CharacterInfo.StatusInfo.MaxHP = GetUInt32(reader, "max_hp");
            character.CharacterInfo.StatusInfo.MaxStamina = GetUInt32(reader, "max_stamina");
            character.CharacterInfo.StatusInfo.WhiteHP = GetUInt32(reader, "white_hp");
            character.CharacterInfo.StatusInfo.GainHP = GetUInt32(reader, "gain_hp");
            character.CharacterInfo.StatusInfo.GainStamina = GetUInt32(reader, "gain_stamina");
            character.CharacterInfo.StatusInfo.GainAttack = GetUInt32(reader, "gain_attack");
            character.CharacterInfo.StatusInfo.GainDefense = GetUInt32(reader, "gain_defense");
            character.CharacterInfo.StatusInfo.GainMagicAttack = GetUInt32(reader, "gain_magic_attack");
            character.CharacterInfo.StatusInfo.GainMagicDefense = GetUInt32(reader, "gain_magic_defense");

            character.CharacterInfo.MatchingProfile.EntryJob = (JobId) GetByte(reader, "entry_job");
            character.CharacterInfo.MatchingProfile.EntryJobLevel = GetUInt32(reader, "entry_job_level");
            character.CharacterInfo.MatchingProfile.CurrentJob = (JobId) GetByte(reader, "current_job");
            character.CharacterInfo.MatchingProfile.CurrentJobLevel = GetUInt32(reader, "current_job_level");
            character.CharacterInfo.MatchingProfile.ObjectiveType1 = GetUInt32(reader, "objective_type1");
            character.CharacterInfo.MatchingProfile.ObjectiveType2 = GetUInt32(reader, "objective_type2");
            character.CharacterInfo.MatchingProfile.PlayStyle = GetUInt32(reader, "play_style");
            character.CharacterInfo.MatchingProfile.Comment = GetString(reader, "comment");
            character.CharacterInfo.MatchingProfile.IsJoinParty = GetByte(reader, "is_join_party");

            character.CharacterInfo.ArisenProfile.BackgroundId = GetByte(reader, "background_id");
            character.CharacterInfo.ArisenProfile.Title.UId = GetUInt32(reader, "title_uid");
            character.CharacterInfo.ArisenProfile.Title.Index = GetUInt32(reader, "title_index");
            character.CharacterInfo.ArisenProfile.MotionId = GetUInt16(reader, "motion_id");
            character.CharacterInfo.ArisenProfile.MotionFrameNo = GetUInt32(reader, "motion_frame_no");

            return character;
        }

        private void AddParameter(TCom command, Character character)
        {
            // CharacterFields
            AddParameter(command, "@account_id", character.AccountId);
            AddParameter(command, "@character_id", character.Id);
            AddParameter(command, "@version", character.CharacterInfo.Version);
            AddParameter(command, "@first_name", character.CharacterInfo.FirstName);
            AddParameter(command, "@last_name", character.CharacterInfo.LastName);
            AddParameter(command, "@created", character.Created);
            AddParameter(command, "@job", (byte) character.CharacterInfo.Job);
            AddParameter(command, "@jewelry_slot_num", character.CharacterInfo.JewelrySlotNum);
            AddParameter(command, "@my_pawn_slot_num", character.CharacterInfo.MyPawnSlotNum);
            AddParameter(command, "@rental_pawn_slot_num", character.CharacterInfo.RentalPawnSlotNum);
            AddParameter(command, "@hide_equip_head", character.CharacterInfo.HideEquipHead);
            AddParameter(command, "@hide_equip_lantern", character.CharacterInfo.HideEquipLantern);
            AddParameter(command, "@hide_equip_head_pawn", character.CharacterInfo.HideEquipHeadPawn);
            AddParameter(command, "@hide_equip_lantern_pawn", character.CharacterInfo.HideEquipLanternPawn);
            AddParameter(command, "@arisen_profile_share_range", character.CharacterInfo.ArisenProfileShareRange);
            // CDataEditInfoFields
            AddParameter(command, "@sex", character.CharacterInfo.EditInfo.Sex);
            AddParameter(command, "@voice", character.CharacterInfo.EditInfo.Voice);
            AddParameter(command, "@voice_pitch", character.CharacterInfo.EditInfo.VoicePitch);
            AddParameter(command, "@personality", character.CharacterInfo.EditInfo.Personality);
            AddParameter(command, "@speech_freq", character.CharacterInfo.EditInfo.SpeechFreq);
            AddParameter(command, "@body_type", character.CharacterInfo.EditInfo.BodyType);
            AddParameter(command, "@hair", character.CharacterInfo.EditInfo.Hair);
            AddParameter(command, "@beard", character.CharacterInfo.EditInfo.Beard);
            AddParameter(command, "@makeup", character.CharacterInfo.EditInfo.Makeup);
            AddParameter(command, "@scar", character.CharacterInfo.EditInfo.Scar);
            AddParameter(command, "@eye_preset_no", character.CharacterInfo.EditInfo.EyePresetNo);
            AddParameter(command, "@nose_preset_no", character.CharacterInfo.EditInfo.NosePresetNo);
            AddParameter(command, "@mouth_preset_no", character.CharacterInfo.EditInfo.MouthPresetNo);
            AddParameter(command, "@eyebrow_tex_no", character.CharacterInfo.EditInfo.EyebrowTexNo);
            AddParameter(command, "@color_skin", character.CharacterInfo.EditInfo.ColorSkin);
            AddParameter(command, "@color_hair", character.CharacterInfo.EditInfo.ColorHair);
            AddParameter(command, "@color_beard", character.CharacterInfo.EditInfo.ColorBeard);
            AddParameter(command, "@color_eyebrow", character.CharacterInfo.EditInfo.ColorEyebrow);
            AddParameter(command, "@color_r_eye", character.CharacterInfo.EditInfo.ColorREye);
            AddParameter(command, "@color_l_eye", character.CharacterInfo.EditInfo.ColorLEye);
            AddParameter(command, "@color_makeup", character.CharacterInfo.EditInfo.ColorMakeup);
            AddParameter(command, "@sokutobu", character.CharacterInfo.EditInfo.Sokutobu);
            AddParameter(command, "@hitai", character.CharacterInfo.EditInfo.Hitai);
            AddParameter(command, "@mimi_jyouge", character.CharacterInfo.EditInfo.MimiJyouge);
            AddParameter(command, "@kannkaku", character.CharacterInfo.EditInfo.Kannkaku);
            AddParameter(command, "@mabisasi_jyouge", character.CharacterInfo.EditInfo.MabisasiJyouge);
            AddParameter(command, "@hanakuchi_jyouge", character.CharacterInfo.EditInfo.HanakuchiJyouge);
            AddParameter(command, "@ago_saki_haba", character.CharacterInfo.EditInfo.AgoSakiHaba);
            AddParameter(command, "@ago_zengo", character.CharacterInfo.EditInfo.AgoZengo);
            AddParameter(command, "@ago_saki_jyouge", character.CharacterInfo.EditInfo.AgoSakiJyouge);
            AddParameter(command, "@hitomi_ookisa", character.CharacterInfo.EditInfo.HitomiOokisa);
            AddParameter(command, "@me_ookisa", character.CharacterInfo.EditInfo.MeOokisa);
            AddParameter(command, "@me_kaiten", character.CharacterInfo.EditInfo.MeKaiten);
            AddParameter(command, "@mayu_kaiten", character.CharacterInfo.EditInfo.MayuKaiten);
            AddParameter(command, "@mimi_ookisa", character.CharacterInfo.EditInfo.MimiOokisa);
            AddParameter(command, "@mimi_muki", character.CharacterInfo.EditInfo.MimiMuki);
            AddParameter(command, "@elf_mimi", character.CharacterInfo.EditInfo.ElfMimi);
            AddParameter(command, "@miken_takasa", character.CharacterInfo.EditInfo.MikenTakasa);
            AddParameter(command, "@miken_haba", character.CharacterInfo.EditInfo.MikenHaba);
            AddParameter(command, "@hohobone_ryou", character.CharacterInfo.EditInfo.HohoboneRyou);
            AddParameter(command, "@hohobone_jyouge", character.CharacterInfo.EditInfo.HohoboneJyouge);
            AddParameter(command, "@hohoniku", character.CharacterInfo.EditInfo.Hohoniku);
            AddParameter(command, "@erahone_jyouge", character.CharacterInfo.EditInfo.ErahoneJyouge);
            AddParameter(command, "@erahone_haba", character.CharacterInfo.EditInfo.ErahoneHaba);
            AddParameter(command, "@hana_jyouge", character.CharacterInfo.EditInfo.HanaJyouge);
            AddParameter(command, "@hana_haba", character.CharacterInfo.EditInfo.HanaHaba);
            AddParameter(command, "@hana_takasa", character.CharacterInfo.EditInfo.HanaTakasa);
            AddParameter(command, "@hana_kakudo", character.CharacterInfo.EditInfo.HanaKakudo);
            AddParameter(command, "@kuchi_haba", character.CharacterInfo.EditInfo.KuchiHaba);
            AddParameter(command, "@kuchi_atsusa", character.CharacterInfo.EditInfo.KuchiAtsusa);
            AddParameter(command, "@eyebrow_uv_offset_x", character.CharacterInfo.EditInfo.EyebrowUVOffsetX);
            AddParameter(command, "@eyebrow_uv_offset_y", character.CharacterInfo.EditInfo.EyebrowUVOffsetY);
            AddParameter(command, "@wrinkle", character.CharacterInfo.EditInfo.Wrinkle);
            AddParameter(command, "@wrinkle_albedo_blend_rate", character.CharacterInfo.EditInfo.WrinkleAlbedoBlendRate);
            AddParameter(command, "@wrinkle_detail_normal_power", character.CharacterInfo.EditInfo.WrinkleDetailNormalPower);
            AddParameter(command, "@muscle_albedo_blend_rate", character.CharacterInfo.EditInfo.MuscleAlbedoBlendRate);
            AddParameter(command, "@muscle_detail_normal_power", character.CharacterInfo.EditInfo.MuscleDetailNormalPower);
            AddParameter(command, "@height", character.CharacterInfo.EditInfo.Height);
            AddParameter(command, "@head_size", character.CharacterInfo.EditInfo.HeadSize);
            AddParameter(command, "@neck_offset", character.CharacterInfo.EditInfo.NeckOffset);
            AddParameter(command, "@neck_scale", character.CharacterInfo.EditInfo.NeckScale);
            AddParameter(command, "@upper_body_scale_x", character.CharacterInfo.EditInfo.UpperBodyScaleX);
            AddParameter(command, "@belly_size", character.CharacterInfo.EditInfo.BellySize);
            AddParameter(command, "@teat_scale", character.CharacterInfo.EditInfo.TeatScale);
            AddParameter(command, "@tekubi_size", character.CharacterInfo.EditInfo.TekubiSize);
            AddParameter(command, "@koshi_offset", character.CharacterInfo.EditInfo.KoshiOffset);
            AddParameter(command, "@koshi_size", character.CharacterInfo.EditInfo.KoshiSize);
            AddParameter(command, "@ankle_offset", character.CharacterInfo.EditInfo.AnkleOffset);
            AddParameter(command, "@fat", character.CharacterInfo.EditInfo.Fat);
            AddParameter(command, "@muscle", character.CharacterInfo.EditInfo.Muscle);
            AddParameter(command, "@motion_filter", character.CharacterInfo.EditInfo.MotionFilter);
            // CDataStatusInfoFields
            AddParameter(command, "@hp", character.CharacterInfo.StatusInfo.HP);
            AddParameter(command, "@stamina", character.CharacterInfo.StatusInfo.Stamina);
            AddParameter(command, "@revive_point", character.CharacterInfo.StatusInfo.RevivePoint);
            AddParameter(command, "@max_hp", character.CharacterInfo.StatusInfo.MaxHP);
            AddParameter(command, "@max_stamina", character.CharacterInfo.StatusInfo.MaxStamina);
            AddParameter(command, "@white_hp", character.CharacterInfo.StatusInfo.WhiteHP);
            AddParameter(command, "@gain_hp", character.CharacterInfo.StatusInfo.GainHP);
            AddParameter(command, "@gain_stamina", character.CharacterInfo.StatusInfo.GainStamina);
            AddParameter(command, "@gain_attack", character.CharacterInfo.StatusInfo.GainAttack);
            AddParameter(command, "@gain_defense", character.CharacterInfo.StatusInfo.GainDefense);
            AddParameter(command, "@gain_magic_attack", character.CharacterInfo.StatusInfo.GainMagicAttack);
            AddParameter(command, "@gain_magic_defense", character.CharacterInfo.StatusInfo.GainMagicDefense);
            // CDataMatchingProfile
            AddParameter(command, "@entry_job", (byte) character.CharacterInfo.MatchingProfile.EntryJob);
            AddParameter(command, "@entry_job_level", character.CharacterInfo.MatchingProfile.EntryJobLevel);
            AddParameter(command, "@current_job", (byte) character.CharacterInfo.MatchingProfile.CurrentJob);
            AddParameter(command, "@current_job_level", character.CharacterInfo.MatchingProfile.CurrentJobLevel);
            AddParameter(command, "@objective_type1", character.CharacterInfo.MatchingProfile.ObjectiveType1);
            AddParameter(command, "@objective_type2", character.CharacterInfo.MatchingProfile.ObjectiveType2);
            AddParameter(command, "@play_style", character.CharacterInfo.MatchingProfile.PlayStyle);
            AddParameter(command, "@comment", character.CharacterInfo.MatchingProfile.Comment);
            AddParameter(command, "@is_join_party", character.CharacterInfo.MatchingProfile.IsJoinParty);
            // CDataArisenProfile
            AddParameter(command, "@background_id", character.CharacterInfo.ArisenProfile.BackgroundId);
            AddParameter(command, "@title_uid", character.CharacterInfo.ArisenProfile.Title.UId);
            AddParameter(command, "@title_index", character.CharacterInfo.ArisenProfile.Title.Index);
            AddParameter(command, "@motion_id", character.CharacterInfo.ArisenProfile.MotionId);
            AddParameter(command, "@motion_frame_no", character.CharacterInfo.ArisenProfile.MotionFrameNo);
        }
    }
}
