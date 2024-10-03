using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestGetMobHuntQuestListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_GET_MOB_HUNT_QUEST_LIST_REQ;

        public uint Unk0 {  get; set; } // Distribution Id?

        public class Serializer : PacketEntitySerializer<C2SQuestGetMobHuntQuestListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestGetMobHuntQuestListReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
            }

            public override C2SQuestGetMobHuntQuestListReq Read(IBuffer buffer)
            {
                C2SQuestGetMobHuntQuestListReq obj = new C2SQuestGetMobHuntQuestListReq();
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
