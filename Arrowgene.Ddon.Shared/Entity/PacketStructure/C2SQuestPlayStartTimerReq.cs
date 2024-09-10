using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestPlayStartTimerReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_PLAY_START_TIMER_REQ;


        public class Serializer : PacketEntitySerializer<C2SQuestPlayStartTimerReq>
        {
            public override void Write(IBuffer buffer, C2SQuestPlayStartTimerReq obj)
            {
            }

            public override C2SQuestPlayStartTimerReq Read(IBuffer buffer)
            {
                C2SQuestPlayStartTimerReq obj = new C2SQuestPlayStartTimerReq();
                return obj;
            }
        }
    }
}
