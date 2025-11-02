using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public partial class DdonSqlDb : SqlDb
    {
        protected static readonly string[] MailFields = new[]
        {
        /* message_id */ "character_id", "message_state", "sender_id", "message_title", "message_body", "send_date"
    };

        private readonly string SqlDeleteMailMessage = "DELETE FROM \"ddon_mail\" WHERE \"message_id\"=@message_id;";

        private readonly string SqlInsertMailMessage =
            $"INSERT INTO \"ddon_mail\" ({BuildQueryField(MailFields)}) VALUES ({BuildQueryInsert(MailFields)});";

        private readonly string SqlSelectMailMessage =
            @"SELECT 
	            mail.*,
	            COALESCE(ch.first_name, 'An Unknown') AS first_name,
	            COALESCE(ch.last_name, 'Arisen') AS last_name,
	            cp.short_name AS clan_name
            FROM ddon_mail mail
            INNER JOIN ddon_character ch ON mail.sender_id = ch.character_id
            LEFT JOIN ddon_clan_membership cm ON cm.character_id = mail.character_id
            LEFT JOIN ddon_clan_param cp ON cp.clan_id = cm.clan_id
            WHERE message_id = @message_id
            ORDER BY send_date DESC 
            LIMIT 100;";

        private readonly string SqlSelectMailMessages =
            @"SELECT 
	            mail.*,
	            COALESCE(ch.first_name, 'An Unknown') AS first_name,
	            COALESCE(ch.last_name, 'Arisen') AS last_name,
	            cp.short_name AS clan_name
            FROM ddon_mail mail
            INNER JOIN ddon_character ch ON mail.sender_id = ch.character_id
            LEFT JOIN ddon_clan_membership cm ON cm.character_id = mail.character_id
            LEFT JOIN ddon_clan_param cp ON cp.clan_id = cm.clan_id
            WHERE mail.character_id = @character_id
            ORDER BY send_date DESC 
            LIMIT 100;";

        private readonly string SqlUpdateMailMessageState = "UPDATE \"ddon_mail\" SET \"message_state\"=@message_state WHERE \"message_id\"=@message_id;";

        public override long InsertMailMessage(MailMessage message, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                ExecuteNonQuery(connection, SqlInsertMailMessage, command =>
                {
                    AddParameter(command, "character_id", message.CharacterId);
                    AddParameter(command, "message_state", (byte)message.MessageState);
                    AddParameter(command, "sender_id", message.BaseInfo.CharacterId);
                    AddParameter(command, "message_title", message.Title);
                    AddParameter(command, "message_body", message.Body);
                    AddParameter(command, "message_title", message.Title);
                    AddParameter(command, "send_date", message.SendDate);
                }, out long autoIncrement);

                return autoIncrement;
            });
        }

        public override List<MailMessage> SelectMailMessages(uint characterId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                List<MailMessage> results = new();

                ExecuteReader(connection, SqlSelectMailMessages,
                    command => { AddParameter(command, "@character_id", characterId); }, reader =>
                    {
                        while (reader.Read())
                        {
                            MailMessage result = ReadMailMessage(reader);
                            results.Add(result);
                        }
                    });

                return results;
            });
        }


        public override MailMessage SelectMailMessage(ulong messageId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                MailMessage result = new();

                ExecuteReader(connection, SqlSelectMailMessage,
                    command => { AddParameter(command, "@message_id", messageId); }, reader =>
                    {
                        if (reader.Read()) result = ReadMailMessage(reader);
                    });

                return result;
            });
        }

        public override bool UpdateMailMessageState(ulong messageId, MailState messageState, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(connection, SqlUpdateMailMessageState, command =>
                {
                    AddParameter(command, "message_id", messageId);
                    AddParameter(command, "message_state", (uint)messageState);
                }) == 1;
            });
        }

        public override bool DeleteMailMessage(ulong messageId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(connection, SqlDeleteMailMessage, command => { AddParameter(command, "@message_id", messageId); }) == 1;
            });
        }

        private MailMessage ReadMailMessage(DbDataReader reader)
        {
            MailMessage obj = new();
            obj.MessageId = GetUInt64(reader, "message_id");
            obj.CharacterId = GetUInt32(reader, "character_id");
            obj.MessageState = (MailState)GetUInt32(reader, "message_state");
            obj.Title = GetString(reader, "message_title");
            obj.Body = GetString(reader, "message_body");
            obj.SendDate = GetUInt64(reader, "send_date");

            obj.BaseInfo.CharacterId = GetUInt32(reader, "sender_id");
            obj.BaseInfo.ClanName = GetStringNullable(reader, "clan_name") ?? string.Empty;
            obj.BaseInfo.CharacterName.FirstName = GetString(reader, "first_name");
            obj.BaseInfo.CharacterName.LastName = GetString(reader, "last_name");

            return obj;
        }
    }
}
