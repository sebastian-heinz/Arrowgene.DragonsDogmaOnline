using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftStartEquipColorChangeRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_START_EQUIP_COLOR_CHANGE_RES;

        public S2CCraftStartEquipColorChangeRes()
        {
            CurrentEquipInfo = new CDataCurrentEquipInfo();
        }

        public byte ColorNo { get; set; } // Not sure why its called this in the PS4 Symbols, just rolling with it for now until i know how this works.
        public CDataCurrentEquipInfo CurrentEquipInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCraftStartEquipColorChangeRes>
        {
            public override void Write(IBuffer buffer, S2CCraftStartEquipColorChangeRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, obj.ColorNo);
                WriteEntity<CDataCurrentEquipInfo>(buffer, obj.CurrentEquipInfo);
            }

            public override S2CCraftStartEquipColorChangeRes Read(IBuffer buffer)
            {
                S2CCraftStartEquipColorChangeRes obj = new S2CCraftStartEquipColorChangeRes();
                ReadServerResponse(buffer, obj);
                obj.ColorNo = ReadByte(buffer);
                obj.CurrentEquipInfo = ReadEntity<CDataCurrentEquipInfo>(buffer);
                return obj;
            }
        }
    }
}
