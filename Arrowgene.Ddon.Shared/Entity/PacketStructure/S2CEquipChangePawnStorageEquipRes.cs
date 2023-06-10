using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipChangePawnStorageEquipRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_CHANGE_PAWN_STORAGE_EQUIP_RES;

        public S2CEquipChangePawnStorageEquipRes()
        {
            CharacterEquipList = new List<CDataCharacterEquipInfo>();
            Unk0 = new CDataJobChangeJobResUnk0();    
        }

        public uint PawnId { get; set; }
        public List<CDataCharacterEquipInfo> CharacterEquipList { get; set; }
        public CDataJobChangeJobResUnk0 Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipChangePawnStorageEquipRes>
        {
            public override void Write(IBuffer buffer, S2CEquipChangePawnStorageEquipRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataCharacterEquipInfo>(buffer, obj.CharacterEquipList);
                WriteEntity<CDataJobChangeJobResUnk0>(buffer, obj.Unk0);
            }

            public override S2CEquipChangePawnStorageEquipRes Read(IBuffer buffer)
            {
                S2CEquipChangePawnStorageEquipRes obj = new S2CEquipChangePawnStorageEquipRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.CharacterEquipList = ReadEntityList<CDataCharacterEquipInfo>(buffer);
                obj.Unk0 = ReadEntity<CDataJobChangeJobResUnk0>(buffer);
                return obj;
            }
        }
    }
}