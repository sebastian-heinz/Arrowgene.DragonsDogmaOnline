using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanConciergeUpdateReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_CONCIERGE_UPDATE_REQ;

        public uint ConciergeId { get; set; }
        public uint RequireCP { get; set; }
        
        public C2SClanClanConciergeUpdateReq()
        {
            ConciergeId = 0;
            RequireCP = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanConciergeUpdateReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanConciergeUpdateReq obj)
            {
                WriteUInt32(buffer, obj.ConciergeId);
                WriteUInt32(buffer, obj.RequireCP);
            }

            public override C2SClanClanConciergeUpdateReq Read(IBuffer buffer)
            {
                C2SClanClanConciergeUpdateReq obj = new C2SClanClanConciergeUpdateReq();
                obj.ConciergeId = ReadUInt32(buffer);
                obj.RequireCP = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
