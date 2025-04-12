using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartnerPawnPresentForPartnerPawnReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTNER_PAWN_PRESENT_FOR_PARTNER_PAWN_REQ;


        public C2SPartnerPawnPresentForPartnerPawnReq()
        {
            ItemUIDList = new List<CDataItemUIDList>();
        }

        public List<CDataItemUIDList> ItemUIDList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPartnerPawnPresentForPartnerPawnReq>
        {
            public override void Write(IBuffer buffer, C2SPartnerPawnPresentForPartnerPawnReq obj)
            {
                WriteEntityList(buffer, obj.ItemUIDList);
            }

            public override C2SPartnerPawnPresentForPartnerPawnReq Read(IBuffer buffer)
            {
                C2SPartnerPawnPresentForPartnerPawnReq obj = new C2SPartnerPawnPresentForPartnerPawnReq();
                obj.ItemUIDList = ReadEntityList<CDataItemUIDList>(buffer);
                return obj;
            }
        }
    }
}
