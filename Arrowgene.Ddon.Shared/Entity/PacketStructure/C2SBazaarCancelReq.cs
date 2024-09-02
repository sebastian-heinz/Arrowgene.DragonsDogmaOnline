using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBazaarCancelReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BAZAAR_CANCEL_REQ;

        public ulong BazaarId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBazaarCancelReq>
        {
            public override void Write(IBuffer buffer, C2SBazaarCancelReq obj)
            {
                WriteUInt64(buffer, obj.BazaarId);
            }

            public override C2SBazaarCancelReq Read(IBuffer buffer)
            {
                C2SBazaarCancelReq obj = new C2SBazaarCancelReq();
                obj.BazaarId = ReadUInt64(buffer);
                return obj;
            }
        }

    }
}
