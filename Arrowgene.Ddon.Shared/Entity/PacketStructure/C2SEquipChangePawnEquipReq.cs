using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipChangePawnEquipReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_EQUIP_CHANGE_PAWN_EQUIP_REQ;

        public C2SEquipChangePawnEquipReq()
        {
            ChangeCharacterEquipList = new List<CDataCharacterEquipInfo>();
        }

        public uint PawnId { get; set; }
        public List<CDataCharacterEquipInfo> ChangeCharacterEquipList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SEquipChangePawnEquipReq>
        {
            public override void Write(IBuffer buffer, C2SEquipChangePawnEquipReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataCharacterEquipInfo>(buffer, obj.ChangeCharacterEquipList);
            }

            public override C2SEquipChangePawnEquipReq Read(IBuffer buffer)
            {
                C2SEquipChangePawnEquipReq obj = new C2SEquipChangePawnEquipReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.ChangeCharacterEquipList = ReadEntityList<CDataCharacterEquipInfo>(buffer);
                return obj;
            }
        }

    }
}