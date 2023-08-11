using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipChangePawnEquipJobItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EQUIP_CHANGE_PAWN_EQUIP_JOB_ITEM_REQ;

        public C2SEquipChangePawnEquipJobItemReq()
        {
            ChangeEquipJobItemList = new List<CDataChangeEquipJobItem>();
        }

        public uint PawnId { get; set; }
        public List<CDataChangeEquipJobItem> ChangeEquipJobItemList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SEquipChangePawnEquipJobItemReq>
        {
            public override void Write(IBuffer buffer, C2SEquipChangePawnEquipJobItemReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList(buffer, obj.ChangeEquipJobItemList);
            }

            public override C2SEquipChangePawnEquipJobItemReq Read(IBuffer buffer)
            {
                C2SEquipChangePawnEquipJobItemReq obj = new C2SEquipChangePawnEquipJobItemReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.ChangeEquipJobItemList = ReadEntityList<CDataChangeEquipJobItem>(buffer);
                return obj;
            }
        }

    }
}