using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] CommunicationShortcutFields = new string[]
        {
            "character_id", "page_no", "button_no", "type", "category", "id"
        };

        private readonly string SqlInsertCommunicationShortcut = $"INSERT INTO `ddon_communication_shortcut` ({BuildQueryField(CommunicationShortcutFields)}) VALUES ({BuildQueryInsert(CommunicationShortcutFields)});";
        private readonly string SqlReplaceCommunicationShortcut = $"INSERT OR REPLACE INTO `ddon_communication_shortcut` ({BuildQueryField(CommunicationShortcutFields)}) VALUES ({BuildQueryInsert(CommunicationShortcutFields)});";
        private static readonly string SqlUpdateCommunicationShortcut = $"UPDATE `ddon_communication_shortcut` SET {BuildQueryUpdate(CommunicationShortcutFields)} WHERE `character_id`=@old_character_id AND `page_no`=@old_page_no AND `button_no`=@old_button_no";
        private static readonly string SqlSelectCommunicationShortcuts = $"SELECT {BuildQueryField(CommunicationShortcutFields)} FROM `ddon_communication_shortcut` WHERE `character_id`=@character_id;";
        private const string SqlDeleteCommunicationShortcut = "DELETE FROM `ddon_communication_shortcut` WHERE `character_id`=@character_id AND `page_no`=@page_no AND `button_no`=@button_no";

        public bool InsertCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut)
        {
            return ExecuteNonQuery(SqlInsertCommunicationShortcut, command =>
            {
                AddParameter(command, characterId, communicationShortcut);
            }) == 1;
        }
        
        public bool ReplaceCommunicationShortcut(uint characterId, CDataCommunicationShortCut communicationShortcut)
        {
            ExecuteNonQuery(SqlReplaceCommunicationShortcut, command =>
            {
                AddParameter(command, characterId, communicationShortcut);
            });
            return true;
        }

        public bool UpdateCommunicationShortcut(uint characterId, uint oldPageNo, uint oldButtonNo, CDataCommunicationShortCut updatedCommunicationShortcut)
        {
            return ExecuteNonQuery(SqlDeleteCommunicationShortcut, command =>
            {
                AddParameter(command, characterId, updatedCommunicationShortcut);
                AddParameter(command, "@old_character_id", characterId);
                AddParameter(command, "@old_page_no", oldPageNo);
                AddParameter(command, "@old_button_no", oldButtonNo);
            }) == 1;
        }

        public bool DeleteCommunicationShortcut(uint characterId, uint pageNo, uint buttonNo)
        {
            return ExecuteNonQuery(SqlDeleteCommunicationShortcut, command =>
            {
                AddParameter(command, "@character_id", characterId);
                AddParameter(command, "@old_page_no", pageNo);
                AddParameter(command, "@old_button_no", buttonNo);
            }) == 1;
        }

        private CDataCommunicationShortCut ReadCommunicationShortCut(DbDataReader reader)
        {
            CDataCommunicationShortCut shortcut = new CDataCommunicationShortCut();
            shortcut.PageNo = GetUInt32(reader, "page_no");
            shortcut.ButtonNo = GetUInt32(reader, "button_no");
            shortcut.Type = GetByte(reader, "type");
            shortcut.Category = GetByte(reader, "category");
            shortcut.Id = GetUInt32(reader, "id");
            return shortcut;
        }

        private void AddParameter(TCom command, uint characterId, CDataCommunicationShortCut shortcut)
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "page_no", shortcut.PageNo);
            AddParameter(command, "button_no", shortcut.ButtonNo);
            AddParameter(command, "type", shortcut.Type);
            AddParameter(command, "category", shortcut.Category);
            AddParameter(command, "id", shortcut.Id);
        }
    }
}