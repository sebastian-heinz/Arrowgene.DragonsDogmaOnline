using System;
using System.Data.Common;
using Arrowgene.Ddon.Database.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertAccount =
            "INSERT INTO `account` (`name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`, `mail_token`, `password_token`, `login_token`, `login_token_created`, `state`, `last_login`, `created`) VALUES (@name, @normal_name, @hash, @mail, @mail_verified, @mail_verified_at, @mail_token, @password_token, @login_token, @login_token_created, @state, @last_login, @created);";

        private const string SqlSelectAccountById =
            "SELECT `id`, `name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`, `mail_token`, `password_token`, `login_token`, `login_token_created`, `state`, `last_login`, `created` FROM `account` WHERE `id`=@id;";

        private const string SqlSelectAccountByName =
            "SELECT `id`, `name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`, `mail_token`, `password_token`, `login_token`, `login_token_created`, `state`, `last_login`, `created` FROM `account` WHERE `name`=@name;";

        private const string SqlSelectAccountByLoginToken =
            "SELECT `id`, `name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`, `mail_token`, `password_token`, `login_token`, `login_token_created`, `state`, `last_login`, `created` FROM `account` WHERE `login_token`=@login_token;";

        private const string SqlUpdateAccount =
            "UPDATE `account` SET `name`=@name, `normal_name`=@normal_name, `hash`=@hash, `mail`=@mail, `mail_verified`=@mail_verified, `mail_verified_at`=@mail_verified_at, `mail_token`=@mail_token, `password_token`=@password_token, `login_token`=@login_token, `login_token_created`=@login_token_created, `state`=@state, `last_login`=@last_login, `created`=@created WHERE `id`=@id;";

        private const string SqlDeleteAccount =
            "DELETE FROM `account` WHERE `id`=@id;";

        public Account CreateAccount(string name, string mail, string hash)
        {
            Account account = new Account();
            account.Name = name;
            account.NormalName = name.ToLowerInvariant();
            account.Mail = mail;
            account.Hash = hash;
            account.State = AccountStateType.User;
            account.Created = DateTime.Now;
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
                AddParameter(command, "@last_login", account.LastLogin);
                AddParameter(command, "@created", account.Created);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return null;
            }

            account.Id = (int) autoIncrement;
            return account;
        }

        public Account SelectAccountByName(string accountName)
        {
            accountName = accountName.ToLowerInvariant();
            Account account = null;
            ExecuteReader(SqlSelectAccountByName,
                command => { AddParameter(command, "@normal_name", accountName); }, reader =>
                {
                    if (reader.Read())
                    {
                        account = ReadAccount(reader);
                    }
                });

            return account;
        }

        public Account SelectAccountById(int accountId)
        {
            Account account = null;
            ExecuteReader(SqlSelectAccountById, command => { AddParameter(command, "@id", accountId); }, reader =>
            {
                if (reader.Read())
                {
                    account = ReadAccount(reader);
                }
            });
            return account;
        }

        public Account SelectAccountByLoginToken(string loginToken)
        {
            Account account = null;
            ExecuteReader(SqlSelectAccountByLoginToken,
                command => { AddParameter(command, "@login_token", loginToken); }, reader =>
                {
                    if (reader.Read())
                    {
                        account = ReadAccount(reader);
                    }
                });

            return account;
        }

        public bool UpdateAccount(Account account)
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
                AddParameter(command, "@last_login", account.LastLogin);
                AddParameter(command, "@created", account.Created);
                AddParameter(command, "@id", account.Id);
            });
            return rowsAffected > NoRowsAffected;
        }
        
        public bool DeleteAccount(int accountId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteAccount,
                command => { AddParameter(command, "@id", accountId); });
            return rowsAffected > NoRowsAffected;
        }

        private Account ReadAccount(DbDataReader reader)
        {
            Account account = new Account();
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
            account.LoginTokenCreated = GetDateTime(reader, "login_token_created");
            account.State = (AccountStateType) GetInt32(reader, "state");
            account.LastLogin = GetDateTimeNullable(reader, "last_login");
            account.Created = GetDateTime(reader, "created");
            return account;
        }
    }
}
