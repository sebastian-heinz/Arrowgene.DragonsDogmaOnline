#nullable enable
using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string SqlDeleteCommunicationShortcut =
        "DELETE FROM \"ddon_communication_shortcut\" WHERE \"character_id\"=@character_id AND \"page_no\"=@page_no AND \"button_no\"=@button_no";

    protected static readonly string[] CommunicationShortcutFields = new[]
    {
        "character_id", "page_no", "button_no", "type", "category", "id"
    };

    private static readonly string SqlUpdateCommunicationShortcut =
        $"UPDATE \"ddon_communication_shortcut\" SET {BuildQueryUpdate(CommunicationShortcutFields)} WHERE \"character_id\"=@old_character_id AND \"page_no\"=@old_page_no AND \"button_no\"=@old_button_no";

    private static readonly string SqlSelectCommunicationShortcuts =
        $"SELECT {BuildQueryField(CommunicationShortcutFields)} FROM \"ddon_communication_shortcut\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlInsertCommunicationShortcut =
        $"INSERT INTO \"ddon_communication_shortcut\" ({BuildQueryField(CommunicationShortcutFields)}) VALUES ({BuildQueryInsert(CommunicationShortcutFields)});";

    private readonly string SqlInsertIfNotExistsCommunicationShortcut =
        $"INSERT INTO \"ddon_communication_shortcut\" ({BuildQueryField(CommunicationShortcutFields)}) SELECT {BuildQueryInsert(CommunicationShortcutFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_communication_shortcut\" WHERE \"character_id\"=@character_id AND \"page_no\"=@page_no AND \"button_no\"=@button_no);";

    public bool InsertIfNotExistsCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertIfNotExistsCommunicationShortcut(connection, characterId, communicationShortcut);
    }

    public bool InsertIfNotExistsCommunicationShortcut(DbConnection connection, uint characterId, CDataCommunicationShortCut communicationShortcut)
    {
        return ExecuteNonQuery(connection, SqlInsertIfNotExistsCommunicationShortcut, command => { AddParameter(command, characterId, communicationShortcut); }) == 1;
    }

    public override bool InsertCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertCommunicationShortcut(connection, characterId, communicationShortcut);
    }

    public bool InsertCommunicationShortcut(DbConnection connection, uint characterId, CDataCommunicationShortCut communicationShortcut)
    {
        return ExecuteNonQuery(connection, SqlInsertCommunicationShortcut, command => { AddParameter(command, characterId, communicationShortcut); }) == 1;
    }

    public override bool ReplaceCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            if (!InsertIfNotExistsCommunicationShortcut(connection, characterId, communicationShortcut))
                return UpdateCommunicationShortcut((DbConnection)connection, characterId, communicationShortcut.PageNo, communicationShortcut.ButtonNo, communicationShortcut);

            return true;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public override bool UpdateCommunicationShortcut(uint characterId, uint oldPageNo, uint oldButtonNo, CDataCommunicationShortCut updatedCommunicationShortcut)
    {
        using DbConnection connection = OpenNewConnection();
        return UpdateCommunicationShortcut(connection, characterId, oldPageNo, oldButtonNo, updatedCommunicationShortcut);
    }

    public bool UpdateCommunicationShortcut(DbConnection connection, uint characterId, uint oldPageNo, uint oldButtonNo,
        CDataCommunicationShortCut updatedCommunicationShortcut)
    {
        return ExecuteNonQuery(connection, SqlUpdateCommunicationShortcut, command =>
        {
            AddParameter(command, characterId, updatedCommunicationShortcut);
            AddParameter(command, "@old_character_id", characterId);
            AddParameter(command, "@old_page_no", oldPageNo);
            AddParameter(command, "@old_button_no", oldButtonNo);
        }) == 1;
    }

    public override bool DeleteCommunicationShortcut(uint characterId, uint pageNo, uint buttonNo)
    {
        return ExecuteNonQuery(SqlDeleteCommunicationShortcut, command =>
        {
            AddParameter(command, "@character_id", characterId);
            AddParameter(command, "@old_page_no", pageNo);
            AddParameter(command, "@old_button_no", buttonNo);
        }) == 1;
    }

    private CDataCommunicationShortCut ReadCommunicationShortCut(DbDataReader reader)
    {
        CDataCommunicationShortCut shortcut = new();
        shortcut.PageNo = GetUInt32(reader, "page_no");
        shortcut.ButtonNo = GetUInt32(reader, "button_no");
        shortcut.Type = GetByte(reader, "type");
        shortcut.Category = GetByte(reader, "category");
        shortcut.Id = GetUInt32(reader, "id");
        return shortcut;
    }

    private void AddParameter(DbCommand command, uint characterId, CDataCommunicationShortCut shortcut)
    {
        AddParameter(command, "character_id", characterId);
        AddParameter(command, "page_no", shortcut.PageNo);
        AddParameter(command, "button_no", shortcut.ButtonNo);
        AddParameter(command, "type", shortcut.Type);
        AddParameter(command, "category", shortcut.Category);
        AddParameter(command, "id", shortcut.Id);
    }
}
