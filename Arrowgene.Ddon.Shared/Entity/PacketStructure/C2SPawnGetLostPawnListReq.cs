using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnGetLostPawnListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_GET_LOST_PAWN_LIST_REQ;

        public uint Data { get; set; }

        public C2SPawnGetLostPawnListReq()
        {
            Data = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SPawnGetLostPawnListReq>
        {
            public override void Write(IBuffer buffer, C2SPawnGetLostPawnListReq obj)
            {
                WriteUInt32(buffer, obj.Data);
            }

            public override C2SPawnGetLostPawnListReq Read(IBuffer buffer)
            {
                C2SPawnGetLostPawnListReq obj = new C2SPawnGetLostPawnListReq();
                obj.Data = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
