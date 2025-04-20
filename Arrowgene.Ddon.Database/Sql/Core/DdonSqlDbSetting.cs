namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string SqlSelectSetting = "SELECT \"value\" FROM \"setting\" WHERE \"key\" = @key;";
    private const string SqlInsertSetting = "INSERT INTO \"setting\" (\"key\", \"value\") VALUES (@key, @value);";
    private const string SqlUpdateSetting = "UPDATE \"setting\" SET \"value\"=@value WHERE \"key\"=@key;";
    private const string SqlDeleteSetting = "DELETE FROM \"setting\" WHERE \"key\"=@key;";


    public bool SetSetting(string key, string value)
    {
        int rowsAffected = ExecuteNonQuery(SqlUpdateSetting, command =>
        {
            AddParameter(command, "@key", key);
            AddParameter(command, "@value", value);
        });
        if (rowsAffected > NoRowsAffected) return true;

        rowsAffected = ExecuteNonQuery(SqlInsertSetting, command =>
        {
            AddParameter(command, "@key", key);
            AddParameter(command, "@value", value);
        });

        return rowsAffected > NoRowsAffected;
    }

    public string GetSetting(string key)
    {
        string value = null;
        ExecuteReader(SqlSelectSetting, command => { AddParameter(command, "@key", key); }, reader => { value = GetString(reader, "value"); });
        return value;
    }

    public bool DeleteSetting(string key)
    {
        int rowsAffected = ExecuteNonQuery(SqlDeleteSetting, command => { AddParameter(command, "@key", key); });
        return rowsAffected > NoRowsAffected;
    }
}
