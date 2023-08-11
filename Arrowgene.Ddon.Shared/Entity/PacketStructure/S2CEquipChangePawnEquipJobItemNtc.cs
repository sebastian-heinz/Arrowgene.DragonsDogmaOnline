using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipChangePawnEquipJobItemNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_EQUIP_CHANGE_PAWN_EQUIP_JOB_ITEM_NTC;

        public S2CEquipChangePawnEquipJobItemNtc()
        {
            EquipJobItemList = new List<CDataEquipJobItem>();
        }

        public uint CharacterId { get; set; }
        public uint PawnId { get; set; }
        public List<CDataEquipJobItem> EquipJobItemList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipChangePawnEquipJobItemNtc>
        {
            public override void Write(IBuffer buffer, S2CEquipChangePawnEquipJobItemNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList(buffer, obj.EquipJobItemList);
            }

            public override S2CEquipChangePawnEquipJobItemNtc Read(IBuffer buffer)
            {
                S2CEquipChangePawnEquipJobItemNtc obj = new S2CEquipChangePawnEquipJobItemNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.EquipJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
                return obj;
            }
        }
    }
}