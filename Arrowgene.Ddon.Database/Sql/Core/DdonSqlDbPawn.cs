using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] PawnFields = new string[]
        {
            "character_common_id", "character_id", "name", "hm_type", "pawn_type"
        };

        private static readonly string[] CDataPawnReactionFields = new string[]
        {
            "pawn_id", "reaction_type", "motion_no"
        };

        private static readonly string[]  CDataSpSkillFields = new string[]
        {
            "pawn_id", "slot", "sp_skill_id", "sp_skill_lv"
        };

        private readonly string SqlInsertPawn = $"INSERT INTO `ddon_pawn` ({BuildQueryField(PawnFields)}) VALUES ({BuildQueryInsert(PawnFields)});";
        private static readonly string SqlUpdatePawn = $"UPDATE `ddon_pawn` SET {BuildQueryUpdate(PawnFields)} WHERE `pawn_id` = @pawn_id;";
        private static readonly string SqlSelectPawn = $"SELECT `ddon_pawn`.`pawn_id`, {BuildQueryField(PawnFields)} FROM `ddon_pawn` WHERE `pawn_id` = @pawn_id;";
        private static readonly string SqlSelectPawnsByCharacterId = $"SELECT `ddon_pawn`.`pawn_id`, {BuildQueryField(PawnFields)} FROM `ddon_pawn` WHERE `character_id` = @character_id;";
        private readonly string SqlSelectAllPawnData = $"SELECT `ddon_pawn`.`pawn_id`, {BuildQueryField("ddon_pawn", PawnFields)}, `ddon_character_common`.`character_common_id`, {BuildQueryField("ddon_character_common", CharacterCommonFields)}, {BuildQueryField("ddon_edit_info", CDataEditInfoFields)}, {BuildQueryField("ddon_status_info", CDataStatusInfoFields)}"
            + "FROM `ddon_pawn` "
            + "LEFT JOIN `ddon_character_common` ON `ddon_character_common`.`character_common_id` = `ddon_pawn`.`character_common_id` "
            + "LEFT JOIN `ddon_edit_info` ON `ddon_edit_info`.`character_common_id` = `ddon_pawn`.`character_common_id` "
            + "LEFT JOIN `ddon_status_info` ON `ddon_status_info`.`character_common_id` = `ddon_pawn`.`character_common_id` "
            + "WHERE `ddon_pawn`.`pawn_id` = @pawn_id";
        private readonly string SqlSelectAllPawnsDataByCharacterId = $"SELECT `ddon_pawn`.`pawn_id`, {BuildQueryField("ddon_pawn", PawnFields)}, `ddon_character_common`.`character_common_id`, {BuildQueryField("ddon_character_common", CharacterCommonFields)}, {BuildQueryField("ddon_edit_info", CDataEditInfoFields)}, {BuildQueryField("ddon_status_info", CDataStatusInfoFields)}"
            + "FROM `ddon_pawn` "
            + "LEFT JOIN `ddon_character_common` ON `ddon_character_common`.`character_common_id` = `ddon_pawn`.`character_common_id` "
            + "LEFT JOIN `ddon_edit_info` ON `ddon_edit_info`.`character_common_id` = `ddon_pawn`.`character_common_id` "
            + "LEFT JOIN `ddon_status_info` ON `ddon_status_info`.`character_common_id` = `ddon_pawn`.`character_common_id` "
            + "WHERE `character_id` = @character_id";
        private const string SqlDeletePawn = "DELETE FROM `ddon_character_common` WHERE EXISTS (SELECT 1 FROM `ddon_pawn` WHERE `pawn_id`=@pawn_id);";

        private readonly string SqlInsertPawnReaction = $"INSERT INTO `ddon_pawn_reaction` ({BuildQueryField(CDataPawnReactionFields)}) VALUES ({BuildQueryInsert(CDataPawnReactionFields)});";
        private readonly string SqlReplacePawnReaction = $"REPLACE INTO `ddon_pawn_reaction` ({BuildQueryField(CDataPawnReactionFields)}) VALUES ({BuildQueryInsert(CDataPawnReactionFields)});";
        private static readonly string SqlUpdatePawnReaction = $"UPDATE `ddon_pawn_reaction` SET {BuildQueryUpdate(CDataPawnReactionFields)} WHERE `pawn_id` = @pawn_id AND `reaction_type`=@reaction_type;";
        private static readonly string SqlSelectPawnReactionByPawnId = $"SELECT {BuildQueryField(CDataPawnReactionFields)} FROM `ddon_pawn_reaction` WHERE `pawn_id` = @pawn_id;";
        private const string SqlDeletePawnReaction = "DELETE FROM `ddon_pawn_reaction` WHERE `pawn_id`=@pawn_id AND `reaction_type`=@reaction_type;";

        public bool CreatePawn(Pawn pawn)
        {
            return ExecuteInTransaction(conn =>
                {                    
                    ExecuteNonQuery(conn, SqlInsertCharacterCommon, command => { AddParameter(command, pawn); }, out long commonId);
                    pawn.CommonId = (uint) commonId;

                    ExecuteNonQuery(conn, SqlInsertPawn, command => { AddParameter(command, pawn); }, out long pawnId);
                    pawn.PawnId = (uint) pawnId;

                    ExecuteNonQuery(conn, SqlInsertEditInfo, command => { AddParameter(command, pawn); });
                    ExecuteNonQuery(conn, SqlInsertStatusInfo, command => { AddParameter(command, pawn); });

                    CreateItems(conn, pawn);
                    
                    StorePawnData(conn, pawn);
                });
        }

        public Pawn SelectPawn(uint pawnId)
        {
            Pawn pawn = null;
            ExecuteInTransaction(conn => {
                ExecuteReader(conn, SqlSelectAllPawnData,
                command => { AddParameter(command, "@pawn_id", pawnId); }, reader =>
                {
                    if (reader.Read())
                    {
                        pawn = ReadAllPawnData(reader);
                    }
                });

                QueryPawnData(conn, pawn);
            });
            return pawn;
        }

        public List<Pawn> SelectPawnsByCharacterId(uint characterId)
        {
            List<Pawn> pawns = new List<Pawn>();
            ExecuteInTransaction(conn => {
                ExecuteReader(conn, SqlSelectAllPawnsDataByCharacterId,
                    command => { AddParameter(command, "@character_id", characterId); }, reader =>
                    {
                        while (reader.Read())
                        {
                            Pawn pawn = ReadAllPawnData(reader);
                            pawns.Add(pawn);

                            QueryPawnData(conn, pawn);
                        }
                    });
            });
            return pawns;
        }

        public bool DeletePawn(uint pawnId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeletePawn,
                command => { AddParameter(command, "@pawn_id", pawnId); });
            return rowsAffected > NoRowsAffected;
        }

        private void QueryPawnData(TCon conn, Pawn pawn)
        {
            QueryCharacterCommonData(conn, pawn);

            // TODO: ddon_pawn_reaction and ddon_sp_skill
        }

        private void StorePawnData(TCon conn, Pawn pawn)
        {
            StoreCharacterCommonData(conn, pawn);

            // TODO: ddon_pawn_reaction and ddon_sp_skill
        }

        private void CreateItems(TCon conn, Pawn pawn)
        {
            // Create equipment items
            foreach (KeyValuePair<JobId, Dictionary<EquipType, List<Item>>> jobEquipment in pawn.Equipment.getAllEquipment())
            {
                JobId job = jobEquipment.Key;
                foreach (KeyValuePair<EquipType, List<Item>> equipment in jobEquipment.Value)
                {
                    EquipType equipType = equipment.Key;
                    for (byte index = 0; index < equipment.Value.Count; index++)
                    {
                        Item item = equipment.Value[index];
                        if(item != null)
                        {
                            byte slot = (byte)(index+1);
                            InsertItem(conn, item);
                            InsertEquipItem(conn, pawn.CommonId, job, equipType, slot, item.UId);
                        }
                    }
                }
            }
        }

        private Pawn ReadAllPawnData(DbDataReader reader)
        {
            Pawn pawn = new Pawn();

            ReadAllCharacterCommonData(reader, pawn);

            pawn.PawnId = GetUInt32(reader, "pawn_id");
            pawn.CharacterId = GetUInt32(reader, "character_id");
            pawn.Name = GetString(reader, "name");
            pawn.HmType = GetByte(reader, "hm_type");
            pawn.PawnType = GetByte(reader, "pawn_type");

            return pawn;
        }

        private void AddParameter(TCom command, Pawn pawn)
        {
            AddParameter(command, (CharacterCommon) pawn);
            // PawnFields
            AddParameter(command, "@pawn_id", pawn.PawnId);
            AddParameter(command, "@character_id", pawn.CharacterId);
            AddParameter(command, "@name", pawn.Name);
            AddParameter(command, "@hm_type", pawn.HmType);
            AddParameter(command, "@pawn_type", pawn.PawnType);
        }
    }
}