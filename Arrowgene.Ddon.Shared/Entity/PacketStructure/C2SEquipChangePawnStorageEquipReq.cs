using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipChangePawnStorageEquipReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EQUIP_CHANGE_PAWN_STORAGE_EQUIP_REQ;

        public C2SEquipChangePawnStorageEquipReq()
        {
            ChangeCharacterEquipList = new List<CDataCharacterEquipInfo>();
        }

        public uint PawnId { get; set; }
        public List<CDataCharacterEquipInfo> ChangeCharacterEquipList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SEquipChangePawnStorageEquipReq>
        {
            public override void Write(IBuffer buffer, C2SEquipChangePawnStorageEquipReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataCharacterEquipInfo>(buffer, obj.ChangeCharacterEquipList);
            }

            public override C2SEquipChangePawnStorageEquipReq Read(IBuffer buffer)
            {
                C2SEquipChangePawnStorageEquipReq obj = new C2SEquipChangePawnStorageEquipReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.ChangeCharacterEquipList = ReadEntityList<CDataCharacterEquipInfo>(buffer);
                return obj;
            }
        }

    }
}