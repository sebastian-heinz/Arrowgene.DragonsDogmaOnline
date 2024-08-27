using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

/// <summary>
///     This NTC has to be sent whenever any interaction concludes an achievement.
///     It makes the client print to console in a regular blue font the following: "Earned achievement "[achievement-name]"."
/// </summary>
public class C2SAchievementCompleteNtc : IPacketStructure
{
    public PacketId Id => PacketId.S2C_ACHIEVEMENT_ACHIEVEMENT_COMPLETE_NTC;

    public List<CDataCommonU32> AchievementIdList { get; set; } = new();

    public class Serializer : PacketEntitySerializer<C2SAchievementCompleteNtc>
    {
        public override void Write(IBuffer buffer, C2SAchievementCompleteNtc obj)
        {
            WriteEntityList(buffer, obj.AchievementIdList);
        }

        public override C2SAchievementCompleteNtc Read(IBuffer buffer)
        {
            C2SAchievementCompleteNtc obj = new C2SAchievementCompleteNtc();

            obj.AchievementIdList = ReadEntityList<CDataCommonU32>(buffer);

            return obj;
        }
    }
}
