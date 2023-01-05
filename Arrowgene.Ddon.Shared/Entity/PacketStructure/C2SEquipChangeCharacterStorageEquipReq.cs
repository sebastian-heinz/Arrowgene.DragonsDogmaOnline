using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipChangeCharacterStorageEquipReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EQUIP_CHANGE_CHARACTER_STORAGE_EQUIP_REQ;

        public List<CDataCharacterEquipInfo> ChangeCharacterEquipList { get; set; }

        public C2SEquipChangeCharacterStorageEquipReq()
        {
            ChangeCharacterEquipList = new List<CDataCharacterEquipInfo>();
        }

        public class Serializer : PacketEntitySerializer<C2SEquipChangeCharacterStorageEquipReq>
        {
            public override void Write(IBuffer buffer, C2SEquipChangeCharacterStorageEquipReq obj)
            {
                WriteEntityList<CDataCharacterEquipInfo>(buffer, obj.ChangeCharacterEquipList);
            }
        
            public override C2SEquipChangeCharacterStorageEquipReq Read(IBuffer buffer)
            {
                C2SEquipChangeCharacterStorageEquipReq obj = new C2SEquipChangeCharacterStorageEquipReq();
                obj.ChangeCharacterEquipList = ReadEntityList<CDataCharacterEquipInfo>(buffer);
                return obj;
            }
        }
    }
}