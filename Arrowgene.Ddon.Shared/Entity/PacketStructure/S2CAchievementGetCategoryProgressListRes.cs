using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class S2CAchievementGetCategoryProgressListRes : ServerResponse
{
    public override PacketId Id => PacketId.S2C_ACHIEVEMENT_ACHIEVEMENT_GET_CATEGORY_PROGRESS_LIST_RES;

    public List<CDataAchievementProgress> AchievementProgressList { get; set; } = new();

    public class Serializer : PacketEntitySerializer<S2CAchievementGetCategoryProgressListRes>
    {
        public override void Write(IBuffer buffer, S2CAchievementGetCategoryProgressListRes obj)
        {
            WriteServerResponse(buffer, obj);

            WriteEntityList(buffer, obj.AchievementProgressList);
        }

        public override S2CAchievementGetCategoryProgressListRes Read(IBuffer buffer)
        {
            S2CAchievementGetCategoryProgressListRes obj = new S2CAchievementGetCategoryProgressListRes();

            ReadServerResponse(buffer, obj);

            obj.AchievementProgressList = ReadEntityList<CDataAchievementProgress>(buffer);

            return obj;
        }
    }
}
