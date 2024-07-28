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

            List<Character> characters = db.SelectAllCharacters(conn);
            foreach (Character character in characters)
            {
                Logger.Info($"Migrating equipments for {character.FirstName} {character.LastName}");
                MigrateEquippedItems(db, conn, character, character);

                character.Pawns = db.SelectPawnsByCharacterId(conn, character.CharacterId);
                foreach (Pawn pawn in character.Pawns)
                {
                    Logger.Info($"Migrating equipment for {character.FirstName} {character.LastName}'s pawn {pawn.Name}");
                    MigrateEquippedItems(db, conn, character, pawn);
                }
            }

            return true;
        }

        private static void MigrateEquippedItems(IDatabase db, DbConnection conn, Character owner, CharacterCommon characterCommon)
        {
            StorageType storageType;
            int storageSlotOffset;
            string name;
            if(characterCommon is Character)
            {
                Character character = (Character) characterCommon;
                storageType = StorageType.CharacterEquipment;
                storageSlotOffset = 0;
                name = character.FirstName + " " + character.LastName;
            }
            else if(characterCommon is Pawn)
            {
                Pawn pawn = (Pawn) characterCommon;
                storageType = StorageType.PawnEquipment;
                storageSlotOffset = owner.Pawns.IndexOf(pawn)*EquipmentTemplate.TOTAL_EQUIP_SLOTS*2;
                name  = pawn.Name;
            }
            else
            {
                throw new Exception("CharacterCommon is not Character or Pawn");
            }

            foreach (var jobAndEquipment in characterCommon.EquipmentTemplate.GetAllEquipment())
            {
                JobId job = jobAndEquipment.Key;
                Dictionary<EquipType, List<Item>> equippedItemsByEquipType = jobAndEquipment.Value;
                List<Item> equippedItems = equippedItemsByEquipType.SelectMany(x => x.Value).ToList();
                if (job == characterCommon.Job)
                {
                    // Add items to the equipment storage type
                    for (int i = 0; i < equippedItems.Count; i++)
                    {
                        Item equippedItem = equippedItems[i];
                        if (equippedItem != null)
                        {
                            int storageSlot = storageSlotOffset + i+1;
                            db.InsertStorageItem(conn, owner.CharacterId, storageType, (ushort) storageSlot, 1, equippedItem);
                        }
                    }
                }
                else
                {
                    // Send mail with the equipped items
                    string jobName = Enum.GetName(typeof(JobId), job);
                    SystemMailMessage mail = new SystemMailMessage()
                    {
                        Title = $"{name}'s {jobName} Items",
                        Body = "This mail contains items which used to be equipped by "+name+" as a "+jobName+".\n" +
                                "\n" +
                                "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n" +
                                "!!!There may be more items than the 3 shown in the UI.!!!\n" +
                                "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n" +
                                "\n\n" +
                                "Press \"Receive All\" to reclaim your items.\n" +
                                "If you are unable to receive all the items. Make room and try again.\n" +
                                "\n" +
                                "NOTE: If your main weapon was unequipped, equip that weapon and relog for it to appear properly.",
                        CharacterId = owner.CharacterId,
                        SenderName = "Item Reclamation Service",
                        MessageState = MailState.Unopened
                    };
                    foreach (Item equippedItem in equippedItems)
                    {                
                        if(equippedItem != null)
                        {
                            mail.Attachments.Add(new SystemMailAttachment()
                            {
                                AttachmentType = SystemMailAttachmentType.Item,
                                Param1 = equippedItem.ItemId,
                                Param2 = 1,
                                MessageId = (ulong)(mail.Attachments.Count + 1),
                                IsReceived = false,
                            });
                        }
                    }

                    if(mail.Attachments.Count > 0)
                    {
                        SystemMailService.DeliverSystemMailMessage(db, conn, mail);
                    }
                }
            }
         }
    }
}
