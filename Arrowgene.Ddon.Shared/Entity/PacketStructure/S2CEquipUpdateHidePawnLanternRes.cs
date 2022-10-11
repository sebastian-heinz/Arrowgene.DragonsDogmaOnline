using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipUpdateHidePawnLanternRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_UPDATE_HIDE_PAWN_LANTERN_RES;

        public bool Hide { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipUpdateHidePawnLanternRes>
        {
            public override void Write(IBuffer buffer, S2CEquipUpdateHidePawnLanternRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteBool(buffer, obj.Hide);
            }

            public override S2CEquipUpdateHidePawnLanternRes Read(IBuffer buffer)
            {
                S2CEquipUpdateHidePawnLanternRes obj = new S2CEquipUpdateHidePawnLanternRes();
                ReadServerResponse(buffer, obj);
                obj.Hide = ReadBool(buffer);
                return obj;
            }
        }
    }
}