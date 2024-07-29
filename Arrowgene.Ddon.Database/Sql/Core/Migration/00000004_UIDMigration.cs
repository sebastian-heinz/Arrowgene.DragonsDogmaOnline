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
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/uid_migration.sql");
            db.Execute(conn, adaptedSchema);

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

            // item_uid, character_id, storage_type, slot_no, item_num
            List<(string, uint, ushort, ushort, uint)> ddon_storage_items = new List<(string, uint, ushort, ushort, uint)>();
            db.ExecuteReader(conn, "SELECT * from ddon_storage_item;",
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

            // Drop the tables
            db.Execute(conn, "DROP TABLE ddon_item;");
            db.Execute(conn, "DROP TABLE ddon_storage_item;");
            db.Execute(conn, "DROP TABLE ddon_equip_item;");
            db.Execute(conn, "DROP TABLE ddon_equip_job_item;");

            // Rename new tables
            db.Execute(conn, "ALTER TABLE ddon_storage_item_temp RENAME TO ddon_storage_item;");
            db.Execute(conn, "ALTER TABLE ddon_equip_item_temp RENAME TO ddon_equip_item;");
            db.Execute(conn, "ALTER TABLE ddon_equip_job_item_temp RENAME TO ddon_equip_job_item;");

            // Drop all existing items
            db.Execute(conn, "DELETE FROM ddon_storage_item;");

            uint uid = 1;
            foreach (var storageItem in ddon_storage_items)
            {
                var item = ddon_items[storageItem.Item1];
                var newUid = $"{uid:X08}";

                // item_uid, character_id, storage_type, slot_no, item_num
                db.ExecuteNonQuery(conn, "INSERT INTO ddon_storage_item (item_uid, character_id, storage_type, slot_no, item_id, item_num, unk3, color, plus_value) VALUES(@item_uid, @character_id, @storage_type, @slot_no, @item_id, @item_num, @unk3, @color, @plus_value);",
                command =>
                {
                    db.AddParameter(command, "item_uid", newUid);
                    db.AddParameter(command, "character_id", storageItem.Item2);
                    db.AddParameter(command, "storage_type", storageItem.Item3);
                    db.AddParameter(command, "slot_no", storageItem.Item4);
                    db.AddParameter(command, "item_id", item.ItemId);
                    db.AddParameter(command, "item_num", storageItem.Item5);
                    db.AddParameter(command, "unk3", item.Unk3);
                    db.AddParameter(command, "color", item.Color);
                    db.AddParameter(command, "plus_value", item.PlusValue);
                });

                uid += 1;
            }

            return true;
        }
    }
}
