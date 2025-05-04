using System;
using System.Data.Common;
using System.Text;
using Arrowgene.Ddon.Database.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string SqlDeleteAccount = "DELETE FROM \"account\" WHERE \"id\"=@id;";

    private static readonly string[] AccountFields =
    [
        "name", "normal_name", "hash", "mail", "mail_verified", "mail_verified_at", "mail_token", "password_token", "login_token", "login_token_created", "state", "last_login",
        "created"
    ];

    private static readonly string SqlInsertAccount = $"INSERT INTO \"account\" ({BuildQueryField(AccountFields)}) VALUES ({BuildQueryInsert(AccountFields)});";
    private static readonly string SqlSelectAccountById = $"SELECT \"id\", {BuildQueryField(AccountFields)} FROM \"account\" WHERE \"id\"=@id;";
    private static readonly string SqlSelectAccountByName = $"SELECT \"id\", {BuildQueryField(AccountFields)} FROM \"account\" WHERE \"normal_name\"=@normal_name;";
    private static readonly string SqlSelectAccountByLoginToken = $"SELECT \"id\", {BuildQueryField(AccountFields)} FROM \"account\" WHERE \"login_token\"=@login_token;";
    private static readonly string SqlUpdateAccount = $"UPDATE \"account\" SET {BuildQueryUpdate(AccountFields)} WHERE \"id\"=@id;";

    public override Account? CreateAccount(string name, string mail, string hash)
    {
        Account account = new();
        account.Name = name;
        account.NormalName = name.ToLowerInvariant();
        account.Mail = mail;
        account.Hash = hash;
        account.State = AccountStateType.User;
        account.Created = DateTime.UtcNow;
        account.MailToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(mail));
        account.PasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(name));
        account.LoginToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(name));
        account.LoginTokenCreated = DateTime.UtcNow;
        account.MailVerifiedAt = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
        account.LastAuthentication = DateTime.UtcNow;
        Logger.Info($"Creating account: {account}");
        int rowsAffected = ExecuteNonQuery(SqlInsertAccount, command =>
        {
            AddParameter(command, "@name", account.Name);
            AddParameter(command, "@normal_name", account.NormalName);
            AddParameter(command, "@hash", account.Hash);
            AddParameter(command, "@mail", account.Mail);
            AddParameter(command, "@mail_verified", account.MailVerified);
            AddParameter(command, "@mail_verified_at", account.MailVerifiedAt);
            AddParameter(command, "@mail_token", account.MailToken);
            AddParameter(command, "@password_token", account.PasswordToken);
            AddParameter(command, "@login_token", account.LoginToken);
            AddParameter(command, "@login_token_created", account.LoginTokenCreated);
            AddParameterEnumInt32(command, "@state", account.State);
            AddParameter(command, "@last_login", account.LastAuthentication);
            AddParameter(command, "@created", account.Created);
        }, out long autoIncrement);
        if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
        {
            Logger.Error("Account could not be created because the database refused to acknowledge the change.");
            return null;
        }

        account.Id = (int)autoIncrement;

        return account;
    }

    public override Account? SelectAccountByName(string accountName)
    {
        accountName = accountName.ToLowerInvariant();
        Account account = null;
        ExecuteReader(SqlSelectAccountByName,
            command => { AddParameter(command, "@normal_name", accountName); }, reader =>
            {
                if (reader.Read()) account = ReadAccount(reader);
            });

        return account;
    }

    public override Account SelectAccountById(int accountId)
    {
        Account account = null;
        ExecuteReader(SqlSelectAccountById, command => { AddParameter(command, "@id", accountId); }, reader =>
        {
            if (reader.Read()) account = ReadAccount(reader);
        });
        return account;
    }

    public override Account? SelectAccountByLoginToken(string loginToken)
    {
        Account account = null;
        ExecuteReader(SqlSelectAccountByLoginToken,
            command => { AddParameter(command, "@login_token", loginToken); }, reader =>
            {
                if (reader.Read()) account = ReadAccount(reader);
            });

        return account;
    }

    public override bool UpdateAccount(Account account)
    {
        int rowsAffected = ExecuteNonQuery(SqlUpdateAccount, command =>
        {
            AddParameter(command, "@name", account.Name);
            AddParameter(command, "@normal_name", account.NormalName);
            AddParameter(command, "@hash", account.Hash);
            AddParameter(command, "@mail", account.Mail);
            AddParameter(command, "@mail_verified", account.MailVerified);
            AddParameter(command, "@mail_verified_at", account.MailVerifiedAt);
            AddParameter(command, "@mail_token", account.MailToken);
            AddParameter(command, "@password_token", account.PasswordToken);
            AddParameter(command, "@login_token", account.LoginToken);
            AddParameter(command, "@login_token_created", account.LoginTokenCreated);
            AddParameterEnumInt32(command, "@state", account.State);
            AddParameter(command, "@last_login", account.LastAuthentication);
            AddParameter(command, "@created", account.Created);
            AddParameter(command, "@id", account.Id);
        });
        return rowsAffected > NoRowsAffected;
    }

    public override bool DeleteAccount(int accountId)
    {
        int rowsAffected = ExecuteNonQuery(SqlDeleteAccount,
            command => { AddParameter(command, "@id", accountId); });
        return rowsAffected > NoRowsAffected;
    }

    private Account ReadAccount(DbDataReader reader)
    {
        Account account = new();
        account.Id = GetInt32(reader, "id");
        account.Name = GetString(reader, "name");
        account.NormalName = GetString(reader, "normal_name");
        account.Hash = GetString(reader, "hash");
        account.Mail = GetString(reader, "mail");
        account.MailVerified = GetBoolean(reader, "mail_verified");
        account.MailVerifiedAt = GetDateTimeNullable(reader, "mail_verified_at");
        account.MailToken = GetStringNullable(reader, "mail_token");
        account.PasswordToken = GetStringNullable(reader, "password_token");
        account.LoginToken = GetStringNullable(reader, "login_token");
        account.LoginTokenCreated = GetDateTimeNullable(reader, "login_token_created");
        account.State = (AccountStateType)GetInt32(reader, "state");
        account.LastAuthentication = GetDateTimeNullable(reader, "last_login");
        account.Created = GetDateTime(reader, "created");
        return account;
    }
}
