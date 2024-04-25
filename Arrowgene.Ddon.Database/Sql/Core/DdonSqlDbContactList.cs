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
           /* id */ "requester_character_id", "requested_character_id", "status", "type"
        };

        private static readonly string SqlInsertContact = $"INSERT INTO \"{TableName}\" ({BuildQueryField(ContactListFields)}) VALUES ({BuildQueryInsert(ContactListFields)});";
        private static readonly string SqlSelectFriends = $"SELECT \"id\", {BuildQueryField(ContactListFields)} FROM \"{TableName}\" WHERE \"requester_character_id\"=@character_id or \"requested_character_id\"=@character_id;";
        private static readonly string SqlSelectContactById = $"SELECT \"id\", {BuildQueryField(ContactListFields)} FROM \"{TableName}\" WHERE \"id\"=@id;";
        private static readonly string SqlSelectContactByName = $"SELECT \"id\", {BuildQueryField(ContactListFields)} FROM \"{TableName}\" WHERE \"normal_name\"=@normal_name;";
        private static readonly string SqlSelectContactByLoginToken = $"SELECT \"id\", {BuildQueryField(ContactListFields)} FROM \"{TableName}\" WHERE \"login_token\"=@login_token;";
        private static readonly string SqlUpdateContact = $"UPDATE \"{TableName}\" SET {BuildQueryUpdate(ContactListFields)} WHERE \"id\"=@id;";
        private static readonly string SqlDeleteContactById = $"DELETE FROM \"{TableName}\" WHERE \"id\"=@id;";
        private static readonly string SqlDeleteContact = $"DELETE FROM \"{TableName}\" WHERE \"requester_character_id\"=@requester_character_id and \"requested_character_id\"=@requested_character_id;";
        
        
        private static readonly string SqlUpdateContactByCharIds = $"UPDATE \"{TableName}\" SET \"status\"=@status, \"type\"=@type WHERE \"requester_character_id\"=@requester_character_id and \"requested_character_id\"=@requested_character_id;";


        public int UpsertContact(int requestingCharacterId, int requestedCharacterId, ContactListStatus status, ContactListType type)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateContactByCharIds, command =>
            {
                AddParameter(command, "@requester_character_id", requestingCharacterId);
                AddParameter(command, "@requested_character_id", requestedCharacterId);
                AddParameter(command, "@status", (byte) status);
                AddParameter(command, "@type", (byte) type);
            });
            if (rowsAffected > NoRowsAffected)
            {
                return rowsAffected * -1;
            }

            rowsAffected = ExecuteNonQuery(SqlInsertContact, command =>
            {
                AddParameter(command, "@requester_character_id", requestingCharacterId);
                AddParameter(command, "@requested_character_id", requestedCharacterId);
                AddParameter(command, "@status", (byte) status);
                AddParameter(command, "@type", (byte) type);
            }, out long autoIncrement);

            if (rowsAffected > NoRowsAffected)
            {
                return (int)autoIncrement;
            }

            return 0;
        }
        
        public int DeleteContact(int requestingCharacterId, int requestedCharacterId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteContact, command =>
            {
                AddParameter(command, "@requester_character_id", requestingCharacterId);
                AddParameter(command, "@requested_character_id", requestedCharacterId);
            });
            return rowsAffected;
        }
        
        public List<ContactListEntity> SelectFriends(int characterId)
        {
            List<ContactListEntity> entities = new List<ContactListEntity>();
            ExecuteReader(SqlSelectFriends,
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

        private ContactListEntity EntityReader(TReader reader)
        {
            ContactListEntity e = new ContactListEntity();
            e.Id = GetInt32(reader, "id");
            e.RequesterCharacterId = GetInt32(reader, "requester_character_id");
            e.RequestedCharacterId = GetInt32(reader, "requested_character_id");
            e.Status = (ContactListStatus) GetByte(reader, "status");
            e.Type = (ContactListType) GetInt32(reader, "type");
            return e;
        }
    }
}


// CREATE TABLE "ddon_contact_list" (
//     "id"	INTEGER UNIQUE,
//     "requester_character_id"	INTEGER NOT NULL,
//     "requested_character_id"	INTEGER NOT NULL,
//     "status"	INTEGER NOT NULL,
//     "type"	INTEGER NOT NULL,
//     FOREIGN KEY("requester_character_id") REFERENCES "ddon_character"("character_id"),
//     FOREIGN KEY("requested_character_id") REFERENCES "ddon_character"("character_id"),
//     PRIMARY KEY("id" AUTOINCREMENT),
//     UNIQUE("requester_character_id","requested_character_id")
// );
