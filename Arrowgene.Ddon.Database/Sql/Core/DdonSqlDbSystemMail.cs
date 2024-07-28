using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Xml;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] SystemMailFields = new string[]
        {
            /* message_id */  "character_id", "message_state", "sender_name", "message_title", "message_body", "send_date"
        };

        private readonly string SqlInsertSystemMailMessage = $"INSERT INTO \"ddon_system_mail\" ({BuildQueryField(SystemMailFields)}) VALUES ({BuildQueryInsert(SystemMailFields)});";
        private readonly string SqlSelectSystemMailMessages = $"SELECT \"message_id\", {BuildQueryField(SystemMailFields)} FROM \"ddon_system_mail\" WHERE \"character_id\" = @character_id;";
        private readonly string SqlSelectSystemMailMessage = $"SELECT \"message_id\", {BuildQueryField(SystemMailFields)} FROM \"ddon_system_mail\" WHERE \"message_id\" = @message_id;";
        private readonly string SqlDeleteSystemMailMessage = $"DELETE FROM \"ddon_system_mail\" WHERE \"message_id\"=@message_id;";
        private readonly string SqlUpdateSystemMailMessageState = $"UPDATE \"ddon_system_mail\" SET \"message_state\"=@message_state WHERE \"message_id\"=@message_id;";

        public long InsertSystemMailMessage(SystemMailMessage message)
        {
            using TCon connection = OpenNewConnection();
            return InsertSystemMailMessage(connection, message);
        }

        public long InsertSystemMailMessage(DbConnection connection, SystemMailMessage message)
        {
            ExecuteNonQuery((TCon) connection, SqlInsertSystemMailMessage, command =>
            {
                AddParameter(command, "character_id", message.CharacterId);
                AddParameter(command, "message_state", (byte) message.MessageState);
                AddParameter(command, "sender_name", message.SenderName);
                AddParameter(command, "message_title", message.Title);
                AddParameter(command, "message_body", message.Body);
                AddParameter(command, "message_title", message.Title);
                AddParameter(command, "send_date", message.SendDate);
            }, out long autoIncrement);

            return autoIncrement;
        }

        public List<SystemMailMessage> SelectSystemMailMessages(uint characterId)
        {
            using TCon connection = OpenNewConnection();
            return SelectSystemMailMessages(connection, characterId);
        }

        public List<SystemMailMessage> SelectSystemMailMessages(TCon conn, uint characterId)
        {
            List<SystemMailMessage> results = new List<SystemMailMessage>();

            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectSystemMailMessages,
                command => {
                        AddParameter(command, "@character_id", characterId);
                    }, reader => {
                        while (reader.Read())
                        {
                            var result = ReadSystemMailMessage(reader);
                            results.Add(result);
                        }
                    });
            });

            return results;
        }

        public SystemMailMessage SelectSystemMailMessage(ulong messageId)
        {
            using TCon connection = OpenNewConnection();
            return SelectSystemMailMessage(connection, messageId);
        }

        public SystemMailMessage SelectSystemMailMessage(TCon conn, ulong messageId)
        {
            SystemMailMessage result = new SystemMailMessage();

            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectSystemMailMessage,
                command => {
                    AddParameter(command, "@message_id", messageId);
                }, reader => {
                    if (reader.Read())
                    {
                        result = ReadSystemMailMessage(reader);
                    }
                });
            });

            return result;
        }

        public bool UpdateSystemMailMessageState(ulong messageId, MailState messageState)
        {
            using TCon connection = OpenNewConnection();
            return UpdateSystemMailMessageState(connection, messageId, messageState);
        }

        public bool UpdateSystemMailMessageState(TCon connection, ulong messageId, MailState messageState)
        {
            return ExecuteNonQuery(connection, SqlUpdateSystemMailMessageState, command =>
            {
                AddParameter(command, "message_id", messageId);
                AddParameter(command, "message_state", (uint) messageState);
            }) == 1;
        }

        public bool DeleteSystemMailMessage(ulong messageId)
        {
            using TCon connection = OpenNewConnection();
            return DeleteSystemMailMessage(connection, messageId);
        }

        public bool DeleteSystemMailMessage(TCon conn, ulong messageId)
        {
            return ExecuteNonQuery(conn, SqlDeleteSystemMailMessage, command =>
            {
                AddParameter(command, "@message_id", messageId);
            }) == 1;
        }

        private SystemMailMessage ReadSystemMailMessage(TReader reader)
        {
            SystemMailMessage obj = new SystemMailMessage();
            obj.MessageId = GetUInt64(reader, "message_id");
            obj.CharacterId = GetUInt32(reader, "character_id");
            obj.MessageState = (MailState) GetUInt32(reader, "message_state");
            obj.SenderName = GetString(reader, "sender_name");
            obj.Title = GetString(reader, "message_title");
            obj.Body = GetString(reader, "message_body");
            obj.SendDate = GetUInt64(reader, "send_date");
            return obj;
        }
    }
}

