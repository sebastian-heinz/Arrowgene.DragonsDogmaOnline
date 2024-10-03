using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPlayTimeupNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_PLAY_TIMEUP_NTC;


        public S2CQuestPlayTimeupNtc()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CQuestPlayTimeupNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestPlayTimeupNtc obj)
            {
            }

            public override S2CQuestPlayTimeupNtc Read(IBuffer buffer)
            {
                S2CQuestPlayTimeupNtc obj = new S2CQuestPlayTimeupNtc();
                return obj;
            }
        }
    }
}
