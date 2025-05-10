using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    /* ddon_skill_augmentation_released_elements */
    protected static readonly string[] SkillAugmentationReleasedElementsFields = new[]
    {
        "character_id", "job_id", "release_id"
    };

    private readonly string SqlInsertSkillAugmentationReleasedElements =
        $"INSERT INTO \"ddon_skill_augmentation_released_elements\" ({BuildQueryField(SkillAugmentationReleasedElementsFields)}) VALUES ({BuildQueryInsert(SkillAugmentationReleasedElementsFields)});";

    private readonly string SqlSelectSkillAugmentationReleasedElements =
        $"SELECT {BuildQueryField(SkillAugmentationReleasedElementsFields)} FROM \"ddon_skill_augmentation_released_elements\" WHERE \"character_id\"=@character_id AND \"job_id\"=@job_id;";

    public override bool InsertSkillAugmentationReleasedElement(uint characterId, JobId jobId, uint releaseId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertSkillAugmentationReleasedElements, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "job_id", (byte)jobId);
                AddParameter(command, "release_id", releaseId);
            }) == 1;
        });
    }

    public override HashSet<uint> GetSkillAugmentationReleasedElements(uint characterId, JobId jobId, DbConnection? connectionIn = null)
    {
        var results = new HashSet<uint>();
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectSkillAugmentationReleasedElements, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "job_id", (byte)jobId);
            }, reader =>
            {
                while (reader.Read())
                {
                    results.Add(GetUInt32(reader, "release_id"));
                }
            });
        });
        return results;
    }
}
