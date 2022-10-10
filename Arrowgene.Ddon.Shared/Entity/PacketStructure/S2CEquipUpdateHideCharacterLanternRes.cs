using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipUpdateHideCharacterLanternRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_UPDATE_HIDE_CHARACTER_LANTERN_RES;

        public bool Hide { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipUpdateHideCharacterLanternRes>
        {
            public override void Write(IBuffer buffer, S2CEquipUpdateHideCharacterLanternRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteBool(buffer, obj.Hide);
            }

            public override S2CEquipUpdateHideCharacterLanternRes Read(IBuffer buffer)
            {
                S2CEquipUpdateHideCharacterLanternRes obj = new S2CEquipUpdateHideCharacterLanternRes();
                ReadServerResponse(buffer, obj);
                obj.Hide = ReadBool(buffer);
                return obj;
            }
        }
    }
}