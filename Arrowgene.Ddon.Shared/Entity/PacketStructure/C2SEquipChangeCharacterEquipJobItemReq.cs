using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipChangeCharacterEquipJobItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EQUIP_CHANGE_CHARACTER_EQUIP_JOB_ITEM_REQ;

        public C2SEquipChangeCharacterEquipJobItemReq()
        {
            ChangeEquipJobItemList = new List<CDataChangeEquipJobItem>();
        }

        public List<CDataChangeEquipJobItem> ChangeEquipJobItemList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SEquipChangeCharacterEquipJobItemReq>
        {
            public override void Write(IBuffer buffer, C2SEquipChangeCharacterEquipJobItemReq obj)
            {
                WriteEntityList<CDataChangeEquipJobItem>(buffer, obj.ChangeEquipJobItemList);
            }

            public override C2SEquipChangeCharacterEquipJobItemReq Read(IBuffer buffer)
            {
                C2SEquipChangeCharacterEquipJobItemReq obj = new C2SEquipChangeCharacterEquipJobItemReq();
                obj.ChangeEquipJobItemList = ReadEntityList<CDataChangeEquipJobItem>(buffer);
                return obj;
            }
        }

    }
}