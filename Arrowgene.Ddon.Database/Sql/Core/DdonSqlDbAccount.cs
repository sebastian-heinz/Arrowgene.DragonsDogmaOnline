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
            "SELECT `id`, `name`, `normal_name`, `hash`, `mail`, `mail_verified`, `mail_verified_at`, `mail_token`, `password_token`, `login_token`, `login_token_created`, `state`, `last_login`, `created` FROM `account` WHERE `normal_name`=@normal_name;";

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

            // TODO TEMP RUMI - ADD Dump Character for entering world
            Rumi(account.Id);
            // TODO TEMP RUMI

            return account;
        }

        private void Rumi(int accountId)
        {
            string sql = "INSERT INTO `ddon_character` (id,account_id,first_name,last_name,created,sex,voice,voice_pitch,personality,speech_freq,body_type,hair,beard,makeup,scar,eye_preset_no,nose_preset_no,mouth_preset_no,eyebrow_tex_no,color_skin,color_hair,color_beard,color_eyebrow,color_r_eye,color_l_eye,color_makeup,sokutobu,hitai,mimi_jyouge,kannkaku,mabisasi_jyouge,hanakuchi_jyouge,ago_saki_haba,ago_zengo,ago_saki_jyouge,hitomi_ookisa,me_ookisa,me_kaiten,mayu_kaiten,mimi_ookisa,mimi_muki,elf_mimi,miken_takasa,miken_haba,hohobone_ryou,hohobone_jyouge,hohoniku,erahone_jyouge,erahone_haba,hana_jyouge,hana_haba,hana_takasa,hana_kakudo,kuchi_haba,kuchi_atsusa,eyebrow_uv_offset_x,eyebrow_uv_offset_y,wrinkle,wrinkle_albedo_blend_rate,wrinkle_detail_normal_power,muscle_albedo_blend_rate,muscle_detail_normal_power,height,head_size,neck_offset,neck_scale,upper_body_scale_x,belly_size,teat_scale,tekubi_size,koshi_offset,koshi_size,ankle_offset,fat,muscle,motion_filter,hp,stamina,revive_point,max_hp,max_stamina,white_hp,gain_hp,gain_stamina,gain_attack,gain_defense,gain_magic_attack,gain_magic_defense)" +
                         "VALUES (2117592," + accountId + ",'Asd','Sdf','2022-01-20 20:50:42.7662046',1,1,30000,1,1,0,25,18,0,0,0,0,0,0,0,41,0,41,18,18,0,30000,30000,30000,29720,29701,30002,29999,29997,30000,29998,29885,30000,30000,29850,30000,30000,30000,29997,29950,29999,29899,30280,29640,30002,29919,30000,30000,29653,29891,30001,29610,30000,30000,30000,30000,30000,48000,40603,30020,38500,40510,39803,40000,40000,28000,29100,29599,29500,31500,29000,0,0,0,0,0,0,0,0,0,0,0,0);";
            Execute(sql);
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
