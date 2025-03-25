using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnGetMyPawnDataRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_GET_MYPAWN_DATA_RES;
        public S2CPawnGetMyPawnDataRes()
        {
            PawnInfo = new CDataPawnInfo();
        }

        public uint PawnId { get; set; }
        public CDataPawnInfo PawnInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnGetMyPawnDataRes>
        {

            public override void Write(IBuffer buffer, S2CPawnGetMyPawnDataRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity<CDataPawnInfo>(buffer, obj.PawnInfo);
            }

            public override S2CPawnGetMyPawnDataRes Read(IBuffer buffer)
            {
                S2CPawnGetMyPawnDataRes obj = new S2CPawnGetMyPawnDataRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.PawnInfo = ReadEntity<CDataPawnInfo>(buffer);
                return obj;
            }

        }
    }
}
