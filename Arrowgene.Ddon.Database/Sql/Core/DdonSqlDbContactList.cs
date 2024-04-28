using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        
        private static readonly string TableName = "ddon_contact_list";
        
        private static readonly string[] ContactListFields = new string[]
        { 
           /* id */ "requester_character_id", "requested_character_id", "status", "type", "requester_favorite", "requested_favorite"
        };

        private static readonly string SqlInsertContact = $"INSERT INTO \"{TableName}\" ({BuildQueryField(ContactListFields)}) VALUES ({BuildQueryInsert(ContactListFields)});";
        private static readonly string SqlSelectContactById = $"SELECT \"id\", {BuildQueryField(ContactListFields)} FROM \"{TableName}\" WHERE \"id\"=@id;";
        private static readonly string SqlSelectContactsByCharacterId = $"SELECT \"id\", {BuildQueryField(ContactListFields)} FROM \"{TableName}\" WHERE \"requester_character_id\"=@character_id or \"requested_character_id\"=@character_id;";
        
        private static readonly string SqlSelectContactsByCharacterIds = $"SELECT \"id\", {BuildQueryField(ContactListFields)} FROM \"{TableName}\" " +
                                                               $"WHERE (\"requester_character_id\"=@character_id_1 and \"requested_character_id\"=@character_id_2) or " +
                                                               $"(\"requester_character_id\"=@character_id_2 and \"requested_character_id\"=@character_id_1);";
        
        private static readonly string SqlDeleteContact = $"DELETE FROM \"{TableName}\" WHERE \"requester_character_id\"=@requester_character_id and \"requested_character_id\"=@requested_character_id;";
        private static readonly string SqlDeleteContactById = $"DELETE FROM \"{TableName}\" WHERE \"id\"=@id;";
        
        
        private static readonly string SqlUpdateContactByCharIds = $"UPDATE \"{TableName}\" SET \"status\"=@status, \"type\"=@type, \"requester_favorite\"=@requester_favorite, \"requested_favorite\"=@requested_favorite WHERE \"requester_character_id\"=@requester_character_id and \"requested_character_id\"=@requested_character_id;";


        public int InsertContact(uint requestingCharacterId, uint requestedCharacterId, ContactListStatus status, ContactListType type, bool requesterFavorite, bool requestedFavorite)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertContact, command =>
            {
                AddParameter(command, "@requester_character_id", requestingCharacterId);
                AddParameter(command, "@requested_character_id", requestedCharacterId);
                AddParameter(command, "@status", (byte) status);
                AddParameter(command, "@type", (byte) type);
                AddParameter(command, "@requester_favorite", requesterFavorite);
                AddParameter(command, "@requested_favorite", requestedFavorite);
            }, out long autoIncrement);

            if (rowsAffected > NoRowsAffected)
            {
                return (int)autoIncrement;
            }

            return 0;
        }
        
        public int UpdateContact(uint requestingCharacterId, uint requestedCharacterId, ContactListStatus status, ContactListType type, bool requesterFavorite, bool requestedFavorite)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateContactByCharIds, command =>
            {
                AddParameter(command, "@requester_character_id", requestingCharacterId);
                AddParameter(command, "@requested_character_id", requestedCharacterId);
                AddParameter(command, "@status", (byte) status);
                AddParameter(command, "@type", (byte) type);
                AddParameter(command, "@requester_favorite", requesterFavorite);
                AddParameter(command, "@requested_favorite", requestedFavorite);
            });

            return rowsAffected;
        }
        
        public int DeleteContact(uint requestingCharacterId, uint requestedCharacterId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteContact, command =>
            {
                AddParameter(command, "@requester_character_id", requestingCharacterId);
                AddParameter(command, "@requested_character_id", requestedCharacterId);
            });
            return rowsAffected;
        }
        
        public int DeleteContactById(uint id)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteContactById, command =>
            {
                AddParameter(command, "@id", id);
            });
            return rowsAffected;
        }
        
        public List<ContactListEntity> SelectContactsByCharacterId(uint characterId)
        {
            List<ContactListEntity> entities = new List<ContactListEntity>();
            ExecuteReader(SqlSelectContactsByCharacterId,
                command => { AddParameter(command, "@character_id", characterId); }, reader =>
                {
                    while (reader.Read())
                    {
                        ContactListEntity e = EntityReader(reader);
                        entities.Add(e);
                    }
                });

            return entities;
        }
        
        public ContactListEntity SelectContactsByCharacterId(uint characterId1, uint characterId2)
        {
            ContactListEntity entity = null;
            ExecuteReader(SqlSelectContactsByCharacterIds,
                command =>
                {
                    AddParameter(command, "@character_id_1", characterId1);
                    AddParameter(command, "@character_id_2", characterId2);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        entity = EntityReader(reader);
                    }
                });

            return entity;
        }
        
        public ContactListEntity SelectContactListById(uint id)
        {
            ContactListEntity entity = null;
            ExecuteReader(SqlSelectContactById,
                command =>
                {
                    AddParameter(command, "@id", id);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        entity = EntityReader(reader);
                    }
                });

            return entity;
        }

        private ContactListEntity EntityReader(TReader reader)
        {
            ContactListEntity e = new ContactListEntity();
            e.Id = GetUInt32(reader, "id");
            e.RequesterCharacterId = GetUInt32(reader, "requester_character_id");
            e.RequestedCharacterId = GetUInt32(reader, "requested_character_id");
            e.Status = (ContactListStatus) GetByte(reader, "status");
            e.Type = (ContactListType) GetByte(reader, "type");
            e.RequesterFavorite = GetBoolean(reader, "requester_favorite");
            e.RequestedFavorite = GetBoolean(reader, "requested_favorite");
            return e;
        }
    }
}
