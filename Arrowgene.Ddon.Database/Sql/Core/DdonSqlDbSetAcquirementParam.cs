using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] CDataSetAcquirementParamFields = new string[]
        {
            "character_id", "job", "type", "slot_no", "acquirement_no", "acquirement_lv"
        };

        private readonly string SqlInsertSetAcquirementParam = $"INSERT INTO `ddon_set_acquirement_param` ({BuildQueryField(CDataSetAcquirementParamFields)}) VALUES ({BuildQueryInsert(CDataSetAcquirementParamFields)});";
        private readonly string SqlReplaceSetAcquirementParam = $"INSERT OR REPLACE INTO `ddon_set_acquirement_param` ({BuildQueryField(CDataSetAcquirementParamFields)}) VALUES ({BuildQueryInsert(CDataSetAcquirementParamFields)});";
        private static readonly string SqlUpdateSetAcquirementParam = $"UPDATE `ddon_set_acquirement_param` SET {BuildQueryUpdate(CDataSetAcquirementParamFields)} WHERE `character_id`=@character_id AND `job`=@job AND `type`=@type AND `slot_no`=@slot_no;";
        private static readonly string SqlSelectCustomSkills = $"SELECT {BuildQueryField(CDataSetAcquirementParamFields)} FROM `ddon_set_acquirement_param` WHERE `character_id`=@character_id AND `type`=0;";
        private static readonly string SqlSelectAbilities = $"SELECT {BuildQueryField(CDataSetAcquirementParamFields)} FROM `ddon_set_acquirement_param` WHERE `character_id`=@character_id AND `type`<>0;";
        private const string SqlDeleteSetAcquirementParam = "DELETE FROM `ddon_set_acquirement_param` WHERE `character_id`=@character_id AND `job`=@job AND `type`=@type AND `slot_no`=@slot_no;";

        public bool ReplaceSetAcquirementParam(uint characterId, CDataSetAcquirementParam setAcquirementParam)
        {
            ExecuteNonQuery(SqlReplaceSetAcquirementParam, command =>
            {
                AddParameter(command, characterId, setAcquirementParam);
            });
            return true;
        }

        public bool DeleteSetAcquirementParam(uint characterId, JobId job, byte type, byte slotNo)
        {
            return ExecuteNonQuery(SqlDeleteSetAcquirementParam, command =>
            {
                AddParameter(command, "@character_id", characterId);
                AddParameter(command, "@job", (byte) job);
                AddParameter(command, "@type", type);
                AddParameter(command, "@slot_no", slotNo);
            }) == 1;
        }

        private CDataSetAcquirementParam ReadSetAcquirementParam(DbDataReader reader)
        {
            CDataSetAcquirementParam setAcquirementParam = new CDataSetAcquirementParam();
            setAcquirementParam.Job = (JobId) GetByte(reader, "job");
            setAcquirementParam.Type = GetByte(reader, "type");
            setAcquirementParam.SlotNo = GetByte(reader, "slot_no");
            setAcquirementParam.AcquirementNo = GetUInt32(reader, "acquirement_no");
            setAcquirementParam.AcquirementLv = GetByte(reader, "acquirement_lv");
            return setAcquirementParam;
        }

        private void AddParameter(TCom command, uint characterId, CDataSetAcquirementParam setAcquirementParam)
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "job", (byte) setAcquirementParam.Job);
            AddParameter(command, "type", setAcquirementParam.Type);
            AddParameter(command, "slot_no", setAcquirementParam.SlotNo);
            AddParameter(command, "acquirement_no", setAcquirementParam.AcquirementNo);
            AddParameter(command, "acquirement_lv", setAcquirementParam.AcquirementLv);
        }
    }
}