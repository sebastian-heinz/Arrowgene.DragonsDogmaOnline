using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentClearNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_BATTLE_CONTENT_CLEAR_NTC;

        public S2CBattleContentClearNtc()
        {
        }

        public uint Unk0 { get; set; }
        public string Unk1 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentClearNtc>
        {
            public override void Write(IBuffer buffer, S2CBattleContentClearNtc obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteMtString(buffer, obj.Unk1);
            }

            public override S2CBattleContentClearNtc Read(IBuffer buffer)
            {
                S2CBattleContentClearNtc obj = new S2CBattleContentClearNtc();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadMtString(buffer);
                return obj;
            }
        }
    }
}

