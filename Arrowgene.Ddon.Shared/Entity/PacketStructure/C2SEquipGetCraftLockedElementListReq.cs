using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipGetCraftLockedElementListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EQUIP_GET_CRAFT_LOCKED_ELEMENT_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SEquipGetCraftLockedElementListReq>
        {
            public override void Write(IBuffer buffer, C2SEquipGetCraftLockedElementListReq obj)
            {
            }

            public override C2SEquipGetCraftLockedElementListReq Read(IBuffer buffer)
            {
                C2SEquipGetCraftLockedElementListReq obj = new C2SEquipGetCraftLockedElementListReq();
                return obj;
            }
        }

    }
}