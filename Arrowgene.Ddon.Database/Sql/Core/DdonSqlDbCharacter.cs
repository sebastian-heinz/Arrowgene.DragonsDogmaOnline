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
            "sex", "voice", "voice_pitch", "personality", "speech_freq", "body_type", "hair", "beard", "makeup", "scar",
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
            "hp", "stamina", "revive_point", "max_hp", "max_stamina", "white_hp", "gain_hp", "gain_stamina",
            "gain_attack", "gain_defense", "gain_magic_attack", "gain_magic_defense"
        };

        private static readonly string[] CDataMatchingProfileFields = new string[]
        {
            "matching_profile_entry_job", "matching_profile_entry_job_level", "matching_profile_current_job", 
            "matching_profile_current_job_level", "matching_profile_objective_type1", "matching_profile_objective_type2", 
            "matching_profile_play_style", "matching_profile_comment", "matching_profile_is_join_party"
        };

        private static readonly string[]  CDataArisenProfileFields = new string[]
        {
            "arisen_profile_background_id", "arisen_profile_title_uid", "arisen_profile_title_index", "arisen_profile_motion_id", 
            "arisen_profile_motion_frame_no"
        };

        private readonly string SqlInsertCharacter = $"INSERT INTO `ddon_character` ({BuildQueryField(CharacterFields, CDataEditInfoFields, CDataStatusInfoFields, CDataMatchingProfileFields, CDataArisenProfileFields)}) VALUES ({BuildQueryInsert(CharacterFields, CDataEditInfoFields, CDataStatusInfoFields, CDataMatchingProfileFields, CDataArisenProfileFields)});";
        private static readonly string SqlUpdateCharacter = $"UPDATE `ddon_character` SET {BuildQueryUpdate(CharacterFields, CDataEditInfoFields, CDataStatusInfoFields, CDataMatchingProfileFields, CDataArisenProfileFields)} WHERE `id` = @id;";
        private static readonly string SqlSelectCharacter = $"SELECT `id`, {BuildQueryField(CharacterFields, CDataEditInfoFields, CDataStatusInfoFields, CDataMatchingProfileFields, CDataArisenProfileFields)} FROM `ddon_character` WHERE `id` = @id;";
        private static readonly string SqlSelectCharactersByAccountId = $"SELECT `id`, {BuildQueryField(CharacterFields, CDataEditInfoFields, CDataStatusInfoFields, CDataMatchingProfileFields, CDataArisenProfileFields)} FROM `ddon_character` WHERE `account_id` = @account_id;";
        private const string SqlDeleteCharacter = "DELETE FROM `ddon_character` WHERE `id`=@id;";


        private static readonly string[] CDataCharacterJobDataFields = new string[]
        {
            "character_id", "job", "exp", "job_point", "lv", "atk", "def", "m_atk", "m_def", "strength", "down_power", "shake_power", 
            "stun_power", "consitution", "guts", "fire_resist", "ice_resist", "thunder_resist", "holy_resist", "dark_resist", "spread_resist", 
            "freeze_resist", "shock_resist", "absorb_resist", "dark_elm_resist", "poison_resist", "slow_resist", "sleep_resist", "stun_resist", 
            "wet_resist", "oil_resist", "seal_resist", "curse_resist", "soft_resist", "stone_resist", "gold_resist", "fire_reduce_resist", 
            "ice_reduce_resist", "thunder_reduce_resist", "holy_reduce_resist", "dark_reduce_resist", "atk_down_resist", "def_down_resist", 
            "m_atk_down_resist", "m_def_down_resist"
        };

        private readonly string SqlInsertCharacterJobData = $"INSERT OR REPLACE INTO `ddon_character_job_data` ({BuildQueryField(CDataCharacterJobDataFields)}) VALUES ({BuildQueryInsert(CDataCharacterJobDataFields)});";
        private static readonly string SqlSelectCharacterJobData = $"SELECT {BuildQueryField(CDataCharacterJobDataFields)} FROM `ddon_character_job_data` WHERE `character_id` = @character_id AND `job` = @job;";
        private static readonly string SqlSelectCharacterJobDataByCharacter = $"SELECT {BuildQueryField(CDataCharacterJobDataFields)} FROM `ddon_character_job_data` WHERE `character_id` = @character_id;";
        private const string SqlDeleteCharacterJobData = "DELETE FROM `ddon_character_job_data` WHERE `character_id`=@character_id AND `job` = @job;";
        

        private static readonly string[] CDataEquipItemInfoFields = new string[]
        {
            "character_id", "job", "item_id", "equip_type", "equip_slot", "color", "plus_value"
        };

        private readonly string SqlInsertEquipItemInfo = $"INSERT OR REPLACE INTO `ddon_equip_item_info` ({BuildQueryField(CDataEquipItemInfoFields)}) VALUES ({BuildQueryInsert(CDataEquipItemInfoFields)});";
        private static readonly string SqlSelectEquipItemInfo = $"SELECT {BuildQueryField(CDataEquipItemInfoFields)} FROM `ddon_equip_item_info` WHERE `character_id` = @character_id AND `job` = @job AND `equip_type`=1;";
        private static readonly string SqlSelectVisualEquipItemInfo = $"SELECT {BuildQueryField(CDataEquipItemInfoFields)} FROM `ddon_equip_item_info` WHERE `character_id` = @character_id AND `job` = @job AND `equip_type`=2;";
        private const string SqlDeleteEquipItemInfo = "DELETE FROM `ddon_equip_item_info` WHERE `character_id`=@character_id AND `job`=@job AND `equip_type`=@equip_type AND `equip_slot`=@equip_slot;";


        private static readonly string[] CDataEquipJobItemFields = new string[]
        {
            "character_id", "job", "job_item_id", "equip_slot_no"
        };

        private readonly string SqlInsertEquipJobItem = $"INSERT OR REPLACE INTO `ddon_equip_job_item` ({BuildQueryField(CDataEquipJobItemFields)}) VALUES ({BuildQueryInsert(CDataEquipJobItemFields)});";
        private static readonly string SqlSelectEquipJobItem = $"SELECT {BuildQueryField(CDataEquipJobItemFields)} FROM `ddon_equip_job_item` WHERE `character_id` = @character_id AND `job` = @job;";
        private const string SqlDeleteEquipJobItem = "DELETE FROM `ddon_equip_job_item` WHERE `character_id`=@character_id AND `job`=@job AND `equip_slot_no`=@equip_slot_no;";


        private static readonly string[] CDataSetAcquirementParamFields = new string[]
        {
            "character_id", "job", "type", "slot_no", "acquirement_no", "acquirement_lv"
        };

        private readonly string SqlInsertSetAcquirementParam = $"INSERT OR REPLACE INTO `ddon_set_acquirement_param` ({BuildQueryField(CDataSetAcquirementParamFields)}) VALUES ({BuildQueryInsert(CDataSetAcquirementParamFields)});";
        private static readonly string SqlSelectSetAcquirementParam = $"SELECT {BuildQueryField(CDataSetAcquirementParamFields)} FROM `ddon_set_acquirement_param` WHERE `character_id` = @character_id;";
        private const string SqlDeleteSetAcquirementParam = "DELETE FROM `ddon_set_acquirement_param` WHERE `character_id`=@character_id AND `job`=@job AND `slot_no`=@slot_no;";


        public bool CreateCharacter(Character character)
        {
            character.Created = DateTime.Now;
            int rowsAffected = ExecuteNonQuery(SqlInsertCharacter, command => { AddParameter(command, character); }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }
            character.Id = (uint) autoIncrement;

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

            StoreCharacterData(character);

            return characterUpdateRowsAffected > NoRowsAffected;
        }

        public Character SelectCharacter(uint characterId)
        {
            Character character = null;
            ExecuteReader(SqlSelectCharacter,
                command => { AddParameter(command, "@id", characterId); }, reader =>
                {
                    if (reader.Read())
                    {
                        character = ReadCharacter(reader);
                    }
                });

            QueryCharacterData(character);

            return character;
        }

        public List<Character> SelectCharactersByAccountId(int accountId)
        {
            List<Character> characters = new List<Character>();
            ExecuteReader(SqlSelectCharactersByAccountId,
                command => { AddParameter(command, "@account_id", accountId); }, reader =>
                {
                    while (reader.Read())
                    {
                        Character character = ReadCharacter(reader);
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

            ExecuteReader(SqlSelectSetAcquirementParam,
                command => { AddParameter(command, "@character_id", character.Id); },
                reader =>
                {
                    while (reader.Read())
                    {
                        character.CustomSkills.Add(ReadSetAcquirementParam(reader));
                    }
                });
        }

        private void StoreCharacterData(Character character)
        {
            foreach(CDataCharacterJobData characterJobData in character.CharacterInfo.CharacterJobDataList)
            {
                ExecuteNonQuery(SqlInsertCharacterJobData, command =>
                {
                    AddParameter(command, character.Id, characterJobData);
                });
            }

            foreach(CDataCharacterEquipData characterEquipData in character.CharacterInfo.CharacterEquipDataList)
            {
                foreach(CDataEquipItemInfo equipItemInfo in characterEquipData.Equips)
                {
                    ExecuteNonQuery(SqlInsertEquipItemInfo, command =>
                    {
                        AddParameter(command, character.Id, character.CharacterInfo.Job, equipItemInfo);
                    });
                }
            }

            foreach(CDataCharacterEquipData characterEquipData in character.CharacterInfo.CharacterEquipViewDataList)
            {
                foreach(CDataEquipItemInfo equipItemInfo in characterEquipData.Equips)
                {
                    ExecuteNonQuery(SqlInsertEquipItemInfo, command =>
                    {
                        AddParameter(command, character.Id, character.CharacterInfo.Job, equipItemInfo);
                    });
                }
            }

            foreach(CDataEquipJobItem equipJobItem in character.CharacterInfo.CharacterEquipJobItemList)
            {
                ExecuteNonQuery(SqlInsertEquipJobItem, command =>
                {
                    AddParameter(command, character.Id, character.CharacterInfo.Job, equipJobItem);
                });
            }

            foreach(CDataSetAcquirementParam setAcquirementParam in character.CustomSkills)
            {
                ExecuteNonQuery(SqlInsertSetAcquirementParam, command =>
                {
                    AddParameter(command, character.Id, setAcquirementParam);
                });
            }
        }

        private Character ReadCharacter(DbDataReader reader)
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

            character.CharacterInfo.MatchingProfile.EntryJob = (JobId) GetByte(reader, "matching_profile_entry_job");
            character.CharacterInfo.MatchingProfile.EntryJobLevel = GetUInt32(reader, "matching_profile_entry_job_level");
            character.CharacterInfo.MatchingProfile.CurrentJob = (JobId) GetByte(reader, "matching_profile_current_job");
            character.CharacterInfo.MatchingProfile.CurrentJobLevel = GetUInt32(reader, "matching_profile_current_job_level");
            character.CharacterInfo.MatchingProfile.ObjectiveType1 = GetUInt32(reader, "matching_profile_objective_type1");
            character.CharacterInfo.MatchingProfile.ObjectiveType2 = GetUInt32(reader, "matching_profile_objective_type2");
            character.CharacterInfo.MatchingProfile.PlayStyle = GetUInt32(reader, "matching_profile_play_style");
            character.CharacterInfo.MatchingProfile.Comment = GetString(reader, "matching_profile_comment");
            character.CharacterInfo.MatchingProfile.IsJoinParty = GetByte(reader, "matching_profile_is_join_party");

            character.CharacterInfo.ArisenProfile.BackgroundId = GetByte(reader, "arisen_profile_background_id");
            character.CharacterInfo.ArisenProfile.Title.UId = GetUInt32(reader, "arisen_profile_title_uid");
            character.CharacterInfo.ArisenProfile.Title.Index = GetUInt32(reader, "arisen_profile_title_index");
            character.CharacterInfo.ArisenProfile.MotionId = GetUInt16(reader, "arisen_profile_motion_id");
            character.CharacterInfo.ArisenProfile.MotionFrameNo = GetUInt32(reader, "arisen_profile_motion_frame_no");

            return character;
        }

        private void AddParameter(TCom command, Character character)
        {
            // CharacterFields
            AddParameter(command, "@account_id", character.AccountId);
            AddParameter(command, "@version", character.CharacterInfo.Version);
            AddParameter(command, "@first_name", character.CharacterInfo.FirstName);
            AddParameter(command, "@last_name", character.CharacterInfo.LastName);
            AddParameter(command, "@created", character.Created);
            AddParameter(command, "job", (byte) character.CharacterInfo.Job);
            AddParameter(command, "jewelry_slot_num", character.CharacterInfo.JewelrySlotNum);
            AddParameter(command, "my_pawn_slot_num", character.CharacterInfo.MyPawnSlotNum);
            AddParameter(command, "rental_pawn_slot_num", character.CharacterInfo.RentalPawnSlotNum);
            AddParameter(command, "hide_equip_head", character.CharacterInfo.HideEquipHead);
            AddParameter(command, "hide_equip_lantern", character.CharacterInfo.HideEquipLantern);
            AddParameter(command, "hide_equip_head_pawn", character.CharacterInfo.HideEquipHeadPawn);
            AddParameter(command, "hide_equip_lantern_pawn", character.CharacterInfo.HideEquipLanternPawn);
            AddParameter(command, "arisen_profile_share_range", character.CharacterInfo.ArisenProfileShareRange);
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
            AddParameter(command, "matching_profile_entry_job", (byte) character.CharacterInfo.MatchingProfile.EntryJob);
            AddParameter(command, "matching_profile_entry_job_level", character.CharacterInfo.MatchingProfile.EntryJobLevel);
            AddParameter(command, "matching_profile_current_job", (byte) character.CharacterInfo.MatchingProfile.CurrentJob);
            AddParameter(command, "matching_profile_current_job_level", character.CharacterInfo.MatchingProfile.CurrentJobLevel);
            AddParameter(command, "matching_profile_objective_type1", character.CharacterInfo.MatchingProfile.ObjectiveType1);
            AddParameter(command, "matching_profile_objective_type2", character.CharacterInfo.MatchingProfile.ObjectiveType2);
            AddParameter(command, "matching_profile_play_style", character.CharacterInfo.MatchingProfile.PlayStyle);
            AddParameter(command, "matching_profile_comment", character.CharacterInfo.MatchingProfile.Comment);
            AddParameter(command, "matching_profile_is_join_party", character.CharacterInfo.MatchingProfile.IsJoinParty);
            // CDataArisenProfile
            AddParameter(command, "arisen_profile_background_id", character.CharacterInfo.ArisenProfile.BackgroundId);
            AddParameter(command, "arisen_profile_title_uid", character.CharacterInfo.ArisenProfile.Title.UId);
            AddParameter(command, "arisen_profile_title_index", character.CharacterInfo.ArisenProfile.Title.Index);
            AddParameter(command, "arisen_profile_motion_id", character.CharacterInfo.ArisenProfile.MotionId);
            AddParameter(command, "arisen_profile_motion_frame_no", character.CharacterInfo.ArisenProfile.MotionFrameNo);
        }
        private CDataCharacterJobData ReadCharacterJobData(DbDataReader reader)
        {
            CDataCharacterJobData characterJobData = new CDataCharacterJobData();
            characterJobData.Job = (JobId) GetByte(reader, "job");
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

        private void AddParameter(TCom command, uint characterId, CDataCharacterJobData characterJobData)
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "job", (byte) characterJobData.Job);
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

        private CDataEquipItemInfo ReadEquipItemInfo(DbDataReader reader)
        {
            CDataEquipItemInfo equipItemInfo = new CDataEquipItemInfo();
            equipItemInfo.ItemId = GetUInt32(reader, "item_id");
            equipItemInfo.EquipType = GetByte(reader, "equip_type");
            equipItemInfo.EquipSlot = GetUInt16(reader, "equip_slot");
            equipItemInfo.Color = GetByte(reader, "color");
            equipItemInfo.PlusValue = GetByte(reader, "plus_value");
            return equipItemInfo;
        }

        private void AddParameter(TCom command, uint characterId, JobId job, CDataEquipItemInfo equipItemInfo)
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "job", (byte) job);
            AddParameter(command, "item_id", equipItemInfo.ItemId);
            AddParameter(command, "equip_type", equipItemInfo.EquipType);
            AddParameter(command, "equip_slot", equipItemInfo.EquipSlot);
            AddParameter(command, "color", equipItemInfo.Color);
            AddParameter(command, "plus_value", equipItemInfo.PlusValue);
        }

        private CDataEquipJobItem ReadEquipJobItem(DbDataReader reader)
        {
            CDataEquipJobItem equipJobItem = new CDataEquipJobItem();
            equipJobItem.JobItemId = GetUInt32(reader, "job_item_id");
            equipJobItem.EquipSlotNo = GetByte(reader, "equip_slot_no");
            return equipJobItem;
        }

        private void AddParameter(TCom command, uint characterId, JobId job, CDataEquipJobItem equipJobItem)
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "job", (byte) job);
            AddParameter(command, "job_item_id", equipJobItem.JobItemId);
            AddParameter(command, "equip_slot_no", equipJobItem.EquipSlotNo);
        }

        private CDataSetAcquirementParam ReadSetAcquirementParam(DbDataReader reader)
        {
            CDataSetAcquirementParam setAcquirementParam = new CDataSetAcquirementParam();
            setAcquirementParam.Job = (JobId) GetByte(reader, "job");
            setAcquirementParam.Type = GetByte(reader, "type");
            setAcquirementParam.SlotNo = GetByte(reader, "slot_no");
            setAcquirementParam.AcquirementNo = GetUInt32(reader, "acquirement_no");
            setAcquirementParam.AcquirementLv = GetByte(reader, "acquirement_lv");
            return setAcquirementParam;
        }

        private void AddParameter(TCom command, uint characterId, CDataSetAcquirementParam setAcquirementParam)
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "job", (byte) setAcquirementParam.Job);
            AddParameter(command, "type", setAcquirementParam.Type);
            AddParameter(command, "slot_no", setAcquirementParam.SlotNo);
            AddParameter(command, "acquirement_no", setAcquirementParam.AcquirementNo);
            AddParameter(command, "acquirement_lv", setAcquirementParam.AcquirementLv);
        }
    }
}
