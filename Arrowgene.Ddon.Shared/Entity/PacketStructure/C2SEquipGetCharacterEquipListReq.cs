using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipGetCharacterEquipListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EQUIP_GET_CHARACTER_EQUIP_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SEquipGetCharacterEquipListReq>
        {
            public override void Write(IBuffer buffer, C2SEquipGetCharacterEquipListReq obj)
            {
            }
            
            public override C2SEquipGetCharacterEquipListReq Read(IBuffer buffer)
            {
                return new C2SEquipGetCharacterEquipListReq();
            }
        }
    }
}