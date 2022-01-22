using System;
using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] CharacterFields = new string[]
        {
            "account_id", "first_name", "last_name", "created"
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

        private readonly string SqlInsertCharacter = $"INSERT INTO `ddon_character` ({BuildQueryField(CharacterFields, CDataEditInfoFields, CDataStatusInfoFields)}) VALUES ({BuildQueryInsert(CharacterFields, CDataEditInfoFields, CDataStatusInfoFields)});";
        private static readonly string SqlUpdateCharacter = $"UPDATE `ddon_character` SET {BuildQueryUpdate(CharacterFields, CDataEditInfoFields, CDataStatusInfoFields)} WHERE `id` = @id;";
        private static readonly string SqlSelectCharacter = $"SELECT `id`, {BuildQueryField(CharacterFields, CDataEditInfoFields, CDataStatusInfoFields)} FROM `ddon_character` WHERE `id` = @id;";
        private static readonly string SqlSelectCharactersByAccountId = $"SELECT `id`, {BuildQueryField(CharacterFields, CDataEditInfoFields, CDataStatusInfoFields)} FROM `ddon_character` WHERE `account_id` = @account_id;";
        private const string SqlDeleteCharacter = "DELETE FROM `ddon_character` WHERE `id`=@id;";

        public bool CreateCharacter(Character character)
        {
            character.Created = DateTime.Now;
            int rowsAffected = ExecuteNonQuery(SqlInsertCharacter, command => { AddParameter(command, character); }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            character.Id = (uint) autoIncrement;
            return true;
        }

        public bool UpdateCharacter(Character character)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateCharacter, command =>
            {
                AddParameter(command, "@id", character.Id);
                AddParameter(command, character);
            });
            return rowsAffected > NoRowsAffected;
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

        private Character ReadCharacter(DbDataReader reader)
        {
            Character character = new Character();
            character.Id = GetUInt32(reader, "id");

            character.AccountId = GetInt32(reader, "account_id");
            character.FirstName = GetString(reader, "first_name");
            character.LastName = GetString(reader, "last_name");
            character.Created = GetDateTime(reader, "created");

            character.Visual.Sex = GetByte(reader, "sex");
            character.Visual.Voice = GetByte(reader, "voice");
            character.Visual.VoicePitch = GetUInt16(reader, "voice_pitch");
            character.Visual.Personality = GetByte(reader, "personality");
            character.Visual.SpeechFreq = GetByte(reader, "speech_freq");
            character.Visual.BodyType = GetByte(reader, "body_type");
            character.Visual.Hair = GetByte(reader, "hair");
            character.Visual.Beard = GetByte(reader, "beard");
            character.Visual.Makeup = GetByte(reader, "makeup");
            character.Visual.Scar = GetByte(reader, "scar");
            character.Visual.EyePresetNo = GetByte(reader, "eye_preset_no");
            character.Visual.NosePresetNo = GetByte(reader, "nose_preset_no");
            character.Visual.MouthPresetNo = GetByte(reader, "mouth_preset_no");
            character.Visual.EyebrowTexNo = GetByte(reader, "eyebrow_tex_no");
            character.Visual.ColorSkin = GetByte(reader, "color_skin");
            character.Visual.ColorHair = GetByte(reader, "color_hair");
            character.Visual.ColorBeard = GetByte(reader, "color_beard");
            character.Visual.ColorEyebrow = GetByte(reader, "color_eyebrow");
            character.Visual.ColorREye = GetByte(reader, "color_r_eye");
            character.Visual.ColorLEye = GetByte(reader, "color_l_eye");
            character.Visual.ColorMakeup = GetByte(reader, "color_makeup");
            character.Visual.Sokutobu = GetUInt16(reader, "sokutobu");
            character.Visual.Hitai = GetUInt16(reader, "hitai");
            character.Visual.MimiJyouge = GetUInt16(reader, "mimi_jyouge");
            character.Visual.Kannkaku = GetUInt16(reader, "kannkaku");
            character.Visual.MabisasiJyouge = GetUInt16(reader, "mabisasi_jyouge");
            character.Visual.HanakuchiJyouge = GetUInt16(reader, "hanakuchi_jyouge");
            character.Visual.AgoSakiHaba = GetUInt16(reader, "ago_saki_haba");
            character.Visual.AgoZengo = GetUInt16(reader, "ago_zengo");
            character.Visual.AgoSakiJyouge = GetUInt16(reader, "ago_saki_jyouge");
            character.Visual.HitomiOokisa = GetUInt16(reader, "hitomi_ookisa");
            character.Visual.MeOokisa = GetUInt16(reader, "me_ookisa");
            character.Visual.MeKaiten = GetUInt16(reader, "me_kaiten");
            character.Visual.MayuKaiten = GetUInt16(reader, "mayu_kaiten");
            character.Visual.MimiOokisa = GetUInt16(reader, "mimi_ookisa");
            character.Visual.MimiMuki = GetUInt16(reader, "mimi_muki");
            character.Visual.ElfMimi = GetUInt16(reader, "elf_mimi");
            character.Visual.MikenTakasa = GetUInt16(reader, "miken_takasa");
            character.Visual.MikenHaba = GetUInt16(reader, "miken_haba");
            character.Visual.HohoboneRyou = GetUInt16(reader, "hohobone_ryou");
            character.Visual.HohoboneJyouge = GetUInt16(reader, "hohobone_jyouge");
            character.Visual.Hohoniku = GetUInt16(reader, "hohoniku");
            character.Visual.ErahoneJyouge = GetUInt16(reader, "erahone_jyouge");
            character.Visual.ErahoneHaba = GetUInt16(reader, "erahone_haba");
            character.Visual.HanaJyouge = GetUInt16(reader, "hana_jyouge");
            character.Visual.HanaHaba = GetUInt16(reader, "hana_haba");
            character.Visual.HanaTakasa = GetUInt16(reader, "hana_takasa");
            character.Visual.HanaKakudo = GetUInt16(reader, "hana_kakudo");
            character.Visual.KuchiHaba = GetUInt16(reader, "kuchi_haba");
            character.Visual.KuchiAtsusa = GetUInt16(reader, "kuchi_atsusa");
            character.Visual.EyebrowUVOffsetX = GetUInt16(reader, "eyebrow_uv_offset_x");
            character.Visual.EyebrowUVOffsetY = GetUInt16(reader, "eyebrow_uv_offset_y");
            character.Visual.Wrinkle = GetUInt16(reader, "wrinkle");
            character.Visual.WrinkleAlbedoBlendRate = GetUInt16(reader, "wrinkle_albedo_blend_rate");
            character.Visual.WrinkleDetailNormalPower = GetUInt16(reader, "wrinkle_detail_normal_power");
            character.Visual.MuscleAlbedoBlendRate = GetUInt16(reader, "muscle_albedo_blend_rate");
            character.Visual.MuscleDetailNormalPower = GetUInt16(reader, "muscle_detail_normal_power");
            character.Visual.Height = GetUInt16(reader, "height");
            character.Visual.HeadSize = GetUInt16(reader, "head_size");
            character.Visual.NeckOffset = GetUInt16(reader, "neck_offset");
            character.Visual.NeckScale = GetUInt16(reader, "neck_scale");
            character.Visual.UpperBodyScaleX = GetUInt16(reader, "upper_body_scale_x");
            character.Visual.BellySize = GetUInt16(reader, "belly_size");
            character.Visual.TeatScale = GetUInt16(reader, "teat_scale");
            character.Visual.TekubiSize = GetUInt16(reader, "tekubi_size");
            character.Visual.KoshiOffset = GetUInt16(reader, "koshi_offset");
            character.Visual.KoshiSize = GetUInt16(reader, "koshi_size");
            character.Visual.AnkleOffset = GetUInt16(reader, "ankle_offset");
            character.Visual.Fat = GetUInt16(reader, "fat");
            character.Visual.Muscle = GetUInt16(reader, "muscle");
            character.Visual.MotionFilter = GetUInt16(reader, "motion_filter");

            character.Status.HP = GetUInt32(reader, "hp");
            character.Status.Stamina = GetUInt32(reader, "stamina");
            character.Status.RevivePoint = GetByte(reader, "revive_point");
            character.Status.MaxHP = GetUInt32(reader, "max_hp");
            character.Status.MaxStamina = GetUInt32(reader, "max_stamina");
            character.Status.WhiteHP = GetUInt32(reader, "white_hp");
            character.Status.GainHP = GetUInt32(reader, "gain_hp");
            character.Status.GainStamina = GetUInt32(reader, "gain_stamina");
            character.Status.GainAttack = GetUInt32(reader, "gain_attack");
            character.Status.GainDefense = GetUInt32(reader, "gain_defense");
            character.Status.GainMagicAttack = GetUInt32(reader, "gain_magic_attack");
            character.Status.GainMagicDefense = GetUInt32(reader, "gain_magic_defense");

            return character;
        }

        private void AddParameter(TCom command, Character character)
        {
            // CharacterFields
            AddParameter(command, "@account_id", character.AccountId);
            AddParameter(command, "@first_name", character.FirstName);
            AddParameter(command, "@last_name", character.LastName);
            AddParameter(command, "@created", character.Created);
            // CDataEditInfoFields
            AddParameter(command, "@sex", character.Visual.Sex);
            AddParameter(command, "@voice", character.Visual.Voice);
            AddParameter(command, "@voice_pitch", character.Visual.VoicePitch);
            AddParameter(command, "@personality", character.Visual.Personality);
            AddParameter(command, "@speech_freq", character.Visual.SpeechFreq);
            AddParameter(command, "@body_type", character.Visual.BodyType);
            AddParameter(command, "@hair", character.Visual.Hair);
            AddParameter(command, "@beard", character.Visual.Beard);
            AddParameter(command, "@makeup", character.Visual.Makeup);
            AddParameter(command, "@scar", character.Visual.Scar);
            AddParameter(command, "@eye_preset_no", character.Visual.EyePresetNo);
            AddParameter(command, "@nose_preset_no", character.Visual.NosePresetNo);
            AddParameter(command, "@mouth_preset_no", character.Visual.MouthPresetNo);
            AddParameter(command, "@eyebrow_tex_no", character.Visual.EyebrowTexNo);
            AddParameter(command, "@color_skin", character.Visual.ColorSkin);
            AddParameter(command, "@color_hair", character.Visual.ColorHair);
            AddParameter(command, "@color_beard", character.Visual.ColorBeard);
            AddParameter(command, "@color_eyebrow", character.Visual.ColorEyebrow);
            AddParameter(command, "@color_r_eye", character.Visual.ColorREye);
            AddParameter(command, "@color_l_eye", character.Visual.ColorLEye);
            AddParameter(command, "@color_makeup", character.Visual.ColorMakeup);
            AddParameter(command, "@sokutobu", character.Visual.Sokutobu);
            AddParameter(command, "@hitai", character.Visual.Hitai);
            AddParameter(command, "@mimi_jyouge", character.Visual.MimiJyouge);
            AddParameter(command, "@kannkaku", character.Visual.Kannkaku);
            AddParameter(command, "@mabisasi_jyouge", character.Visual.MabisasiJyouge);
            AddParameter(command, "@hanakuchi_jyouge", character.Visual.HanakuchiJyouge);
            AddParameter(command, "@ago_saki_haba", character.Visual.AgoSakiHaba);
            AddParameter(command, "@ago_zengo", character.Visual.AgoZengo);
            AddParameter(command, "@ago_saki_jyouge", character.Visual.AgoSakiJyouge);
            AddParameter(command, "@hitomi_ookisa", character.Visual.HitomiOokisa);
            AddParameter(command, "@me_ookisa", character.Visual.MeOokisa);
            AddParameter(command, "@me_kaiten", character.Visual.MeKaiten);
            AddParameter(command, "@mayu_kaiten", character.Visual.MayuKaiten);
            AddParameter(command, "@mimi_ookisa", character.Visual.MimiOokisa);
            AddParameter(command, "@mimi_muki", character.Visual.MimiMuki);
            AddParameter(command, "@elf_mimi", character.Visual.ElfMimi);
            AddParameter(command, "@miken_takasa", character.Visual.MikenTakasa);
            AddParameter(command, "@miken_haba", character.Visual.MikenHaba);
            AddParameter(command, "@hohobone_ryou", character.Visual.HohoboneRyou);
            AddParameter(command, "@hohobone_jyouge", character.Visual.HohoboneJyouge);
            AddParameter(command, "@hohoniku", character.Visual.Hohoniku);
            AddParameter(command, "@erahone_jyouge", character.Visual.ErahoneJyouge);
            AddParameter(command, "@erahone_haba", character.Visual.ErahoneHaba);
            AddParameter(command, "@hana_jyouge", character.Visual.HanaJyouge);
            AddParameter(command, "@hana_haba", character.Visual.HanaHaba);
            AddParameter(command, "@hana_takasa", character.Visual.HanaTakasa);
            AddParameter(command, "@hana_kakudo", character.Visual.HanaKakudo);
            AddParameter(command, "@kuchi_haba", character.Visual.KuchiHaba);
            AddParameter(command, "@kuchi_atsusa", character.Visual.KuchiAtsusa);
            AddParameter(command, "@eyebrow_uv_offset_x", character.Visual.EyebrowUVOffsetX);
            AddParameter(command, "@eyebrow_uv_offset_y", character.Visual.EyebrowUVOffsetY);
            AddParameter(command, "@wrinkle", character.Visual.Wrinkle);
            AddParameter(command, "@wrinkle_albedo_blend_rate", character.Visual.WrinkleAlbedoBlendRate);
            AddParameter(command, "@wrinkle_detail_normal_power", character.Visual.WrinkleDetailNormalPower);
            AddParameter(command, "@muscle_albedo_blend_rate", character.Visual.MuscleAlbedoBlendRate);
            AddParameter(command, "@muscle_detail_normal_power", character.Visual.MuscleDetailNormalPower);
            AddParameter(command, "@height", character.Visual.Height);
            AddParameter(command, "@head_size", character.Visual.HeadSize);
            AddParameter(command, "@neck_offset", character.Visual.NeckOffset);
            AddParameter(command, "@neck_scale", character.Visual.NeckScale);
            AddParameter(command, "@upper_body_scale_x", character.Visual.UpperBodyScaleX);
            AddParameter(command, "@belly_size", character.Visual.BellySize);
            AddParameter(command, "@teat_scale", character.Visual.TeatScale);
            AddParameter(command, "@tekubi_size", character.Visual.TekubiSize);
            AddParameter(command, "@koshi_offset", character.Visual.KoshiOffset);
            AddParameter(command, "@koshi_size", character.Visual.KoshiSize);
            AddParameter(command, "@ankle_offset", character.Visual.AnkleOffset);
            AddParameter(command, "@fat", character.Visual.Fat);
            AddParameter(command, "@muscle", character.Visual.Muscle);
            AddParameter(command, "@motion_filter", character.Visual.MotionFilter);
            // CDataStatusInfoFields
            AddParameter(command, "@hp", character.Status.HP);
            AddParameter(command, "@stamina", character.Status.Stamina);
            AddParameter(command, "@revive_point", character.Status.RevivePoint);
            AddParameter(command, "@max_hp", character.Status.MaxHP);
            AddParameter(command, "@max_stamina", character.Status.MaxStamina);
            AddParameter(command, "@white_hp", character.Status.WhiteHP);
            AddParameter(command, "@gain_hp", character.Status.GainHP);
            AddParameter(command, "@gain_stamina", character.Status.GainStamina);
            AddParameter(command, "@gain_attack", character.Status.GainAttack);
            AddParameter(command, "@gain_defense", character.Status.GainDefense);
            AddParameter(command, "@gain_magic_attack", character.Status.GainMagicAttack);
            AddParameter(command, "@gain_magic_defense", character.Status.GainMagicDefense);
        }
    }
}
