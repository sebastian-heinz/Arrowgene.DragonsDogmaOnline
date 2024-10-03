using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Security.Claims;
using System.Xml;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.BattleContent;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        /* ddon_quest_progress */
        protected static readonly string[] BitterBlackMazeCharacterMapFields = new string[]
        {
            "character_id", "bbm_character_id"
        };

        private readonly string SqlInsertBBMCharacterId = $"INSERT INTO \"ddon_bbm_character_map\" ({BuildQueryField(BitterBlackMazeCharacterMapFields)}) VALUES ({BuildQueryInsert(BitterBlackMazeCharacterMapFields)});";
        private readonly string SqlSelectBBMCharacterId = $"SELECT {BuildQueryField(BitterBlackMazeCharacterMapFields)} FROM ddon_bbm_character_map WHERE \"character_id\"=@character_id;";
        private readonly string SqlSelectBBMNormalCharacterId = $"SELECT {BuildQueryField(BitterBlackMazeCharacterMapFields)} FROM ddon_bbm_character_map WHERE \"bbm_character_id\"=@bbm_character_id;";

        public bool InsertBBMCharacterId(uint characterId, uint bbmCharacterId)
        {
            using TCon connection = OpenNewConnection();
            return InsertBBMCharacterId(connection, characterId, bbmCharacterId);
        }

        public bool InsertBBMCharacterId(TCon connection, uint characterId, uint bbmCharacterId)
        {
            return ExecuteNonQuery(connection, SqlInsertBBMCharacterId, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "bbm_character_id", bbmCharacterId);
            }) == 1;
        }

        public uint SelectBBMCharacterId(uint characterId, DbConnection? connectionIn = null)
        {
            uint bbmCharacterId = 0;
            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                ExecuteReader(connection, SqlSelectBBMCharacterId, command =>
                {
                    AddParameter(command, "character_id", characterId);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        bbmCharacterId = GetUInt32(reader, "bbm_character_id");
                    }
                });
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }
            return bbmCharacterId;
        }

        public uint SelectBBMNormalCharacterId(uint bbmCharacterId)
        {
            using TCon connection = OpenNewConnection();
            return SelectBBMNormalCharacterId(connection, bbmCharacterId);
        }

        public uint SelectBBMNormalCharacterId(TCon connection, uint bbmCharacterId)
        {
            uint characterId = 0;
            ExecuteInTransaction(connection =>
            {
                ExecuteReader(connection, SqlSelectBBMNormalCharacterId, command =>
                {
                    AddParameter(command, "bbm_character_id", bbmCharacterId);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        characterId = GetUInt32(reader, "character_id");
                    }
                });
            });

            return characterId;
        }
    }
}

