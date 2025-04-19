#nullable enable
using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string SqlDeleteWalletPoint = "DELETE FROM \"ddon_wallet_point\" WHERE \"character_id\"=@character_id AND \"type\"=@type";

    protected static readonly string[] WalletPointFields = new[]
    {
        "character_id", "type", "value"
    };

    private static readonly string SqlUpdateWalletPoint =
        $"UPDATE \"ddon_wallet_point\" SET {BuildQueryUpdate(WalletPointFields)} WHERE \"character_id\"=@character_id AND \"type\"=@type";

    private static readonly string SqlSelectWalletPoints = $"SELECT {BuildQueryField(WalletPointFields)} FROM \"ddon_wallet_point\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlInsertIfNotExistsWalletPoint =
        $"INSERT INTO \"ddon_wallet_point\" ({BuildQueryField(WalletPointFields)}) SELECT {BuildQueryInsert(WalletPointFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_wallet_point\" WHERE \"character_id\"=@character_id AND \"type\"=@type);";

    private readonly string SqlInsertWalletPoint = $"INSERT INTO \"ddon_wallet_point\" ({BuildQueryField(WalletPointFields)}) VALUES ({BuildQueryInsert(WalletPointFields)});";

    public bool InsertIfNotExistsWalletPoint(uint characterId, CDataWalletPoint walletPoint)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertIfNotExistsWalletPoint(connection, characterId, walletPoint);
    }

    public bool InsertIfNotExistsWalletPoint(DbConnection connection, uint characterId, CDataWalletPoint walletPoint)
    {
        return ExecuteNonQuery(connection, SqlInsertIfNotExistsWalletPoint, command => { AddParameter(command, characterId, walletPoint); }) == 1;
    }

    public override bool InsertWalletPoint(uint characterId, CDataWalletPoint walletPoint)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertWalletPoint(connection, characterId, walletPoint);
    }

    public bool InsertWalletPoint(DbConnection connection, uint characterId, CDataWalletPoint walletPoint)
    {
        return ExecuteNonQuery(connection, SqlInsertWalletPoint, command => { AddParameter(command, characterId, walletPoint); }) == 1;
    }

    public override bool ReplaceWalletPoint(uint characterId, CDataWalletPoint walletPoint)
    {
        using DbConnection connection = OpenNewConnection();
        return ReplaceWalletPoint(connection, characterId, walletPoint);
    }

    public bool ReplaceWalletPoint(DbConnection connection, uint characterId, CDataWalletPoint walletPoint)
    {
        Logger.Debug("Inserting wallet point.");
        if (!InsertIfNotExistsWalletPoint(connection, characterId, walletPoint))
        {
            Logger.Debug("Wallet point already exists, replacing.");
            return UpdateWalletPoint(characterId, walletPoint, connection);
        }

        return true;
    }

    public override bool UpdateWalletPoint(uint characterId, CDataWalletPoint updatedWalletPoint, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            return ExecuteNonQuery(connection, SqlUpdateWalletPoint, command => { AddParameter(command, characterId, updatedWalletPoint); }) == 1;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public override bool DeleteWalletPoint(uint characterId, WalletType type)
    {
        return ExecuteNonQuery(SqlDeleteWalletPoint, command =>
        {
            AddParameter(command, "@character_id", characterId);
            AddParameter(command, "@type", (byte)type);
        }) == 1;
    }

    private CDataWalletPoint ReadWalletPoint(DbDataReader reader)
    {
        CDataWalletPoint walletPoint = new();
        walletPoint.Type = (WalletType)GetByte(reader, "type");
        walletPoint.Value = GetUInt32(reader, "value");
        return walletPoint;
    }

    private void AddParameter(DbCommand command, uint characterId, CDataWalletPoint walletPoint)
    {
        AddParameter(command, "character_id", characterId);
        AddParameter(command, "type", (byte)walletPoint.Type);
        AddParameter(command, "value", walletPoint.Value);
    }
}
