#nullable enable
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] WalletPointFields = new string[]
        {
            "character_id", "type", "value"
        };

        private readonly string SqlInsertWalletPoint = $"INSERT INTO \"ddon_wallet_point\" ({BuildQueryField(WalletPointFields)}) VALUES ({BuildQueryInsert(WalletPointFields)});";
        private readonly string SqlInsertIfNotExistsWalletPoint = $"INSERT INTO \"ddon_wallet_point\" ({BuildQueryField(WalletPointFields)}) SELECT {BuildQueryInsert(WalletPointFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_wallet_point\" WHERE \"character_id\"=@character_id AND \"type\"=@type);";
        private static readonly string SqlUpdateWalletPoint = $"UPDATE \"ddon_wallet_point\" SET {BuildQueryUpdate(WalletPointFields)} WHERE \"character_id\"=@character_id AND \"type\"=@type";
        private static readonly string SqlSelectWalletPoints = $"SELECT {BuildQueryField(WalletPointFields)} FROM \"ddon_wallet_point\" WHERE \"character_id\"=@character_id;";
        private const string SqlDeleteWalletPoint = "DELETE FROM \"ddon_wallet_point\" WHERE \"character_id\"=@character_id AND \"type\"=@type";

        public bool InsertIfNotExistsWalletPoint(uint characterId, CDataWalletPoint walletPoint)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsWalletPoint(connection, characterId, walletPoint);
        }
        
        public bool InsertIfNotExistsWalletPoint(TCon connection, uint characterId, CDataWalletPoint walletPoint)
        {
            return ExecuteNonQuery(connection, SqlInsertIfNotExistsWalletPoint, command =>
            {
                AddParameter(command, characterId, walletPoint);
            }) == 1;
        }
        
        public bool InsertWalletPoint(uint characterId, CDataWalletPoint walletPoint)
        {
            using TCon connection = OpenNewConnection();
            return InsertWalletPoint(connection, characterId, walletPoint);
        }
        
        public bool InsertWalletPoint(TCon connection, uint characterId, CDataWalletPoint walletPoint)
        {
            return ExecuteNonQuery(connection, SqlInsertWalletPoint, command =>
            {
                AddParameter(command, characterId, walletPoint);
            }) == 1;
        }
        
        public bool ReplaceWalletPoint(uint characterId, CDataWalletPoint walletPoint)
        {
            using TCon connection = OpenNewConnection();
            return ReplaceWalletPoint(connection, characterId, walletPoint);
        }        
        
        public bool ReplaceWalletPoint(TCon connection, uint characterId, CDataWalletPoint walletPoint)
        {
            Logger.Debug("Inserting wallet point.");
            if (!InsertIfNotExistsWalletPoint(connection, characterId, walletPoint))
            {
                Logger.Debug("Wallet point already exists, replacing.");
                return UpdateWalletPoint(characterId, walletPoint, connection);
            }
            return true;
        }

        public bool UpdateWalletPoint(uint characterId, CDataWalletPoint updatedWalletPoint, DbConnection? connectionIn = null)
        {
            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                return ExecuteNonQuery(connection, SqlUpdateWalletPoint, command =>
                {
                    AddParameter(command, characterId, updatedWalletPoint);
                }) == 1;
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }
        }

        public bool DeleteWalletPoint(uint characterId, WalletType type)
        {
            return ExecuteNonQuery(SqlDeleteWalletPoint, command =>
            {
                AddParameter(command, "@character_id", characterId);
                AddParameter(command, "@type", (byte) type);
            }) == 1;
        }

        private CDataWalletPoint ReadWalletPoint(TReader reader)
        {
            CDataWalletPoint walletPoint = new CDataWalletPoint();
            walletPoint.Type = (WalletType) GetByte(reader, "type");
            walletPoint.Value = GetUInt32(reader, "value");
            return walletPoint;
        }

        private void AddParameter(TCom command, uint characterId, CDataWalletPoint walletPoint)
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "type", (byte) walletPoint.Type);
            AddParameter(command, "value", walletPoint.Value);
        }
    }
}
