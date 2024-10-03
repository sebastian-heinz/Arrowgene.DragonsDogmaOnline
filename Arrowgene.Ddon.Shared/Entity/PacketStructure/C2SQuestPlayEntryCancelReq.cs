using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestPlayEntryCancelReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_PLAY_ENTRY_CANCEL_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestPlayEntryCancelReq>
        {
            public override void Write(IBuffer buffer, C2SQuestPlayEntryCancelReq obj)
            {
            }

            public override C2SQuestPlayEntryCancelReq Read(IBuffer buffer)
            {
                C2SQuestPlayEntryCancelReq obj = new C2SQuestPlayEntryCancelReq();
                return obj;
            }
        }
    }
}
