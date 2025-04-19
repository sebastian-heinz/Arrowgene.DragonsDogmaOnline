using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    protected static readonly string[] DragonForceAugmentationFields = new[]
    {
        "character_common_id", "element_id", "page_no", "group_no", "index_no"
    };

    private readonly string SqlInsertDragonForceAugment =
        $"INSERT INTO \"ddon_dragon_force_augmentation\" ({BuildQueryField(DragonForceAugmentationFields)}) VALUES ({BuildQueryInsert(DragonForceAugmentationFields)});";

    private readonly string SqlInsertIfNotExistsDragonForceAugment =
        $"INSERT INTO \"ddon_dragon_force_augmentation\" ({BuildQueryField(DragonForceAugmentationFields)}) SELECT " +
        $"{BuildQueryInsert(DragonForceAugmentationFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_dragon_force_augmentation\" WHERE " +
        $"\"character_common_id\" = @character_common_id AND \"element_id\" = @element_id);";

    private readonly string SqlSelectAllDragonForceAugment =
        $"SELECT {BuildQueryField(DragonForceAugmentationFields)} FROM \"ddon_dragon_force_augmentation\" WHERE \"character_common_id\" = @character_common_id;";

    public override bool InsertIfNotExistsDragonForceAugmentation(uint commonId, uint elementId, uint pageNo, uint groupNo, uint indexNo, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertIfNotExistsDragonForceAugment, command =>
            {
                AddParameter(command, "character_common_id", commonId);
                AddParameter(command, "element_id", elementId);
                AddParameter(command, "page_no", pageNo);
                AddParameter(command, "group_no", groupNo);
                AddParameter(command, "index_no", indexNo);
            }) == 1;
        });
    }

    public override List<CDataReleaseOrbElement> SelectOrbReleaseElementFromDragonForceAugmentation(uint commonId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            List<CDataReleaseOrbElement> results = new();

            ExecuteReader(connection, SqlSelectAllDragonForceAugment,
                command => { AddParameter(command, "@character_common_id", commonId); }, reader =>
                {
                    while (reader.Read())
                    {
                        CDataReleaseOrbElement ReleaseOrbElement = ReadReleaseOrbElement(reader);
                        results.Add(ReleaseOrbElement);
                    }
                });

            return results;
        });
    }

    private CDataReleaseOrbElement ReadReleaseOrbElement(DbDataReader reader)
    {
        CDataReleaseOrbElement obj = new();
        obj.ElementId = GetUInt32(reader, "element_id");
        obj.PageNo = GetByte(reader, "page_no");
        obj.GroupNo = GetByte(reader, "group_no");
        obj.Index = GetByte(reader, "index_no");
        return obj;
    }

    private void AddParameter(DbCommand command, uint commonId, CDataReleaseOrbElement obj)
    {
        AddParameter(command, "character_common_id", commonId);
        AddParameter(command, "element_id", obj.ElementId);
        AddParameter(command, "page_no", obj.PageNo);
        AddParameter(command, "group_no", obj.GroupNo);
        AddParameter(command, "index_no", obj.Index);
    }
}
