using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestPlayInterruptReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_PLAY_INTERRUPT_REQ;


        public class Serializer : PacketEntitySerializer<C2SQuestPlayInterruptReq>
        {
            public override void Write(IBuffer buffer, C2SQuestPlayInterruptReq obj)
            {
            }

            public override C2SQuestPlayInterruptReq Read(IBuffer buffer)
            {
                C2SQuestPlayInterruptReq obj = new C2SQuestPlayInterruptReq();
                return obj;
            }
        }
    }
}
