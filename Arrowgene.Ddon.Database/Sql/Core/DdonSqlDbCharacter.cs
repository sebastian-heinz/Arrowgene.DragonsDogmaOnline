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

        // Im not convinced most of these fields has to be stored in DB
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
            return ExecuteInTransaction(conn =>
                {
                    character.Created = DateTime.Now;
                    ExecuteNonQuery(conn, SqlInsertCharacter, command => { AddParameter(command, character); }, out long autoIncrement);
                    character.Id = (uint) autoIncrement;

                    ExecuteNonQuery(conn, SqlInsertCharacterEditInfo, command => { AddParameter(command, character); });
                    ExecuteNonQuery(conn, SqlInsertCharacterStatusInfo, command => { AddParameter(command, character); });
                    ExecuteNonQuery(conn, SqlInsertCharacterMatchingProfile, command => { AddParameter(command, character); });
                    ExecuteNonQuery(conn, SqlInsertCharacterArisenProfile, command => { AddParameter(command, character); });

                    StoreCharacterData(conn, character);

                    CreateItems(conn, character);
                });
        }

        public bool UpdateCharacterBaseInfo(Character character)
        {
            return UpdateCharacterBaseInfo(null, character);
        }

        public bool UpdateCharacterBaseInfo(TCon conn, Character character)
        {
            int characterUpdateRowsAffected = ExecuteNonQuery(conn, SqlUpdateCharacter, command =>
            {
                AddParameter(command, "@id", character.Id);
                AddParameter(command, character);
            });

            return characterUpdateRowsAffected > NoRowsAffected;
        }

        public bool UpdateCharacter(Character character)
        {
            return ExecuteInTransaction(conn =>
                {
                    UpdateCharacterBaseInfo(character);

                    ExecuteNonQuery(conn, SqlUpdateCharacterEditInfo, command =>
                    {
                        AddParameter(command, "@id", character.Id);
                        AddParameter(command, character);
                    });
                    ExecuteNonQuery(conn, SqlUpdateCharacterStatusInfo, command =>
                    {
                        AddParameter(command, "@id", character.Id);
                        AddParameter(command, character);
                    });
                    ExecuteNonQuery(conn, SqlUpdateCharacterMatchingProfile, command =>
                    {
                        AddParameter(command, "@id", character.Id);
                        AddParameter(command, character);
                    });
                    ExecuteNonQuery(conn, SqlUpdateCharacterArisenProfile, command =>
                    {
                        AddParameter(command, "@id", character.Id);
                        AddParameter(command, character);
                    });

                    StoreCharacterData(conn, character);

                    // TODO: Synchronize equipment items and storage items
                });
        }

        public Character SelectCharacter(uint characterId)
        {
            Character character = null;
            ExecuteInTransaction(conn => {
                ExecuteReader(conn, SqlSelectAllCharacterData,
                command => { AddParameter(command, "@id", characterId); }, reader =>
                {
                    if (reader.Read())
                    {
                        character = ReadAllCharacterData(reader);
                    }
                });

                QueryCharacterData(conn, character);
            });
            return character;
        }

        public List<Character> SelectCharactersByAccountId(int accountId)
        {
            List<Character> characters = new List<Character>();
            ExecuteInTransaction(conn => {
                ExecuteReader(conn, SqlSelectAllCharactersDataByAccountId,
                    command => { AddParameter(command, "@account_id", accountId); }, reader =>
                    {
                        while (reader.Read())
                        {
                            Character character = ReadAllCharacterData(reader);
                            characters.Add(character);

                            QueryCharacterData(conn, character);
                        }
                    });
            });
            return characters;
        }

        public bool DeleteCharacter(uint characterId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteCharacter,
                command => { AddParameter(command, "@id", characterId); });
            return rowsAffected > NoRowsAffected;
        }

        private void QueryCharacterData(TCon conn, Character character)
        {
            // Job data
            ExecuteReader(conn, SqlSelectCharacterJobDataByCharacter,
                command => { AddParameter(command, "@character_id", character.Id); }, reader =>
                {
                    while (reader.Read())
                    {
                        character.CharacterJobDataList.Add(ReadCharacterJobData(reader));
                    }
                });

            // Equips
            ExecuteReader(conn, SqlSelectEquipItemByCharacter,
                command => { AddParameter(command, "@character_id", character.Id); }, 
                reader =>
                {
                    while (reader.Read())
                    {
                        string UId = GetString(reader, "item_uid");
                        JobId job = (JobId) GetByte(reader, "job");
                        EquipType equipType = (EquipType) GetByte(reader, "equip_type");
                        byte equipSlot = GetByte(reader, "equip_slot");
                        ExecuteReader(conn, SqlSelectItem,
                            command2 => { AddParameter(command2, "@uid", UId); },
                            reader2 => 
                            {
                                if(reader2.Read())
                                {
                                    Item item = ReadItem(reader2);
                                    character.Equipment.setEquipItem(item, job, equipType, equipSlot);
                                }
                            });
                    }
                });            

            // Job Items
            ExecuteReader(conn, SqlSelectEquipJobItemByCharacter,
                command => { AddParameter(command, "@character_id", character.Id); }, 
                reader =>
                {
                    while (reader.Read())
                    {
                        JobId job = (JobId) GetByte(reader, "job");
                        CDataEquipJobItem equipJobItem = ReadEquipJobItem(reader);
                        if(!character.CharacterEquipJobItemListDictionary.ContainsKey(job))
                        {
                            character.CharacterEquipJobItemListDictionary.Add(job, new List<CDataEquipJobItem>());
                        }
                        character.CharacterEquipJobItemListDictionary[job].Add(equipJobItem);
                    }
                });

            // Normal Skills
            ExecuteReader(conn, SqlSelectNormalSkillParam,
                command => { AddParameter(command, "@character_id", character.Id); },
                reader =>
                {
                    while (reader.Read())
                    {
                        character.NormalSkills.Add(ReadNormalSkillParam(reader));
                    }
                });

            // Custom Skills
            ExecuteReader(conn, SqlSelectEquippedCustomSkills,
                command => { AddParameter(command, "@character_id", character.Id); },
                reader =>
                {
                    while (reader.Read())
                    {
                        character.CustomSkills.Add(ReadCustomSkill(reader));
                    }
                });

            // Abilities
            ExecuteReader(conn, SqlSelectEquippedAbilities,
                command => { AddParameter(command, "@character_id", character.Id); },
                reader =>
                {
                    while (reader.Read())
                    {
                        character.Abilities.Add(ReadAbility(reader));
                    }
                });

            // Shortcuts
            ExecuteReader(conn, SqlSelectShortcuts,
                command => { AddParameter(command, "@character_id", character.Id); },
                reader =>
                {
                    while (reader.Read())
                    {
                        character.ShortCutList.Add(ReadShortCut(reader));
                    }
                });

            // CommunicationShortcuts
            ExecuteReader(conn, SqlSelectCommunicationShortcuts,
                command => { AddParameter(command, "@character_id", character.Id); },
                reader =>
                {
                    while (reader.Read())
                    {
                        character.CommunicationShortCutList.Add(ReadCommunicationShortCut(reader));
                    }
                });

            // Storage
            ExecuteReader(conn, SqlSelectAllStoragesByCharacter,
                command => { AddParameter(command, "@character_id", character.Id); },
                reader =>
                {
                    while (reader.Read())
                    {
                        Tuple<StorageType, Storage> tuple = ReadStorage(reader);
                        character.Storage.addStorage(tuple.Item1, tuple.Item2);
                    }

                    ExecuteReader(conn, SqlSelectStorageItemsByCharacter,
                    command2 => { AddParameter(command2, "@character_id", character.Id); },
                    reader2 =>
                    {
                        while(reader2.Read())
                        {
                            StorageType storageType = (StorageType) GetByte(reader2, "storage_type");
                            ushort slot = GetUInt16(reader2, "slot_no");
                            string UId = GetString(reader2, "item_uid");

                            ExecuteReader(conn, SqlSelectItem,
                                command3 => { AddParameter(command3, "@uid", UId); },
                                reader3 => 
                                {
                                    if(reader3.Read())
                                    {
                                        Item item = ReadItem(reader3);
                                        character.Storage.setStorageItem(item, storageType, slot);
                                    }
                                });
                        }
                    });
                });
        }

        private void StoreCharacterData(TCon conn, Character character)
        {
            foreach(CDataCharacterJobData characterJobData in character.CharacterJobDataList)
            {
                ExecuteNonQuery(conn, SqlReplaceCharacterJobData, command =>
                {
                    AddParameter(command, character.Id, characterJobData);
                });
            }

            foreach(KeyValuePair<JobId, List<CDataEquipJobItem>> characterEquipJobItemListByJob in character.CharacterEquipJobItemListDictionary)
            {
                foreach(CDataEquipJobItem equipJobItem in characterEquipJobItemListByJob.Value)
                {
                    ExecuteNonQuery(conn, SqlReplaceEquipJobItem, command =>
                    {
                        AddParameter(command, character.Id, characterEquipJobItemListByJob.Key, equipJobItem);
                    });
                }
            }

            foreach(CDataNormalSkillParam normalSkillParam in character.NormalSkills)
            {
                ExecuteNonQuery(conn, SqlReplaceNormalSkillParam, command =>
                {
                    AddParameter(command, character.Id, normalSkillParam);
                });
            }

            foreach(CustomSkill skill in character.CustomSkills)
            {
                ExecuteNonQuery(conn, SqlReplaceEquippedCustomSkill, command =>
                {
                    AddParameter(command, character.Id, skill);
                });
            }

            foreach(Ability ability in character.Abilities)
            {
                ExecuteNonQuery(conn, SqlReplaceEquippedAbility, command =>
                {
                    AddParameter(command, character.Id, ability);
                });
            }

            foreach(CDataShortCut shortcut in character.ShortCutList)
            {
                ExecuteNonQuery(conn, SqlReplaceShortcut, command =>
                {
                    AddParameter(command, character.Id, shortcut);
                });
            }

            foreach(CDataCommunicationShortCut communicationShortcut in character.CommunicationShortCutList)
            {
                ExecuteNonQuery(conn, SqlReplaceCommunicationShortcut, command =>
                {
                    AddParameter(command, character.Id, communicationShortcut);
                });
            }

            foreach(StorageType storageType in character.Storage.getAllStorages().Keys)
            {
                ExecuteNonQuery(conn, SqlReplaceStorage, command =>
                {
                    AddParameter(command, character.Id, storageType, character.Storage.getStorage(storageType));
                });
            }
        }

        private void CreateItems(TCon conn, Character character)
        {
            // Create storage items
            foreach (KeyValuePair<StorageType, Storage> storage in character.Storage.getAllStorages())
            {
                StorageType storageType = storage.Key;
                for(ushort index=0; index < storage.Value.Items.Count; index++)
                {
                    Item item = storage.Value.Items[index];
                    if(item != null)
                    {
                        ushort slot = (ushort)(index+1);
                        InsertItem(conn, item);
                        InsertStorageItem(conn, character.Id, storageType, slot, item.UId);
                    }
                }
            }

            // Create equipment items
            foreach (KeyValuePair<JobId, Dictionary<EquipType, List<Item>>> jobEquipment in character.Equipment.getAllEquipment())
            {
                JobId job = jobEquipment.Key;
                foreach (KeyValuePair<EquipType, List<Item>> equipment in jobEquipment.Value)
                {
                    EquipType equipType = equipment.Key;
                    for (byte index = 0; index < equipment.Value.Count; index++)
                    {
                        Item item = equipment.Value[index];
                        if(item != null)
                        {
                            byte slot = (byte)(index+1);
                            InsertItem(conn, item);
                            InsertEquipItem(conn, character.Id, job, equipType, slot, item.UId);
                        }
                    }
                }
            }
        }

        private Character ReadAllCharacterData(DbDataReader reader)
        {
            Character character = new Character();
            character.Id = GetUInt32(reader, "id");

            character.AccountId = GetInt32(reader, "account_id");
            character.Version = GetUInt32(reader, "version");
            character.FirstName = GetString(reader, "first_name");
            character.LastName = GetString(reader, "last_name");
            character.Created = GetDateTime(reader, "created");
            character.Job = (JobId) GetByte(reader, "job");
            character.JewelrySlotNum = GetByte(reader, "jewelry_slot_num");
            character.MyPawnSlotNum = GetByte(reader, "my_pawn_slot_num");
            character.RentalPawnSlotNum = GetByte(reader, "rental_pawn_slot_num");
            character.HideEquipHead = GetBoolean(reader, "hide_equip_head");
            character.HideEquipLantern = GetBoolean(reader, "hide_equip_lantern");
            character.HideEquipHeadPawn = GetBoolean(reader, "hide_equip_head_pawn");
            character.HideEquipLanternPawn = GetBoolean(reader, "hide_equip_lantern_pawn");
            character.ArisenProfileShareRange = GetByte(reader, "arisen_profile_share_range");

            character.EditInfo.Sex = GetByte(reader, "sex");
            character.EditInfo.Voice = GetByte(reader, "voice");
            character.EditInfo.VoicePitch = GetUInt16(reader, "voice_pitch");
            character.EditInfo.Personality = GetByte(reader, "personality");
            character.EditInfo.SpeechFreq = GetByte(reader, "speech_freq");
            character.EditInfo.BodyType = GetByte(reader, "body_type");
            character.EditInfo.Hair = GetByte(reader, "hair");
            character.EditInfo.Beard = GetByte(reader, "beard");
            character.EditInfo.Makeup = GetByte(reader, "makeup");
            character.EditInfo.Scar = GetByte(reader, "scar");
            character.EditInfo.EyePresetNo = GetByte(reader, "eye_preset_no");
            character.EditInfo.NosePresetNo = GetByte(reader, "nose_preset_no");
            character.EditInfo.MouthPresetNo = GetByte(reader, "mouth_preset_no");
            character.EditInfo.EyebrowTexNo = GetByte(reader, "eyebrow_tex_no");
            character.EditInfo.ColorSkin = GetByte(reader, "color_skin");
            character.EditInfo.ColorHair = GetByte(reader, "color_hair");
            character.EditInfo.ColorBeard = GetByte(reader, "color_beard");
            character.EditInfo.ColorEyebrow = GetByte(reader, "color_eyebrow");
            character.EditInfo.ColorREye = GetByte(reader, "color_r_eye");
            character.EditInfo.ColorLEye = GetByte(reader, "color_l_eye");
            character.EditInfo.ColorMakeup = GetByte(reader, "color_makeup");
            character.EditInfo.Sokutobu = GetUInt16(reader, "sokutobu");
            character.EditInfo.Hitai = GetUInt16(reader, "hitai");
            character.EditInfo.MimiJyouge = GetUInt16(reader, "mimi_jyouge");
            character.EditInfo.Kannkaku = GetUInt16(reader, "kannkaku");
            character.EditInfo.MabisasiJyouge = GetUInt16(reader, "mabisasi_jyouge");
            character.EditInfo.HanakuchiJyouge = GetUInt16(reader, "hanakuchi_jyouge");
            character.EditInfo.AgoSakiHaba = GetUInt16(reader, "ago_saki_haba");
            character.EditInfo.AgoZengo = GetUInt16(reader, "ago_zengo");
            character.EditInfo.AgoSakiJyouge = GetUInt16(reader, "ago_saki_jyouge");
            character.EditInfo.HitomiOokisa = GetUInt16(reader, "hitomi_ookisa");
            character.EditInfo.MeOokisa = GetUInt16(reader, "me_ookisa");
            character.EditInfo.MeKaiten = GetUInt16(reader, "me_kaiten");
            character.EditInfo.MayuKaiten = GetUInt16(reader, "mayu_kaiten");
            character.EditInfo.MimiOokisa = GetUInt16(reader, "mimi_ookisa");
            character.EditInfo.MimiMuki = GetUInt16(reader, "mimi_muki");
            character.EditInfo.ElfMimi = GetUInt16(reader, "elf_mimi");
            character.EditInfo.MikenTakasa = GetUInt16(reader, "miken_takasa");
            character.EditInfo.MikenHaba = GetUInt16(reader, "miken_haba");
            character.EditInfo.HohoboneRyou = GetUInt16(reader, "hohobone_ryou");
            character.EditInfo.HohoboneJyouge = GetUInt16(reader, "hohobone_jyouge");
            character.EditInfo.Hohoniku = GetUInt16(reader, "hohoniku");
            character.EditInfo.ErahoneJyouge = GetUInt16(reader, "erahone_jyouge");
            character.EditInfo.ErahoneHaba = GetUInt16(reader, "erahone_haba");
            character.EditInfo.HanaJyouge = GetUInt16(reader, "hana_jyouge");
            character.EditInfo.HanaHaba = GetUInt16(reader, "hana_haba");
            character.EditInfo.HanaTakasa = GetUInt16(reader, "hana_takasa");
            character.EditInfo.HanaKakudo = GetUInt16(reader, "hana_kakudo");
            character.EditInfo.KuchiHaba = GetUInt16(reader, "kuchi_haba");
            character.EditInfo.KuchiAtsusa = GetUInt16(reader, "kuchi_atsusa");
            character.EditInfo.EyebrowUVOffsetX = GetUInt16(reader, "eyebrow_uv_offset_x");
            character.EditInfo.EyebrowUVOffsetY = GetUInt16(reader, "eyebrow_uv_offset_y");
            character.EditInfo.Wrinkle = GetUInt16(reader, "wrinkle");
            character.EditInfo.WrinkleAlbedoBlendRate = GetUInt16(reader, "wrinkle_albedo_blend_rate");
            character.EditInfo.WrinkleDetailNormalPower = GetUInt16(reader, "wrinkle_detail_normal_power");
            character.EditInfo.MuscleAlbedoBlendRate = GetUInt16(reader, "muscle_albedo_blend_rate");
            character.EditInfo.MuscleDetailNormalPower = GetUInt16(reader, "muscle_detail_normal_power");
            character.EditInfo.Height = GetUInt16(reader, "height");
            character.EditInfo.HeadSize = GetUInt16(reader, "head_size");
            character.EditInfo.NeckOffset = GetUInt16(reader, "neck_offset");
            character.EditInfo.NeckScale = GetUInt16(reader, "neck_scale");
            character.EditInfo.UpperBodyScaleX = GetUInt16(reader, "upper_body_scale_x");
            character.EditInfo.BellySize = GetUInt16(reader, "belly_size");
            character.EditInfo.TeatScale = GetUInt16(reader, "teat_scale");
            character.EditInfo.TekubiSize = GetUInt16(reader, "tekubi_size");
            character.EditInfo.KoshiOffset = GetUInt16(reader, "koshi_offset");
            character.EditInfo.KoshiSize = GetUInt16(reader, "koshi_size");
            character.EditInfo.AnkleOffset = GetUInt16(reader, "ankle_offset");
            character.EditInfo.Fat = GetUInt16(reader, "fat");
            character.EditInfo.Muscle = GetUInt16(reader, "muscle");
            character.EditInfo.MotionFilter = GetUInt16(reader, "motion_filter");

            character.StatusInfo.HP = GetUInt32(reader, "hp");
            character.StatusInfo.Stamina = GetUInt32(reader, "stamina");
            character.StatusInfo.RevivePoint = GetByte(reader, "revive_point");
            character.StatusInfo.MaxHP = GetUInt32(reader, "max_hp");
            character.StatusInfo.MaxStamina = GetUInt32(reader, "max_stamina");
            character.StatusInfo.WhiteHP = GetUInt32(reader, "white_hp");
            character.StatusInfo.GainHP = GetUInt32(reader, "gain_hp");
            character.StatusInfo.GainStamina = GetUInt32(reader, "gain_stamina");
            character.StatusInfo.GainAttack = GetUInt32(reader, "gain_attack");
            character.StatusInfo.GainDefense = GetUInt32(reader, "gain_defense");
            character.StatusInfo.GainMagicAttack = GetUInt32(reader, "gain_magic_attack");
            character.StatusInfo.GainMagicDefense = GetUInt32(reader, "gain_magic_defense");

            character.MatchingProfile.EntryJob = (JobId) GetByte(reader, "entry_job");
            character.MatchingProfile.EntryJobLevel = GetUInt32(reader, "entry_job_level");
            character.MatchingProfile.CurrentJob = (JobId) GetByte(reader, "current_job");
            character.MatchingProfile.CurrentJobLevel = GetUInt32(reader, "current_job_level");
            character.MatchingProfile.ObjectiveType1 = GetUInt32(reader, "objective_type1");
            character.MatchingProfile.ObjectiveType2 = GetUInt32(reader, "objective_type2");
            character.MatchingProfile.PlayStyle = GetUInt32(reader, "play_style");
            character.MatchingProfile.Comment = GetString(reader, "comment");
            character.MatchingProfile.IsJoinParty = GetByte(reader, "is_join_party");

            character.ArisenProfile.BackgroundId = GetByte(reader, "background_id");
            character.ArisenProfile.Title.UId = GetUInt32(reader, "title_uid");
            character.ArisenProfile.Title.Index = GetUInt32(reader, "title_index");
            character.ArisenProfile.MotionId = GetUInt16(reader, "motion_id");
            character.ArisenProfile.MotionFrameNo = GetUInt32(reader, "motion_frame_no");

            return character;
        }

        private void AddParameter(TCom command, Character character)
        {
            // CharacterFields
            AddParameter(command, "@account_id", character.AccountId);
            AddParameter(command, "@character_id", character.Id);
            AddParameter(command, "@version", character.Version);
            AddParameter(command, "@first_name", character.FirstName);
            AddParameter(command, "@last_name", character.LastName);
            AddParameter(command, "@created", character.Created);
            AddParameter(command, "@job", (byte) character.Job);
            AddParameter(command, "@jewelry_slot_num", character.JewelrySlotNum);
            AddParameter(command, "@my_pawn_slot_num", character.MyPawnSlotNum);
            AddParameter(command, "@rental_pawn_slot_num", character.RentalPawnSlotNum);
            AddParameter(command, "@hide_equip_head", character.HideEquipHead);
            AddParameter(command, "@hide_equip_lantern", character.HideEquipLantern);
            AddParameter(command, "@hide_equip_head_pawn", character.HideEquipHeadPawn);
            AddParameter(command, "@hide_equip_lantern_pawn", character.HideEquipLanternPawn);
            AddParameter(command, "@arisen_profile_share_range", character.ArisenProfileShareRange);
            // CDataEditInfoFields
            AddParameter(command, "@sex", character.EditInfo.Sex);
            AddParameter(command, "@voice", character.EditInfo.Voice);
            AddParameter(command, "@voice_pitch", character.EditInfo.VoicePitch);
            AddParameter(command, "@personality", character.EditInfo.Personality);
            AddParameter(command, "@speech_freq", character.EditInfo.SpeechFreq);
            AddParameter(command, "@body_type", character.EditInfo.BodyType);
            AddParameter(command, "@hair", character.EditInfo.Hair);
            AddParameter(command, "@beard", character.EditInfo.Beard);
            AddParameter(command, "@makeup", character.EditInfo.Makeup);
            AddParameter(command, "@scar", character.EditInfo.Scar);
            AddParameter(command, "@eye_preset_no", character.EditInfo.EyePresetNo);
            AddParameter(command, "@nose_preset_no", character.EditInfo.NosePresetNo);
            AddParameter(command, "@mouth_preset_no", character.EditInfo.MouthPresetNo);
            AddParameter(command, "@eyebrow_tex_no", character.EditInfo.EyebrowTexNo);
            AddParameter(command, "@color_skin", character.EditInfo.ColorSkin);
            AddParameter(command, "@color_hair", character.EditInfo.ColorHair);
            AddParameter(command, "@color_beard", character.EditInfo.ColorBeard);
            AddParameter(command, "@color_eyebrow", character.EditInfo.ColorEyebrow);
            AddParameter(command, "@color_r_eye", character.EditInfo.ColorREye);
            AddParameter(command, "@color_l_eye", character.EditInfo.ColorLEye);
            AddParameter(command, "@color_makeup", character.EditInfo.ColorMakeup);
            AddParameter(command, "@sokutobu", character.EditInfo.Sokutobu);
            AddParameter(command, "@hitai", character.EditInfo.Hitai);
            AddParameter(command, "@mimi_jyouge", character.EditInfo.MimiJyouge);
            AddParameter(command, "@kannkaku", character.EditInfo.Kannkaku);
            AddParameter(command, "@mabisasi_jyouge", character.EditInfo.MabisasiJyouge);
            AddParameter(command, "@hanakuchi_jyouge", character.EditInfo.HanakuchiJyouge);
            AddParameter(command, "@ago_saki_haba", character.EditInfo.AgoSakiHaba);
            AddParameter(command, "@ago_zengo", character.EditInfo.AgoZengo);
            AddParameter(command, "@ago_saki_jyouge", character.EditInfo.AgoSakiJyouge);
            AddParameter(command, "@hitomi_ookisa", character.EditInfo.HitomiOokisa);
            AddParameter(command, "@me_ookisa", character.EditInfo.MeOokisa);
            AddParameter(command, "@me_kaiten", character.EditInfo.MeKaiten);
            AddParameter(command, "@mayu_kaiten", character.EditInfo.MayuKaiten);
            AddParameter(command, "@mimi_ookisa", character.EditInfo.MimiOokisa);
            AddParameter(command, "@mimi_muki", character.EditInfo.MimiMuki);
            AddParameter(command, "@elf_mimi", character.EditInfo.ElfMimi);
            AddParameter(command, "@miken_takasa", character.EditInfo.MikenTakasa);
            AddParameter(command, "@miken_haba", character.EditInfo.MikenHaba);
            AddParameter(command, "@hohobone_ryou", character.EditInfo.HohoboneRyou);
            AddParameter(command, "@hohobone_jyouge", character.EditInfo.HohoboneJyouge);
            AddParameter(command, "@hohoniku", character.EditInfo.Hohoniku);
            AddParameter(command, "@erahone_jyouge", character.EditInfo.ErahoneJyouge);
            AddParameter(command, "@erahone_haba", character.EditInfo.ErahoneHaba);
            AddParameter(command, "@hana_jyouge", character.EditInfo.HanaJyouge);
            AddParameter(command, "@hana_haba", character.EditInfo.HanaHaba);
            AddParameter(command, "@hana_takasa", character.EditInfo.HanaTakasa);
            AddParameter(command, "@hana_kakudo", character.EditInfo.HanaKakudo);
            AddParameter(command, "@kuchi_haba", character.EditInfo.KuchiHaba);
            AddParameter(command, "@kuchi_atsusa", character.EditInfo.KuchiAtsusa);
            AddParameter(command, "@eyebrow_uv_offset_x", character.EditInfo.EyebrowUVOffsetX);
            AddParameter(command, "@eyebrow_uv_offset_y", character.EditInfo.EyebrowUVOffsetY);
            AddParameter(command, "@wrinkle", character.EditInfo.Wrinkle);
            AddParameter(command, "@wrinkle_albedo_blend_rate", character.EditInfo.WrinkleAlbedoBlendRate);
            AddParameter(command, "@wrinkle_detail_normal_power", character.EditInfo.WrinkleDetailNormalPower);
            AddParameter(command, "@muscle_albedo_blend_rate", character.EditInfo.MuscleAlbedoBlendRate);
            AddParameter(command, "@muscle_detail_normal_power", character.EditInfo.MuscleDetailNormalPower);
            AddParameter(command, "@height", character.EditInfo.Height);
            AddParameter(command, "@head_size", character.EditInfo.HeadSize);
            AddParameter(command, "@neck_offset", character.EditInfo.NeckOffset);
            AddParameter(command, "@neck_scale", character.EditInfo.NeckScale);
            AddParameter(command, "@upper_body_scale_x", character.EditInfo.UpperBodyScaleX);
            AddParameter(command, "@belly_size", character.EditInfo.BellySize);
            AddParameter(command, "@teat_scale", character.EditInfo.TeatScale);
            AddParameter(command, "@tekubi_size", character.EditInfo.TekubiSize);
            AddParameter(command, "@koshi_offset", character.EditInfo.KoshiOffset);
            AddParameter(command, "@koshi_size", character.EditInfo.KoshiSize);
            AddParameter(command, "@ankle_offset", character.EditInfo.AnkleOffset);
            AddParameter(command, "@fat", character.EditInfo.Fat);
            AddParameter(command, "@muscle", character.EditInfo.Muscle);
            AddParameter(command, "@motion_filter", character.EditInfo.MotionFilter);
            // CDataStatusInfoFields
            AddParameter(command, "@hp", character.StatusInfo.HP);
            AddParameter(command, "@stamina", character.StatusInfo.Stamina);
            AddParameter(command, "@revive_point", character.StatusInfo.RevivePoint);
            AddParameter(command, "@max_hp", character.StatusInfo.MaxHP);
            AddParameter(command, "@max_stamina", character.StatusInfo.MaxStamina);
            AddParameter(command, "@white_hp", character.StatusInfo.WhiteHP);
            AddParameter(command, "@gain_hp", character.StatusInfo.GainHP);
            AddParameter(command, "@gain_stamina", character.StatusInfo.GainStamina);
            AddParameter(command, "@gain_attack", character.StatusInfo.GainAttack);
            AddParameter(command, "@gain_defense", character.StatusInfo.GainDefense);
            AddParameter(command, "@gain_magic_attack", character.StatusInfo.GainMagicAttack);
            AddParameter(command, "@gain_magic_defense", character.StatusInfo.GainMagicDefense);
            // CDataMatchingProfile
            AddParameter(command, "@entry_job", (byte) character.MatchingProfile.EntryJob);
            AddParameter(command, "@entry_job_level", character.MatchingProfile.EntryJobLevel);
            AddParameter(command, "@current_job", (byte) character.MatchingProfile.CurrentJob);
            AddParameter(command, "@current_job_level", character.MatchingProfile.CurrentJobLevel);
            AddParameter(command, "@objective_type1", character.MatchingProfile.ObjectiveType1);
            AddParameter(command, "@objective_type2", character.MatchingProfile.ObjectiveType2);
            AddParameter(command, "@play_style", character.MatchingProfile.PlayStyle);
            AddParameter(command, "@comment", character.MatchingProfile.Comment);
            AddParameter(command, "@is_join_party", character.MatchingProfile.IsJoinParty);
            // CDataArisenProfile
            AddParameter(command, "@background_id", character.ArisenProfile.BackgroundId);
            AddParameter(command, "@title_uid", character.ArisenProfile.Title.UId);
            AddParameter(command, "@title_index", character.ArisenProfile.Title.Index);
            AddParameter(command, "@motion_id", character.ArisenProfile.MotionId);
            AddParameter(command, "@motion_frame_no", character.ArisenProfile.MotionFrameNo);
        }
    }
}
