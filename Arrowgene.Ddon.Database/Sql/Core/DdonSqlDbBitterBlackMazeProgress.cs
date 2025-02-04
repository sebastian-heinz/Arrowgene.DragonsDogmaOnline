using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
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
        protected static readonly string[] BitterBlackMazeProgressFields = new string[]
        {
            "character_id", "start_time", "content_id", "content_mode", "tier", "killed_death", "last_ticket_time"
        };

        private readonly string SqlSelectBBMProgress = $"SELECT {BuildQueryField(BitterBlackMazeProgressFields)} FROM \"ddon_bbm_progress\" WHERE \"character_id\"=@character_id;";
        private readonly string SqlInsertBBMProgress = $"INSERT INTO \"ddon_bbm_progress\" ({BuildQueryField(BitterBlackMazeProgressFields)}) VALUES ({BuildQueryInsert(BitterBlackMazeProgressFields)});";
        private readonly string SqlUpdateBBMProgress = $"UPDATE \"ddon_bbm_progress\" SET {BuildQueryUpdate(BitterBlackMazeProgressFields)} WHERE \"character_id\"=@character_id;";
        private readonly string SqlDeleteBBMProgress = $"DELETE FROM \"ddon_bbm_progress\" WHERE \"character_id\"=@character_id;";

        public bool InsertBBMProgress(uint characterId, ulong startTime, uint contentId, BattleContentMode contentMode, uint tier, bool killedDeath, ulong lastTicketTime)
        {
            using TCon connection = OpenNewConnection();
            return InsertBBMProgress(connection, characterId, startTime, contentId, contentMode, tier, killedDeath, lastTicketTime);
        }

        public bool InsertBBMProgress(TCon connection, uint characterId, ulong startTime, uint contentId, BattleContentMode contentMode, uint tier, bool killedDeath, ulong lastTicketTime)
        {
            return ExecuteNonQuery(connection, SqlInsertBBMProgress, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "start_time", startTime);
                AddParameter(command, "content_id", contentId);
                AddParameter(command, "content_mode", (uint) contentMode);
                AddParameter(command, "tier", tier);
                AddParameter(command, "killed_death", killedDeath);
                AddParameter(command, "last_ticket_time", lastTicketTime);
            }) == 1;
        }
        public bool UpdateBBMProgress(uint characterId, BitterblackMazeProgress progress, DbConnection? connectionIn = null)
        {
            return UpdateBBMProgress(characterId, progress.StartTime, progress.ContentId, progress.ContentMode, progress.Tier, progress.KilledDeath, progress.LastTicketTime, connectionIn);
        }

        public bool UpdateBBMProgress(uint characterId, ulong startTime, uint contentId, BattleContentMode contentMode, uint tier, bool killedDeath, ulong lastTicketTime, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlUpdateBBMProgress, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "start_time", startTime);
                    AddParameter(command, "content_id", contentId);
                    AddParameter(command, "content_mode", (uint)contentMode);
                    AddParameter(command, "tier", tier);
                    AddParameter(command, "killed_death", killedDeath);
                    AddParameter(command, "last_ticket_time", lastTicketTime);
                }) == 1;
            });
        }

        public bool RemoveBBMProgress(uint characterId)
        {
            using TCon connection = OpenNewConnection();
            return RemoveBBMProgress(connection, characterId);
        }

        public bool RemoveBBMProgress(TCon connection, uint characterId)
        {
            return ExecuteNonQuery(connection, SqlDeleteBBMProgress, command =>
            {
                AddParameter(command, "character_id", characterId);
            }) == 1;
        }

        public BitterblackMazeProgress SelectBBMProgress(uint characterId)
        {
            using TCon connection = OpenNewConnection();
            return SelectBBMProgress(connection, characterId);
        }

        public BitterblackMazeProgress SelectBBMProgress(TCon connection, uint characterId)
        {
            BitterblackMazeProgress result = null;
            ExecuteInTransaction(connection =>
            {
                ExecuteReader(connection, SqlSelectBBMProgress, command =>
                {
                    AddParameter(command, "character_id", characterId);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        result = new BitterblackMazeProgress();
                        result.StartTime = GetUInt64(reader, "start_time");
                        result.ContentId = GetUInt32(reader, "content_id");
                        result.ContentMode = (BattleContentMode) GetUInt32(reader, "content_mode");
                        result.Tier = GetUInt32(reader, "tier");
                        result.KilledDeath = GetBoolean(reader, "killed_death");
                        result.LastTicketTime = GetUInt64(reader, "last_ticket_time");
                    }
                });
            });

            return result;
        }
    }
}
