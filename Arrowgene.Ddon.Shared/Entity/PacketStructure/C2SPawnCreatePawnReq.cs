using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnCreatePawnReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_CREATE_MYPAWN_REQ;

        public C2SPawnCreatePawnReq()
        {
            PawnInfo = new CDataPawnInfo();
        }

        public byte SlotNo {  get; set; }
        public CDataPawnInfo PawnInfo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnCreatePawnReq>
        {
            public override void Write(IBuffer buffer, C2SPawnCreatePawnReq obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteEntity(buffer, obj.PawnInfo);
            }

            public override C2SPawnCreatePawnReq Read(IBuffer buffer)
            {
                C2SPawnCreatePawnReq obj = new C2SPawnCreatePawnReq();
                obj.SlotNo = ReadByte(buffer);
                obj.PawnInfo = ReadEntity<CDataPawnInfo>(buffer);
                return obj;
            }
        }
    }
}

