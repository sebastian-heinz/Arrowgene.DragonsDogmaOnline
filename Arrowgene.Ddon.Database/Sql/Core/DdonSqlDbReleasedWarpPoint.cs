using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] ReleasedWarpPointFields = new string[]
        {
            "character_id", "warp_point_id", "favorite_slot_no"
        };

        private readonly string SqlInsertReleasedWarpPoint = $"INSERT INTO \"ddon_released_warp_point\" ({BuildQueryField(ReleasedWarpPointFields)}) VALUES ({BuildQueryInsert(ReleasedWarpPointFields)});";
        private readonly string SqlInsertIfNotExistsReleasedWarpPoint = $"INSERT INTO \"ddon_released_warp_point\" ({BuildQueryField(ReleasedWarpPointFields)}) SELECT {BuildQueryInsert(ReleasedWarpPointFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_released_warp_point\" WHERE \"character_id\"=@character_id AND \"warp_point_id\"=@warp_point_id);";
        private static readonly string SqlUpdateReleasedWarpPoint = $"UPDATE \"ddon_released_warp_point\" SET {BuildQueryUpdate(ReleasedWarpPointFields)} WHERE \"character_id\"=@character_id AND \"warp_point_id\"=@warp_point_id";
        private static readonly string SqlSelectReleasedWarpPoints = $"SELECT {BuildQueryField(ReleasedWarpPointFields)} FROM \"ddon_released_warp_point\" WHERE \"character_id\"=@character_id;";
        private const string SqlDeleteReleasedWarpPoint = "DELETE FROM \"ddon_released_warp_point\" WHERE \"character_id\"=@character_id AND \"warp_point_id\"=@warp_point_id";

        public List<ReleasedWarpPoint> SelectReleasedWarpPoints(uint characterId)
        {
            using TCon connection = OpenNewConnection();
            return SelectReleasedWarpPoints(connection, characterId);
        }
        
        public List<ReleasedWarpPoint> SelectReleasedWarpPoints(TCon connection, uint characterId)
        {
            List<ReleasedWarpPoint> rwps = new List<ReleasedWarpPoint>();
            ExecuteReader(connection, SqlSelectReleasedWarpPoints,
            command =>
            {
                AddParameter(command, "@character_id", characterId);
            },
            reader =>
            {
                while (reader.Read())
                {
                    rwps.Add(ReadReleasedWarpPoint(reader));
                }
            });
            return rwps;
        }

        public bool InsertIfNotExistsReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsReleasedWarpPoint(connection, characterId, ReleasedWarpPoint);
        }
        
        public bool InsertIfNotExistsReleasedWarpPoint(TCon connection, uint characterId, ReleasedWarpPoint ReleasedWarpPoint)
        {
            return ExecuteNonQuery(connection, SqlInsertIfNotExistsReleasedWarpPoint, command =>
            {
                AddParameter(command, characterId, ReleasedWarpPoint);
            }) == 1;
        }

        public bool InsertIfNotExistsReleasedWarpPoints(uint characterId, List<ReleasedWarpPoint> releasedWarpPoints)
        {
            return ExecuteInTransaction(connection => {
                foreach (ReleasedWarpPoint releasedWarpPoint in releasedWarpPoints)
                {
                    InsertIfNotExistsReleasedWarpPoint(connection, characterId, releasedWarpPoint);
                }
            });
        }
        
        public bool InsertReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint)
        {
            using TCon connection = OpenNewConnection();
            return InsertReleasedWarpPoint(connection, characterId, ReleasedWarpPoint);
        }
        
        public bool InsertReleasedWarpPoint(TCon connection, uint characterId, ReleasedWarpPoint ReleasedWarpPoint)
        {
            return ExecuteNonQuery(connection, SqlInsertReleasedWarpPoint, command =>
            {
                AddParameter(command, characterId, ReleasedWarpPoint);
            }) == 1;
        }
        
        public bool ReplaceReleasedWarpPoint(uint characterId, ReleasedWarpPoint ReleasedWarpPoint)
        {
            using TCon connection = OpenNewConnection();
            return ReplaceReleasedWarpPoint(connection, characterId, ReleasedWarpPoint);
        }        
        
        public bool ReplaceReleasedWarpPoint(TCon connection, uint characterId, ReleasedWarpPoint ReleasedWarpPoint)
        {
            Logger.Debug("Inserting wallet point.");
            if (!InsertIfNotExistsReleasedWarpPoint(connection, characterId, ReleasedWarpPoint))
            {
                Logger.Debug("Wallet point already exists, replacing.");
                return UpdateReleasedWarpPoint(connection, characterId, ReleasedWarpPoint);
            }
            return true;
        }

        public bool UpdateReleasedWarpPoint(uint characterId, ReleasedWarpPoint updatedReleasedWarpPoint)
        {
            using TCon connection = OpenNewConnection();
            return UpdateReleasedWarpPoint(connection, characterId, updatedReleasedWarpPoint);
        }
        
        public bool UpdateReleasedWarpPoint(TCon connection, uint characterId, ReleasedWarpPoint updatedReleasedWarpPoint)
        {
            return ExecuteNonQuery(connection, SqlUpdateReleasedWarpPoint, command =>
            {
                AddParameter(command, characterId, updatedReleasedWarpPoint);
            }) == 1;
        }

        public bool DeleteReleasedWarpPoint(uint characterId, uint warpPointId)
        {
            return ExecuteNonQuery(SqlDeleteReleasedWarpPoint, command =>
            {
                AddParameter(command, "@character_id", characterId);
                AddParameter(command, "@warp_point_id", (byte) warpPointId);
            }) == 1;
        }

        private ReleasedWarpPoint ReadReleasedWarpPoint(TReader reader)
        {
            ReleasedWarpPoint ReleasedWarpPoint = new ReleasedWarpPoint
            {
                WarpPointId = GetUInt32(reader, "warp_point_id"),
                FavoriteSlotNo = GetUInt32(reader, "favorite_slot_no")
            };
            return ReleasedWarpPoint;
        }

        private void AddParameter(TCom command, uint characterId, ReleasedWarpPoint ReleasedWarpPoint)
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "warp_point_id", ReleasedWarpPoint.WarpPointId);
            AddParameter(command, "favorite_slot_no", ReleasedWarpPoint.FavoriteSlotNo);
        }
    }
}
