using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    protected static readonly string[] SystemMailFields = new[]
    {
        /* message_id */ "character_id", "message_state", "sender_name", "message_title", "message_body", "send_date"
    };

    private readonly string SqlDeleteSystemMailMessage = "DELETE FROM \"ddon_system_mail\" WHERE \"message_id\"=@message_id;";

    private readonly string SqlInsertSystemMailMessage =
        $"INSERT INTO \"ddon_system_mail\" ({BuildQueryField(SystemMailFields)}) VALUES ({BuildQueryInsert(SystemMailFields)});";

    private readonly string SqlSelectSystemMailMessage =
        $"SELECT \"message_id\", {BuildQueryField(SystemMailFields)} FROM \"ddon_system_mail\" WHERE \"message_id\" = @message_id;";

    private readonly string SqlSelectSystemMailMessages =
        $"SELECT \"message_id\", {BuildQueryField(SystemMailFields)} FROM \"ddon_system_mail\" WHERE \"character_id\" = @character_id;";

    private readonly string SqlUpdateSystemMailMessageState = "UPDATE \"ddon_system_mail\" SET \"message_state\"=@message_state WHERE \"message_id\"=@message_id;";

    public override long InsertSystemMailMessage(SystemMailMessage message)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertSystemMailMessage(connection, message);
    }

    public override long InsertSystemMailMessage(DbConnection connection, SystemMailMessage message)
    {
        ExecuteNonQuery(connection, SqlInsertSystemMailMessage, command =>
        {
            AddParameter(command, "character_id", message.CharacterId);
            AddParameter(command, "message_state", (byte)message.MessageState);
            AddParameter(command, "sender_name", message.SenderName);
            AddParameter(command, "message_title", message.Title);
            AddParameter(command, "message_body", message.Body);
            AddParameter(command, "message_title", message.Title);
            AddParameter(command, "send_date", message.SendDate);
        }, out long autoIncrement);

        return autoIncrement;
    }

    public override List<SystemMailMessage> SelectSystemMailMessages(uint characterId)
    {
        using DbConnection connection = OpenNewConnection();
        return SelectSystemMailMessages(connection, characterId);
    }

    public List<SystemMailMessage> SelectSystemMailMessages(DbConnection conn, uint characterId)
    {
        List<SystemMailMessage> results = new();

        ExecuteInTransaction(conn =>
        {
            ExecuteReader(conn, SqlSelectSystemMailMessages,
                command => { AddParameter(command, "@character_id", characterId); }, reader =>
                {
                    while (reader.Read())
                    {
                        SystemMailMessage result = ReadSystemMailMessage(reader);
                        results.Add(result);
                    }
                });
        });

        return results;
    }

    public override SystemMailMessage SelectSystemMailMessage(ulong messageId)
    {
        using DbConnection connection = OpenNewConnection();
        return SelectSystemMailMessage(connection, messageId);
    }

    public SystemMailMessage SelectSystemMailMessage(DbConnection conn, ulong messageId)
    {
        SystemMailMessage result = new();

        ExecuteInTransaction(conn =>
        {
            ExecuteReader(conn, SqlSelectSystemMailMessage,
                command => { AddParameter(command, "@message_id", messageId); }, reader =>
                {
                    if (reader.Read()) result = ReadSystemMailMessage(reader);
                });
        });

        return result;
    }

    public override bool UpdateSystemMailMessageState(ulong messageId, MailState messageState)
    {
        using DbConnection connection = OpenNewConnection();
        return UpdateSystemMailMessageState(connection, messageId, messageState);
    }

    public bool UpdateSystemMailMessageState(DbConnection connection, ulong messageId, MailState messageState)
    {
        return ExecuteNonQuery(connection, SqlUpdateSystemMailMessageState, command =>
        {
            AddParameter(command, "message_id", messageId);
            AddParameter(command, "message_state", (uint)messageState);
        }) == 1;
    }

    public override bool DeleteSystemMailMessage(ulong messageId)
    {
        using DbConnection connection = OpenNewConnection();
        return DeleteSystemMailMessage(connection, messageId);
    }

    public bool DeleteSystemMailMessage(DbConnection conn, ulong messageId)
    {
        return ExecuteNonQuery(conn, SqlDeleteSystemMailMessage, command => { AddParameter(command, "@message_id", messageId); }) == 1;
    }

    private SystemMailMessage ReadSystemMailMessage(DbDataReader reader)
    {
        SystemMailMessage obj = new();
        obj.MessageId = GetUInt64(reader, "message_id");
        obj.CharacterId = GetUInt32(reader, "character_id");
        obj.MessageState = (MailState)GetUInt32(reader, "message_state");
        obj.SenderName = GetString(reader, "sender_name");
        obj.Title = GetString(reader, "message_title");
        obj.Body = GetString(reader, "message_body");
        obj.SendDate = GetUInt64(reader, "send_date");
        return obj;
    }
}
