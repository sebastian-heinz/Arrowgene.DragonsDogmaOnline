using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class S2CAchievementGetProgressListRes : ServerResponse
{
    public override PacketId Id => PacketId.S2C_ACHIEVEMENT_ACHIEVEMENT_GET_PROGRESS_LIST_RES;

    public List<CDataAchievementProgress> AchievementProgressList { get; set; } = new();

    public class Serializer : PacketEntitySerializer<S2CAchievementGetProgressListRes>
    {
        public override void Write(IBuffer buffer, S2CAchievementGetProgressListRes obj)
        {
            WriteServerResponse(buffer, obj);

            WriteEntityList(buffer, obj.AchievementProgressList);
        }

        public override S2CAchievementGetProgressListRes Read(IBuffer buffer)
        {
            S2CAchievementGetProgressListRes obj = new S2CAchievementGetProgressListRes();

            ReadServerResponse(buffer, obj);

            obj.AchievementProgressList = ReadEntityList<CDataAchievementProgress>(buffer);

            return obj;
        }
    }
}
