using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraft_30_21_16_NTC : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CRAFT_30_21_16_NTC;

        public S2CCraft_30_21_16_NTC()
        {
            Unk0 = 0;
            Unk1 = 0;
            Unk2 = new List<CDataEquipItemInfo>();
            Unk3 = new List<CDataEquipItemInfo>();
        }

        // TODO: Figure out when this NTC should be sent.
        public uint Unk0 { get; set; } // most likely charid
        public uint Unk1 { get; set; } // most likely pawnid
        public List<CDataEquipItemInfo> Unk2 { get; set; } // EquipType Performance?
        public List<CDataEquipItemInfo> Unk3 { get; set; } // EquipType Visual?

        public class Serializer : PacketEntitySerializer<S2CCraft_30_21_16_NTC>
        {
            public override void Write(IBuffer buffer, S2CCraft_30_21_16_NTC obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteEntityList<CDataEquipItemInfo>(buffer, obj.Unk2);
                WriteEntityList<CDataEquipItemInfo>(buffer, obj.Unk3);
            }

            public override S2CCraft_30_21_16_NTC Read(IBuffer buffer)
            {
                S2CCraft_30_21_16_NTC obj = new S2CCraft_30_21_16_NTC();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadEntityList<CDataEquipItemInfo>(buffer);
                obj.Unk3 = ReadEntityList<CDataEquipItemInfo>(buffer);
                return obj;
            }
        }
    }
}
