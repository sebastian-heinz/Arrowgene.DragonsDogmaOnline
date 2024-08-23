using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipChangeCharacterEquipReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EQUIP_CHANGE_CHARACTER_EQUIP_REQ;

        public C2SEquipChangeCharacterEquipReq()
        {
            ChangeCharacterEquipList = new List<CDataCharacterEquipInfo>();
        }

        public List<CDataCharacterEquipInfo> ChangeCharacterEquipList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SEquipChangeCharacterEquipReq>
        {
            public override void Write(IBuffer buffer, C2SEquipChangeCharacterEquipReq obj)
            {
                WriteEntityList<CDataCharacterEquipInfo>(buffer, obj.ChangeCharacterEquipList);
            }

            public override C2SEquipChangeCharacterEquipReq Read(IBuffer buffer)
            {
                C2SEquipChangeCharacterEquipReq obj = new C2SEquipChangeCharacterEquipReq();
                obj.ChangeCharacterEquipList = ReadEntityList<CDataCharacterEquipInfo>(buffer);
                return obj;
            }
        }
    }
}
