using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipUpdateHidePawnLanternReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EQUIP_UPDATE_HIDE_PAWN_LANTERN_REQ;

        public bool Hide { get; set; }

        public class Serializer : PacketEntitySerializer<C2SEquipUpdateHidePawnLanternReq>
        {
            public override void Write(IBuffer buffer, C2SEquipUpdateHidePawnLanternReq obj)
            {
                WriteBool(buffer, obj.Hide);
            }

            public override C2SEquipUpdateHidePawnLanternReq Read(IBuffer buffer)
            {
                C2SEquipUpdateHidePawnLanternReq obj = new C2SEquipUpdateHidePawnLanternReq();
                obj.Hide = ReadBool(buffer);
                return obj;
            }
        }
    }
}