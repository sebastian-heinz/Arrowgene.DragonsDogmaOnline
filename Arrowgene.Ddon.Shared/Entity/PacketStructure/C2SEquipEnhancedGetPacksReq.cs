using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipEnhancedGetPacksReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EQUIP_ENHANCED_GET_PACKS_REQ;

        public byte Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SEquipEnhancedGetPacksReq>
        {
            public override void Write(IBuffer buffer, C2SEquipEnhancedGetPacksReq obj)
            {
                WriteByte(buffer, obj.Unk0);
            }

            public override C2SEquipEnhancedGetPacksReq Read(IBuffer buffer)
            {
                C2SEquipEnhancedGetPacksReq obj = new C2SEquipEnhancedGetPacksReq();
                obj.Unk0 = ReadByte(buffer);
                return obj;
            }
        }

    }
}