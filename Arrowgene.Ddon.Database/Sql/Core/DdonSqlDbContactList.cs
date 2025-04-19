using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private static readonly string ContactListTableName = "ddon_contact_list";

    private static readonly string[] ContactListFields = new[]
    {
        /* id */ "requester_character_id", "requested_character_id", "status", "type", "requester_favorite", "requested_favorite"
    };

    private static readonly string SqlInsertContact =
        $"INSERT INTO \"{ContactListTableName}\" ({BuildQueryField(ContactListFields)}) VALUES ({BuildQueryInsert(ContactListFields)});";

    private static readonly string SqlSelectContactById = $"SELECT \"id\", {BuildQueryField(ContactListFields)} FROM \"{ContactListTableName}\" WHERE \"id\"=@id;";

    private static readonly string SqlSelectContactsByCharacterId =
        $"SELECT \"id\", {BuildQueryField(ContactListFields)} FROM \"{ContactListTableName}\" WHERE \"requester_character_id\"=@character_id or \"requested_character_id\"=@character_id;";

    private static readonly string SqlSelectContactsByCharacterIds = $"SELECT \"id\", {BuildQueryField(ContactListFields)} FROM \"{ContactListTableName}\" " +
                                                                     $"WHERE (\"requester_character_id\"=@character_id_1 and \"requested_character_id\"=@character_id_2) or " +
                                                                     $"(\"requester_character_id\"=@character_id_2 and \"requested_character_id\"=@character_id_1);";

    private static readonly string SqlDeleteContact =
        $"DELETE FROM \"{ContactListTableName}\" WHERE \"requester_character_id\"=@requester_character_id and \"requested_character_id\"=@requested_character_id;";

    private static readonly string SqlDeleteContactById = $"DELETE FROM \"{ContactListTableName}\" WHERE \"id\"=@id;";


    private static readonly string SqlUpdateContactByCharIds =
        $"UPDATE \"{ContactListTableName}\" SET \"status\"=@status, \"type\"=@type, \"requester_favorite\"=@requester_favorite, \"requested_favorite\"=@requested_favorite WHERE \"requester_character_id\"=@requester_character_id and \"requested_character_id\"=@requested_character_id;";

    private static readonly string SqlSelectFullContactsByCharacterId = @"
            WITH ContactInfo AS (
                SELECT 
                    cl.*,
                    CASE 
                        WHEN cl.requester_character_id = @character_id THEN cl.requested_character_id 
                        ELSE cl.requester_character_id 
                    END AS other_id
                FROM ddon_contact_list cl
            )
            SELECT 
                ci.*,
	            c.first_name, 
	            c.last_name, 
	            j.job, 
	            j.lv, 
	            mp.comment,
                cp.name AS clan_name
            FROM ContactInfo ci
            INNER JOIN ddon_character c ON c.character_id = other_id
            INNER JOIN ddon_character_common cc ON cc.character_common_id = c.character_common_id
            INNER JOIN ddon_character_job_data j ON j.character_common_id = cc.character_common_id AND j.job = cc.job
            INNER JOIN ddon_character_matching_profile mp ON mp.character_id = other_id
            LEFT JOIN ddon_clan_membership cm ON cm.character_id = ci.other_id
            LEFT JOIN ddon_clan_param cp ON cp.clan_id = cm.clan_id
            WHERE ci.requested_character_id = @character_id OR ci.requester_character_id = @character_id;";

    public override int InsertContact(uint requestingCharacterId, uint requestedCharacterId, ContactListStatus status, ContactListType type, bool requesterFavorite,
        bool requestedFavorite)
    {
        int rowsAffected = ExecuteNonQuery(SqlInsertContact, command =>
        {
            AddParameter(command, "@requester_character_id", requestingCharacterId);
            AddParameter(command, "@requested_character_id", requestedCharacterId);
            AddParameter(command, "@status", (byte)status);
            AddParameter(command, "@type", (byte)type);
            AddParameter(command, "@requester_favorite", requesterFavorite);
            AddParameter(command, "@requested_favorite", requestedFavorite);
        }, out long autoIncrement);

        if (rowsAffected > NoRowsAffected) return (int)autoIncrement;

        return 0;
    }

    public override int UpdateContact(uint requestingCharacterId, uint requestedCharacterId, ContactListStatus status, ContactListType type, bool requesterFavorite,
        bool requestedFavorite)
    {
        int rowsAffected = ExecuteNonQuery(SqlUpdateContactByCharIds, command =>
        {
            AddParameter(command, "@requester_character_id", requestingCharacterId);
            AddParameter(command, "@requested_character_id", requestedCharacterId);
            AddParameter(command, "@status", (byte)status);
            AddParameter(command, "@type", (byte)type);
            AddParameter(command, "@requester_favorite", requesterFavorite);
            AddParameter(command, "@requested_favorite", requestedFavorite);
        });

        return rowsAffected;
    }

    public override int DeleteContact(uint requestingCharacterId, uint requestedCharacterId)
    {
        int rowsAffected = ExecuteNonQuery(SqlDeleteContact, command =>
        {
            AddParameter(command, "@requester_character_id", requestingCharacterId);
            AddParameter(command, "@requested_character_id", requestedCharacterId);
        });
        return rowsAffected;
    }

    public override int DeleteContactById(uint id)
    {
        int rowsAffected = ExecuteNonQuery(SqlDeleteContactById, command => { AddParameter(command, "@id", id); });
        return rowsAffected;
    }

    public override List<ContactListEntity> SelectContactsByCharacterId(uint characterId)
    {
        List<ContactListEntity> entities = new();
        ExecuteReader(SqlSelectContactsByCharacterId,
            command => { AddParameter(command, "@character_id", characterId); }, reader =>
            {
                while (reader.Read())
                {
                    ContactListEntity e = ReadContactListEntity(reader);
                    entities.Add(e);
                }
            });

        return entities;
    }

    public override ContactListEntity SelectContactsByCharacterId(uint characterId1, uint characterId2)
    {
        ContactListEntity entity = null;
        ExecuteReader(SqlSelectContactsByCharacterIds,
            command =>
            {
                AddParameter(command, "@character_id_1", characterId1);
                AddParameter(command, "@character_id_2", characterId2);
            }, reader =>
            {
                if (reader.Read()) entity = ReadContactListEntity(reader);
            });

        return entity;
    }

    public override ContactListEntity SelectContactListById(uint id)
    {
        ContactListEntity entity = null;
        ExecuteReader(SqlSelectContactById,
            command => { AddParameter(command, "@id", id); }, reader =>
            {
                if (reader.Read()) entity = ReadContactListEntity(reader);
            });

        return entity;
    }

    public override List<(ContactListEntity, CDataCharacterListElement)> SelectFullContactListByCharacterId(uint characterId, DbConnection? connectionIn = null)
    {
        List<(ContactListEntity, CDataCharacterListElement)> list = new();

        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            ExecuteReader(
                connection,
                SqlSelectFullContactsByCharacterId,
                command => { AddParameter(command, "@character_id", characterId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        ContactListEntity contactListEntity = ReadContactListEntity(reader);
                        CDataCharacterListElement characterListElement = new();

                        characterListElement.CommunityCharacterBaseInfo.CharacterId = GetUInt32(reader, "other_id");
                        characterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = GetString(reader, "first_name");
                        characterListElement.CommunityCharacterBaseInfo.CharacterName.LastName = GetString(reader, "last_name");
                        characterListElement.CommunityCharacterBaseInfo.ClanName = GetStringNullable(reader, "clan_name") ?? string.Empty;
                        characterListElement.CurrentJobBaseInfo.Job = (JobId)GetByte(reader, "job");
                        characterListElement.CurrentJobBaseInfo.Level = GetByte(reader, "lv");
                        characterListElement.EntryJobBaseInfo = characterListElement.CurrentJobBaseInfo;
                        characterListElement.MatchingProfile = GetString(reader, "comment");

                        list.Add((contactListEntity, characterListElement));
                    }
                });
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }

        return list;
    }

    private ContactListEntity ReadContactListEntity(DbDataReader reader)
    {
        ContactListEntity e = new();
        e.Id = GetUInt32(reader, "id");
        e.RequesterCharacterId = GetUInt32(reader, "requester_character_id");
        e.RequestedCharacterId = GetUInt32(reader, "requested_character_id");
        e.Status = (ContactListStatus)GetByte(reader, "status");
        e.Type = (ContactListType)GetByte(reader, "type");
        e.RequesterFavorite = GetBoolean(reader, "requester_favorite");
        e.RequestedFavorite = GetBoolean(reader, "requested_favorite");
        return e;
    }
}
