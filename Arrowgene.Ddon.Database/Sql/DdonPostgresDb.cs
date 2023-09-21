using System;
using System.Data;
using Arrowgene.Ddon.Database.Sql.Core;
using Arrowgene.Logging;
using Npgsql;

namespace Arrowgene.Ddon.Database.Sql
{
    /// <summary>
    /// PostgreSQL Ddon database.
    /// </summary>
    public class DdonPostgresDb : DdonSqlDb<NpgsqlConnection, NpgsqlCommand, NpgsqlDataReader>, IDatabase
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonPostgresDb));

        private readonly DatabaseSetting _settings;
        private string _connectionString;
        private NpgsqlDataSource _dataSource;
        private NpgsqlConnection _reusableConnection;

        public DdonPostgresDb(DatabaseSetting settings)
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

            if (_dataSource == null)
            {
                var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
                dataSourceBuilder.EnableParameterLogging();
                _dataSource = dataSourceBuilder.Build();
            }

            _reusableConnection = _dataSource.OpenConnection();
            
            if (_settings.WipeOnStartup)
            {
                try
                {
                    using NpgsqlCommand command = _dataSource.CreateCommand("DROP SCHEMA public CASCADE;");
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return true;
        }

        private string BuildConnectionString(DatabaseSetting settings)
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder
            {
                Host = settings.Host,
                Username = settings.User,
                Password = settings.Password,
                Database = settings.Database
            };
            string connectionString = builder.ToString();
            Logger.Info($"Connection String: {connectionString}");
            return connectionString;
        }

        protected override NpgsqlConnection OpenNewConnection()
        {
            return _dataSource.OpenConnection();
        }

        protected override NpgsqlConnection OpenExistingConnection()
        {
            if (_reusableConnection.State == ConnectionState.Closed)
            {
                _reusableConnection.Open();
            }else if (_reusableConnection.State == ConnectionState.Broken)
            {
                _reusableConnection.Close();
                _reusableConnection.Open();
            }
            return _reusableConnection;
        }

        protected override NpgsqlCommand Command(string query, NpgsqlConnection connection)
        {
            return new NpgsqlCommand(query, connection);
        }

        /// <summary>
        /// Safe within the same connection session (transaction?), but unsafe if triggers are involved.
        /// https://stackoverflow.com/questions/2944297/postgresql-function-for-last-inserted-id
        /// </summary>
        protected override long AutoIncrement(NpgsqlConnection connection, NpgsqlCommand command)
        {
            using NpgsqlCommand lastIdCommand = new NpgsqlCommand("SELECT LASTVAL();", connection);
            return (long)lastIdCommand.ExecuteScalar();
        }

        public override int Upsert(string table, string[] columns, object[] values, string whereColumn,
            object whereValue,
            out long autoIncrement)
        {
            throw new NotImplementedException();
        }

        protected override void AddParameter(NpgsqlCommand command, string name, ushort value)
        {
            AddParameter(command, name, (short)value, DbType.Int16);
        }
        
        protected override ushort GetUInt16(NpgsqlDataReader reader, string column)
        {
            return (ushort)reader.GetInt16(reader.GetOrdinal(column));
        }

        protected override void AddParameter(NpgsqlCommand command, string name, uint value)
        {
            AddParameter(command, name, (int)value, DbType.Int32);
        }

        protected override void AddParameter(NpgsqlCommand command, string name, DateTime? value)
        {
            if (value.HasValue)
            {
                AddParameter(command, name, DateTime.SpecifyKind(value.Value, DateTimeKind.Utc), DbType.DateTime);
            }
            else
            {
                AddParameter(command, name, value, DbType.DateTime);
            }
        }

        protected override void AddParameter(NpgsqlCommand command, string name, DateTime value)
        {
            AddParameter(command, name, DateTime.SpecifyKind(value, DateTimeKind.Utc), DbType.DateTime);
        }
        
        protected override DateTime GetDateTime(NpgsqlDataReader reader, string column)
        {
            return DateTime.SpecifyKind(reader.GetDateTime(reader.GetOrdinal(column)), DateTimeKind.Utc);
        }

        protected override DateTime? GetDateTimeNullable(NpgsqlDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return DateTime.SpecifyKind(reader.GetDateTime(ordinal), DateTimeKind.Utc);
        }

        protected override uint GetUInt32(NpgsqlDataReader reader, string column)
        {
            return (uint)reader.GetInt32(reader.GetOrdinal(column));
        }

        protected override string SqlInsertOrIgnoreItem =>
            $"INSERT INTO \"ddon_item\" ({BuildQueryField(ItemFields)}) VALUES ({BuildQueryInsert(ItemFields)}) ON CONFLICT DO NOTHING;";

        protected override string SqlReplaceCharacterJobData =>
            $"INSERT INTO \"ddon_character_job_data\" ({BuildQueryField(CDataCharacterJobDataFields)}) VALUES ({BuildQueryInsert(CDataCharacterJobDataFields)}) ON CONFLICT ON CONSTRAINT pk_character_job_data DO UPDATE SET {BuildQueryUpdateWithPrefix("excluded.", CDataCharacterJobDataFields)};";

        protected override string SqlReplaceStorageItem =>
            $"INSERT INTO \"ddon_storage_item\" ({BuildQueryField(StorageItemFields)}) VALUES ({BuildQueryInsert(StorageItemFields)}) ON CONFLICT ON CONSTRAINT pk_ddon_storage_item DO UPDATE SET {BuildQueryUpdateWithPrefix("excluded.", StorageItemFields)};";

        protected override string SqlReplaceStorage =>
            $"INSERT INTO \"ddon_storage\" ({BuildQueryField(StorageFields)}) VALUES ({BuildQueryInsert(StorageFields)}) ON CONFLICT ON CONSTRAINT pk_ddon_storage DO UPDATE SET {BuildQueryUpdateWithPrefix("excluded.", StorageFields)};";

        protected override string SqlReplaceCommunicationShortcut =>
            $"INSERT INTO \"ddon_communication_shortcut\" ({BuildQueryField(CommunicationShortcutFields)}) VALUES ({BuildQueryInsert(CommunicationShortcutFields)}) ON CONFLICT ON CONSTRAINT pk_ddon_communication_shortcut DO UPDATE SET {BuildQueryUpdateWithPrefix("excluded.", CommunicationShortcutFields)};";

        protected override string SqlReplaceEquipItem =>
            $"INSERT INTO \"ddon_equip_item\" ({BuildQueryField(CDataEquipItemFields)}) VALUES ({BuildQueryInsert(CDataEquipItemFields)}) ON CONFLICT ON CONSTRAINT pk_ddon_equip_item DO UPDATE SET {BuildQueryUpdateWithPrefix("excluded.", CDataEquipItemFields)};";

        protected override string SqlReplaceEquipJobItem =>
            $"INSERT INTO \"ddon_equip_job_item\" ({BuildQueryField(CDataEquipJobItemFields)}) VALUES ({BuildQueryInsert(CDataEquipJobItemFields)}) ON CONFLICT ON CONSTRAINT pk_ddon_equip_job_item DO UPDATE SET {BuildQueryUpdateWithPrefix("excluded.", CDataEquipJobItemFields)};";

        protected override string SqlReplaceEquippedAbility =>
            $"INSERT INTO \"ddon_equipped_ability\" ({BuildQueryField(EquippedAbilityFields)}) VALUES ({BuildQueryInsert(EquippedAbilityFields)}) ON CONFLICT ON CONSTRAINT pk_ddon_equipped_ability DO UPDATE SET {BuildQueryUpdateWithPrefix("excluded.", EquippedAbilityFields)};";

        protected override string SqlReplaceEquippedCustomSkill =>
            $"INSERT INTO \"ddon_equipped_custom_skill\" ({BuildQueryField(EquippedCustomSkillFields)}) VALUES ({BuildQueryInsert(EquippedCustomSkillFields)}) ON CONFLICT ON CONSTRAINT pk_ddon_equipped_custom_skill DO UPDATE SET {BuildQueryUpdateWithPrefix("excluded.", EquippedCustomSkillFields)};";

        protected override string SqlReplaceNormalSkillParam =>
            $"INSERT INTO \"ddon_normal_skill_param\" ({BuildQueryField(CDataNormalSkillParamFields)}) VALUES ({BuildQueryInsert(CDataNormalSkillParamFields)}) ON CONFLICT ON CONSTRAINT pk_ddon_normal_skill_param DO UPDATE SET {BuildQueryUpdateWithPrefix("excluded.", CDataNormalSkillParamFields)};";

        protected override string SqlReplaceShortcut =>
            $"INSERT INTO \"ddon_shortcut\" ({BuildQueryField(ShortcutFields)}) VALUES ({BuildQueryInsert(ShortcutFields)}) ON CONFLICT ON CONSTRAINT pk_ddon_shortcut DO UPDATE SET {BuildQueryUpdateWithPrefix("excluded.", ShortcutFields)};";

        protected override string SqlReplaceWalletPoint =>
            $"INSERT INTO \"ddon_wallet_point\" ({BuildQueryField(WalletPointFields)}) VALUES ({BuildQueryInsert(WalletPointFields)}) ON CONFLICT ON CONSTRAINT pk_ddon_wallet_point DO UPDATE SET {BuildQueryUpdateWithPrefix("excluded.", WalletPointFields)};";

        protected override string SqlReplacePawnReaction => $"INSERT INTO \"ddon_pawn_reaction\" ({BuildQueryField(CDataPawnReactionFields)}) VALUES ({BuildQueryInsert(CDataPawnReactionFields)}) ON CONFLICT ON CONSTRAINT pk_ddon_pawn_reaction DO UPDATE SET {BuildQueryUpdateWithPrefix("excluded.", CDataPawnReactionFields)};";

        protected override string SqlReplaceSpSkill => $"INSERT INTO \"ddon_sp_skill\" ({BuildQueryField(CDataSpSkillFields)}) VALUES ({BuildQueryInsert(CDataSpSkillFields)}) ON CONFLICT ON CONSTRAINT pk_ddon_sp_skill DO UPDATE SET {BuildQueryUpdateWithPrefix("excluded.", CDataSpSkillFields)};";
    }
}
