using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Xml.Linq;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class UIDMigration : IMigrationStrategy
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(UIDMigration));

        public uint From => 3;
        public uint To => 4;

        private readonly DatabaseSetting DatabaseSetting;

        public UIDMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            Logger.Info("Inserting temporary data tables...");
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/uid_migration.sql");
            db.Execute(conn, adaptedSchema);

            Logger.Info("Fetching old ddon_item entries...");
            Dictionary<string, Item> ddon_items = new Dictionary<string, Item>();
            db.ExecuteReader(conn, "SELECT * FROM ddon_item;",
                command => {}, 
                reader =>
                {
                    while (reader.Read())
                    {
                        var item = new Item();
                        item.UId = db.GetString(reader, "uid");
                        item.ItemId = db.GetUInt32(reader, "item_id");
                        item.SafetySetting = db.GetByte(reader, "unk3");
                        item.Color = db.GetByte(reader, "color");
                        item.PlusValue = db.GetByte(reader, "plus_value");
                        ddon_items[item.UId] = item;
                    }
                }
            );

            // In the dd-on.com database, there seems to have old item/equipment entries
            // for characters that no longer exist. This has to be handled in the query

            Logger.Info("Fetching old ddon_storage_item entries...");
            // item_uid, character_id, storage_type, slot_no, item_num
            var ddon_storage_items = new List<(string, uint, ushort, ushort, uint)>();
            db.ExecuteReader(conn, "SELECT * from ddon_storage_item WHERE EXISTS(SELECT 1 FROM ddon_character WHERE ddon_character.character_id = ddon_storage_item.character_id);",
               command => { },
               reader =>
               {
                   while (reader.Read())
                   {
                       var item = (
                            db.GetString(reader, "item_uid"),
                            db.GetUInt32(reader, "character_id"),
                            db.GetUInt16(reader, "storage_type"),
                            db.GetUInt16(reader, "slot_no"),
                            db.GetUInt32(reader, "item_num")
                       );
                       ddon_storage_items.Add(item);
                   }
               });

            Logger.Info("Fetching old ddon_equip_item entries...");
            // item_uid, character_common_id, job, equip_type, equip_slot
            var ddon_equip_items = new List<(string, uint, JobId, EquipType, ushort)>();
            db.ExecuteReader(conn, "SELECT * from ddon_equip_item WHERE EXISTS(SELECT 1 FROM ddon_character_common WHERE ddon_character_common.character_common_id = ddon_equip_item.character_common_id);",
               command => { },
               reader =>
               {
                   while (reader.Read())
                   {
                       var item = (
                            db.GetString(reader, "item_uid"),
                            db.GetUInt32(reader, "character_common_id"),
                            (JobId) db.GetByte(reader, "job"),
                            (EquipType) db.GetByte(reader, "equip_type"),
                            db.GetUInt16(reader, "equip_slot")
                       );
                       ddon_equip_items.Add(item);
                   }
               });

            Logger.Info("Fetching old ddon_equip_job_item entries...");
            // item_uid, character_common_id, job, equip_slot
            var ddon_equip_job_items = new List<(string, uint, JobId, byte)>();
            db.ExecuteReader(conn, "SELECT * from ddon_equip_job_item WHERE EXISTS(SELECT 1 FROM ddon_character_common WHERE ddon_character_common.character_common_id = ddon_equip_job_item.character_common_id);",
               command => { },
               reader =>
               {
                   while (reader.Read())
                   {
                       var item = (
                            db.GetString(reader, "item_uid"),
                            db.GetUInt32(reader, "character_common_id"),
                            (JobId) db.GetByte(reader, "job"),
                            db.GetByte(reader, "equip_slot")
                       );
                       ddon_equip_job_items.Add(item);
                   }
               });


            // Drop the tables
            Logger.Info("Deleting old tables...");
            db.Execute(conn, "DROP TABLE ddon_equip_job_item;");
            db.Execute(conn, "DROP TABLE ddon_equip_item;");
            db.Execute(conn, "DROP TABLE ddon_storage_item;");
            db.Execute(conn, "DROP TABLE ddon_item;");

            // Rename new tables
            Logger.Info("Renaming temporary tables...");
            db.Execute(conn, "ALTER TABLE ddon_storage_item_temp RENAME TO ddon_storage_item;");
            db.Execute(conn, "ALTER TABLE ddon_equip_item_temp RENAME TO ddon_equip_item;");
            db.Execute(conn, "ALTER TABLE ddon_equip_job_item_temp RENAME TO ddon_equip_job_item;");

            // Map old uid with a list of uids for the new items
            var characterUIDsMapping = new Dictionary<uint, Dictionary<string, List<string>>>();

            // Insert all items into the new ddon_storage_item
            Logger.Info("Migrating storage items...");
            uint uid = 1;
            foreach (var storageItem in ddon_storage_items)
            {
                var oldUid = storageItem.Item1;
                var item = ddon_items[oldUid];
                var characterId = storageItem.Item2;
                var newUid = $"{uid:X08}";

                // Use TryAdd to create the dictionary if it doesnt already exist
                characterUIDsMapping.TryAdd(characterId, new Dictionary<string, List<string>>());
                characterUIDsMapping[characterId].TryAdd(oldUid, new List<string>());
                characterUIDsMapping[characterId][oldUid].Add(newUid);

                // item_uid, character_id, storage_type, slot_no, item_num
                db.ExecuteNonQuery(conn, "INSERT INTO ddon_storage_item (item_uid, character_id, storage_type, slot_no, item_id, item_num, unk3, color, plus_value) VALUES(@item_uid, @character_id, @storage_type, @slot_no, @item_id, @item_num, @unk3, @color, @plus_value);",
                command =>
                {
                    db.AddParameter(command, "item_uid", newUid);
                    db.AddParameter(command, "character_id", characterId);
                    db.AddParameter(command, "storage_type", storageItem.Item3);
                    db.AddParameter(command, "slot_no", storageItem.Item4);
                    db.AddParameter(command, "item_id", item.ItemId);
                    db.AddParameter(command, "item_num", storageItem.Item5);
                    db.AddParameter(command, "unk3", item.SafetySetting);
                    db.AddParameter(command, "color", item.Color);
                    db.AddParameter(command, "plus_value", item.PlusValue);
                });

                uid += 1;
            }

            Logger.Info("Migrating equipment templates...");
            // Insert all equipment template items into the new ddon_equip_item
            var availableCharacterJobUIDsMappings = new Dictionary<(uint, JobId), Dictionary<string, Stack<string>>>();
            foreach (var equipItem in ddon_equip_items)
            {
                var oldUid = equipItem.Item1;
                var commonId = equipItem.Item2;
                var job = equipItem.Item3;

                uint characterId = 0;
                db.ExecuteReader(conn, "select character_id from ddon_character where character_common_id = @character_common_id union select character_id from ddon_pawn where character_common_id = @character_common_id;",
                    command => { db.AddParameter(command, "character_common_id", commonId); },
                    reader => { if(reader.Read()) { characterId = db.GetUInt32(reader, "character_id"); }});
                if(characterId == 0)
                {
                    // Another thing that shouldn't ever happen and yet it does
                    Logger.Info($"Couldn't find a character id for common id {commonId}");
                    continue;
                }

                // Clone UID mappings for each unique pair of character and job, if they exist, use an empty list if not. 
                // This is because jobs can share items, but the same item cant be repeated in the same job
                availableCharacterJobUIDsMappings.TryAdd((commonId, job), new Dictionary<string, Stack<string>>());
                availableCharacterJobUIDsMappings[(commonId, job)].TryAdd(oldUid, new Stack<string>(characterUIDsMapping.GetValueOrDefault(characterId, new Dictionary<string, List<string>>()).GetValueOrDefault(oldUid, new List<string>())));
                // Pop it from the stack so it can't be repeated again for this character and job pair
                if(!availableCharacterJobUIDsMappings[(commonId, job)][oldUid].TryPop(out string newUid))
                {
                    Logger.Info($"Failed to find a new UID for item {oldUid} in {job} for character with id {characterId}. This equipment template item is for an item that's not in any storage, it will be ignored.");
                    continue;
                }

                db.ExecuteNonQuery(conn, "INSERT INTO ddon_equip_item (item_uid, character_common_id, job, equip_type, equip_slot) VALUES(@item_uid, @character_common_id, @job, @equip_type, @equip_slot);",
                command =>
                {
                    db.AddParameter(command, "item_uid", newUid);
                    db.AddParameter(command, "character_common_id", commonId);
                    db.AddParameter(command, "job", (byte) job);
                    db.AddParameter(command, "equip_type", (byte) equipItem.Item4);
                    db.AddParameter(command, "equip_slot", equipItem.Item5);
                });
            }

            Logger.Info("Migrating equipped job item shortcuts...");
            // Insert all equipped job item shortcuts into the new ddon_equip_job_item
            foreach (var equipJobItem in ddon_equip_job_items)
            {
                var oldUid = equipJobItem.Item1;
                var commonId = equipJobItem.Item2;

                uint characterId = 0;
                db.ExecuteReader(conn, "select character_id from ddon_character where character_common_id = @character_common_id union select character_id from ddon_pawn where character_common_id = @character_common_id;",
                    command => { db.AddParameter(command, "character_common_id", commonId); },
                    reader => { if(reader.Read()) { characterId = db.GetUInt32(reader, "character_id"); }});
                if(characterId == 0)
                {
                    // Another thing that shouldn't ever happen and yet it does
                    Logger.Info($"Couldn't find a character id for common id {commonId}");
                    continue;
                }

                string newUid;
                try
                {
                    newUid = characterUIDsMapping[characterId][oldUid][0];
                }
                catch (Exception e)
                {
                    Logger.Info($"Couldn't find a new UID for job item {oldUid} in {equipJobItem.Item3} for character with id {characterId}. This job item is no longer in any storage, it will be ignored.");
                    continue;
                }

                db.ExecuteNonQuery(conn, "INSERT INTO ddon_equip_job_item (item_uid, character_common_id, job, equip_slot) VALUES (@item_uid, @character_common_id, @job, @equip_slot);",
                command =>
                {
                    db.AddParameter(command, "item_uid", newUid);
                    db.AddParameter(command, "character_common_id", commonId);
                    db.AddParameter(command, "job", (byte) equipJobItem.Item3);
                    db.AddParameter(command, "equip_slot", equipJobItem.Item4);
                });
            }

            Logger.Info("Items migrated to the new UID system");

            return true;
        }
    }
}
