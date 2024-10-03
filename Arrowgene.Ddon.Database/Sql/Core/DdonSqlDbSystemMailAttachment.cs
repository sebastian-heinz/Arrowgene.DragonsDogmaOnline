using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
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
        protected static readonly string[] SystemMailAttachmentFields = new string[]
        {
            /* "attachment_id" */ "message_id",  "attachment_type", "is_received", "param0", "param1", "param2", "param3"
        };

        private readonly string SqlInsertSystemMailAttachment = $"INSERT INTO \"ddon_system_mail_attachment\" ({BuildQueryField(SystemMailAttachmentFields)}) VALUES ({BuildQueryInsert(SystemMailAttachmentFields)});";
        private readonly string SqlSelectSystemMailAttachments = $"SELECT \"attachment_id\", {BuildQueryField(SystemMailAttachmentFields)} FROM \"ddon_system_mail_attachment\" WHERE \"message_id\" = @message_id;";
        private readonly string SqlDeleteSystemMailAttachment = $"DELETE FROM \"ddon_system_mail_attachment\" WHERE \"message_id\"=@message_id AND \"attachment_id\"=@attachment_id;";

        private readonly string SqlUpdateSystemMailAttachment = $"UPDATE \"ddon_system_mail_attachment\" SET \"is_received\"=@is_received WHERE \"message_id\"=@message_id AND \"attachment_id\"=@attachment_id;";

        public long InsertSystemMailAttachment(SystemMailAttachment attachment)
        {
            using TCon connection = OpenNewConnection();
            return InsertSystemMailAttachment(connection, attachment);
        }

        public long InsertSystemMailAttachment(DbConnection connection, SystemMailAttachment attachment)
        {
            ExecuteNonQuery((TCon)connection, SqlInsertSystemMailAttachment, command =>
            {
                AddParameter(command, "message_id", attachment.MessageId);
                AddParameter(command, "attachment_type", (uint) attachment.AttachmentType);
                AddParameter(command, "is_received", attachment.IsReceived);
                AddParameter(command, "param0", attachment.Param0);
                AddParameter(command, "param1", attachment.Param1);
                AddParameter(command, "param2", attachment.Param2);
                AddParameter(command, "param3", attachment.Param3);
            }, out long autoIncrement);

            return autoIncrement;
        }

        public List<SystemMailAttachment> SelectAttachmentsForSystemMail(ulong messageId)
        {
            using TCon connection = OpenNewConnection();
            return SelectAttachmentsForSystemMail(connection, messageId);
        }

        public List<SystemMailAttachment> SelectAttachmentsForSystemMail(TCon conn, ulong messageId)
        {
            List<SystemMailAttachment> results = new List<SystemMailAttachment>();

            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectSystemMailAttachments,
                command => {
                    AddParameter(command, "@message_id", messageId);
                }, reader => {
                    while (reader.Read())
                    {
                        var result = ReadSystemMailAttachment(reader);
                        results.Add(result);
                    }
                });
            });

            return results;
        }

        public bool UpdateSystemMailAttachmentReceivedStatus(ulong messageId, ulong attachmentId, bool isReceived)
        {
            using TCon connection = OpenNewConnection();
            return UpdateSystemMailAttachmentReceivedStatus(connection, messageId, attachmentId, isReceived);
        }

        bool UpdateSystemMailAttachmentReceivedStatus(TCon conn, ulong messageId, ulong attachmentId, bool isReceived)
        {
            return ExecuteNonQuery(conn, SqlUpdateSystemMailAttachment, command =>
            {
                AddParameter(command, "message_id", messageId);
                AddParameter(command, "attachment_id", attachmentId);
                AddParameter(command, "is_received", isReceived);
            }) == 1; ;
        }

        public bool DeleteSystemMailAttachment(ulong messageId)
        {
            using TCon connection = OpenNewConnection();
            return DeleteSystemMailAttachment(connection, messageId);
        }

        public bool DeleteSystemMailAttachment(TCon conn, ulong messageId)
        {
            return ExecuteNonQuery(conn, SqlDeleteSystemMailAttachment, command =>
            {
                AddParameter(command, "@message_id", messageId);
            }) == 1;
        }

        private SystemMailAttachment ReadSystemMailAttachment(TReader reader)
        {
            SystemMailAttachment obj = new SystemMailAttachment();
            obj.MessageId = GetUInt64(reader, "message_id");
            obj.AttachmentId = GetUInt64(reader, "attachment_id");
            obj.AttachmentType = (SystemMailAttachmentType)GetUInt32(reader, "attachment_type");
            obj.IsReceived = GetBoolean(reader, "is_received");
            obj.Param0 = GetString(reader, "param0");
            obj.Param1 = GetUInt32(reader, "param1");
            obj.Param2 = GetUInt32(reader, "param2");
            obj.Param3 = GetUInt32(reader, "param3");
            return obj;
        }
    }
}

