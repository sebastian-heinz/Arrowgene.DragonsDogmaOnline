using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEventEndNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EVENT_END_NTC;

        public C2SEventEndNtc()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SEventEndNtc>
        {
            public override void Write(IBuffer buffer, C2SEventEndNtc obj)
            {
            }

            public override C2SEventEndNtc Read(IBuffer buffer)
            {
                C2SEventEndNtc obj = new C2SEventEndNtc();
                return obj;
            }
        }
    }
}

