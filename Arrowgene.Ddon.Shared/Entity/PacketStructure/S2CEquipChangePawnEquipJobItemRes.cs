using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipChangePawnEquipJobItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_CHANGE_PAWN_EQUIP_JOB_ITEM_RES;

        public S2CEquipChangePawnEquipJobItemRes()
        {
            EquipJobItemList = new List<CDataEquipJobItem>();
        }

        public uint PawnId { get; set; }
        public List<CDataEquipJobItem> EquipJobItemList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipChangePawnEquipJobItemRes>
        {
            public override void Write(IBuffer buffer, S2CEquipChangePawnEquipJobItemRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList(buffer, obj.EquipJobItemList);
            }

            public override S2CEquipChangePawnEquipJobItemRes Read(IBuffer buffer)
            {
                S2CEquipChangePawnEquipJobItemRes obj = new S2CEquipChangePawnEquipJobItemRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.EquipJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
                return obj;
            }
        }
    }
}