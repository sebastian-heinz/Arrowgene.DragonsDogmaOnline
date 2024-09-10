using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestPlayerStartReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_PLAY_START_REQ;

        public uint QuestScheduleId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestPlayerStartReq>
        {
            public override void Write(IBuffer buffer, C2SQuestPlayerStartReq obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
            }

            public override C2SQuestPlayerStartReq Read(IBuffer buffer)
            {
                C2SQuestPlayerStartReq obj = new C2SQuestPlayerStartReq();
                obj.QuestScheduleId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
