using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class EquipMigration : IMigrationStrategy
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(EquipMigration));

        public uint From => 2;
        public uint To => 3;

        private readonly DatabaseSetting DatabaseSetting;

        public EquipMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            HashSet<uint> unknownCommonIds = new HashSet<uint>();
            Dictionary<uint, uint> characterIdMapping = new Dictionary<uint, uint>();
            db.ExecuteReader("SELECT * FROM ddon_character;", reader =>
            {
                while (reader.Read())
                {
                    uint commonId = db.GetUInt32(reader, "character_common_id");
                    uint characterId = db.GetUInt32(reader, "character_id");
                    characterIdMapping[commonId] = characterId;
                }
            });

            db.ExecuteReader("SELECT * FROM ddon_pawn;", reader =>
            {
                while (reader.Read())
                {
                    uint commonId = db.GetUInt32(reader, "character_common_id");
                    uint characterId = db.GetUInt32(reader, "character_id");
                    characterIdMapping[commonId] = characterId;
                }
            });

            List<EquipItem> equippedItems = new List<EquipItem>();
            db.ExecuteReader("SELECT * FROM ddon_equip_item;", reader =>
            {
                while (reader.Read())
                {
                    var obj = new EquipItem();
                    obj.ItemUId = db.GetString(reader, "item_uid");
                    obj.CharacterCommonId = db.GetUInt32(reader, "character_common_id");
                    obj.Job = (JobId)db.GetByte(reader, "job");
                    obj.EquipType = db.GetUInt16(reader, "equip_type");
                    obj.EquipSlot = db.GetUInt16(reader, "equip_slot");
                    equippedItems.Add(obj);
                }
            });

            db.ExecuteReader("SELECT * FROM ddon_equip_job_item", reader =>
            {
                while (reader.Read())
                {
                    var obj = new EquipItem();
                    obj.ItemUId = db.GetString(reader, "item_uid");
                    obj.CharacterCommonId = db.GetUInt32(reader, "character_common_id");
                    obj.Job = (JobId)db.GetByte(reader, "job");
                    obj.EquipSlot = db.GetUInt16(reader, "equip_slot");
                    equippedItems.Add(obj);
                }
            });

            // Update existing storage records for all players
            db.ExecuteNonQuery(conn, "UPDATE ddon_storage SET slot_max=30 WHERE storage_type=14;", command => {});
            db.ExecuteNonQuery(conn, "UPDATE ddon_storage SET slot_max=90 WHERE storage_type=15;", command => {});

            // Update ddon_system_mail_attachment by making 'attachment_id' a primary key and applying auto increment to the field.
            db.ExecuteNonQuery(conn, "DROP TABLE IF EXISTS ddon_system_mail_attachment;", command => {});

            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/equip_migration_sqlite.sql");
            db.Execute(conn, adaptedSchema);

            Dictionary<uint, SystemMailMessage> deliveries = new Dictionary<uint, SystemMailMessage>();
            foreach (var equippedItem in equippedItems)
            {
                if (!characterIdMapping.ContainsKey(equippedItem.CharacterCommonId))
                {
                    unknownCommonIds.Add(equippedItem.CharacterCommonId);
                    continue;
                }

                uint characterId = characterIdMapping[equippedItem.CharacterCommonId];
                if (!deliveries.ContainsKey(characterId))
                {
                    deliveries[characterId] = new SystemMailMessage()
                    {
                        Title = "Item Reclamation Service",
                        Body = "This mail contains items which used to be equipped to the player character and characters pawns.\n" +
                               "\n" +
                               "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n" +
                               "!!!There may be more items than the 3 shown in the UI.!!!\n" +
                               "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n" +
                               "\n\n" +
                               "Press \"Receieve All\" to reclaim your items.\n" +
                               "If you are unable to receive all the items. Make room and try again.\n" +
                               "\n" +
                               "NOTE: If your main weapon was unequipped, equip that weapon and relog for it to appear properly.",
                        CharacterId = characterId,
                        SenderName = "Rumis' Ghost",
                        MessageState = MailState.Unopened
                    };
                }

                var itemDesc = db.SelectItem(conn, equippedItem.ItemUId);
                deliveries[characterId].Attachments.Add(new SystemMailAttachment()
                {
                    AttachmentType = SystemMailAttachmentType.Item,
                    Param1 = itemDesc.ItemId,
                    Param2 = 1,
                    MessageId = (ulong)(deliveries[characterId].Attachments.Count + 1),
                    IsReceived = false,
                });
            }

            foreach (var delivery in deliveries.Values)
            {
                SystemMailService.DeliverSystemMailMessage(db, conn, delivery);
            }

            if (unknownCommonIds.Count > 0)
            {
                Logger.Error($"Failed to migrate items for {unknownCommonIds.Count} common ids (unable to match 'character_common_id' to a 'character_id')");
                foreach (var commonId in unknownCommonIds)
                {
                    Logger.Info($"character_common_id = {commonId}");
                }
            }

            // The point of no return
            Logger.Info("Removing all items from ddon_equip_item and ddon_equip_job_item");
            db.ExecuteNonQuery(conn, "DELETE FROM ddon_equip_item;", command => { });
            db.ExecuteNonQuery(conn, "DELETE FROM ddon_equip_job_item;", command => { });

            return true;
        }
    }
}
