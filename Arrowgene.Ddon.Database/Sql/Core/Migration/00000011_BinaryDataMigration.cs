using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class BinaryDataMigration : IMigrationStrategy
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(BinaryDataMigration));

        public uint From => 10;
        public uint To => 11;

        private readonly DatabaseSetting DatabaseSetting;

        public BinaryDataMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/binarydata_migration.sql");
            db.Execute(conn, adaptedSchema);

            byte[] blankBlob = new byte[C2SBinarySaveSetCharacterBinSaveDataReq.ARRAY_SIZE];

            db.ExecuteReader(conn, @"SELECT ddon_character.character_id FROM ddon_character;", action => { }, reader =>
            {
                while (reader.Read())
                {
                    uint characterId = db.GetUInt32(reader, "character_id");
                    Logger.Info($"Assigning blank binary data to character ID {characterId}");
                    db.ExecuteNonQuery(conn, @"INSERT INTO ddon_binary_data(character_id, binary_data)
                                                        VALUES(@character_id, @binary_data);",
                            action =>
                            {
                                db.AddParameter(action, "character_id", characterId);
                                db.AddParameter(action, "binary_data", blankBlob);
                            });
                }
            });

            return true;
        }
      }
}
