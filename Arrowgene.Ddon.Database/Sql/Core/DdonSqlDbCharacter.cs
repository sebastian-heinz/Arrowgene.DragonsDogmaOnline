using System;
using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertCharacter = "INSERT INTO `ddo_character` (`account_id`, `first_name`, `last_name`, `created`) VALUES (@account_id, @first_name, @last_name, @created);";
        private const string SqlUpdateCharacter = "UPDATE `ddo_character` SET `account_id`=@account_id, `first_name`=@first_name, `last_name`=@last_name, `created`=@created WHERE `id` = @id;";
        private const string SqlSelectCharacter = "SELECT `id`, `account_id`, `first_name`, `last_name`, `created` FROM `ddo_character` WHERE `id` = @id;";
        private const string SqlSelectCharactersByAccountId = "SELECT `id`, `account_id`, `first_name`, `last_name`, `created` FROM `ddo_character` WHERE `account_id` = @account_id;";
        private const string SqlDeleteCharacter = "DELETE FROM `ddo_character` WHERE `id`=@id;";

        public Character CreateCharacter(Character character)
        {
            character.Created = DateTime.Now;
            int rowsAffected = ExecuteNonQuery(SqlInsertCharacter, command =>
            {
                AddParameter(command, "@account_id", character.AccountId);
                AddParameter(command, "@first_name", character.FirstName);
                AddParameter(command, "@last_name", character.LastName);
                AddParameter(command, "@created", character.Created);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return null;
            }

            character.Id = (int) autoIncrement;
            return character;
        }

        public bool UpdateCharacter(Character character)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateCharacter, command =>
            {
                AddParameter(command, "@account_id", character.AccountId);
                AddParameter(command, "@first_name", character.FirstName);
                AddParameter(command, "@last_name", character.LastName);
                AddParameter(command, "@created", character.Created);
                AddParameter(command, "@id", character.Id);
            });
            return rowsAffected > NoRowsAffected;
        }

        public Character SelectCharacter(int characterId)
        {
            Character character = null;
            ExecuteReader(SqlSelectCharacter,
                command => { AddParameter(command, "@id", characterId); }, reader =>
                {
                    if (reader.Read())
                    {
                        character = ReadCharacter(reader);
                    }
                });

            return character;
        }

        public List<Character> SelectCharactersByAccountId(int accountId)
        {
            List<Character> characters = new List<Character>();
            ExecuteReader(SqlSelectCharactersByAccountId,
                command => { AddParameter(command, "@account_id", accountId); }, reader =>
                {
                    while (reader.Read())
                    {
                        Character character = ReadCharacter(reader);
                        characters.Add(character);
                    }
                });

            return characters;
        }

        public bool DeleteCharacter(int characterId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteCharacter,
                command => { AddParameter(command, "@id", characterId); });
            return rowsAffected > NoRowsAffected;
        }

        private Character ReadCharacter(DbDataReader reader)
        {
            Character character = new Character();
            character.Id = GetInt32(reader, "id");
            character.AccountId = GetInt32(reader, "account_id");
            character.FirstName = GetString(reader, "first_name");
            character.LastName = GetString(reader, "last_name");
            character.Created = GetDateTime(reader, "created");
            return character;
        }
    }
}
