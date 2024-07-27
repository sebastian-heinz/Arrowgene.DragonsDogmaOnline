using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] CDataJobPlayPointFields = new string[]
        {
            "character_common_id", "job", "play_point", "exp_mode"
        };

        private readonly string SqlInsertCharacterPlayPointData =
            $"INSERT INTO \"ddon_character_playpoint_data\" ({BuildQueryField(CDataJobPlayPointFields)}) VALUES ({BuildQueryInsert(CDataJobPlayPointFields)});";

        protected virtual string SqlInsertIfNotExistsCharacterPlayPointData { get; } =
            $"INSERT INTO \"ddon_character_playpoint_data\" ({BuildQueryField(CDataJobPlayPointFields)}) SELECT {BuildQueryInsert(CDataJobPlayPointFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_character_playpoint_data\" WHERE \"character_common_id\" = @character_common_id AND \"job\" = @job);";

        private static readonly string SqlUpdateCharacterPlayPointData =
            $"UPDATE \"ddon_character_playpoint_data\" SET {BuildQueryUpdate(CDataJobPlayPointFields)} WHERE \"character_common_id\" = @character_common_id AND \"job\" = @job;";

        private static readonly string SqlSelectCharacterPlayPointData =
            $"SELECT {BuildQueryField(CDataJobPlayPointFields)} FROM \"ddon_character_playpoint_data\" WHERE \"character_common_id\" = @character_common_id AND \"job\" = @job;";

        private static readonly string SqlSelectCharacterPlayPointDataByCharacter =
            $"SELECT {BuildQueryField(CDataJobPlayPointFields)} FROM \"ddon_character_playpoint_data\" WHERE \"character_common_id\" = @character_common_id;";

        private const string SqlDeleteCharacterPlayPointData = "DELETE FROM \"ddon_character_playpoint_data\" WHERE \"character_common_id\"=@character_common_id AND \"job\" = @job;";

        public bool ReplaceCharacterPlayPointData(uint commonId, CDataJobPlayPoint replacedCharacterPlayPointData)
        {
            using TCon connection = OpenNewConnection();
            return ReplaceCharacterPlayPointData(connection, commonId, replacedCharacterPlayPointData);
        }

        public bool ReplaceCharacterPlayPointData(TCon connection, uint commonId, CDataJobPlayPoint replacedCharacterPlayPointData)
        {
            Logger.Debug("Inserting character playpoint data.");
            if (!InsertIfNotExistsCharacterPlayPointData(connection, commonId, replacedCharacterPlayPointData))
            {
                Logger.Debug("Character playpoint data already exists, replacing.");
                return UpdateCharacterPlayPointData(connection, commonId, replacedCharacterPlayPointData);
            }
            return true;
        }

        public bool InsertCharacterPlayPointData(uint commonId, CDataJobPlayPoint updatedCharacterPlayPointData)
        {
            using TCon connection = OpenNewConnection();
            return InsertCharacterPlayPointData(connection, commonId, updatedCharacterPlayPointData);
        }

        public bool InsertCharacterPlayPointData(TCon connection, uint commonId, CDataJobPlayPoint updatedCharacterPlayPointData)
        {
            return ExecuteNonQuery(connection, SqlInsertCharacterPlayPointData, command => { AddParameter(command, commonId, updatedCharacterPlayPointData); }) == 1;
        }

        public bool InsertIfNotExistsCharacterPlayPointData(uint commonId, CDataJobPlayPoint updatedCharacterPlayPointData)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsCharacterPlayPointData(connection, commonId, updatedCharacterPlayPointData);
        }

        public bool InsertIfNotExistsCharacterPlayPointData(TCon connection, uint commonId, CDataJobPlayPoint updatedCharacterPlayPointData)
        {
            return ExecuteNonQuery(connection, SqlInsertIfNotExistsCharacterPlayPointData, command => { AddParameter(command, commonId, updatedCharacterPlayPointData); }) == 1;
        }

        public bool UpdateCharacterPlayPointData(uint commonId, CDataJobPlayPoint updatedCharacterPlayPointData)
        {
            using TCon connection = OpenNewConnection();
            return UpdateCharacterPlayPointData(connection, commonId, updatedCharacterPlayPointData);
        }

        public bool UpdateCharacterPlayPointData(TCon connection, uint commonId, CDataJobPlayPoint updatedCharacterPlayPointData)
        {
            return ExecuteNonQuery(connection, SqlUpdateCharacterPlayPointData, command => { AddParameter(command, commonId, updatedCharacterPlayPointData); }) == 1;
        }

        private void AddParameter(TCom command, uint commonId, CDataJobPlayPoint characterPlayPointData)
        {
            AddParameter(command, "character_common_id", commonId);
            AddParameter(command, "job", (byte)characterPlayPointData.Job);
            AddParameter(command, "play_point", characterPlayPointData.PlayPoint.PlayPoint);
            AddParameter(command, "exp_mode", characterPlayPointData.PlayPoint.ExpMode);
        }

        private CDataJobPlayPoint ReadCharacterPlayPointData(TReader reader)
        {
            CDataJobPlayPoint characterPlayPointData = new CDataJobPlayPoint();
            characterPlayPointData.Job = (JobId)GetByte(reader, "job");
            characterPlayPointData.PlayPoint.PlayPoint = GetUInt32(reader, "play_point");
            characterPlayPointData.PlayPoint.ExpMode = GetByte(reader, "exp_mode");

            return characterPlayPointData;
        }
    }
}
