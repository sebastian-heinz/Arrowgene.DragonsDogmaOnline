using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipGetCraftLockedElementListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_GET_CRAFT_LOCKED_ELEMENT_LIST_RES;

        public S2CEquipGetCraftLockedElementListRes()
        {
            LockedElementList = new List<CDataItemEquipElement>();
        }

        public List<CDataItemEquipElement> LockedElementList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipGetCraftLockedElementListRes>
        {
            public override void Write(IBuffer buffer, S2CEquipGetCraftLockedElementListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataItemEquipElement>(buffer, obj.LockedElementList);
            }

            public override S2CEquipGetCraftLockedElementListRes Read(IBuffer buffer)
            {
                S2CEquipGetCraftLockedElementListRes obj = new S2CEquipGetCraftLockedElementListRes();
                ReadServerResponse(buffer, obj);
                obj.LockedElementList = ReadEntityList<CDataItemEquipElement>(buffer);
                return obj;
            }
        }
    }
}