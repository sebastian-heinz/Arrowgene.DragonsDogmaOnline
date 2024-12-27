using Arrowgene.Ddon.Shared.Csv;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        private static readonly string[] CharacterFields = new string[]
        {
            /* character_id */ "version", "character_common_id", "account_id", "first_name", "last_name", "created", "my_pawn_slot_num", "rental_pawn_slot_num", "hide_equip_head_pawn", "hide_equip_lantern_pawn", "arisen_profile_share_range", "fav_warp_slot_num", "max_bazaar_exhibits", "game_mode"
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

        private static readonly string[] CharacterBinaryDataFields = new string[]
        {
            "character_id", "binary_data"
        };

        private readonly string SqlInsertCharacter = $"INSERT INTO \"ddon_character\" ({BuildQueryField(CharacterFields)}) VALUES ({BuildQueryInsert(CharacterFields)});";
        private static readonly string SqlUpdateCharacter = $"UPDATE \"ddon_character\" SET {BuildQueryUpdate(CharacterFields)} WHERE \"character_id\" = @character_id;";
        private static readonly string SqlSelectCharacter = $"SELECT \"ddon_character\".\"character_id\", {BuildQueryField(CharacterFields)} FROM \"ddon_character\" WHERE \"character_id\" = @character_id;";
        private static readonly string SqlSelectCharactersByAccountId = $"SELECT \"ddon_character\".\"character_id\", {BuildQueryField(CharacterFields)} FROM \"ddon_character\" WHERE \"account_id\" = @account_id;";
        private readonly string SqlSelectAllCharacterData = $"SELECT \"ddon_character\".\"character_id\", {BuildQueryField("ddon_character", CharacterFields)}, \"ddon_character_common\".\"character_common_id\", {BuildQueryField("ddon_character_common", CharacterCommonFields)}, {BuildQueryField("ddon_edit_info", CDataEditInfoFields)}, {BuildQueryField("ddon_status_info", CDataStatusInfoFields)}, {BuildQueryField("ddon_character_matching_profile", CDataMatchingProfileFields)}, {BuildQueryField("ddon_character_arisen_profile", CDataArisenProfileFields)}, {BuildQueryField("ddon_binary_data", CharacterBinaryDataFields)} "
            + "FROM \"ddon_character\" "
            + "LEFT JOIN \"ddon_character_common\" ON \"ddon_character_common\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" "
            + "LEFT JOIN \"ddon_edit_info\" ON \"ddon_edit_info\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" "
            + "LEFT JOIN \"ddon_status_info\" ON \"ddon_status_info\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" "
            + "LEFT JOIN \"ddon_character_matching_profile\" ON \"ddon_character_matching_profile\".\"character_id\" = \"ddon_character\".\"character_id\" "
            + "LEFT JOIN \"ddon_character_arisen_profile\" ON \"ddon_character_arisen_profile\".\"character_id\" = \"ddon_character\".\"character_id\" "
            + "LEFT JOIN \"ddon_binary_data\" ON \"ddon_binary_data\".\"character_id\" = \"ddon_character\".\"character_id\" "
            + "WHERE \"ddon_character\".\"character_id\" = @character_id";
        private readonly string SqlSelectAllCharactersDataByAccountId = $"SELECT \"ddon_character\".\"character_id\", {BuildQueryField("ddon_character", CharacterFields)}, \"ddon_character_common\".\"character_common_id\", {BuildQueryField("ddon_character_common", CharacterCommonFields)}, {BuildQueryField("ddon_edit_info", CDataEditInfoFields)}, {BuildQueryField("ddon_status_info", CDataStatusInfoFields)}, {BuildQueryField("ddon_character_matching_profile", CDataMatchingProfileFields)}, {BuildQueryField("ddon_character_arisen_profile", CDataArisenProfileFields)}, {BuildQueryField("ddon_binary_data", CharacterBinaryDataFields)} "
            + "FROM \"ddon_character\" "
            + "LEFT JOIN \"ddon_character_common\" ON \"ddon_character_common\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" "
            + "LEFT JOIN \"ddon_edit_info\" ON \"ddon_edit_info\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" "
            + "LEFT JOIN \"ddon_status_info\" ON \"ddon_status_info\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" "
            + "LEFT JOIN \"ddon_character_matching_profile\" ON \"ddon_character_matching_profile\".\"character_id\" = \"ddon_character\".\"character_id\" "
            + "LEFT JOIN \"ddon_character_arisen_profile\" ON \"ddon_character_arisen_profile\".\"character_id\" = \"ddon_character\".\"character_id\" "
            + "LEFT JOIN \"ddon_binary_data\" ON \"ddon_binary_data\".\"character_id\" = \"ddon_character\".\"character_id\" "
            + "WHERE \"account_id\" = @account_id AND \"game_mode\" = @game_mode;";
        private readonly string SqlSelectAllCharactersData = $"SELECT \"ddon_character\".\"character_id\", {BuildQueryField("ddon_character", CharacterFields)}, \"ddon_character_common\".\"character_common_id\", {BuildQueryField("ddon_character_common", CharacterCommonFields)}, {BuildQueryField("ddon_edit_info", CDataEditInfoFields)}, {BuildQueryField("ddon_status_info", CDataStatusInfoFields)}, {BuildQueryField("ddon_character_matching_profile", CDataMatchingProfileFields)}, {BuildQueryField("ddon_character_arisen_profile", CDataArisenProfileFields)}, {BuildQueryField("ddon_binary_data", CharacterBinaryDataFields)} "
            + "FROM \"ddon_character\" "
            + "LEFT JOIN \"ddon_character_common\" ON \"ddon_character_common\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" "
            + "LEFT JOIN \"ddon_edit_info\" ON \"ddon_edit_info\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" "
            + "LEFT JOIN \"ddon_status_info\" ON \"ddon_status_info\".\"character_common_id\" = \"ddon_character\".\"character_common_id\" "
            + "LEFT JOIN \"ddon_character_matching_profile\" ON \"ddon_character_matching_profile\".\"character_id\" = \"ddon_character\".\"character_id\" "
            + "LEFT JOIN \"ddon_character_arisen_profile\" ON \"ddon_character_arisen_profile\".\"character_id\" = \"ddon_character\".\"character_id\" "
            + "LEFT JOIN \"ddon_binary_data\" ON \"ddon_binary_data\".\"character_id\" = \"ddon_character\".\"character_id\" ";
        private const string SqlDeleteCharacter = "DELETE FROM \"ddon_character_common\" WHERE EXISTS (SELECT 1 FROM \"ddon_character\" WHERE \"ddon_character_common\".\"character_common_id\"=\"ddon_character\".\"character_common_id\" AND \"character_id\"=@character_id);";
        private const string SqlUpdateMyPawnSlot = "UPDATE \"ddon_character\" SET \"my_pawn_slot_num\"=@my_pawn_slot_num WHERE \"character_id\"=@character_id;";
        private const string SqlUpdateRentalPawnSlot = "UPDATE \"ddon_character\" SET \"rental_pawn_slot_num\"=@rental_pawn_slot_num WHERE \"character_id\"=@character_id;";
        private const string SqlSelectCharacterNameByCharacterId = "SELECT \"first_name\", \"last_name\" FROM \"ddon_character\" WHERE \"character_id\"=@character_id;";


        private readonly string SqlInsertCharacterMatchingProfile = $"INSERT INTO \"ddon_character_matching_profile\" ({BuildQueryField(CDataMatchingProfileFields)}) VALUES ({BuildQueryInsert(CDataMatchingProfileFields)});";
        private static readonly string SqlUpdateCharacterMatchingProfile = $"UPDATE \"ddon_character_matching_profile\" SET {BuildQueryUpdate(CDataMatchingProfileFields)} WHERE \"character_id\" = @character_id;";
        private static readonly string SqlSelectCharacterMatchingProfile = $"SELECT {BuildQueryField(CDataMatchingProfileFields)} FROM \"ddon_character_matching_profile\" WHERE \"character_id\" = @character_id;";
        private const string SqlDeleteCharacterMatchingProfile = "DELETE FROM \"ddon_character_matching_profile\" WHERE \"character_id\"=@character_id;";


        private readonly string SqlInsertCharacterArisenProfile = $"INSERT INTO \"ddon_character_arisen_profile\" ({BuildQueryField(CDataArisenProfileFields)}) VALUES ({BuildQueryInsert(CDataArisenProfileFields)});";
        private static readonly string SqlUpdateCharacterArisenProfile = $"UPDATE \"ddon_character_arisen_profile\" SET {BuildQueryUpdate(CDataArisenProfileFields)} WHERE \"character_id\" = @character_id;";
        private static readonly string SqlSelectCharacterArisenProfile = $"SELECT {BuildQueryField(CDataArisenProfileFields)} FROM \"ddon_character_arisen_profile\" WHERE \"character_id\" = @character_id;";
        private const string SqlDeleteCharacterArisenProfile = "DELETE FROM \"ddon_character_arisen_profile\" WHERE \"character_id\"=@character_id;";

        private readonly string SqlInsertCharacterBinaryData = $"INSERT INTO \"ddon_binary_data\" ({BuildQueryField(CharacterBinaryDataFields)}) VALUES ({BuildQueryInsert(CharacterBinaryDataFields)});";
        private readonly string SqlUpdateCharacterBinaryData = $"UPDATE \"ddon_binary_data\" SET {BuildQueryUpdate(CharacterBinaryDataFields)} WHERE \"character_id\" = @character_id;";
        private static readonly string SqlSelectCharacterBinaryData = $"SELECT {BuildQueryField(CharacterBinaryDataFields)} FROM \"ddon_binary_data\" WHERE \"character_id\" = @character_id;";

        public bool CreateCharacter(Character character)
        {
            return ExecuteInTransaction((Action<TCon>)(conn =>
                {
                    character.Created = DateTime.UtcNow;

                    ExecuteNonQuery(conn, SqlInsertCharacterCommon, command => { AddParameter(command, character); }, out long commonId);
                    character.CommonId = (uint) commonId;

                    ExecuteNonQuery(conn, SqlInsertCharacter, command => { AddParameter(command, character); }, out long characterId);
                    character.CharacterId = (uint) characterId;
                    
                    if (character.GameMode == GameMode.BitterblackMaze)
                    {
                        character.BbmCharacterId = (uint) characterId;
                    }

                    ExecuteNonQuery(conn, SqlInsertEditInfo, command => { AddParameter(command, character); });
                    ExecuteNonQuery(conn, SqlInsertStatusInfo, command => { AddParameter(command, character); });
                    ExecuteNonQuery(conn, SqlInsertCharacterMatchingProfile, command => { AddParameter(command, character); });
                    ExecuteNonQuery(conn, SqlInsertCharacterArisenProfile, command => { AddParameter(command, character); });
                    ExecuteNonQuery(conn, SqlInsertCharacterBinaryData, command => { AddParameter(command, character);});

                    CreateItems(conn, character);

                    StoreCharacterData(conn, character);
                }));
        }

        public bool UpdateCharacterBaseInfo(Character character)
        {
            using TCon connection = OpenNewConnection();
            return UpdateCharacterBaseInfo(connection, character);
        }

        public bool UpdateCharacterBaseInfo(TCon conn, Character character)
        {
            int characterUpdateRowsAffected = ExecuteNonQuery(conn, SqlUpdateCharacter, command =>
            {
                AddParameter(command, character);
            });

            return characterUpdateRowsAffected > NoRowsAffected;
        }

        public bool UpdateCharacterMatchingProfile(Character character)
        {
            using TCon connection = OpenNewConnection();
            return UpdateCharacterMatchingProfile(connection, character);
        }

        public bool UpdateCharacterMatchingProfile(TCon conn, Character character)
        {
            int characterUpdateRowsAffected = ExecuteNonQuery(conn, SqlUpdateCharacterMatchingProfile, command =>
            {
                AddParameter(command, character);
            });

            return characterUpdateRowsAffected > NoRowsAffected;
        }

        public bool UpdateCharacterArisenProfile(Character character)
        {
            using TCon connection = OpenNewConnection();
            return UpdateCharacterArisenProfile(connection, character);
        }

        public bool UpdateCharacterArisenProfile(TCon conn, Character character)
        {
            int characterUpdateRowsAffected = ExecuteNonQuery(conn, SqlUpdateCharacterArisenProfile, command =>
            {
                AddParameter(command, character);
            });

            return characterUpdateRowsAffected > NoRowsAffected;
        }

        public Character SelectCharacter(uint characterId, DbConnection? connectionIn = null)
        {
            Character character = null;
            ExecuteQuerySafe(connectionIn, conn => {
                ExecuteReader(conn, SqlSelectAllCharacterData,
                command => { AddParameter(command, "@character_id", characterId); }, reader =>
                {
                    if (reader.Read())
                    {
                        character = ReadAllCharacterData(reader);
                    }
                });

                if (character != null)
                {
                    QueryCharacterData(conn, character);
                }
            });
            return character;
        }

        public List<Character> SelectCharactersByAccountId(int accountId, GameMode gameMode)
        {
            List<Character> characters = new List<Character>();
            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectAllCharactersDataByAccountId,
                    command => { 
                        AddParameter(command, "@account_id", accountId);
                        AddParameter(command, "@game_mode", (uint) gameMode);
                    }, reader =>
                    {
                        while (reader.Read())
                        {
                            Character character = ReadAllCharacterData(reader);
                            characters.Add(character);
                        }
                    });
                foreach (var character in characters)
                {
                    QueryCharacterData(conn, character);
                }
            });
            return characters;
        }

        public List<Character> SelectAllCharacters()
        {
            List<Character> characters = null;
            ExecuteInTransaction(conn =>
            {
                characters = SelectAllCharacters(conn);
            });
            return characters;
        }
                
        public List<Character> SelectAllCharacters(DbConnection conn)
        {
            List<Character> characters = new List<Character>();
            ExecuteReader((TCon) conn, SqlSelectAllCharactersData,
                command => {}, reader =>
                {
                    while (reader.Read())
                    {
                        Character character = ReadAllCharacterData(reader);
                        characters.Add(character);
                    }
                });
            foreach (var character in characters)
            {
                QueryCharacterData((TCon) conn, character);
            }
            return characters;
        }

        public bool DeleteCharacter(uint characterId)
        {
            int rowsAffected = 0;
            ExecuteInTransaction(conn =>
            {
                var clan = SelectClanMembershipByCharacterId(characterId, conn);
                if (clan != 0)
                {
                    if (GetClanMemberList(clan, conn).Count == 1)
                    {
                        DeleteClan(clan, conn);
                    }
                    else
                    {
                        IncrementClanMemberNum(-1, clan, conn);
                    }
                }

                uint bbmCharacterId = SelectBBMCharacterId(characterId, conn);
                if (bbmCharacterId > 0)
                {
                    ExecuteNonQuery(conn, SqlDeleteCharacter,
                    command => { AddParameter(command, "@character_id", bbmCharacterId); });
                }

                rowsAffected = ExecuteNonQuery(conn, SqlDeleteCharacter,
                    command => { AddParameter(command, "@character_id", characterId); });
            });
            
            return rowsAffected > NoRowsAffected;
        }

        private void QueryCharacterData(TCon conn, Character character)
        {
            QueryCharacterCommonData(conn, character);

            // Shortcuts
            ExecuteReader(conn, SqlSelectShortcuts,
                (Action<TCom>)(                command => { AddParameter(command, "@character_id", (uint)character.CharacterId); }),
                reader =>
                {
                    while (reader.Read())
                    {
                        character.ShortCutList.Add(ReadShortCut(reader));
                    }
                });

            // CommunicationShortcuts
            ExecuteReader(conn, SqlSelectCommunicationShortcuts,
                (Action<TCom>)(                command => { AddParameter(command, "@character_id", (uint)character.CharacterId); }),
                reader =>
                {
                    while (reader.Read())
                    {
                        character.CommunicationShortCutList.Add(ReadCommunicationShortCut(reader));
                    }
                });

            // Storage
            ExecuteReader(conn, SqlSelectAllStoragesByCharacter,
                (Action<TCom>)(                command => { AddParameter(command, "@character_id", (uint)character.CharacterId); }),
                reader =>
                {
                    while (reader.Read())
                    {
                        Tuple<StorageType, Storage> tuple = ReadStorage(reader);
                        character.Storage.AddStorage(tuple.Item1, tuple.Item2);
                    }
                });
            ExecuteReader(conn, SqlSelectStorageItemsByCharacter,
                (Action<DbCommand>)(                command2 => { AddParameter(command2, "@character_id", (uint)character.CharacterId); }),
                reader2 =>
                {
                    while(reader2.Read())
                    {

                        StorageType storageType = (StorageType)GetByte(reader2, "storage_type");
                        ushort slot = GetUInt16(reader2, "slot_no");
                        uint itemNum = GetUInt32(reader2, "item_num");
                        var item = new Item();
                        
                        item.UId = GetString(reader2, "item_uid");
                        item.ItemId = GetUInt32(reader2, "item_id");
                        item.SafetySetting = GetByte(reader2, "safety");
                        item.Color = GetByte(reader2, "color");
                        item.PlusValue = GetByte(reader2, "plus_value");
                        item.EquipPoints = GetUInt32(reader2, "equip_points");

                        using TCon connection = OpenNewConnection();
                        ExecuteReader(connection, SqlSelectAllCrestData,
                        command4 => {
                            AddParameter(command4, "character_common_id", character.CommonId);
                            AddParameter(command4, "item_uid", item.UId);
                        }, reader4 => {
                            while (reader4.Read())
                            {
                                var result = ReadCrestData(reader4);
                                item.EquipElementParamList.Add(result.ToCDataEquipElementParam());
                            }
                        });

                        character.Storage.GetStorage(storageType).SetItem(item, itemNum, slot);
                    }
                });

            // Wallet Points
            ExecuteReader(conn, SqlSelectWalletPoints,
                (Action<TCom>)(                command => { AddParameter(command, "@character_id", (uint)character.CharacterId); }),
                reader =>
                {
                    while (reader.Read())
                    {
                        character.WalletPointList.Add(ReadWalletPoint(reader));
                    }
                });

            // Warp Points
            ExecuteReader(conn, SqlSelectReleasedWarpPoints,
                (Action<TCom>)(                command => { AddParameter(command, "@character_id", (uint)character.CharacterId); }),
                reader =>
                {
                    while (reader.Read())
                    {
                        character.ReleasedWarpPoints.Add(ReadReleasedWarpPoint(reader));
                    }
                });

            // Play Points
            ExecuteReader(conn, SqlSelectCharacterPlayPointDataByCharacter,
                (Action<TCom>)(                command => { AddParameter(command, "@character_id", (uint)character.CharacterId); }),
                reader =>
                {
                    while (reader.Read())
                    {
                        character.PlayPointList.Add(ReadCharacterPlayPointData(reader));
                    }
                });

            // Login Stamp
            ExecuteReader(conn, SqlSelectCharacterStamp,
                (Action<TCom>)(                command => { AddParameter(command, "@character_id", (uint)character.CharacterId); }),
                reader =>
                {
                    while (reader.Read())
                    {
                        character.StampBonus = ReadCharacterStampData(reader);
                    }
                });

            // Ability Presets
            ExecuteReader(conn, SqlSelectAbilityPresets,
                (Action<TCom>)(                command => { AddParameter(command, "@character_id", (uint)character.CharacterId); }),
                reader =>
                {
                    while (reader.Read())
                    {
                        character.AbilityPresets.Add(ReadAbilityPreset(reader));
                    }
                });

            // Quest Completion
            foreach (var questType in Enum.GetValues(typeof(QuestType)).Cast<QuestType>())
            {
                ExecuteReader(conn, SqlSelectCompletedQuestByType,
                   command => {
                       AddParameter(command, "@character_common_id", character.CommonId);
                       AddParameter(command, "@quest_type", (uint)questType);
                   }, reader => {
                       while (reader.Read())
                       {
                           var quest = new CompletedQuest()
                           {
                               QuestId = (QuestId)GetUInt32(reader, "quest_id"),
                               QuestType = questType,
                               ClearCount = GetUInt32(reader, "clear_count")
                           };

                           character.CompletedQuests.TryAdd(quest.QuestId, quest);
                       }
                   });
            }

            //Clan membership
            character.ClanId = SelectClanMembershipByCharacterId(character.CharacterId, conn);
            character.ClanName = GetClanNameByClanId(character.ClanId);

            // Area Ranks
            character.AreaRanks = SelectAreaRank(character.CharacterId, conn);
            character.AreaSupply = SelectAreaRankSupply(character.CharacterId, conn);
        }

        public bool UpdateMyPawnSlot(uint characterId, uint num)
        {
            using TCon connection = OpenNewConnection();
            return UpdateMyPawnSlot(connection, characterId, num);
        }

        public bool UpdateMyPawnSlot(TCon conn, uint characterId, uint num)
        {
            return ExecuteNonQuery(conn, SqlUpdateMyPawnSlot, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "my_pawn_slot_num", num);
            })  == 1;
        }
        
        public bool UpdateRentalPawnSlot(uint characterId, uint num)
        {
            using TCon connection = OpenNewConnection();
            return UpdateRentalPawnSlot(connection, characterId, num);
        }

        public bool UpdateRentalPawnSlot(TCon conn, uint characterId, uint num)
        {
            return ExecuteNonQuery(conn, SqlUpdateRentalPawnSlot, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "rental_pawn_slot_num", num);
            })  == 1;
        }

        private void StoreCharacterData(TCon conn, Character character)
        {
            StoreCharacterCommonData(conn, character);

            foreach(CDataShortCut shortcut in character.ShortCutList)
            {
                ReplaceShortcut(character.ContentCharacterId, shortcut, conn);
            }

            foreach(CDataCommunicationShortCut communicationShortcut in character.CommunicationShortCutList)
            {
                ReplaceCommunicationShortcut(character.ContentCharacterId, communicationShortcut, conn);
            }

            foreach(StorageType storageType in character.Storage.GetAllStorages().Keys)
            {
                ReplaceStorage(conn, character.ContentCharacterId, storageType, character.Storage.GetStorage(storageType));
            }

            foreach(CDataWalletPoint walletPoint in character.WalletPointList)
            {
                ReplaceWalletPoint(conn, character.ContentCharacterId, walletPoint);
            }

            foreach(CDataJobPlayPoint playPoint in character.PlayPointList)
            {
                ReplaceCharacterPlayPointData(character.ContentCharacterId, playPoint, conn);
            }

            ExecuteNonQuery(conn, SqlInsertCharacterStamp, (Action<TCom>)(command =>
            {
                AddParameter(command, (uint)character.ContentCharacterId, character.StampBonus);
            }));
        }

        public void CreateItems(DbConnection conn, Character character)
        {
            // Create storage items
            foreach (KeyValuePair<StorageType, Storage> storage in character.Storage.GetAllStorages())
            {
                StorageType storageType = storage.Key;
                for(ushort index=0; index < storage.Value.Items.Count; index++)
                {
                    if(storage.Value.Items[index] != null)
                    {
                        Item item = storage.Value.Items[index].Item1;
                        uint itemNum = storage.Value.Items[index].Item2;
                        ushort slot = (ushort)(index+1);
                        InsertStorageItem(character.ContentCharacterId, storageType, slot, itemNum, item, conn);
                    }
                }
            }

            var storageBoxNormal = character.Storage.GetAllStorages()[StorageType.StorageBoxNormal];
            if (character.GameMode == GameMode.Normal)
            {
                var equipmentTemplates = character.EquipmentTemplate.GetAllEquipment()[character.Job];
                foreach (var equipment in equipmentTemplates)
                {
                    for (byte index = 0; index < equipment.Value.Count; index++)
                    {
                        Item item = equipment.Value[index];
                        if (item != null)
                        {
                            byte slot = (byte)(index + 1);
                            InsertEquipItem((TCon) conn, character.CommonId, character.Job, equipment.Key, slot, item.UId);
                        }
                    }
                }

                foreach (KeyValuePair<JobId, Dictionary<EquipType, List<Item>>> jobEquipment in character.EquipmentTemplate.GetAllEquipment())
                {
                    JobId job = jobEquipment.Key;
                    if (job == character.Job)
                    {
                        // Skip the current job as we already populated data for it.
                        continue;
                    }

                    // Give starter weapon for all classes
                    // If creating a character for normal mode, Wwe are only interested in slot 1 and 2
                    for (byte i = 0; i < 2; i++)
                    {
                        Item item = jobEquipment.Value[EquipType.Performance][i];
                        if (item != null)
                        {
                            ushort slot = storageBoxNormal.AddItem(item, 0);
                            InsertEquipItem((TCon) conn, character.CommonId, job, EquipType.Performance, (byte)(i + 1), item.UId);
                            InsertStorageItem(character.ContentCharacterId, StorageType.StorageBoxNormal, slot, 1, item, conn);
                        }
                    }

                    // Requip the base armor to the other jobs without creating new items
                    var baseJob = character.EquipmentTemplate.GetAllEquipment()[character.Job];
                    for (byte i = 2; i < baseJob[EquipType.Performance].Count; i++)
                    {
                        Item item = baseJob[EquipType.Performance][i];
                        if (item != null)
                        {
                            InsertEquipItem((TCon) conn, character.CommonId, job, EquipType.Performance, (byte)(i + 1), item.UId);
                        }
                    }
                }
            }
            else if (character.GameMode == GameMode.BitterblackMaze)
            {
                // If creating a character for BBM, we need gear for all classes.
                foreach (var (jobId, equipmentTemplate) in character.EquipmentTemplate.GetAllEquipment())
                {
                    var equipment = equipmentTemplate[EquipType.Performance];
                    for (byte i = 0; i < equipment.Count; i++)
                    {
                        Item item = equipment[i];
                        if (item != null && item.ItemId > 0)
                        {
                            ushort slot = storageBoxNormal.AddItem(item, 0);
                            InsertEquipItem((TCon) conn, character.CommonId, jobId, EquipType.Performance, (byte)(i + 1), item.UId);

                            if (jobId != character.Job)
                            {
                                InsertStorageItem(character.ContentCharacterId, StorageType.StorageBoxNormal, slot, 1, item, conn);
                            }
                        }
                    }
                }
            }
        }

        public Storages SelectAllStoragesByCharacterId(uint characterId)
        {
            using TCon connection = OpenNewConnection();
            return SelectAllStoragesByCharacterId(connection, characterId);
        }

        public Storages SelectAllStoragesByCharacterId(TCon connection, uint characterId)
        {
            Storages storages = new Storages(new Dictionary<StorageType, ushort>());

            ExecuteReader(connection, SqlSelectAllStoragesByCharacter,
                command => { AddParameter(command, "@character_id", characterId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        Tuple<StorageType, Storage> tuple = ReadStorage(reader);
                        storages.AddStorage(tuple.Item1, tuple.Item2);
                    }
                });

            ExecuteReader(connection, SqlSelectStorageItemsByCharacter,
                command => { 
                    AddParameter(command, "@character_id", characterId);
                },
                reader =>
                {
                    while (reader.Read())
                    {

                        StorageType storageType = (StorageType)GetByte(reader, "storage_type");
                        ushort slot = GetUInt16(reader, "slot_no");
                        uint itemNum = GetUInt32(reader, "item_num");
                        var item = new Item();

                        item.UId = GetString(reader, "item_uid");
                        item.ItemId = GetUInt32(reader, "item_id");
                        item.SafetySetting = GetByte(reader, "safety");
                        item.Color = GetByte(reader, "color");
                        item.PlusValue = GetByte(reader, "plus_value");
                        item.EquipPoints = GetUInt32(reader, "equip_points");

                        ExecuteReader(connection, SqlSelectAllCrestDataByUid,
                        command2 => {
                            AddParameter(command2, "item_uid", item.UId);
                        }, reader2 => {
                            while (reader2.Read())
                            {
                                var result = ReadCrestData(reader2);
                                item.EquipElementParamList.Add(result.ToCDataEquipElementParam());
                            }
                        });

                        storages.GetStorage(storageType).SetItem(item, itemNum, slot);
                    }
                });

            return storages;
        }

        public CDataCharacterSearchParam SelectCharacterNameById(uint characterId)
        {
            using TCon connection = OpenNewConnection();
            return SelectCharacterNameById(connection, characterId);
        }

        public CDataCharacterSearchParam SelectCharacterNameById(DbConnection connection, uint characterId)
        {
            CDataCharacterSearchParam result = new CDataCharacterSearchParam();
            ExecuteReader(connection, SqlSelectCharacterNameByCharacterId,
                command => { AddParameter(command, "@character_id", characterId); },
                reader =>
                {
                    if (reader.Read())
                    {
                        result.FirstName = GetString(reader, "first_name");
                        result.LastName = GetString(reader, "last_name");
                    }
                });
            return result;
        }

        private Character ReadAllCharacterData(TReader reader)
        {
            Character character = new Character();

            ReadAllCharacterCommonData(reader, character);

            character.CharacterId = GetUInt32(reader, "character_id");
            character.AccountId = GetInt32(reader, "account_id");
            character.Version = GetUInt32(reader, "version");
            character.FirstName = GetString(reader, "first_name");
            character.LastName = GetString(reader, "last_name");
            character.Created = GetDateTime(reader, "created");
            character.MyPawnSlotNum = GetByte(reader, "my_pawn_slot_num");
            character.RentalPawnSlotNum = GetByte(reader, "rental_pawn_slot_num");
            character.HideEquipHeadPawn = GetBoolean(reader, "hide_equip_head_pawn");
            character.HideEquipLanternPawn = GetBoolean(reader, "hide_equip_lantern_pawn");
            character.ArisenProfileShareRange = GetByte(reader, "arisen_profile_share_range");
            character.GameMode = (GameMode) GetUInt32(reader, "game_mode");

            character.MatchingProfile.EntryJob = (JobId) GetByte(reader, "entry_job");
            character.MatchingProfile.EntryJobLevel = GetUInt32(reader, "entry_job_level");
            character.MatchingProfile.CurrentJob = (JobId) GetByte(reader, "current_job");
            character.MatchingProfile.CurrentJobLevel = GetUInt32(reader, "current_job_level");
            character.MatchingProfile.ObjectiveType1 = GetUInt32(reader, "objective_type1");
            character.MatchingProfile.ObjectiveType2 = GetUInt32(reader, "objective_type2");
            character.MatchingProfile.PlayStyle = GetUInt32(reader, "play_style");
            character.MatchingProfile.Comment = GetString(reader, "comment");
            character.MatchingProfile.IsJoinParty = GetBoolean(reader, "is_join_party");

            character.ArisenProfile.BackgroundId = GetByte(reader, "background_id");
            character.ArisenProfile.Title.UId = GetUInt32(reader, "title_uid");
            character.ArisenProfile.Title.Index = GetUInt32(reader, "title_index");
            character.ArisenProfile.MotionId = GetUInt16(reader, "motion_id");
            character.ArisenProfile.MotionFrameNo = GetUInt32(reader, "motion_frame_no");

            character.FavWarpSlotNum = GetUInt32(reader, "fav_warp_slot_num");

            character.MaxBazaarExhibits = GetUInt32(reader, "max_bazaar_exhibits");

            character.BinaryData = GetBytes(reader, "binary_data", 0x400);

            return character;
        }

        private void AddParameter(TCom command, Character character)
        {
            AddParameter(command, (CharacterCommon) character);
            // CharacterFields
            AddParameter(command, "@account_id", character.AccountId);
            AddParameter(command, "@character_id", character.ContentCharacterId);
            AddParameter(command, "@version", character.Version);
            AddParameter(command, "@first_name", character.FirstName);
            AddParameter(command, "@last_name", character.LastName);
            AddParameter(command, "@created", character.Created);
            AddParameter(command, "@my_pawn_slot_num", character.MyPawnSlotNum);
            AddParameter(command, "@rental_pawn_slot_num", character.RentalPawnSlotNum);
            AddParameter(command, "@hide_equip_head_pawn", character.HideEquipHeadPawn);
            AddParameter(command, "@hide_equip_lantern_pawn", character.HideEquipLanternPawn);
            AddParameter(command, "@arisen_profile_share_range", character.ArisenProfileShareRange);
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

            AddParameter(command, "@fav_warp_slot_num", character.FavWarpSlotNum);

            AddParameter(command, "@max_bazaar_exhibits", character.MaxBazaarExhibits);

            AddParameter(command, "@binary_data", character.BinaryData);
            AddParameter(command, "@game_mode", (uint)character.GameMode);
        }
    
        public bool UpdateCharacterBinaryData(uint characterId, byte[] data)
        {
            using TCon connection = OpenNewConnection();
            return UpdateCharacterBinaryData(connection, characterId, data);
        }

        public bool UpdateCharacterBinaryData(TCon conn, uint characterId, byte[] data)
        {
            int rowsAffected = ExecuteNonQuery(conn, SqlUpdateCharacterBinaryData, command =>
            {
                AddParameter(command, "@character_id", characterId);
                AddParameter(command, "@binary_data", data);
            });

            return rowsAffected > NoRowsAffected;
        }
    }
}
