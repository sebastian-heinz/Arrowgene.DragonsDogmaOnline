using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class C2SAchievementGetCategoryProgressListReq : IPacketStructure
{
    public PacketId Id => PacketId.C2S_ACHIEVEMENT_ACHIEVEMENT_GET_CATEGORY_PROGRESS_LIST_REQ;

    public byte Category { get; set; }

    public class Serializer : PacketEntitySerializer<C2SAchievementGetCategoryProgressListReq>
    {
        public override void Write(IBuffer buffer, C2SAchievementGetCategoryProgressListReq obj)
        {
            WriteByte(buffer, obj.Category);
        }

        public override C2SAchievementGetCategoryProgressListReq Read(IBuffer buffer)
        {
            C2SAchievementGetCategoryProgressListReq obj = new C2SAchievementGetCategoryProgressListReq();

            obj.Category = ReadByte(buffer);

            return obj;
        }
    }
}
