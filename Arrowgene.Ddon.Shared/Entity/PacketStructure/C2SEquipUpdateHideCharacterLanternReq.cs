using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipUpdateHideCharacterLanternReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EQUIP_UPDATE_HIDE_CHARACTER_LANTERN_REQ;

        public bool Hide { get; set; }

        public class Serializer : PacketEntitySerializer<C2SEquipUpdateHideCharacterLanternReq>
        {
            public override void Write(IBuffer buffer, C2SEquipUpdateHideCharacterLanternReq obj)
            {
                WriteBool(buffer, obj.Hide);
            }

            public override C2SEquipUpdateHideCharacterLanternReq Read(IBuffer buffer)
            {
                C2SEquipUpdateHideCharacterLanternReq obj = new C2SEquipUpdateHideCharacterLanternReq();
                obj.Hide = ReadBool(buffer);
                return obj;
            }
        }
    }
}