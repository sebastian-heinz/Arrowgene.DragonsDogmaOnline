using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEventStartNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EVENT_START_NTC;

        public C2SEventStartNtc()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SEventStartNtc>
        {
            public override void Write(IBuffer buffer, C2SEventStartNtc obj)
            {
            }

            public override C2SEventStartNtc Read(IBuffer buffer)
            {
                C2SEventStartNtc obj = new C2SEventStartNtc();
                return obj;
            }
        }
    }
}

