using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestPlayEntryReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_PLAY_ENTRY_REQ;

        public class Serializer : PacketEntitySerializer<C2SQuestPlayEntryReq>
        {
            public override void Write(IBuffer buffer, C2SQuestPlayEntryReq obj)
            {
            }

            public override C2SQuestPlayEntryReq Read(IBuffer buffer)
            {
                C2SQuestPlayEntryReq obj = new C2SQuestPlayEntryReq();
                return obj;
            }
        }
    }
}
