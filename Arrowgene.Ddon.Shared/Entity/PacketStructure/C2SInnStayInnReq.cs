using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInnStayInnReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INN_STAY_INN_REQ;

        public C2SInnStayInnReq() 
        {
            PawnHpMaxList = new List<CDataPawnHp>();
        }
        
        public uint InnId { get; set; }
        public uint Price { get; set; }
        public uint HpMax { get; set; }
        public List<CDataPawnHp> PawnHpMaxList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SInnStayInnReq>
        {
            public override void Write(IBuffer buffer, C2SInnStayInnReq obj)
            {
                WriteUInt32(buffer, obj.InnId);
                WriteUInt32(buffer, obj.Price);
                WriteUInt32(buffer, obj.HpMax);
                WriteEntityList<CDataPawnHp>(buffer, obj.PawnHpMaxList);
            }

            public override C2SInnStayInnReq Read(IBuffer buffer)
            {
                C2SInnStayInnReq obj = new C2SInnStayInnReq();
                obj.InnId = ReadUInt32(buffer);
                obj.Price = ReadUInt32(buffer);
                obj.HpMax = ReadUInt32(buffer);
                obj.PawnHpMaxList = ReadEntityList<CDataPawnHp>(buffer);
                return obj;
            }
        }
    }
}
