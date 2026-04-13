#nullable enable
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string Table = "\"ddon_communication_shortcut\"";
    
    private const string SqlDeleteCommunicationShortcut =
        "DELETE FROM \"ddon_communication_shortcut\" WHERE \"character_id\"=@character_id AND \"page_no\"=@page_no AND \"button_no\"=@button_no";

    // All columns (PKs first)
    protected static readonly string[] CommunicationShortcutFields =
    [
        "character_id", "page_no", "button_no", "type", "category", "id"
    ];
    
    // Build "col = EXCLUDED.col" for non-PK cols:
    private static string UpdateList = string.Join(", ",
        CommunicationShortcutFields
            .Skip(3)     // skip character_id, page_no, button_no
            .Select(c => $"\"{c}\" = EXCLUDED.\"{c}\""));

    private static readonly string SqlUpdateCommunicationShortcut =
        $"UPDATE \"ddon_communication_shortcut\" SET {BuildQueryUpdate(CommunicationShortcutFields)} WHERE \"character_id\"=@old_character_id AND \"page_no\"=@old_page_no AND \"button_no\"=@old_button_no";

    private static readonly string SqlSelectCommunicationShortcuts =
        $"SELECT {BuildQueryField(CommunicationShortcutFields)} FROM \"ddon_communication_shortcut\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlInsertCommunicationShortcut =
        $"INSERT INTO \"ddon_communication_shortcut\" ({BuildQueryField(CommunicationShortcutFields)}) VALUES ({BuildQueryInsert(CommunicationShortcutFields)});";

    private static readonly string SqlUpsertCommunicationShortcut =
        $@"INSERT INTO {Table} ({BuildQueryField(CommunicationShortcutFields)})
           VALUES ({BuildQueryInsert(CommunicationShortcutFields)})
           ON CONFLICT (""character_id"", ""page_no"", ""button_no"")
           DO UPDATE SET {UpdateList};";

    private static readonly string SqlUpsertCommunicationMessageSet =
        """
            INSERT INTO ddon_communication_message_set (character_id, set_no, set_name)
            VALUES (@character_id, @set_no, @set_name)
            ON CONFLICT (character_id, set_no)
            DO UPDATE SET set_name = EXCLUDED.set_name;
        """;

    private static readonly string SqlUpsertCommunicationMessage =
        """
            INSERT INTO ddon_communication_message ("character_id", "set_no", "message_no", "message", "emotion", "emotochat")
            VALUES (@character_id, @set_no, @message_no, @message, @emotion, @emotochat)
            ON CONFLICT (character_id, set_no, message_no)
            DO UPDATE SET message = EXCLUDED.message, emotion = EXCLUDED.emotion, emotochat = EXCLUDED.emotochat;
        """;

    private static readonly string SqlSelectCommunicationMessages =
        """
            SELECT * 
            FROM ddon_communication_message
            NATURAL JOIN ddon_communication_message_set
            WHERE character_id = @character_id;
        """;

    public override bool InsertCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn,
            connection => { return ExecuteNonQuery(connection, SqlInsertCommunicationShortcut, command => { AddParameter(command, characterId, communicationShortcut); }) == 1; });
    }

    public override bool ReplaceCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn,
            connection =>
            {
                return ExecuteNonQuery(connection, SqlUpsertCommunicationShortcut, command =>
                {
                    AddParameter(command, characterId, communicationShortcut);
                }) == 1;
            });
    }


    public override bool UpdateCommunicationShortcut(uint characterId, uint oldPageNo, uint oldButtonNo,
        CDataCommunicationShortCut updatedCommunicationShortcut, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpdateCommunicationShortcut, command =>
            {
                AddParameter(command, characterId, updatedCommunicationShortcut);
                AddParameter(command, "@old_character_id", characterId);
                AddParameter(command, "@old_page_no", oldPageNo);
                AddParameter(command, "@old_button_no", oldButtonNo);
            }) == 1;
        });
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

    public override int UpsertCommunicationSet(uint characterId, List<CDataCharacterMsgSet> messages, DbConnection? connectionIn = null) 
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            int rowsAffected = 0;
            foreach(var messageSet in messages)
            {
                rowsAffected += ExecuteNonQuery(connection, SqlUpsertCommunicationMessageSet, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "set_no", messageSet.SetNo);
                    AddParameter(command, "set_name", messageSet.MsgSetName);
                });

                foreach (var message in messageSet.CharacterMessageList)
                {
                    rowsAffected += ExecuteNonQuery(connection, SqlUpsertCommunicationMessage, command =>
                    {
                        AddParameter(command, "character_id", characterId);
                        AddParameter(command, "set_no", messageSet.SetNo);
                        AddParameter(command, "message_no", message.MessageNo);
                        AddParameter(command, "message", message.Message);
                        AddParameter(command, "emotion", message.Emotion);
                        AddParameter(command, "emotochat", message.EmotoChat);
                    });
                }
            }

            return rowsAffected;
        });
    }

    public override List<CDataCharacterMsgSet> SelectCommunicationSet(uint characterId, DbConnection? connectionIn = null)
    {
        Dictionary<uint, CDataCharacterMsgSet> result = new();

        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectCommunicationMessages,
                command => { AddParameter(command, "@character_id", characterId); },
                reader => { 
                    while (reader.Read())
                    {
                        var setNo = GetUInt32(reader, "set_no");
                        var setName = GetString(reader, "set_name");
                        var messageNo = GetUInt32(reader, "message_no");
                        var message = GetString(reader, "message");
                        var emotion = GetUInt32(reader, "emotion");
                        var emotochat = GetBoolean(reader, "emotochat");

                        if (!result.ContainsKey(setNo))
                        {
                            result.Add(setNo, new()
                            {
                                SetNo = setNo,
                                MsgSetName = setName
                            });
                        }

                        result[setNo].CharacterMessageList.Add(new()
                        {
                            MessageNo = messageNo,
                            Message = message,
                            Emotion = emotion,
                            EmotoChat = emotochat
                        });
                    };
                }
            );
        });

        return result.OrderBy(x => x.Key).Select(x => x.Value).ToList();
    }
}
