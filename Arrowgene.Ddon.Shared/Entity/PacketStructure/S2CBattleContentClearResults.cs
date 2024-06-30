using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentClearResults : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_BATTLE_71_14_16_NTC;

        public S2CBattleContentClearResults()
        {
        }

        public uint Unk0 { get; set; }
        public string Unk1 { get; set; }
        public uint Unk2 { get; set; }
        public bool Unk3 {  get; set; }
        public ulong ClearTime { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentClearResults>
        {
            public override void Write(IBuffer buffer, S2CBattleContentClearResults obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteMtString(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteBool(buffer, obj.Unk3);
                WriteUInt64(buffer, obj.ClearTime);
            }

            public override S2CBattleContentClearResults Read(IBuffer buffer)
            {
                S2CBattleContentClearResults obj = new S2CBattleContentClearResults();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadMtString(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadBool(buffer);
                obj.ClearTime = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}

