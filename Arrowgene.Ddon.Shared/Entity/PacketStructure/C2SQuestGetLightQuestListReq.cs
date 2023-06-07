using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetLightQuestListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_LIGHT_QUEST_LIST_REQ;

        public uint BaseId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestGetLightQuestListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetLightQuestListReq obj)
            {
                WriteUInt32(buffer, obj.BaseId);
            }

            public override C2SQuestGetLightQuestListReq Read(IBuffer buffer)
            {
                C2SQuestGetLightQuestListReq obj = new C2SQuestGetLightQuestListReq();
                obj.BaseId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}