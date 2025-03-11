using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        /* ddon_partner_pawn_pending_rewards */
        protected static readonly string[] PartnerPawnPendingRewardFields = new string[]
        {
            "character_id", "pawn_id", "reward_level"
        };

        private readonly string SqlSelectPartnerPawnPendingRewards = $"SELECT {BuildQueryField(PartnerPawnPendingRewardFields)} FROM \"ddon_partner_pawn_pending_rewards\" WHERE \"character_id\"=@character_id AND \"pawn_id\" = @pawn_id;";
        private readonly string SqlInsertPartnerPawnPendingReward = $"INSERT INTO \"ddon_partner_pawn_pending_rewards\" ({BuildQueryField(PartnerPawnPendingRewardFields)}) VALUES ({BuildQueryInsert(PartnerPawnPendingRewardFields)});";
        private readonly string SqlDeletePartnerPawnPendingReward = $"DELETE FROM ddon_partner_pawn_pending_rewards WHERE \"character_id\"=@character_id AND \"pawn_id\" = @pawn_id AND \"reward_level\"=@reward_level;";

        public HashSet<uint> GetPartnerPawnPendingRewards(uint characterId, uint pawnId, DbConnection? connectionIn = null)
        {
            var results = new HashSet<uint>();
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteReader(connection, SqlSelectPartnerPawnPendingRewards, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "pawn_id", pawnId);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        results.Add(GetUInt32(reader, "reward_level"));
                    }
                });
            });
            return results;
        }

        public bool InsertPartnerPawnPendingReward(uint characterId, uint pawnId, uint rewardLevel, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlInsertPartnerPawnPendingReward, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "pawn_id", pawnId);
                    AddParameter(command, "reward_level", rewardLevel);
                }) == 1;
            });
        }

        public void DeletePartnerPawnPendingReward(uint characterId, uint pawnId, uint rewardLevel, DbConnection? connectionIn = null)
        {
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteNonQuery(connection, SqlDeletePartnerPawnPendingReward, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "pawn_id", pawnId);
                    AddParameter(command, "reward_level", rewardLevel);
                });
            });
        }
    }
}
