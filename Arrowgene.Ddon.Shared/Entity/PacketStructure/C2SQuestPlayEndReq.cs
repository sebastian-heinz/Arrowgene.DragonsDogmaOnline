using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestPlayEndReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_PLAY_END_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestPlayEndReq>
        {
            public override void Write(IBuffer buffer, C2SQuestPlayEndReq obj)
            {
            }

            public override C2SQuestPlayEndReq Read(IBuffer buffer)
            {
                C2SQuestPlayEndReq obj = new C2SQuestPlayEndReq();
                return obj;
            }
        }
    }
}
