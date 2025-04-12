using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemGetEquipRareTypeItemsRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_GET_EQUIP_RARE_TYPE_ITEMS_RES;

        public S2CItemGetEquipRareTypeItemsRes()
        {
            Unk0List = new List<CDataCommonU32>();
        }

        public List<CDataCommonU32> Unk0List { get; set; } // Most likely item ids?

        public class Serializer : PacketEntitySerializer<S2CItemGetEquipRareTypeItemsRes>
        {
            public override void Write(IBuffer buffer, S2CItemGetEquipRareTypeItemsRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.Unk0List);
            }

            public override S2CItemGetEquipRareTypeItemsRes Read(IBuffer buffer)
            {
                S2CItemGetEquipRareTypeItemsRes obj = new S2CItemGetEquipRareTypeItemsRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0List = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
