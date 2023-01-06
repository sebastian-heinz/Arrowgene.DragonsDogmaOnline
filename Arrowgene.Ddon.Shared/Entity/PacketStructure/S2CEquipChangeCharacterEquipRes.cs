using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipChangeCharacterEquipRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_CHANGE_CHARACTER_EQUIP_RES;

        public S2CEquipChangeCharacterEquipRes()
        {
            CharacterEquipList = new List<CDataCharacterEquipInfo>();
            Unk0 = new CDataJobChangeJobResUnk0();
        }

        public List<CDataCharacterEquipInfo> CharacterEquipList { get; set; }
        public CDataJobChangeJobResUnk0 Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipChangeCharacterEquipRes>
        {
            public override void Write(IBuffer buffer, S2CEquipChangeCharacterEquipRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataCharacterEquipInfo>(buffer, obj.CharacterEquipList);
                WriteEntity<CDataJobChangeJobResUnk0>(buffer, obj.Unk0);
            }

            public override S2CEquipChangeCharacterEquipRes Read(IBuffer buffer)
            {
                S2CEquipChangeCharacterEquipRes obj = new S2CEquipChangeCharacterEquipRes();
                ReadServerResponse(buffer, obj);
                obj.CharacterEquipList = ReadEntityList<CDataCharacterEquipInfo>(buffer);
                obj.Unk0 = ReadEntity<CDataJobChangeJobResUnk0>(buffer);
                return obj;
            }
        }
    }
}