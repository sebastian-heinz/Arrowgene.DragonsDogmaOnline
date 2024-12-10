using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        /* ddon_epitaph_road_unlocks */
        protected static readonly string[] EpitaphRoadUnlocksFields = new string[]
        {
            "character_id", "epitaph_id"
        };

        private readonly string SqlSelectEpitaphUnlocks = $"SELECT {BuildQueryField(EpitaphRoadUnlocksFields)} FROM \"ddon_epitaph_road_unlocks\" WHERE \"character_id\"=@character_id;";
        private readonly string SqlInsertEpitaphUnlocks = $"INSERT INTO \"ddon_epitaph_road_unlocks\" ({BuildQueryField(EpitaphRoadUnlocksFields)}) VALUES ({BuildQueryInsert(EpitaphRoadUnlocksFields)});";

        public bool InsertEpitaphRoadUnlock(uint characterId, uint epitaphId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlInsertEpitaphUnlocks, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "epitaph_id", epitaphId);
                }) == 1;
            });
        }

        public HashSet<uint> GetEpitaphRoadUnlocks(uint characterId, DbConnection? connectionIn = null)
        {
            var results = new HashSet<uint>();
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteReader(connection, SqlSelectEpitaphUnlocks, command =>
                {
                    AddParameter(command, "character_id", characterId);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        results.Add(GetUInt32(reader, "epitaph_id"));
                    }
                });
            });
            return results;
        }
    }
}
