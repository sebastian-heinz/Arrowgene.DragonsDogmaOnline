using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipGetEquipPresetListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EQUIP_GET_EQUIP_PRESET_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SEquipGetEquipPresetListReq>
        {
            public override void Write(IBuffer buffer, C2SEquipGetEquipPresetListReq obj)
            {
            }

            public override C2SEquipGetEquipPresetListReq Read(IBuffer buffer)
            {
                C2SEquipGetEquipPresetListReq obj = new C2SEquipGetEquipPresetListReq();
                return obj;
            }
        }
    }
}
