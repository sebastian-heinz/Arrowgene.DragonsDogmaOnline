using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] CDataEquipItemInfoFields = new string[]
        {
            "character_id", "job", "item_id", "equip_type", "equip_slot", "color", "plus_value"
        };

        private readonly string SqlInsertEquipItemInfo = $"INSERT INTO `ddon_equip_item_info` ({BuildQueryField(CDataEquipItemInfoFields)}) VALUES ({BuildQueryInsert(CDataEquipItemInfoFields)});";
        private readonly string SqlReplaceEquipItemInfo = $"INSERT OR REPLACE INTO `ddon_equip_item_info` ({BuildQueryField(CDataEquipItemInfoFields)}) VALUES ({BuildQueryInsert(CDataEquipItemInfoFields)});";
        private static readonly string SqlUpdateEquipItemInfo = $"UPDATE `ddon_equip_item_info` SET {BuildQueryUpdate(CDataEquipItemInfoFields)} WHERE `character_id` = @character_id AND `job` = @job AND `equip_type`=@equip_type AND `equip_slot`=@equip_slot;";
        private static readonly string SqlSelectEquipItemInfo = $"SELECT {BuildQueryField(CDataEquipItemInfoFields)} FROM `ddon_equip_item_info` WHERE `character_id` = @character_id AND `job` = @job AND `equip_type`=1;";
        private static readonly string SqlSelectVisualEquipItemInfo = $"SELECT {BuildQueryField(CDataEquipItemInfoFields)} FROM `ddon_equip_item_info` WHERE `character_id` = @character_id AND `job` = @job AND `equip_type`=2;";
        private const string SqlDeleteEquipItemInfo = "DELETE FROM `ddon_equip_item_info` WHERE `character_id`=@character_id AND `job`=@job AND `equip_type`=@equip_type AND `equip_slot`=@equip_slot;";

        private CDataEquipItemInfo ReadEquipItemInfo(DbDataReader reader)
        {
            CDataEquipItemInfo equipItemInfo = new CDataEquipItemInfo();
            equipItemInfo.ItemId = GetUInt32(reader, "item_id");
            equipItemInfo.EquipType = GetByte(reader, "equip_type");
            equipItemInfo.EquipSlot = GetUInt16(reader, "equip_slot");
            equipItemInfo.Color = GetByte(reader, "color");
            equipItemInfo.PlusValue = GetByte(reader, "plus_value");
            return equipItemInfo;
        }

        private void AddParameter(TCom command, uint characterId, JobId job, CDataEquipItemInfo equipItemInfo)
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "job", (byte) job);
            AddParameter(command, "item_id", equipItemInfo.ItemId);
            AddParameter(command, "equip_type", equipItemInfo.EquipType);
            AddParameter(command, "equip_slot", equipItemInfo.EquipSlot);
            AddParameter(command, "color", equipItemInfo.Color);
            AddParameter(command, "plus_value", equipItemInfo.PlusValue);
        }
    }
}