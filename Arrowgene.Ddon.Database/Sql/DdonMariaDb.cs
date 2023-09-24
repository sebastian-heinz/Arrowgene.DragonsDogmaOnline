using System;
using Arrowgene.Ddon.Database.Sql.Core;
using Arrowgene.Logging;
using MySqlConnector;

namespace Arrowgene.Ddon.Database.Sql
{
    public class DdonMariaDb : DdonSqlDb<MySqlConnection, MySqlCommand, MySqlDataReader>, IDatabase
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonMariaDb));

        private string _connectionString;

        public DdonMariaDb(string host, string user, string password, string database, bool wipeOnStartup)
        {
            _connectionString = BuildConnectionString(host, user, password, database);
            if (wipeOnStartup)
            {
                Logger.Info($"WipeOnStartup is currently not supported.");
            }
        }

        public bool CreateDatabase()
        {
            if (_connectionString == null)
            {
                Logger.Error($"Failed to build connection string");
                return false;
            }

            ReusableConnection = new MySqlConnection(_connectionString);
            return true;
        }

        private string BuildConnectionString(string host, string user, string password, string database)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = host,
                UserID = user,
                Password = password,
                Database = database,
                IgnoreCommandTransaction = true,
                Pooling = true
            };
            string connectionString = builder.ToString();
            Logger.Info($"Connection String: {connectionString}");
            return connectionString;
        }

        protected override MySqlConnection OpenNewConnection()
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
