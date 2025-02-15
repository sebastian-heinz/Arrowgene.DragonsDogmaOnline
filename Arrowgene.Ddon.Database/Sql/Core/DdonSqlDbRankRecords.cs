using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;


namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        private static readonly string RankRecordTableName = "ddon_rank_record";


        private static readonly string[] RankRecordFields = new string[]
        { 
           /* record_id */ "character_id", "quest_id", "score", "date"
        };

        private static readonly string SqlInsertRankRecord = $"INSERT INTO \"{RankRecordTableName}\" ({BuildQueryField(RankRecordFields)}) VALUES ({BuildQueryInsert(RankRecordFields)});";
        private static readonly string SqlDeleteAllRankRecords = $"TRUNCATE TABLE {RankRecordTableName}";

        private static readonly string SqlSelectUsedQuests = $"SELECT DISTINCT \"quest_id\" FROM \"{RankRecordTableName}\"";

        private static readonly string SqlSelectRankRecords =
            @"WITH BestRank AS (
	            SELECT 
		            rr.*,
		            ROW_NUMBER() OVER (PARTITION BY character_id ORDER BY score DESC) AS rn
	            FROM ddon_rank_record rr
	            WHERE quest_id = @quest_id
            ), RankedRank AS (
	            SELECT 
		            br.*,
		            RANK() OVER (ORDER BY score DESC) sparse_rank
	            FROM BestRank br
	            WHERE br.rn = 1
            )
            SELECT 
	            rar.*,
	            ch.first_name,
	            ch.last_name,
	            cp.name AS clan_name
            FROM RankedRank rar
            INNER JOIN ddon_character ch ON rar.character_id = ch.character_id
            LEFT JOIN ddon_clan_membership cm ON cm.character_id = rar.character_id
            LEFT JOIN ddon_clan_param cp ON cp.clan_id = cm.clan_id
            WHERE rar.character_id = @character_id;";

        private static readonly string SqlSelectTopRankRecords =
            @"WITH BestRank AS (
                SELECT 
                    rr.*,
                    ROW_NUMBER() OVER (PARTITION BY character_id ORDER BY score DESC) AS rn
                FROM ddon_rank_record rr
                WHERE quest_id = @quest_id
            )
            SELECT 
                br.*,
                ch.first_name,
                ch.last_name,
                cp.name AS clan_name,
                RANK() OVER (ORDER BY score) sparse_rank
            FROM BestRank br
            INNER JOIN ddon_character ch ON br.character_id = ch.character_id
            LEFT JOIN ddon_clan_membership cm ON cm.character_id = br.character_id
            LEFT JOIN ddon_clan_param cp ON cp.clan_id = cm.clan_id
            WHERE br.rn = 1
            LIMIT @limit;";

        public bool InsertRankRecord(uint characterId, uint questId, long score, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(
                    connection,
                    SqlInsertRankRecord,
                    command =>
                    {
                        AddParameter(command, "@character_id", characterId);
                        AddParameter(command, "@quest_id", questId);
                        AddParameter(command, "@score", score);
                        AddParameter(command, "@date", DateTime.Now);
                    },
                    out long recordId
                ) > 1;
            });
        }

        public List<uint> SelectUsedRankingBoardQuests(DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                var list = new List<uint>();
                ExecuteReader(connection,
                    SqlSelectUsedQuests,
                    command => {},
                    reader =>
                    {
                        while (reader.Read())
                        {
                            list.Add(GetUInt32(reader, "quest_id"));
                        }
                    });

                return list;
            });
        }

        public List<CDataRankingData> SelectRankingDataByCharacterId(uint characterId, uint questId, uint limit = 1000, DbConnection? connectionIn = null)
        {
            var list = ExecuteQuerySafe(connectionIn, connection =>
            {
                var list = new List<CDataRankingData>();
                ExecuteReader(connection,
                    SqlSelectRankRecords,
                    command => {
                        AddParameter(command, "@character_id", characterId);
                        AddParameter(command, "@quest_id", questId);
                        AddParameter(command, "@limit", limit);
                    },
                    reader =>
                    {
                        while (reader.Read())
                        {
                            CDataRankingData rankingData = new();
                            rankingData.Serial = GetUInt32(reader, "record_id");
                            rankingData.Score = GetUInt32(reader, "score");
                            rankingData.Rank = GetUInt32(reader, "sparse_rank");
                            rankingData.CommunityCharacterBaseInfo.CharacterId = GetUInt32(reader, "character_id");
                            rankingData.CommunityCharacterBaseInfo.CharacterName.FirstName = GetString(reader, "first_name");
                            rankingData.CommunityCharacterBaseInfo.CharacterName.LastName = GetString(reader, "last_name");
                            rankingData.CommunityCharacterBaseInfo.ClanName = GetStringNullable(reader, "clan_name") ?? "";

                            list.Add(rankingData);
                        }
                    });

                return list;
            });

            return list;
        }

        public List<CDataRankingData> SelectRankingData(uint questId, uint limit = 1000, DbConnection? connectionIn = null)
        {
            var list = ExecuteQuerySafe(connectionIn, connection =>
            {
                var list = new List<CDataRankingData>();
                ExecuteReader(connection,
                    SqlSelectTopRankRecords,
                    command => {
                        AddParameter(command, "@quest_id", questId);
                        AddParameter(command, "@limit", limit);
                    },
                    reader =>
                    {
                        while (reader.Read())
                        {
                            CDataRankingData rankingData = new();
                            rankingData.Serial = GetUInt32(reader, "record_id");
                            rankingData.Score = GetUInt32(reader, "score");
                            rankingData.Rank = GetUInt32(reader, "sparse_rank");
                            rankingData.CommunityCharacterBaseInfo.CharacterId = GetUInt32(reader, "character_id");
                            rankingData.CommunityCharacterBaseInfo.CharacterName.FirstName = GetString(reader, "first_name");
                            rankingData.CommunityCharacterBaseInfo.CharacterName.LastName = GetString(reader, "last_name");
                            rankingData.CommunityCharacterBaseInfo.ClanName = GetStringNullable(reader, "clan_name") ?? "";

                            list.Add(rankingData);
                        }
                    });

                return list;
            });

            return list;
        }

        public bool DeleteAllRankRecords(DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(
                    connection,
                    SqlDeleteAllRankRecords,
                    command => { }
                    ) > 0;
            });
        }
    }
}

