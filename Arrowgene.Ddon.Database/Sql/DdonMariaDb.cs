using System;
using System.Data;
using Arrowgene.Ddon.Database.Sql.Core;
using Arrowgene.Logging;
using MySqlConnector;

namespace Arrowgene.Ddon.Database.Sql
{
    /// <summary>
    /// MariaDB Ddon database.
    /// </summary>
    public class DdonMariaDb : DdonSqlDb<MySqlConnection, MySqlCommand, MySqlDataReader>, IDatabase
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonMariaDb));

        private readonly DatabaseSetting _settings;
        private string _connectionString;

        public DdonMariaDb(DatabaseSetting settings)
        {
            _settings = settings;
            Logger.Info($"Database Path: {settings.SqLiteFolder}");
        }

        public bool CreateDatabase()
        {
            _connectionString = BuildConnectionString(_settings);
            if (_connectionString == null)
            {
                Logger.Error($"Failed to build connection string");
                return false;
            }

            if (_settings.WipeOnStartup)
            {
                Logger.Info($"WipeOnStartup is currently not supported by '{_settings.Database}'.");
            }

            return true;
        }

        private string BuildConnectionString(DatabaseSetting settings)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = settings.Host,
                UserID = settings.User,
                Password = settings.Password,
                Database = settings.Database,
                IgnoreCommandTransaction = true
            };
            string connectionString = builder.ToString();
            Logger.Info($"Connection String: {connectionString}");
            return connectionString;
        }

        protected override MySqlConnection OpenConnection()
        {
            MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        protected override MySqlCommand Command(string query, MySqlConnection connection)
        {
            return new MySqlCommand(query, connection);
        }

        /// <summary>
        /// Always returns the first generated ID in a multi-statement environment. Ideally should be used on a per-connection basis.
        /// https://dev.mysql.com/doc/refman/8.0/en/information-functions.html#function_last-insert-id
        /// </summary>
        protected override long AutoIncrement(MySqlConnection connection, MySqlCommand command)
        {
            return command.LastInsertedId;
        }

        public override int Upsert(string table, string[] columns, object[] values, string whereColumn,
            object whereValue,
            out long autoIncrement)
        {
            throw new NotImplementedException();
        }

        protected override void AddParameter(MySqlCommand command, string name, ushort value)
        {
            AddParameter(command, name, value, DbType.UInt16);
        }
        
        protected override ushort GetUInt16(MySqlDataReader reader, string column)
        {
            return reader.GetUInt16(column);
        }

        protected override void AddParameter(MySqlCommand command, string name, uint value)
        {
            AddParameter(command, name, value, DbType.UInt32);
        }

        protected override uint GetUInt32(MySqlDataReader reader, string column)
        {
            return reader.GetUInt32(column);
        }

        protected override string SqlInsertOrIgnoreItem => $"INSERT IGNORE INTO \"ddon_item\" ({BuildQueryField(ItemFields)}) VALUES ({BuildQueryInsert(ItemFields)});";

        protected override string SqlReplaceCharacterJobData =>
            $"INSERT INTO \"ddon_character_job_data\" ({BuildQueryField(CDataCharacterJobDataFields)}) VALUES ({BuildQueryInsert(CDataCharacterJobDataFields)}) ON DUPLICATE KEY UPDATE {BuildQueryUpdateWithPrefix(string.Empty, CDataCharacterJobDataFields)};";

        protected override string SqlReplaceStorageItem =>
            $"INSERT INTO \"ddon_storage_item\" ({BuildQueryField(StorageItemFields)}) VALUES ({BuildQueryInsert(StorageItemFields)}) ON DUPLICATE KEY UPDATE {BuildQueryUpdateWithPrefix(string.Empty, StorageItemFields)};";

        protected override string SqlReplaceStorage =>
            $"INSERT INTO \"ddon_storage\" ({BuildQueryField(StorageFields)}) VALUES ({BuildQueryInsert(StorageFields)}) ON DUPLICATE KEY UPDATE {BuildQueryUpdateWithPrefix(string.Empty, StorageFields)};";

        protected override string SqlReplaceCommunicationShortcut =>
            $"INSERT INTO \"ddon_communication_shortcut\" ({BuildQueryField(CommunicationShortcutFields)}) VALUES ({BuildQueryInsert(CommunicationShortcutFields)}) ON DUPLICATE KEY UPDATE {BuildQueryUpdateWithPrefix(string.Empty, CommunicationShortcutFields)};";

        protected override string SqlReplaceEquipItem =>
            $"INSERT INTO \"ddon_equip_item\" ({BuildQueryField(CDataEquipItemFields)}) VALUES ({BuildQueryInsert(CDataEquipItemFields)}) ON DUPLICATE KEY UPDATE {BuildQueryUpdateWithPrefix(string.Empty, CDataEquipItemFields)};";

        protected override string SqlReplaceEquipJobItem =>
            $"INSERT INTO \"ddon_equip_job_item\" ({BuildQueryField(CDataEquipJobItemFields)}) VALUES ({BuildQueryInsert(CDataEquipJobItemFields)}) ON DUPLICATE KEY UPDATE {BuildQueryUpdateWithPrefix(string.Empty, CDataEquipJobItemFields)};";

        protected override string SqlReplaceEquippedAbility =>
            $"INSERT INTO \"ddon_equipped_ability\" ({BuildQueryField(EquippedAbilityFields)}) VALUES ({BuildQueryInsert(EquippedAbilityFields)}) ON DUPLICATE KEY UPDATE {BuildQueryUpdateWithPrefix(string.Empty, EquippedAbilityFields)};";

        protected override string SqlReplaceEquippedCustomSkill =>
            $"INSERT INTO \"ddon_equipped_custom_skill\" ({BuildQueryField(EquippedCustomSkillFields)}) VALUES ({BuildQueryInsert(EquippedCustomSkillFields)}) ON DUPLICATE KEY UPDATE {BuildQueryUpdateWithPrefix(string.Empty, EquippedCustomSkillFields)};";

        protected override string SqlReplaceNormalSkillParam =>
            $"INSERT INTO \"ddon_normal_skill_param\" ({BuildQueryField(CDataNormalSkillParamFields)}) VALUES ({BuildQueryInsert(CDataNormalSkillParamFields)}) ON DUPLICATE KEY UPDATE {BuildQueryUpdateWithPrefix(string.Empty, CDataNormalSkillParamFields)};";

        protected override string SqlReplaceShortcut =>
            $"INSERT INTO \"ddon_shortcut\" ({BuildQueryField(ShortcutFields)}) VALUES ({BuildQueryInsert(ShortcutFields)}) ON DUPLICATE KEY UPDATE {BuildQueryUpdateWithPrefix(string.Empty, ShortcutFields)};";

        protected override string SqlReplaceWalletPoint =>
            $"INSERT INTO \"ddon_wallet_point\" ({BuildQueryField(WalletPointFields)}) VALUES ({BuildQueryInsert(WalletPointFields)}) ON DUPLICATE KEY UPDATE {BuildQueryUpdateWithPrefix(string.Empty, WalletPointFields)};";

        protected override string SqlReplacePawnReaction =>
            $"INSERT INTO \"ddon_pawn_reaction\" ({BuildQueryField(CDataPawnReactionFields)}) VALUES ({BuildQueryInsert(CDataPawnReactionFields)}) ON DUPLICATE KEY UPDATE {BuildQueryUpdateWithPrefix(string.Empty, CDataPawnReactionFields)};";

        protected override string SqlReplaceSpSkill =>
            $"INSERT INTO \"ddon_sp_skill\" ({BuildQueryField(CDataSpSkillFields)}) VALUES ({BuildQueryInsert(CDataSpSkillFields)}) ON DUPLICATE KEY UPDATE {BuildQueryUpdateWithPrefix(string.Empty, CDataSpSkillFields)};";
    }
}
