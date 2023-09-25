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
        private static readonly string[] PawnFields = new string[]
        {
            "character_common_id", "character_id", "name", "hm_type", "pawn_type"
        };

        protected static readonly string[] CDataPawnReactionFields = new string[]
        {
            "pawn_id", "reaction_type", "motion_no"
        };

        protected static readonly string[]  CDataSpSkillFields = new string[]
        {
            "pawn_id", "sp_skill_id", "sp_skill_lv"
        };

        private readonly string SqlInsertPawn = $"INSERT INTO \"ddon_pawn\" ({BuildQueryField(PawnFields)}) VALUES ({BuildQueryInsert(PawnFields)});";
        private static readonly string SqlUpdatePawn = $"UPDATE \"ddon_pawn\" SET {BuildQueryUpdate(PawnFields)} WHERE \"pawn_id\" = @pawn_id;";
        private static readonly string SqlSelectPawn = $"SELECT \"ddon_pawn\".\"pawn_id\", {BuildQueryField(PawnFields)} FROM \"ddon_pawn\" WHERE \"pawn_id\" = @pawn_id;";
        private static readonly string SqlSelectPawnsByCharacterId = $"SELECT \"ddon_pawn\".\"pawn_id\", {BuildQueryField(PawnFields)} FROM \"ddon_pawn\" WHERE \"character_id\" = @character_id;";
        private readonly string SqlSelectAllPawnData = $"SELECT \"ddon_pawn\".\"pawn_id\", {BuildQueryField("ddon_pawn", PawnFields)}, \"ddon_character_common\".\"character_common_id\", {BuildQueryField("ddon_character_common", CharacterCommonFields)}, {BuildQueryField("ddon_edit_info", CDataEditInfoFields)}, {BuildQueryField("ddon_status_info", CDataStatusInfoFields)}"
            + "FROM \"ddon_pawn\" "
            + "LEFT JOIN \"ddon_character_common\" ON \"ddon_character_common\".\"character_common_id\" = \"ddon_pawn\".\"character_common_id\" "
            + "LEFT JOIN \"ddon_edit_info\" ON \"ddon_edit_info\".\"character_common_id\" = \"ddon_pawn\".\"character_common_id\" "
            + "LEFT JOIN \"ddon_status_info\" ON \"ddon_status_info\".\"character_common_id\" = \"ddon_pawn\".\"character_common_id\" "
            + "WHERE \"ddon_pawn\".\"pawn_id\" = @pawn_id";
        private readonly string SqlSelectAllPawnsDataByCharacterId = $"SELECT \"ddon_pawn\".\"pawn_id\", {BuildQueryField("ddon_pawn", PawnFields)}, \"ddon_character_common\".\"character_common_id\", {BuildQueryField("ddon_character_common", CharacterCommonFields)}, {BuildQueryField("ddon_edit_info", CDataEditInfoFields)}, {BuildQueryField("ddon_status_info", CDataStatusInfoFields)}"
            + "FROM \"ddon_pawn\" "
            + "LEFT JOIN \"ddon_character_common\" ON \"ddon_character_common\".\"character_common_id\" = \"ddon_pawn\".\"character_common_id\" "
            + "LEFT JOIN \"ddon_edit_info\" ON \"ddon_edit_info\".\"character_common_id\" = \"ddon_pawn\".\"character_common_id\" "
            + "LEFT JOIN \"ddon_status_info\" ON \"ddon_status_info\".\"character_common_id\" = \"ddon_pawn\".\"character_common_id\" "
            + "WHERE \"character_id\" = @character_id";
        private const string SqlDeletePawn = "DELETE FROM \"ddon_character_common\" WHERE EXISTS (SELECT 1 FROM \"ddon_pawn\" WHERE \"pawn_id\"=@pawn_id);";

        private readonly string SqlInsertPawnReaction = $"INSERT INTO \"ddon_pawn_reaction\" ({BuildQueryField(CDataPawnReactionFields)}) VALUES ({BuildQueryInsert(CDataPawnReactionFields)});";
        private readonly string SqlInsertIfNotExistsPawnReaction = $"INSERT INTO \"ddon_pawn_reaction\" ({BuildQueryField(CDataPawnReactionFields)}) SELECT {BuildQueryInsert(CDataPawnReactionFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_pawn_reaction\" WHERE \"pawn_id\"=@pawn_id AND \"reaction_type\"=@reaction_type);";
        private static readonly string SqlUpdatePawnReaction = $"UPDATE \"ddon_pawn_reaction\" SET {BuildQueryUpdate(CDataPawnReactionFields)} WHERE \"pawn_id\" = @pawn_id AND \"reaction_type\"=@reaction_type;";
        private static readonly string SqlSelectPawnReactionByPawnId = $"SELECT {BuildQueryField(CDataPawnReactionFields)} FROM \"ddon_pawn_reaction\" WHERE \"pawn_id\" = @pawn_id;";
        private const string SqlDeletePawnReaction = "DELETE FROM \"ddon_pawn_reaction\" WHERE \"pawn_id\"=@pawn_id AND \"reaction_type\"=@reaction_type;";

        private readonly string SqlInsertSpSkill = $"INSERT INTO \"ddon_sp_skill\" ({BuildQueryField(CDataSpSkillFields)}) VALUES ({BuildQueryInsert(CDataSpSkillFields)});";
        private readonly string SqlInsertIfNotExistsSpSkill = $"INSERT INTO \"ddon_sp_skill\" ({BuildQueryField(CDataSpSkillFields)}) SELECT {BuildQueryInsert(CDataSpSkillFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_sp_skill\" WHERE \"pawn_id\" = @pawn_id);";
        private static readonly string SqlUpdateSpSkill = $"UPDATE \"ddon_sp_skill\" SET {BuildQueryUpdate(CDataSpSkillFields)} WHERE \"pawn_id\" = @pawn_id AND \"sp_skill_id\"=@sp_skill_id;";
        private static readonly string SqlSelectSpSkillByPawnId = $"SELECT {BuildQueryField(CDataSpSkillFields)} FROM \"ddon_sp_skill\" WHERE \"pawn_id\" = @pawn_id;";
        private const string SqlDeleteSpSkill = "DELETE FROM \"ddon_sp_skill\" WHERE \"pawn_id\"=@pawn_id AND \"sp_skill_id\"=@sp_skill_id;";

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
            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectAllPawnsDataByCharacterId,
                    command => { AddParameter(command, "@character_id", characterId); }, reader =>
                    {
                        while (reader.Read())
                        {
                            Pawn pawn = ReadAllPawnData(reader);
                            pawns.Add(pawn);
                        }
                    });
                foreach (var pawn in pawns)
                {
                    QueryPawnData(conn, pawn);
                }
            });
            return pawns;
        }

        public bool DeletePawn(uint pawnId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeletePawn,
                command =>
                {
                    AddParameter(command, "@pawn_id", pawnId);
                });
            return rowsAffected > NoRowsAffected;
        }

        public bool UpdatePawnBaseInfo(Pawn pawn)
        {
            using TCon connection = OpenNewConnection();
            return UpdatePawnBaseInfo(connection, pawn);
        }

        public bool UpdatePawnBaseInfo(TCon conn, Pawn pawn)
        {
            int characterUpdateRowsAffected = ExecuteNonQuery(conn, SqlUpdatePawn, command =>
            {
                AddParameter(command, pawn);
            });

            return characterUpdateRowsAffected > NoRowsAffected;
        }

        private void QueryPawnData(TCon conn, Pawn pawn)
        {
            QueryCharacterCommonData(conn, pawn);

            ExecuteReader(conn, SqlSelectPawnReactionByPawnId,
                command => { AddParameter(command, "@pawn_id", pawn.PawnId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        pawn.PawnReactionList.Add(ReadPawnReaction(reader));
                    }
                });

            ExecuteReader(conn, SqlSelectSpSkillByPawnId,
                command => { AddParameter(command, "@pawn_id", pawn.PawnId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        pawn.SpSkillList.Add(ReadSpSkill(reader));
                    }
                });
        }

        private void StorePawnData(TCon conn, Pawn pawn)
        {
            StoreCharacterCommonData(conn, pawn);

            foreach (CDataPawnReaction pawnReaction in pawn.PawnReactionList)
            {
                ReplacePawnReaction(conn, pawn.PawnId, pawnReaction);
            }

            foreach (CDataSpSkill spSkill in pawn.SpSkillList)
            {
                ReplaceSpSkill(conn, pawn.PawnId, spSkill);
            }
        }

        private void CreateItems(TCon conn, Pawn pawn)
        {
            // Create equipment items
            foreach (KeyValuePair<JobId, Dictionary<EquipType, List<Item>>> jobEquipment in pawn.Equipment.GetAllEquipment())
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
        
        public bool InsertIfNotExistsSpSkill(uint pawnId, CDataSpSkill spSkill)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsSpSkill(connection, pawnId, spSkill);
        }

        public bool InsertIfNotExistsSpSkill(TCon conn, uint pawnId, CDataSpSkill spSkill)
        {
            return ExecuteNonQuery(conn, SqlInsertIfNotExistsSpSkill, command =>
            {
                AddParameter(command, pawnId, spSkill);
            }) == 1;
        }
        
        public bool InsertSpSkill(uint pawnId, CDataSpSkill spSkill)
        {
            using TCon connection = OpenNewConnection();
            return InsertSpSkill(connection, pawnId, spSkill);
        }

        public bool InsertSpSkill(TCon conn, uint pawnId, CDataSpSkill spSkill)
        {
            return ExecuteNonQuery(conn, SqlInsertSpSkill, command =>
            {
                AddParameter(command, pawnId, spSkill);
            }) == 1;
        }

        public bool ReplaceSpSkill(uint pawnId, CDataSpSkill spSkill)
        {
            using TCon connection = OpenNewConnection();
            return ReplaceSpSkill(connection, pawnId, spSkill);
        }

        public bool ReplaceSpSkill(TCon conn, uint pawnId, CDataSpSkill spSkill)
        {
            Logger.Debug("Inserting SP Skill.");
            if (!InsertIfNotExistsSpSkill(conn, pawnId, spSkill))
            {
                Logger.Debug("SP skill already exists, replacing.");
                return UpdateSpSkill(conn, pawnId, spSkill);
            }
            return true;
        }

        public bool UpdateSpSkill(uint pawnId, CDataSpSkill spSkill)
        {
            using TCon connection = OpenNewConnection();
            return UpdateSpSkill(connection, pawnId, spSkill);
        }

        public bool UpdateSpSkill(TCon connection, uint pawnId, CDataSpSkill spSkill)
        {
            return ExecuteNonQuery(connection, SqlUpdateSpSkill, command =>
            {
                AddParameter(command, pawnId, spSkill);
            }) == 1;
        }
        
        public bool DeleteSpSkill(uint pawnId, byte spSkillId)
        {
            return ExecuteNonQuery(SqlDeleteSpSkill, command =>
            {
                AddParameter(command, "@pawn_id", pawnId);
                AddParameter(command, "@sp_skill_id", spSkillId);
            }) == 1;
        }
        
        public bool InsertIfNotExistsPawnReaction(uint pawnId, CDataPawnReaction pawnReaction)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsPawnReaction(connection, pawnId, pawnReaction);
        }

        public bool InsertIfNotExistsPawnReaction(TCon conn, uint pawnId, CDataPawnReaction pawnReaction)
        {
            return ExecuteNonQuery(conn, SqlInsertIfNotExistsPawnReaction, command =>
            {
                AddParameter(command, pawnId, pawnReaction);
            }) == 1;
        }
        
        public bool InsertPawnReaction(uint pawnId, CDataPawnReaction pawnReaction)
        {
            using TCon connection = OpenNewConnection();
            return InsertPawnReaction(connection, pawnId, pawnReaction);
        }

        public bool InsertPawnReaction(TCon conn, uint pawnId, CDataPawnReaction pawnReaction)
        {
            return ExecuteNonQuery(conn, SqlInsertPawnReaction, command =>
            {
                AddParameter(command, pawnId, pawnReaction);
            }) == 1;
        }

        public bool ReplacePawnReaction(uint pawnId, CDataPawnReaction pawnReaction)
        {
            using TCon connection = OpenNewConnection();
            return ReplacePawnReaction(connection, pawnId, pawnReaction);
        }

        public bool ReplacePawnReaction(TCon conn, uint pawnId, CDataPawnReaction pawnReaction)
        {
            Logger.Debug("Inserting pawn reaction.");
            if (!InsertIfNotExistsPawnReaction(conn, pawnId, pawnReaction))
            {
                Logger.Debug("Pawn reaction already exists, replacing.");
                return UpdatePawnReaction(conn, pawnId, pawnReaction);
            }
            return true;
        }

        public bool UpdatePawnReaction(uint pawnId, CDataPawnReaction pawnReaction)
        {
            using TCon connection = OpenNewConnection();
            return UpdatePawnReaction(connection, pawnId, pawnReaction);
        }

        public bool UpdatePawnReaction(TCon connection, uint pawnId, CDataPawnReaction pawnReaction)
        {
            return ExecuteNonQuery(connection, SqlUpdatePawnReaction, command =>
            {
                AddParameter(command, pawnId, pawnReaction);
            }) == 1;
        }
        
        public bool DeleteNormalSkillParam(uint pawnId, byte reactionType)
        {
            return ExecuteNonQuery(SqlDeletePawnReaction, command =>
            {
                AddParameter(command, "@pawn_id", pawnId);
                AddParameter(command, "@reaction_type", reactionType);
            }) == 1;
        }

        private Pawn ReadAllPawnData(TReader reader)
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
 
        private CDataPawnReaction ReadPawnReaction(TReader reader)
        {
            CDataPawnReaction pawnReaction = new CDataPawnReaction();
            pawnReaction.ReactionType = GetByte(reader, "reaction_type");
            pawnReaction.MotionNo = GetUInt32(reader, "motion_no");
            return pawnReaction;
        }
        
        private void AddParameter(TCom command, uint pawnId, CDataPawnReaction pawnReaction)
        {
            AddParameter(command, "pawn_id", pawnId);
            AddParameter(command, "reaction_type", pawnReaction.ReactionType);
            AddParameter(command, "motion_no", pawnReaction.MotionNo);
        }

        private CDataSpSkill ReadSpSkill(TReader reader)
        {
            CDataSpSkill spSkill = new CDataSpSkill();
            spSkill.SpSkillId = GetByte(reader, "sp_skill_id");
            spSkill.SpSkillLv = GetByte(reader, "sp_skill_lv");
            return spSkill;
        }
        
        private void AddParameter(TCom command, uint pawnId, CDataSpSkill spSkill)
        {
            AddParameter(command, "pawn_id", pawnId);
            AddParameter(command, "sp_skill_id", spSkill.SpSkillId);
            AddParameter(command, "sp_skill_lv", spSkill.SpSkillLv);
        }
    }
}
