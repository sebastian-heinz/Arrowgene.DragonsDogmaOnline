using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestPlayInterruptAnswerReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_PLAY_INTERRUPT_ANSWER_REQ;

        public bool IsAnswer {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestPlayInterruptAnswerReq>
        {
            public override void Write(IBuffer buffer, C2SQuestPlayInterruptAnswerReq obj)
            {
                WriteBool(buffer, obj.IsAnswer);
            }

            public override C2SQuestPlayInterruptAnswerReq Read(IBuffer buffer)
            {
                C2SQuestPlayInterruptAnswerReq obj = new C2SQuestPlayInterruptAnswerReq();
                obj.IsAnswer = ReadBool(buffer);
                return obj;
            }
        }
    }
}
