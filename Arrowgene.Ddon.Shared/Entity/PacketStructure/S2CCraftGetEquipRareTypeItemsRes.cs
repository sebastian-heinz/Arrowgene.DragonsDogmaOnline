using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftGetEquipRareTypeItemsRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_GET_EQUIP_RARE_TYPE_ITEMS_RES;

        public S2CCraftGetEquipRareTypeItemsRes()
        {        
        }

        public List<CDataCommonU32> UnkList { get; set; }
        

        public class Serializer : PacketEntitySerializer<S2CCraftGetEquipRareTypeItemsRes>
        {
            public override void Write(IBuffer buffer, S2CCraftGetEquipRareTypeItemsRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataCommonU32>(buffer, obj.UnkList);
            }

            public override S2CCraftGetEquipRareTypeItemsRes Read(IBuffer buffer)
            {
                S2CCraftGetEquipRareTypeItemsRes obj = new S2CCraftGetEquipRareTypeItemsRes();
                ReadServerResponse(buffer, obj);
                obj.UnkList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}