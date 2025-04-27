using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string SqlDeletePawn =
        "DELETE FROM \"ddon_character_common\" WHERE EXISTS (SELECT 1 FROM \"ddon_pawn\" WHERE \"ddon_character_common\".\"character_common_id\"=\"ddon_pawn\".\"character_common_id\" AND \"ddon_pawn\".\"pawn_id\"=@pawn_id)";

    private const string SqlSelectOfficialPawns =
        @"SELECT * FROM ddon_pawn WHERE is_official_pawn=true;";

    private const string SqlSelectAllPlayerPawns =
        @"SELECT * FROM ddon_pawn WHERE is_official_pawn=false LIMIT @limit;";

    private const string SqlSelectPawnOwnerId =
        "SELECT * FROM ddon_pawn WHERE \"pawn_id\" = @pawn_id;";

    private const string SqlSelectRegisteredPawns =
        "SELECT \"ddon_pawn\".\"name\", \"ddon_pawn\".\"pawn_id\", \"active_job\".\"job\", \"active_job\".\"lv\", \"ddon_pawn\".\"craft_rank\", \"ddon_edit_info\".\"sex\" "
        + "FROM \"ddon_pawn\" "
        + "LEFT JOIN \"ddon_character_common\" ON \"ddon_character_common\".\"character_common_id\" = \"ddon_pawn\".\"character_common_id\" "
        + "LEFT JOIN \"ddon_edit_info\" ON \"ddon_edit_info\".\"character_common_id\" = \"ddon_pawn\".\"character_common_id\" "
        + "LEFT JOIN \"ddon_character_job_data\" \"active_job\" ON \"active_job\".\"character_common_id\" = \"ddon_pawn\".\"character_common_id\" AND \"active_job\".\"job\" = \"ddon_character_common\".\"job\" "
        + "LEFT JOIN \"ddon_character\" \"owner\" ON \"owner\".\"character_id\" = \"ddon_pawn\".\"character_id\" "
        + "WHERE \"ddon_pawn\".\"character_id\" != @character_id "
        + "AND (@dont_filter_by_owner_name OR ( "
        + " LOWER(\"owner\".\"first_name\") LIKE LOWER(@first_name) "
        + " AND LOWER(\"owner\".\"last_name\") LIKE LOWER(@last_name))) "
        + "AND (@dont_filter_by_pawn_name OR LOWER(\"ddon_pawn\".\"name\") LIKE LOWER(@pawn_name)) "
        + "AND (@dont_filter_by_sex OR \"ddon_edit_info\".\"sex\" = @sex) "
        + "AND (@dont_filter_by_vocation OR \"active_job\".\"lv\" BETWEEN @vocation_min AND @vocation_max) "
        + "AND (@dont_filter_by_craft_rank OR \"ddon_pawn\".\"craft_rank\" BETWEEN @craft_rank_min AND @craft_rank_max) "
        // TODO: Filter by CraftSkillList, LevelMin/LevelMax, ItemRankMin/ItemRankMax, IsFriend, IsClan and DragonAbilitiesList
        + "AND (@dont_filter_by_jobs OR ((1 << \"active_job\".\"job\") & @job_bitfield) != 0) "
        + "LIMIT 250;"; // Any more and we overflow the client UI's button IDs (It uses a byte)

    private const string SqlSelectClanPawns =
        @"SELECT ""ddon_pawn"".* FROM ddon_pawn 
                INNER JOIN ""ddon_clan_membership"" ON ""ddon_pawn"".""character_id"" = ""ddon_clan_membership"".""character_id"" 
                WHERE ""clan_id"" = @clan_id AND ""ddon_pawn"".""character_id"" <> @character_id 
                LIMIT @limit;";

    private const string SqlDeletePawnReaction =
        "DELETE FROM \"ddon_pawn_reaction\" WHERE \"pawn_id\"=@pawn_id AND \"reaction_type\"=@reaction_type;";

    private const string SqlDeleteSpSkill =
        "DELETE FROM \"ddon_sp_skill\" WHERE \"pawn_id\"=@pawn_id AND \"job\"=@job AND \"sp_skill_id\"=@sp_skill_id;";

    private const string SqlDeleteSpSkillByPawn =
        "DELETE FROM \"ddon_sp_skill\" WHERE \"pawn_id\"=@pawn_id;";

    private static readonly string[] PawnFields = new[]
    {
        "character_common_id",
        "character_id",
        "name",
        "hm_type",
        "pawn_type",
        "pawn_state",
        "training_points",
        "available_training",
        "craft_exp",
        "craft_rank",
        "craft_rank_limit",
        "craft_points",
        "production_speed_level",
        "equipment_enhancement_level",
        "equipment_quality_level",
        "consumable_quantity_level",
        "cost_performance_level",
        "is_official_pawn"
    };

    protected static readonly string[] CDataPawnReactionFields = new[]
    {
        "pawn_id",
        "reaction_type",
        "motion_no"
    };

    protected static readonly string[] CDataSpSkillFields = new[]
    {
        "pawn_id",
        "job",
        "sp_skill_id",
        "sp_skill_lv"
    };

    private static readonly string SqlUpdatePawn =
        $"UPDATE \"ddon_pawn\" SET {BuildQueryUpdate(PawnFields)} WHERE \"pawn_id\" = @pawn_id;";

    private static readonly string SqlSelectPawn =
        $"SELECT \"ddon_pawn\".\"pawn_id\", {BuildQueryField(PawnFields)} FROM \"ddon_pawn\" WHERE \"pawn_id\" = @pawn_id;";

    private static readonly string SqlSelectPawnsByCharacterId =
        $"SELECT \"ddon_pawn\".\"pawn_id\", {BuildQueryField(PawnFields)} FROM \"ddon_pawn\" WHERE \"character_id\" = @character_id;";

    private static readonly string SqlUpdatePawnReaction =
        $"UPDATE \"ddon_pawn_reaction\" SET {BuildQueryUpdate(CDataPawnReactionFields)} WHERE \"pawn_id\" = @pawn_id AND \"reaction_type\"=@reaction_type;";

    private static readonly string SqlSelectPawnReactionByPawnId =
        $"SELECT {BuildQueryField(CDataPawnReactionFields)} FROM \"ddon_pawn_reaction\" WHERE \"pawn_id\" = @pawn_id;";

    private static readonly string SqlUpdateSpSkill =
        $"UPDATE \"ddon_sp_skill\" SET {BuildQueryUpdate(CDataSpSkillFields)} WHERE \"pawn_id\" = @pawn_id AND \"job\"=@job AND \"sp_skill_id\"=@sp_skill_id;";

    private static readonly string SqlSelectSpSkillsByPawnId =
        $"SELECT {BuildQueryField(CDataSpSkillFields)} FROM \"ddon_sp_skill\" WHERE \"pawn_id\" = @pawn_id;";

    private readonly string SqlInsertIfNotExistsPawnReaction =
        $"INSERT INTO \"ddon_pawn_reaction\" ({BuildQueryField(CDataPawnReactionFields)}) SELECT {BuildQueryInsert(CDataPawnReactionFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_pawn_reaction\" WHERE \"pawn_id\"=@pawn_id AND \"reaction_type\"=@reaction_type);";

    private readonly string SqlInsertIfNotExistsSpSkill =
        $"INSERT INTO \"ddon_sp_skill\" ({BuildQueryField(CDataSpSkillFields)}) SELECT {BuildQueryInsert(CDataSpSkillFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_sp_skill\" WHERE \"pawn_id\" = @pawn_id AND \"job\"=@job AND \"sp_skill_id\"=@sp_skill_id);";

    private readonly string SqlInsertPawn =
        $"INSERT INTO \"ddon_pawn\" ({BuildQueryField(PawnFields)}) VALUES ({BuildQueryInsert(PawnFields)});";

    private readonly string SqlInsertPawnReaction =
        $"INSERT INTO \"ddon_pawn_reaction\" ({BuildQueryField(CDataPawnReactionFields)}) VALUES ({BuildQueryInsert(CDataPawnReactionFields)});";

    private readonly string SqlInsertSpSkill =
        $"INSERT INTO \"ddon_sp_skill\" ({BuildQueryField(CDataSpSkillFields)}) VALUES ({BuildQueryInsert(CDataSpSkillFields)});";

    private readonly string SqlSelectAllPawnData =
        $"SELECT \"ddon_pawn\".\"pawn_id\", {BuildQueryField("ddon_pawn", PawnFields)}, \"ddon_character_common\".\"character_common_id\", {BuildQueryField("ddon_character_common", CharacterCommonFields)}, {BuildQueryField("ddon_edit_info", CDataEditInfoFields)}, {BuildQueryField("ddon_status_info", CDataStatusInfoFields)}"
        + "FROM \"ddon_pawn\" "
        + "LEFT JOIN \"ddon_character_common\" ON \"ddon_character_common\".\"character_common_id\" = \"ddon_pawn\".\"character_common_id\" "
        + "LEFT JOIN \"ddon_edit_info\" ON \"ddon_edit_info\".\"character_common_id\" = \"ddon_pawn\".\"character_common_id\" "
        + "LEFT JOIN \"ddon_status_info\" ON \"ddon_status_info\".\"character_common_id\" = \"ddon_pawn\".\"character_common_id\" "
        + "WHERE \"ddon_pawn\".\"pawn_id\" = @pawn_id";

    private readonly string SqlSelectAllPawnsDataByCharacterId =
        $"SELECT \"ddon_pawn\".\"pawn_id\", {BuildQueryField("ddon_pawn", PawnFields)}, \"ddon_character_common\".\"character_common_id\", {BuildQueryField("ddon_character_common", CharacterCommonFields)}, {BuildQueryField("ddon_edit_info", CDataEditInfoFields)}, {BuildQueryField("ddon_status_info", CDataStatusInfoFields)}"
        + "FROM \"ddon_pawn\" "
        + "LEFT JOIN \"ddon_character_common\" ON \"ddon_character_common\".\"character_common_id\" = \"ddon_pawn\".\"character_common_id\" "
        + "LEFT JOIN \"ddon_edit_info\" ON \"ddon_edit_info\".\"character_common_id\" = \"ddon_pawn\".\"character_common_id\" "
        + "LEFT JOIN \"ddon_status_info\" ON \"ddon_status_info\".\"character_common_id\" = \"ddon_pawn\".\"character_common_id\" "
        + "WHERE \"character_id\" = @character_id "
        + "ORDER BY \"pawn_id\"";

    public override bool CreatePawn(Pawn pawn)
    {
        return ExecuteInTransaction(conn =>
        {
            ExecuteNonQuery(
                conn,
                SqlInsertCharacterCommon,
                command => { AddParameter(command, pawn); },
                out long commonId
            );
            pawn.CommonId = (uint)commonId;

            ExecuteNonQuery(
                conn,
                SqlInsertPawn,
                command => { AddParameter(command, pawn); },
                out long pawnId
            );
            pawn.PawnId = (uint)pawnId;

            ExecuteNonQuery(
                conn,
                SqlInsertEditInfo,
                command => { AddParameter(command, pawn); }
            );
            ExecuteNonQuery(
                conn,
                SqlInsertStatusInfo,
                command => { AddParameter(command, pawn); }
            );

            StorePawnData(conn, pawn);
        });
    }

    public override Pawn SelectPawn(uint pawnId)
    {
        using DbConnection connection = OpenNewConnection();
        return SelectPawn(connection, pawnId);
    }

    public override Pawn SelectPawn(DbConnection connection, uint pawnId)
    {
        Pawn pawn = null;
        ExecuteReader(
            connection,
            SqlSelectAllPawnData,
            command => { AddParameter(command, "@pawn_id", pawnId); },
            reader =>
            {
                if (reader.Read())
                {
                    pawn = ReadAllPawnData(reader);
                    using DbConnection connection2 = OpenNewConnection();
                    QueryPawnData(connection2, pawn);
                }
            }
        );
        return pawn;
    }

    public override List<uint> SelectAllPlayerPawns(uint limit = 100)
    {
        using DbConnection connection = OpenNewConnection();
        return SelectAllPlayerPawns(connection, limit);
    }

    public override List<uint> SelectAllPlayerPawns(DbConnection connection, uint limit = 100)
    {
        List<uint> pawns = new();
        ExecuteReader(
            connection,
            SqlSelectAllPlayerPawns,
            command => { AddParameter(command, "@limit", limit); },
            reader =>
            {
                while (reader.Read())
                {
                    uint pawnId = GetUInt32(reader, "pawn_id");
                    pawns.Add(pawnId);
                }
            }
        );
        return pawns;
    }

    public override List<uint> SelectClanPawns(uint clanId, uint characterId = 0, uint limit = 100, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            List<uint> pawns = new();
            ExecuteReader(
                connection,
                SqlSelectClanPawns,
                command =>
                {
                    AddParameter(command, "@limit", limit);
                    AddParameter(command, "@clan_id", clanId);
                    AddParameter(command, "@character_id", characterId);
                },
                reader =>
                {
                    while (reader.Read())
                    {
                        uint pawnId = GetUInt32(reader, "pawn_id");
                        pawns.Add(pawnId);
                    }
                }
            );
            return pawns;
        });
    }

    public override List<CDataRegisterdPawnList> SelectRegisteredPawns(
        Character searchingCharacter,
        CDataPawnSearchParameter searchParams
    )
    {
        using DbConnection conn = OpenNewConnection();
        return SelectRegisteredPawns(conn, searchingCharacter, searchParams);
    }

    public override List<CDataRegisterdPawnList> SelectRegisteredPawns(
        DbConnection conn,
        Character searchingCharacter,
        CDataPawnSearchParameter searchParams
    )
    {
        List<CDataRegisterdPawnList> registeredPawns = new();
        ExecuteReader(
            conn,
            SqlSelectRegisteredPawns,
            command => { AddParameter(command, searchingCharacter, searchParams); },
            reader =>
            {
                while (reader.Read())
                {
                    uint lv = GetUInt32(reader, "lv");
                    registeredPawns.Add(
                        new CDataRegisterdPawnList
                        {
                            Name = GetString(reader, "name"),
                            PawnId = GetUInt32(reader, "pawn_id"),
                            RentalCost = lv * 10,
                            Sex = GetByte(reader, "sex"),
                            Updated = 0, // TODO: Updated
                            PawnListData = new CDataPawnListData
                            {
                                Job = (JobId)GetByte(reader, "job"),
                                Level = lv,
                                CraftRank = GetUInt32(reader, "craft_rank")
                            }
                        }
                    );
                }
            }
        );
        return registeredPawns;
    }

    public override uint GetPawnOwnerCharacterId(uint pawnId, DbConnection? connectionIn = null)
    {
        uint ownerCharacterId = 0;
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(
                connection,
                SqlSelectPawnOwnerId,
                command => { AddParameter(command, "@pawn_id", pawnId); },
                reader =>
                {
                    if (reader.Read()) ownerCharacterId = GetUInt32(reader, "character_id");
                }
            );
        });

        return ownerCharacterId;
    }

    public override List<Pawn> SelectPawnsByCharacterId(uint characterId, DbConnection? connectionIn = null)
    {
        List<Pawn> pawns = new();
        ExecuteQuerySafe(connectionIn, conn =>
        {
            ExecuteReader(
                conn,
                SqlSelectAllPawnsDataByCharacterId,
                command => { AddParameter(command, "@character_id", characterId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        Pawn pawn = ReadAllPawnData(reader);
                        pawns.Add(pawn);
                    }
                });

            foreach (Pawn pawn in pawns) QueryPawnData(conn, pawn);
        });
        return pawns;
    }

    public override List<uint> SelectOfficialPawns()
    {
        using DbConnection connection = OpenNewConnection();
        return SelectOfficialPawns(connection);
    }

    public List<uint> SelectOfficialPawns(DbConnection connection)
    {
        List<uint> pawns = new();
        ExecuteReader(
            connection,
            SqlSelectOfficialPawns,
            command => { },
            reader =>
            {
                while (reader.Read())
                {
                    uint pawnId = GetUInt32(reader, "pawn_id");
                    pawns.Add(pawnId);
                }
            }
        );
        return pawns;
    }

    public override bool DeletePawn(uint pawnId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(
                connection,
                SqlDeletePawn,
                command => { AddParameter(command, "@pawn_id", pawnId); }
            );
        }) > NoRowsAffected;
    }

    public override bool UpdatePawnBaseInfo(Pawn pawn, DbConnection? connectionIn = null)
    {
        // TODO: Make this less super dangerous.
        // This has a potential for pawn kidnapping by overwriting the pawn's characterId.
        // Until we better implement rental pawns, this should ONLY write for main pawns.

        if (pawn.PawnType != PawnType.Main) return false;

        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(
                connection,
                SqlUpdatePawn,
                command => { AddParameter(command, pawn); }
            );
        }) > NoRowsAffected;
    }

    private void QueryPawnData(DbConnection conn, Pawn pawn)
    {
        QueryCharacterCommonData(conn, pawn);

        ExecuteReader(
            conn,
            SqlSelectPawnReactionByPawnId,
            command => { AddParameter(command, "@pawn_id", pawn.PawnId); },
            reader =>
            {
                while (reader.Read()) pawn.PawnReactionList.Add(ReadPawnReaction(reader));
            }
        );

        ExecuteReader(
            conn,
            SqlSelectSpSkillsByPawnId,
            command => { AddParameter(command, "@pawn_id", pawn.PawnId); },
            reader =>
            {
                while (reader.Read())
                {
                    JobId job = (JobId)GetByte(reader, "job");
                    if (!pawn.SpSkills.ContainsKey(job)) pawn.SpSkills.Add(job, new List<CDataSpSkill>());

                    pawn.SpSkills[job].Add(ReadSpSkill(reader));
                }
            }
        );

        ExecuteReader(
            conn,
            SqlSelectPawnTrainingStatusByPawn,
            command => { AddParameter(command, "@pawn_id", pawn.PawnId); },
            reader =>
            {
                while (reader.Read())
                {
                    JobId job = (JobId)GetByte(reader, "job");
                    byte[] trainingStatus = GetBytes(reader, "training_status", 64);
                    pawn.TrainingStatus.Add(job, trainingStatus);
                }
            }
        );

        pawn.PartnerPawnData = GetPartnerPawnRecord(pawn.CharacterId, pawn.PawnId, conn) ?? new PartnerPawnData();
    }

    private void StorePawnData(DbConnection conn, Pawn pawn)
    {
        StoreCharacterCommonData(conn, pawn);

        foreach (CDataPawnReaction pawnReaction in pawn.PawnReactionList) ReplacePawnReaction(pawn.PawnId, pawnReaction, conn);

        DeleteSpSkills(conn, pawn.PawnId);
        foreach (KeyValuePair<JobId, List<CDataSpSkill>> jobAndSpSkills in pawn.SpSkills)
        {
            JobId job = jobAndSpSkills.Key;
            foreach (CDataSpSkill spSkill in jobAndSpSkills.Value) ReplaceSpSkill(conn, pawn.PawnId, job, spSkill);
        }

        foreach ((JobId job, byte[] trainingStatus) in pawn.TrainingStatus) ReplacePawnTrainingStatus(pawn.PawnId, job, trainingStatus, conn);
    }

    public bool InsertIfNotExistsSpSkill(uint pawnId, JobId job, CDataSpSkill spSkill)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertIfNotExistsSpSkill(connection, pawnId, job, spSkill);
    }

    public bool InsertIfNotExistsSpSkill(
        DbConnection conn,
        uint pawnId,
        JobId job,
        CDataSpSkill spSkill
    )
    {
        return ExecuteNonQuery(
            conn,
            SqlInsertIfNotExistsSpSkill,
            command => { AddParameter(command, pawnId, job, spSkill); }
        ) == 1;
    }

    public override bool InsertSpSkill(uint pawnId, JobId job, CDataSpSkill spSkill)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertSpSkill(connection, pawnId, job, spSkill);
    }

    public bool InsertSpSkill(DbConnection conn, uint pawnId, JobId job, CDataSpSkill spSkill)
    {
        return ExecuteNonQuery(
            conn,
            SqlInsertSpSkill,
            command => { AddParameter(command, pawnId, job, spSkill); }
        ) == 1;
    }

    public bool ReplaceSpSkill(uint pawnId, JobId job, CDataSpSkill spSkill)
    {
        using DbConnection connection = OpenNewConnection();
        return ReplaceSpSkill(connection, pawnId, job, spSkill);
    }

    public bool ReplaceSpSkill(DbConnection conn, uint pawnId, JobId job, CDataSpSkill spSkill)
    {
        Logger.Debug("Inserting SP Skill.");
        if (!InsertIfNotExistsSpSkill(conn, pawnId, job, spSkill))
        {
            Logger.Debug("SP skill already exists, replacing.");
            return UpdateSpSkill(conn, pawnId, job, spSkill);
        }

        return true;
    }

    public bool UpdateSpSkill(uint pawnId, JobId job, CDataSpSkill spSkill)
    {
        using DbConnection connection = OpenNewConnection();
        return UpdateSpSkill(connection, pawnId, job, spSkill);
    }

    public bool UpdateSpSkill(DbConnection connection, uint pawnId, JobId job, CDataSpSkill spSkill)
    {
        return ExecuteNonQuery(
            connection,
            SqlUpdateSpSkill,
            command => { AddParameter(command, pawnId, job, spSkill); }
        ) == 1;
    }

    public override bool DeleteSpSkill(uint pawnId, JobId job, byte spSkillId)
    {
        using DbConnection connection = OpenNewConnection();
        return DeleteSpSkill(connection, pawnId, job, spSkillId);
    }

    public bool DeleteSpSkill(DbConnection conn, uint pawnId, JobId job, byte spSkillId)
    {
        return ExecuteNonQuery(
            conn,
            SqlDeleteSpSkill,
            command =>
            {
                AddParameter(command, "@pawn_id", pawnId);
                AddParameter(command, "@job", (byte)job);
                AddParameter(command, "@sp_skill_id", spSkillId);
            }
        ) == 1;
    }

    public bool DeleteSpSkills(uint pawnId)
    {
        using DbConnection connection = OpenNewConnection();
        return DeleteSpSkills(connection, pawnId);
    }

    public bool DeleteSpSkills(DbConnection conn, uint pawnId)
    {
        return ExecuteNonQuery(
            conn,
            SqlDeleteSpSkillByPawn,
            command => { AddParameter(command, "@pawn_id", pawnId); }
        ) == 1;
    }

    public bool InsertIfNotExistsPawnReaction(uint pawnId, CDataPawnReaction pawnReaction)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertIfNotExistsPawnReaction(connection, pawnId, pawnReaction);
    }

    public bool InsertIfNotExistsPawnReaction(
        DbConnection conn,
        uint pawnId,
        CDataPawnReaction pawnReaction
    )
    {
        return ExecuteNonQuery(
            conn,
            SqlInsertIfNotExistsPawnReaction,
            command => { AddParameter(command, pawnId, pawnReaction); }
        ) == 1;
    }

    public bool InsertPawnReaction(uint pawnId, CDataPawnReaction pawnReaction)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertPawnReaction(connection, pawnId, pawnReaction);
    }

    public bool InsertPawnReaction(DbConnection conn, uint pawnId, CDataPawnReaction pawnReaction)
    {
        return ExecuteNonQuery(
            conn,
            SqlInsertPawnReaction,
            command => { AddParameter(command, pawnId, pawnReaction); }
        ) == 1;
    }

    public override bool ReplacePawnReaction(uint pawnId, CDataPawnReaction pawnReaction, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            if (!InsertIfNotExistsPawnReaction(connection, pawnId, pawnReaction)) return UpdatePawnReaction(connection, pawnId, pawnReaction);
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }

        return true;
    }

    public bool UpdatePawnReaction(uint pawnId, CDataPawnReaction pawnReaction)
    {
        using DbConnection connection = OpenNewConnection();
        return UpdatePawnReaction(connection, pawnId, pawnReaction);
    }

    public bool UpdatePawnReaction(DbConnection connection, uint pawnId, CDataPawnReaction pawnReaction)
    {
        return ExecuteNonQuery(
            connection,
            SqlUpdatePawnReaction,
            command => { AddParameter(command, pawnId, pawnReaction); }
        ) == 1;
    }

    public bool DeleteNormalSkillParam(uint pawnId, byte reactionType)
    {
        return ExecuteNonQuery(
            SqlDeletePawnReaction,
            command =>
            {
                AddParameter(command, "@pawn_id", pawnId);
                AddParameter(command, "@reaction_type", reactionType);
            }
        ) == 1;
    }

    private Pawn ReadAllPawnData(DbDataReader reader)
    {
        Pawn pawn = new();

        ReadAllCharacterCommonData(reader, pawn);

        pawn.PawnId = GetUInt32(reader, "pawn_id");
        pawn.CharacterId = GetUInt32(reader, "character_id");
        pawn.Name = GetString(reader, "name");
        pawn.HmType = GetByte(reader, "hm_type");
        pawn.PawnType = (PawnType)GetByte(reader, "pawn_type");
        pawn.PawnState = (PawnState)GetByte(reader, "pawn_state");
        pawn.TrainingPoints = GetUInt32(reader, "training_points");
        pawn.AvailableTraining = GetUInt32(reader, "available_training");
        pawn.IsOfficialPawn = GetBoolean(reader, "is_official_pawn");

        pawn.CraftData.CraftExp = GetUInt32(reader, "craft_exp");
        pawn.CraftData.CraftRank = GetUInt32(reader, "craft_rank");
        pawn.CraftData.CraftRankLimit = GetUInt32(reader, "craft_rank_limit");
        pawn.CraftData.CraftPoint = GetUInt32(reader, "craft_points");

        pawn
            .CraftData.PawnCraftSkillList.Find(skill =>
                skill.Type == CraftSkillType.ProductionSpeed
            )
            .Level = GetUInt32(reader, "production_speed_level");
        pawn
            .CraftData.PawnCraftSkillList.Find(skill =>
                skill.Type == CraftSkillType.EquipmentEnhancement
            )
            .Level = GetUInt32(reader, "equipment_enhancement_level");
        pawn
            .CraftData.PawnCraftSkillList.Find(skill =>
                skill.Type == CraftSkillType.EquipmentQuality
            )
            .Level = GetUInt32(reader, "equipment_quality_level");
        pawn
            .CraftData.PawnCraftSkillList.Find(skill =>
                skill.Type == CraftSkillType.ConsumableQuantity
            )
            .Level = GetUInt32(reader, "consumable_quantity_level");
        pawn
            .CraftData.PawnCraftSkillList.Find(skill =>
                skill.Type == CraftSkillType.CostPerformance
            )
            .Level = GetUInt32(reader, "cost_performance_level");

        return pawn;
    }

    private void AddParameter(DbCommand command, Pawn pawn)
    {
        AddParameter(command, (CharacterCommon)pawn);
        // PawnFields
        AddParameter(command, "@pawn_id", pawn.PawnId);
        AddParameter(command, "@character_id", pawn.CharacterId);
        AddParameter(command, "@name", pawn.Name);
        AddParameter(command, "@hm_type", pawn.HmType);
        AddParameter(command, "@pawn_type", (byte)pawn.PawnType);
        AddParameter(command, "@pawn_state", (byte)pawn.PawnState);
        AddParameter(command, "@training_points", pawn.TrainingPoints);
        AddParameter(command, "@available_training", pawn.AvailableTraining);
        AddParameter(command, "@is_official_pawn", false);

        AddParameter(command, "@craft_exp", pawn.CraftData.CraftExp);
        AddParameter(command, "@craft_rank", pawn.CraftData.CraftRank);
        AddParameter(command, "@craft_rank_limit", pawn.CraftData.CraftRankLimit);
        AddParameter(command, "@craft_points", pawn.CraftData.CraftPoint);

        AddParameter(
            command,
            "@production_speed_level",
            pawn.CraftData.PawnCraftSkillList.Find(skill =>
                skill.Type == CraftSkillType.ProductionSpeed
            ).Level
        );
        AddParameter(
            command,
            "@equipment_enhancement_level",
            pawn.CraftData.PawnCraftSkillList.Find(skill =>
                skill.Type == CraftSkillType.EquipmentEnhancement
            ).Level
        );
        AddParameter(
            command,
            "@equipment_quality_level",
            pawn.CraftData.PawnCraftSkillList.Find(skill =>
                skill.Type == CraftSkillType.EquipmentQuality
            ).Level
        );
        AddParameter(
            command,
            "@consumable_quantity_level",
            pawn.CraftData.PawnCraftSkillList.Find(skill =>
                skill.Type == CraftSkillType.ConsumableQuantity
            ).Level
        );
        AddParameter(
            command,
            "@cost_performance_level",
            pawn.CraftData.PawnCraftSkillList.Find(skill =>
                skill.Type == CraftSkillType.CostPerformance
            ).Level
        );
    }

    private CDataPawnReaction ReadPawnReaction(DbDataReader reader)
    {
        CDataPawnReaction pawnReaction = new();
        pawnReaction.ReactionType = GetByte(reader, "reaction_type");
        pawnReaction.MotionNo = GetUInt32(reader, "motion_no");
        return pawnReaction;
    }

    private void AddParameter(DbCommand command, uint pawnId, CDataPawnReaction pawnReaction)
    {
        AddParameter(command, "pawn_id", pawnId);
        AddParameter(command, "reaction_type", pawnReaction.ReactionType);
        AddParameter(command, "motion_no", pawnReaction.MotionNo);
    }

    private CDataSpSkill ReadSpSkill(DbDataReader reader)
    {
        CDataSpSkill spSkill = new();
        spSkill.SpSkillId = GetByte(reader, "sp_skill_id");
        spSkill.SpSkillLv = GetByte(reader, "sp_skill_lv");
        return spSkill;
    }

    private void AddParameter(DbCommand command, uint pawnId, JobId job, CDataSpSkill spSkill)
    {
        AddParameter(command, "pawn_id", pawnId);
        AddParameter(command, "job", (byte)job);
        AddParameter(command, "sp_skill_id", spSkill.SpSkillId);
        AddParameter(command, "sp_skill_lv", spSkill.SpSkillLv);
    }

    private void AddParameter(
        DbCommand command,
        Character searchingCharacter,
        CDataPawnSearchParameter searchParams
    )
    {
        AddParameter(command, "character_id", searchingCharacter.CharacterId);
        AddParameter(
            command,
            "dont_filter_by_owner_name",
            searchParams.OwnerCharacterName.FirstName.Length == 0
            && searchParams.OwnerCharacterName.LastName.Length == 0
        );
        AddParameter(command, "first_name", "%" + searchParams.OwnerCharacterName.FirstName + "%");
        AddParameter(command, "last_name", "%" + searchParams.OwnerCharacterName.LastName + "%");
        AddParameter(command, "dont_filter_by_pawn_name", searchParams.PawnName.Length == 0);
        AddParameter(command, "pawn_name", "%" + searchParams.PawnName + "%");
        AddParameter(command, "dont_filter_by_sex", searchParams.Sex == PawnSex.Any);
        AddParameter(command, "sex", (byte)searchParams.Sex);
        AddParameter(
            command,
            "dont_filter_by_vocation",
            searchParams.CharacterParam.VocationMin == 0
            && searchParams.CharacterParam.VocationMax == 0
        );
        AddParameter(command, "vocation_min", searchParams.CharacterParam.VocationMin);
        AddParameter(command, "vocation_max", searchParams.CharacterParam.VocationMax);
        AddParameter(
            command,
            "dont_filter_by_craft_rank",
            searchParams.CraftRankMin == 0 && searchParams.CraftRankMax == 0
        );
        AddParameter(command, "craft_rank_min", searchParams.CraftRankMin);
        AddParameter(command, "craft_rank_max", searchParams.CraftRankMax);
        AddParameter(command, "dont_filter_by_jobs", searchParams.CharacterParam.Job == 0);
        AddParameter(command, "job_bitfield", searchParams.CharacterParam.Job);
    }
}
