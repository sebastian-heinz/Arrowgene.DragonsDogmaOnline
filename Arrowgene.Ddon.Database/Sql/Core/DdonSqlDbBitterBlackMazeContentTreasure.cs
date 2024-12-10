using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Xml;
using Arrowgene.Ddon.Database.Model;
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
        protected static readonly string[] BitterBlackMazeContentTreasure = new string[]
        {
            "character_id", "content_id", "amount"
        };

        private readonly string SqlSelectBBMContentTreasure = $"SELECT {BuildQueryField(BitterBlackMazeContentTreasure)} FROM \"ddon_bbm_content_treasure\" WHERE \"character_id\"=@character_id;";
        private readonly string SqlInsertBBMContentTreasure = $"INSERT INTO \"ddon_bbm_content_treasure\" ({BuildQueryField(BitterBlackMazeContentTreasure)}) VALUES ({BuildQueryInsert(BitterBlackMazeContentTreasure)});";
        private readonly string SqlUpdateBBMContentTreasure = $"UPDATE \"ddon_bbm_content_treasure\" SET {BuildQueryUpdate(BitterBlackMazeContentTreasure)} WHERE \"character_id\"=@character_id and \"content_id\"=@content_id;";
        private readonly string SqlDeleteBBMContentTreasure = $"DELETE FROM \"ddon_bbm_content_treasure\" WHERE \"character_id\"=@character_id;";

        public bool InsertBBMContentTreasure(uint characterId, BitterblackMazeTreasure treasure, DbConnection? connectionIn = null)
        {
            return InsertBBMContentTreasure(characterId, treasure.ContentId, treasure.Amount, connectionIn);
        }

        public bool InsertBBMContentTreasure(uint characterId, uint contentId, uint amount, DbConnection? connectionIn = null)
        {
            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
            try
            {
                return ExecuteNonQuery(connection, SqlInsertBBMContentTreasure, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "content_id", contentId);
                    AddParameter(command, "amount", amount);
                }) == 1;
            }
            finally
            {
                if (!isTransaction) connection.Dispose();
            }
        }

        public bool UpdateBBMContentTreasure(uint characterId, BitterblackMazeTreasure treasure)
        {
            return UpdateBBMContentTreasure(characterId, treasure.ContentId, treasure.Amount);
        }

        public bool UpdateBBMContentTreasure(uint characterId, uint contentId, uint amount)
        {
            using TCon connection = OpenNewConnection();
            return UpdateBBMContentTreasure(connection, characterId, contentId, amount);
        }

        public bool UpdateBBMContentTreasure(TCon connection, uint characterId, uint contentId, uint amount)
        {
            return ExecuteNonQuery(connection, SqlUpdateBBMContentTreasure, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "content_id", contentId);
                AddParameter(command, "amount", amount);
            }) == 1; ;
        }

        public bool RemoveBBMContentTreasure(uint characterId)
        {
            using TCon connection = OpenNewConnection();
            return RemoveBBMContentTreasure(connection, characterId);
        }

        public bool RemoveBBMContentTreasure(TCon connection, uint characterId)
        {
            return ExecuteNonQuery(connection, SqlDeleteBBMContentTreasure, command =>
            {
                AddParameter(command, "character_id", characterId);
            }) == 1;
        }

        public List<BitterblackMazeTreasure> SelectBBMContentTreasure(uint characterId, DbConnection? connectionIn = null)
        {
            List<BitterblackMazeTreasure> results = new List<BitterblackMazeTreasure>();
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteReader(connection, SqlSelectBBMContentTreasure, command =>
                {
                    AddParameter(command, "character_id", characterId);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        var result = new BitterblackMazeTreasure();
                        result.ContentId = GetUInt32(reader, "content_id");
                        result.Amount = GetUInt32(reader, "amount");
                        results.Add(result);
                    }
                });
            });
            return results;
        }
    }
}
