using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2S_QUEST_11_60_16_NTC : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_11_60_16_NTC;

        public C2S_QUEST_11_60_16_NTC()
        {
        }

        public class Serializer : PacketEntitySerializer<C2S_QUEST_11_60_16_NTC>
        {
            public override void Write(IBuffer buffer, C2S_QUEST_11_60_16_NTC obj)
            {
            }

            public override C2S_QUEST_11_60_16_NTC Read(IBuffer buffer)
            {
                C2S_QUEST_11_60_16_NTC obj = new C2S_QUEST_11_60_16_NTC();
                return obj;
            }
        }
    }
}
