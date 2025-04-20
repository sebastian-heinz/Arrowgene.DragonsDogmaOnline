using Arrowgene.Ddon.Shared.Entity;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    protected static readonly string[] MetaFields = new[]
    {
        "db_version"
    };

    private static readonly string SqlInsertOrIgnoreMeta =
        $"INSERT INTO \"meta\" ({BuildQueryField(MetaFields)}) SELECT ({BuildQueryInsert(MetaFields)}) WHERE NOT EXISTS (SELECT 1 FROM \"meta\" LIMIT 1);";

    private static readonly string SqlUpdateMeta = $"UPDATE \"meta\" SET {BuildQueryUpdate(MetaFields)};";
    private static readonly string SqlSelectMeta = $"SELECT {BuildQueryField(MetaFields)} FROM \"meta\" LIMIT 1;";


    public override bool CreateMeta(DatabaseMeta meta)
    {
        int rowsAffected = ExecuteNonQuery(SqlInsertOrIgnoreMeta, command => { AddParameter(command, "@db_version", meta.DatabaseVersion); });

        return rowsAffected > NoRowsAffected;
    }

    public override bool SetMeta(DatabaseMeta meta)
    {
        int rowsAffected = ExecuteNonQuery(SqlUpdateMeta, command => { AddParameter(command, "@db_version", meta.DatabaseVersion); });

        return rowsAffected > NoRowsAffected;
    }

    public override DatabaseMeta GetMeta()
    {
        DatabaseMeta meta = new();
        ExecuteReader(SqlSelectMeta, command => { }, reader =>
        {
            if (reader.Read()) meta.DatabaseVersion = GetUInt32(reader, "db_version");
        });
        return meta;
    }
}
