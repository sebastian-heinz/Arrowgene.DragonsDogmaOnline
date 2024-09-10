using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetEndContentsGroupReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_END_CONTENTS_GROUP_REQ;
        public uint GroupId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestGetEndContentsGroupReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetEndContentsGroupReq obj)
            {
                WriteUInt32(buffer, obj.GroupId);
            }

            public override C2SQuestGetEndContentsGroupReq Read(IBuffer buffer)
            {
                C2SQuestGetEndContentsGroupReq obj = new C2SQuestGetEndContentsGroupReq();
                obj.GroupId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
