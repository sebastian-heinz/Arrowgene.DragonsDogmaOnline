using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
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
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/uid_migration.sql");
            db.Execute(conn, adaptedSchema);

            Dictionary<uint, List<uint>> CharacterIdMap = new Dictionary<uint, List<uint>>();
            db.ExecuteReader(conn, "SELECT * FROM ddon_character;",
                command => { },
                reader =>
                {
                    while (reader.Read())
                    {
                        var characterId = db.GetUInt32(reader, "character_id");
                        var commonId = db.GetUInt32(reader, "character_common_id");
                        if (!CharacterIdMap.ContainsKey(characterId))
                        {
                            CharacterIdMap[characterId] = new List<uint>();
                        }
                        CharacterIdMap[characterId].Add(commonId);
                    }
                }
            );

            db.ExecuteReader(conn, "SELECT * FROM ddon_pawn;",
                command => { },
                reader =>
                {
                    while (reader.Read())
                    {
                        var characterId = db.GetUInt32(reader, "character_id");
                        var commonId = db.GetUInt32(reader, "character_common_id");
                        if (!CharacterIdMap.ContainsKey(characterId))
                        {
                            CharacterIdMap[characterId] = new List<uint>();
                        }
                        CharacterIdMap[characterId].Add(commonId);
                    }
                }
            );

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
                        item.Unk3 = db.GetByte(reader, "unk3");
                        item.Color = db.GetByte(reader, "color");
                        item.PlusValue = db.GetByte(reader, "plus_value");
                        ddon_items[item.UId] = item;
                    }
                }
            );

            List<(string, uint)> ddon_equip_items = new List<(string, uint)>();
            db.ExecuteReader(conn, "SELECT * from ddon_equip_item;",
                command => { },
                reader =>
                {
                    while (reader.Read())
                    {
                        var item = (db.GetString(reader, "item_uid"), db.GetUInt32(reader, "character_common_id"));
                        ddon_equip_items.Add(item);
                    }
                });

            List<(string, uint)> ddon_equip_job_items = new List<(string, uint)>();
            db.ExecuteReader(conn, "SELECT * from ddon_equip_job_item;",
               command => { },
               reader =>
               {
                   while (reader.Read())
                   {
                       var item = (db.GetString(reader, "item_uid"), db.GetUInt32(reader, "character_common_id"));
                       ddon_equip_job_items.Add(item);
                   }
               });

            List<(string, uint)> ddon_storage_items = new List<(string, uint)>();
            db.ExecuteReader(conn, "SELECT * from ddon_storage_item;",
               command => { },
               reader =>
               {
                   while (reader.Read())
                   {
                       var item = (db.GetString(reader, "item_uid"), db.GetUInt32(reader, "character_id"));
                       ddon_storage_items.Add(item);
                   }
               });

            // Drop the tables
            db.Execute(conn, "DROP TABLE ddon_item;");
            db.Execute(conn, "DROP TABLE ddon_storage_item;");
            db.Execute(conn, "DROP TABLE ddon_equip_item;");
            db.Execute(conn, "DROP TABLE ddon_equip_job_item;");

            // Rename new tables
            db.Execute(conn, "ALTER TABLE ddon_storage_item_temp RENAME TO ddon_storage_item;");
            db.Execute(conn, "ALTER TABLE ddon_equip_item_temp RENAME TO ddon_equip_item;");
            db.Execute(conn, "ALTER TABLE ddon_equip_job_item_temp RENAME TO ddon_equip_job_item;");

            uint uid = 1;
            foreach (var storageItem in ddon_storage_items)
            {
                var item = ddon_items[storageItem.Item1];
                var newUid = $"{uid:X08}";

                db.ExecuteNonQuery(conn, "UPDATE ddon_storage_item SET item_uid=@new_uid, item_id=@item_id, unk3=@unk3, color=@color, plus_value=@plus_value WHERE item_uid=@old_uid AND character_id=@character_id;", command =>
                {
                    db.AddParameter(command, "old_uid", item.UId);
                    db.AddParameter(command, "new_uid", newUid);
                    db.AddParameter(command, "character_id", storageItem.Item2);
                    db.AddParameter(command, "item_id", item.ItemId);
                    db.AddParameter(command, "unk3", item.Unk3);
                    db.AddParameter(command, "color", item.Color);
                    db.AddParameter(command, "plus_value", item.PlusValue);
                });

                List<uint> commonIds = CharacterIdMap[storageItem.Item2];

                var matches0 = ddon_equip_items.Where(x => x.Item1 == item.UId && commonIds.Contains(x.Item2)).ToList();
                foreach (var match in matches0)
                {
                    // Update the UID
                    db.ExecuteNonQuery(conn, "UPDATE ddon_equip_item SET item_uid=@new_uid WHERE item_uid=@old_uid AND character_common_id=@character_common_id;", command =>
                    {
                        db.AddParameter(command, "old_uid", item.UId);
                        db.AddParameter(command, "new_uid", newUid);
                        db.AddParameter(command, "character_common_id", match.Item2);
                    });
                }

                var matches1 = ddon_equip_job_items.Where(x => x.Item1 == item.UId && commonIds.Contains(x.Item2)).ToList();
                foreach (var match in matches1)
                {
                    // Update the UID
                    db.ExecuteNonQuery(conn, "UPDATE ddon_equip_job_item SET item_uid=@new_uid WHERE item_uid=@old_uid AND character_common_id=@character_common_id;", command =>
                    {
                        db.AddParameter(command, "old_uid", item.UId);
                        db.AddParameter(command, "new_uid", newUid);
                        db.AddParameter(command, "character_common_id", match.Item2);
                    });
                }

                uid += 1;
            }

            return true;
        }
    }
}
