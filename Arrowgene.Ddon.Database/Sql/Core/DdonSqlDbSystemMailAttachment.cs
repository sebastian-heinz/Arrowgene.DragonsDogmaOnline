using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    protected static readonly string[] SystemMailAttachmentFields = new[]
    {
        /* "attachment_id" */ "message_id", "attachment_type", "is_received", "param0", "param1", "param2", "param3"
    };

    private readonly string SqlDeleteSystemMailAttachment =
        "DELETE FROM \"ddon_system_mail_attachment\" WHERE \"message_id\"=@message_id AND \"attachment_id\"=@attachment_id;";

    private readonly string SqlInsertSystemMailAttachment =
        $"INSERT INTO \"ddon_system_mail_attachment\" ({BuildQueryField(SystemMailAttachmentFields)}) VALUES ({BuildQueryInsert(SystemMailAttachmentFields)});";

    private readonly string SqlSelectSystemMailAttachments =
        $"SELECT \"attachment_id\", {BuildQueryField(SystemMailAttachmentFields)} FROM \"ddon_system_mail_attachment\" WHERE \"message_id\" = @message_id;";

    private readonly string SqlUpdateSystemMailAttachment =
        "UPDATE \"ddon_system_mail_attachment\" SET \"is_received\"=@is_received WHERE \"message_id\"=@message_id AND \"attachment_id\"=@attachment_id;";

    public override long InsertSystemMailAttachment(SystemMailAttachment attachment)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertSystemMailAttachment(connection, attachment);
    }

    public override long InsertSystemMailAttachment(DbConnection connection, SystemMailAttachment attachment)
    {
        ExecuteNonQuery(connection, SqlInsertSystemMailAttachment, command =>
        {
            AddParameter(command, "message_id", attachment.MessageId);
            AddParameter(command, "attachment_type", (uint)attachment.AttachmentType);
            AddParameter(command, "is_received", attachment.IsReceived);
            AddParameter(command, "param0", attachment.Param0);
            AddParameter(command, "param1", attachment.Param1);
            AddParameter(command, "param2", attachment.Param2);
            AddParameter(command, "param3", attachment.Param3);
        }, out long autoIncrement);

        return autoIncrement;
    }

    public override List<SystemMailAttachment> SelectAttachmentsForSystemMail(ulong messageId)
    {
        using DbConnection connection = OpenNewConnection();
        return SelectAttachmentsForSystemMail(connection, messageId);
    }

    public List<SystemMailAttachment> SelectAttachmentsForSystemMail(DbConnection conn, ulong messageId)
    {
        List<SystemMailAttachment> results = new();

        ExecuteInTransaction(conn =>
        {
            ExecuteReader(conn, SqlSelectSystemMailAttachments,
                command => { AddParameter(command, "@message_id", messageId); }, reader =>
                {
                    while (reader.Read())
                    {
                        SystemMailAttachment result = ReadSystemMailAttachment(reader);
                        results.Add(result);
                    }
                });
        });

        return results;
    }

    public override bool UpdateSystemMailAttachmentReceivedStatus(ulong messageId, ulong attachmentId, bool isReceived)
    {
        using DbConnection connection = OpenNewConnection();
        return UpdateSystemMailAttachmentReceivedStatus(connection, messageId, attachmentId, isReceived);
    }

    private bool UpdateSystemMailAttachmentReceivedStatus(DbConnection conn, ulong messageId, ulong attachmentId, bool isReceived)
    {
        return ExecuteNonQuery(conn, SqlUpdateSystemMailAttachment, command =>
        {
            AddParameter(command, "message_id", messageId);
            AddParameter(command, "attachment_id", attachmentId);
            AddParameter(command, "is_received", isReceived);
        }) == 1;
        ;
    }

    public override bool DeleteSystemMailAttachment(ulong messageId)
    {
        using DbConnection connection = OpenNewConnection();
        return DeleteSystemMailAttachment(connection, messageId);
    }

    public bool DeleteSystemMailAttachment(DbConnection conn, ulong messageId)
    {
        return ExecuteNonQuery(conn, SqlDeleteSystemMailAttachment, command => { AddParameter(command, "@message_id", messageId); }) == 1;
    }

    private SystemMailAttachment ReadSystemMailAttachment(DbDataReader reader)
    {
        SystemMailAttachment obj = new();
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
