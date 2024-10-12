using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Xml;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] EquipmentPresetFields = new string[]
        {
            "character_common_id", "job_id", "preset_no", "preset_name"
        };

        private readonly string SqlInsertEquipmentPreset = $"INSERT INTO \"ddon_equip_preset\" ({BuildQueryField(EquipmentPresetFields)}) VALUES ({BuildQueryInsert(EquipmentPresetFields)});";
        private static readonly string SqlSelectEquipmentPresets = $"SELECT {BuildQueryField(EquipmentPresetFields)} FROM \"ddon_equip_preset\" WHERE \"character_common_id\" = @character_common_id AND \"job_id\" = @job_id;";
        private static readonly string SqlUpdateEquipmentPreset = $"UPDATE \"ddon_equip_preset\" SET {BuildQueryUpdate(EquipmentPresetFields)} WHERE \"character_common_id\"=@character_common_id AND \"preset_no\"=@preset_no";
        private static readonly string SqlDeleteEquipmentPreset = $"DELETE FROM \"ddon_equip_preset\" WHERE \"character_common_id\"=@character_common_id AND \"job_id\"=@job_id AND \"preset_no\"=@preset_no;";

        public bool InsertEquipmentPreset(uint characterCommonId, JobId jobId, uint presetNo, string presetName)
        {
            using TCon connection = OpenNewConnection();
            return InsertEquipmentPreset(connection, characterCommonId, jobId, presetNo, presetName);
        }

        public bool InsertEquipmentPreset(TCon conn, uint characterCommonId, JobId jobId, uint presetNo, string presetName)
        {
            return ExecuteNonQuery(conn, SqlInsertEquipmentPreset, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "job_id", (byte)jobId);
                AddParameter(command, "preset_no", presetNo);
                AddParameter(command, "preset_name", presetName);
            }) == 1;
        }

        public bool UpdateEquipmentPreset(uint characterCommonId, JobId jobId, uint presetNo, string presetName)
        {
            using TCon connection = OpenNewConnection();
            return UpdateEquipmentPreset(connection, characterCommonId, jobId, presetNo, presetName);
        }

        public bool UpdateEquipmentPreset(TCon conn, uint characterCommonId, JobId jobId, uint presetNo, string presetName)
        {
            return ExecuteNonQuery(conn, SqlUpdateEquipmentPreset, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "job_id", (byte)jobId);
                AddParameter(command, "preset_no", presetNo);
                AddParameter(command, "preset_name", presetName);
            }) == 1;
        }

        public List<CDataEquipPreset> SelectEquipmentPresets(uint characterCommonId, JobId jobId)
        {
            using TCon connection = OpenNewConnection();
            return SelectEquipmentPresets(connection, characterCommonId, jobId);
        }

        public List<CDataEquipPreset> SelectEquipmentPresets(TCon conn, uint characterCommonId, JobId jobId)
        {
            var results = new List<CDataEquipPreset>();
            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectEquipmentPresets,
                command => {
                        AddParameter(command, "@character_common_id", characterCommonId);
                        AddParameter(command, "@job_id", (byte) jobId);
                }, reader => {
                        while (reader.Read())
                        {
                            results.Add(new CDataEquipPreset()
                            {
                                Job = (JobId)GetByte(reader, "job_id"),
                                PresetNo = GetUInt32(reader, "preset_no"),
                                PresetName = GetString(reader, "preset_name")
                            });
                        }
                    });
            });
            return results;
        }

        public bool DeleteEquipmentPreset(uint characterCommonId, JobId jobId, uint presetNo)
        {
            using TCon connection = OpenNewConnection();
            return DeleteEquipmentPreset(connection, characterCommonId, jobId, presetNo);
        }

        public bool DeleteEquipmentPreset(TCon conn, uint characterCommonId, JobId jobId, uint presetNo)
        {
            return ExecuteNonQuery(conn, SqlDeleteEquipmentPreset, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "job_id", (byte) jobId);
                AddParameter(command, "preset_no", presetNo);
            }) == 1;
        }
    }
}
