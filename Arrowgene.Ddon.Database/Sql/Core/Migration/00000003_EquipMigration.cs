using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
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
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/equip_migration_sqlite.sql");
            db.Execute(conn, adaptedSchema);

            db.ExecuteReader(conn, @"SELECT ddon_character.character_id, ddon_character.character_common_id, ddon_character.first_name, ddon_character.last_name, ddon_character_common.job 
                                    FROM ddon_character 
                                    LEFT JOIN ddon_character_common 
                                    ON ddon_character.character_common_id = ddon_character_common.character_common_id;", action => {}, reader => 
            {
                while(reader.Read())
                {
                    uint characterId = db.GetUInt32(reader, "character_id");
                    uint characterCommonId = db.GetUInt32(reader, "character_common_id");
                    string charFirstName = db.GetString(reader, "first_name");
                    string charLastName = db.GetString(reader, "last_name");
                    JobId charJob = (JobId) db.GetByte(reader, "job");
                    
                    Logger.Info($"Migrating equipments for {charFirstName} {charLastName}");
                    MigrateEquippedItems(db, conn, characterId, characterCommonId, $"{charFirstName} {charLastName}", charJob);

                    int pawnIdx = 0;
                    db.ExecuteReader(conn, @"SELECT ddon_pawn.pawn_id, ddon_pawn.character_common_id, ddon_pawn.name, ddon_character_common.job 
                                            FROM ddon_pawn 
                                            LEFT JOIN ddon_character_common 
                                            ON ddon_pawn.character_common_id = ddon_character_common.character_common_id 
                                            WHERE character_id = @character_id;",
                    action => { db.AddParameter(action, "character_id", characterId); },
                    reader2 =>
                    {
                        while(reader2.Read())
                        {
                            uint pawnId = db.GetUInt32(reader2, "pawn_id");
                            uint pawnCommonId = db.GetUInt32(reader2, "character_common_id");
                            string pawnName = db.GetString(reader2, "name");
                            JobId pawnJob = (JobId) db.GetByte(reader2, "job");
                    
                            Logger.Info($"Migrating equipment for {charFirstName} {charLastName}'s pawn {pawnName}");
                            MigrateEquippedItems(db, conn, characterId, pawnCommonId, pawnName, pawnJob, pawnIdx);

                            pawnIdx++;
                        }
                    });
                }
            });

            return true;
        }

        private static void MigrateEquippedItems(IDatabase db, DbConnection conn, uint characterId, uint commonId, string name, JobId activeJob, int pawnIdx = -1)
        {
            StorageType storageType;
            int storageSlotOffset;
            if(pawnIdx == -1)
            {
                storageType = StorageType.CharacterEquipment;
                storageSlotOffset = 0;
            }
            else
            {
                storageType = StorageType.PawnEquipment;
                storageSlotOffset = pawnIdx*EquipmentTemplate.TOTAL_EQUIP_SLOTS*2;
            }

            foreach (JobId job in Enum.GetValues(typeof(JobId)))
            {
                db.ExecuteReader(conn, @"SELECT ddon_equip_item.item_uid, ddon_equip_item.equip_type, ddon_equip_item.equip_slot, ddon_item.item_id
                                        FROM ddon_equip_item
                                        LEFT JOIN ddon_item
                                        ON ddon_equip_item.item_uid = ddon_item.uid
                                        WHERE character_common_id = @character_common_id
                                        AND job = @job;",
                action => 
                {
                    db.AddParameter(action, "character_common_id", commonId);
                    db.AddParameter(action, "job", (byte) job);
                },
                reader =>
                {
                    List<uint> mailItemIds = new List<uint>();

                    while (reader.Read())
                    {
                        string uid = db.GetString(reader, "item_uid");
                        EquipType equipType = (EquipType) db.GetByte(reader, "equip_type");
                        byte equipSlot = db.GetByte(reader, "equip_slot");
                        uint itemId = db.GetUInt32(reader, "item_id");

                        int equipTypeOffset = equipType == EquipType.Performance ? 0 : EquipmentTemplate.TOTAL_EQUIP_SLOTS;

                        if (job == activeJob)
                        {
                            // Add items to storage
                            db.ExecuteNonQuery(conn, @"INSERT INTO ddon_storage_item(item_uid, character_id, storage_type, slot_no, item_num)
                                                        VALUES(@item_uid, @character_id, @storage_type, @slot_no, @item_num);",
                            action => 
                            {
                                db.AddParameter(action, "item_uid", uid);
                                db.AddParameter(action, "character_id", characterId);
                                db.AddParameter(action, "storage_type", (byte) storageType);
                                db.AddParameter(action, "slot_no", storageSlotOffset+equipTypeOffset+equipSlot);
                                db.AddParameter(action, "item_num", 1);
                            });
                        }
                        else
                        {
                            // Add items to mail
                            mailItemIds.Add(itemId);
                        }
                    }

                    if (mailItemIds.Count > 0)
                    {
                        // Send mail with the equipped items for this job
                        SystemMailMessage mail = new SystemMailMessage()
                        {
                            Title = $"{name}'s {job} Items",
                            Body = $"This mail contains items which used to be equipped by {name} as a {job}\n" +
                                    "\n" +
                                    "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n" +
                                    "!!!There may be more items than the 3 shown in the UI.!!!\n" +
                                    "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n" +
                                    "\n\n" +
                                    "Press \"Receive All\" to reclaim your items.\n" +
                                    "If you are unable to receive all the items. Make room and try again.\n" +
                                    "\n" +
                                    "NOTE: If your main weapon was unequipped, equip that weapon and relog for it to appear properly.",
                            CharacterId = characterId,
                            SenderName = "Item Reclamation Service",
                            MessageState = MailState.Unopened
                        };
                        foreach (uint itemId in mailItemIds)
                        {                
                            mail.Attachments.Add(new SystemMailAttachment()
                            {
                                AttachmentType = SystemMailAttachmentType.Item,
                                Param1 = itemId,
                                Param2 = 1,
                                MessageId = (ulong)(mail.Attachments.Count + 1),
                                IsReceived = false,
                            });
                        }
                        SystemMailService.DeliverSystemMailMessage(db, conn, mail);
                    }
                });
            }
        }
    }
}
