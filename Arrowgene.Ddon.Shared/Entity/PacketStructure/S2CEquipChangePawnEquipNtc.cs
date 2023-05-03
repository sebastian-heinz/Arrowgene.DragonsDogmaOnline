using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipChangePawnEquipNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_EQUIP_CHANGE_PAWN_EQUIP_NTC;

        public S2CEquipChangePawnEquipNtc()
        {
            EquipItemList = new List<CDataEquipItemInfo>();
            VisualEquipItemList = new List<CDataEquipItemInfo>();
            Unk0 = new CDataJobChangeJobResUnk0();
        }

        public uint CharacterId;
        public uint PawnId;
        public List<CDataEquipItemInfo> EquipItemList;
        public List<CDataEquipItemInfo> VisualEquipItemList;
        public CDataJobChangeJobResUnk0 Unk0;

        public class Serializer : PacketEntitySerializer<S2CEquipChangePawnEquipNtc>
        {
            public override void Write(IBuffer buffer, S2CEquipChangePawnEquipNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataEquipItemInfo>(buffer, obj.EquipItemList);
                WriteEntityList<CDataEquipItemInfo>(buffer, obj.VisualEquipItemList);
                WriteEntity<CDataJobChangeJobResUnk0>(buffer, obj.Unk0);
            }

            public override S2CEquipChangePawnEquipNtc Read(IBuffer buffer)
            {
                S2CEquipChangePawnEquipNtc obj = new S2CEquipChangePawnEquipNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.EquipItemList = ReadEntityList<CDataEquipItemInfo>(buffer);
                obj.VisualEquipItemList = ReadEntityList<CDataEquipItemInfo>(buffer);
                obj.Unk0 = ReadEntity<CDataJobChangeJobResUnk0>(buffer);
                return obj;
            }
        }

    }
}