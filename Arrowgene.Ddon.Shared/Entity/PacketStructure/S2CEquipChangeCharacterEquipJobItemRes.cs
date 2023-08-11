using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipChangeCharacterEquipJobItemRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_CHANGE_CHARACTER_EQUIP_JOB_ITEM_RES;

        public S2CEquipChangeCharacterEquipJobItemRes()
        {
            EquipJobItemList = new List<CDataEquipJobItem>();
        }

        public List<CDataEquipJobItem> EquipJobItemList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipChangeCharacterEquipJobItemRes>
        {
            public override void Write(IBuffer buffer, S2CEquipChangeCharacterEquipJobItemRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataEquipJobItem>(buffer, obj.EquipJobItemList);
            }

            public override S2CEquipChangeCharacterEquipJobItemRes Read(IBuffer buffer)
            {
                S2CEquipChangeCharacterEquipJobItemRes obj = new S2CEquipChangeCharacterEquipJobItemRes();
                ReadServerResponse(buffer, obj);
                obj.EquipJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
                return obj;
            }
        }
    }
}