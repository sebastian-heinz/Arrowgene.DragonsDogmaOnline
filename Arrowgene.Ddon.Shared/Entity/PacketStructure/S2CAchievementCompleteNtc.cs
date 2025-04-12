using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

/// <summary>
///     This NTC has to be sent whenever any interaction concludes an achievement.
///     It makes the client print to console in a regular blue font the following: "Earned achievement "[achievement-name]"."
/// </summary>
public class S2CAchievementCompleteNtc : IPacketStructure
{
    public PacketId Id => PacketId.S2C_ACHIEVEMENT_ACHIEVEMENT_COMPLETE_NTC;

    public List<CDataCommonU32> AchievementIdList { get; set; } = new();

    public class Serializer : PacketEntitySerializer<S2CAchievementCompleteNtc>
    {
        public override void Write(IBuffer buffer, S2CAchievementCompleteNtc obj)
        {
            WriteEntityList(buffer, obj.AchievementIdList);
        }

        public override S2CAchievementCompleteNtc Read(IBuffer buffer)
        {
            S2CAchievementCompleteNtc obj = new S2CAchievementCompleteNtc();

            obj.AchievementIdList = ReadEntityList<CDataCommonU32>(buffer);

            return obj;
        }
    }
}
