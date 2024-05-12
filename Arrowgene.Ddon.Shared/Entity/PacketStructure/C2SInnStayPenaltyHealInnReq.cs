using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInnStayPenaltyHealInnReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INN_STAY_PENALTY_HEAL_INN_REQ;

        public C2SInnStayPenaltyHealInnReq()
        {
            PawnHpMaxList = new List<CDataPawnHp>();
        }

        public uint Price { get; set; }
        public uint HpMax { get; set; }
        public List<CDataPawnHp> PawnHpMaxList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SInnStayPenaltyHealInnReq>
        {
            public override void Write(IBuffer buffer, C2SInnStayPenaltyHealInnReq obj)
            {
                WriteUInt32(buffer, obj.Price);
                WriteUInt32(buffer, obj.HpMax);
                WriteEntityList<CDataPawnHp>(buffer, obj.PawnHpMaxList);
            }

            public override C2SInnStayPenaltyHealInnReq Read(IBuffer buffer)
            {
                C2SInnStayPenaltyHealInnReq obj = new C2SInnStayPenaltyHealInnReq();
                obj.Price = ReadUInt32(buffer);
                obj.HpMax = ReadUInt32(buffer);
                obj.PawnHpMaxList = ReadEntityList<CDataPawnHp>(buffer);
                return obj;
            }
        }

    }
}