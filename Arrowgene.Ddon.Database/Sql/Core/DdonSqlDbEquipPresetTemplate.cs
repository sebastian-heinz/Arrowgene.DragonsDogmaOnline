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
        protected static readonly string[] EquipmentTmeplateFields = new string[]
        {
            "character_common_id", "job_id", "preset_no", "slot_no", "item_uid"
        };

        private readonly string SqlInsertEquipmentPresetTemplate = $"INSERT INTO \"ddon_equip_preset_template\" ({BuildQueryField(EquipmentTmeplateFields)}) VALUES ({BuildQueryInsert(EquipmentTmeplateFields)});";
        private static readonly string SqlSelectEquipmentPresetTemplate = $"SELECT {BuildQueryField(EquipmentTmeplateFields)} FROM \"ddon_equip_preset_template\" WHERE \"character_common_id\" = @character_common_id AND \"job_id\" = @job_id AND \"preset_no\" = @preset_no;";
        private static readonly string SqlDeleteEquipmentPresetTemplate = $"DELETE FROM \"ddon_equip_preset_template\" WHERE \"character_common_id\"=@character_common_id AND \"job_id\"=@job_id AND \"preset_no\"=@preset_no;";

        public bool InsertEquipmentPresetTemplate(uint characterCommonId, JobId jobId, uint presetNo, uint slotNo, string itemUId)
        {
            using TCon connection = OpenNewConnection();
            return InsertEquipmentPresetTemplate(connection, characterCommonId, jobId, presetNo, slotNo, itemUId);
        }

        public bool InsertEquipmentPresetTemplate(TCon conn, uint characterCommonId, JobId jobId, uint presetNo, uint slotNo, string itemUId)
        {
            return ExecuteNonQuery(conn, SqlInsertEquipmentPresetTemplate, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "job_id", (byte)jobId);
                AddParameter(command, "preset_no", presetNo);
                AddParameter(command, "slot_no", slotNo);
                AddParameter(command, "item_uid", itemUId);
            }) == 1;
        }

        public List<CDataPresetEquipInfo> SelectEquipmentPresetTemplate(uint characterCommonId, JobId jobId, uint presetNo)
        {
            using TCon connection = OpenNewConnection();
            return SelectEquipmentPresetTemplate(connection, characterCommonId, jobId, presetNo);
        }

        public List<CDataPresetEquipInfo> SelectEquipmentPresetTemplate(TCon conn, uint characterCommonId, JobId jobId, uint presetNo)
        {
            var results = new List<CDataPresetEquipInfo>();
            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectEquipmentPresetTemplate,
                command => {
                    AddParameter(command, "character_common_id", characterCommonId);
                    AddParameter(command, "job_id", (byte)jobId);
                    AddParameter(command, "preset_no", presetNo);
                }, reader => {
                    while (reader.Read())
                    {
                        results.Add(new CDataPresetEquipInfo()
                        {
                            ItemUId = GetString(reader, "item_uid"),
                            EquipSlotNo = GetByte(reader, "slot_no")
                        });
                    }
                });
            });
            return results;
        }

        public bool DeleteEquipmentPresetTemplate(uint characterCommonId, JobId jobId, uint presetNo)
        {
            using TCon connection = OpenNewConnection();
            return DeleteEquipmentPresetTemplate(connection, characterCommonId, jobId, presetNo);
        }

        public bool DeleteEquipmentPresetTemplate(TCon conn, uint characterCommonId, JobId jobId, uint presetNo)
        {
            return ExecuteNonQuery(conn, SqlDeleteEquipmentPresetTemplate, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "job_id", (byte)jobId);
                AddParameter(command, "preset_no", presetNo);
            }) == 1;
        }
    }
}
