using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2C_BATTLE_71_12_16_NTC : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_BATTLE_71_12_16_NTC;

        public S2C_BATTLE_71_12_16_NTC()
        {
            Unk5 = new List<CDataBattleContentUnk5>();
        }

        public uint Unk0 { get; set; }
        public string Unk1 { get; set; }
        public uint StageId { get; set; }
        public uint StartPos { get; set; }
        public bool Unk4 { get; set; }
        public List<CDataBattleContentUnk5> Unk5 { get; set; }

        public class Serializer : PacketEntitySerializer<S2C_BATTLE_71_12_16_NTC>
        {
            public override void Write(IBuffer buffer, S2C_BATTLE_71_12_16_NTC obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteMtString(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.StageId);
                WriteUInt32(buffer, obj.StartPos);
                WriteBool(buffer, obj.Unk4);
                WriteEntityList(buffer, obj.Unk5);
            }

            public override S2C_BATTLE_71_12_16_NTC Read(IBuffer buffer)
            {
                S2C_BATTLE_71_12_16_NTC obj = new S2C_BATTLE_71_12_16_NTC();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadMtString(buffer);
                obj.StageId = ReadUInt32(buffer);
                obj.StartPos = ReadUInt32(buffer);
                obj.Unk4 = ReadBool(buffer);
                obj.Unk5 = ReadEntityList<CDataBattleContentUnk5>(buffer);
                return obj;
            }
        }
    }
}


