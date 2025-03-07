using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentClearContentNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_BATTLE_CONTENT_CLEAR_NTC;

        public S2CBattleContentClearContentNtc()
        {
        }

        public uint Unk0 { get; set; }
        public string ContentName { get; set; } = string.Empty;
        public uint Unk2 { get; set; }
        public bool Unk3 {  get; set; }
        public ulong ClearTime { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentClearContentNtc>
        {
            public override void Write(IBuffer buffer, S2CBattleContentClearContentNtc obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteMtString(buffer, obj.ContentName);
                WriteUInt32(buffer, obj.Unk2);
                WriteBool(buffer, obj.Unk3);
                WriteUInt64(buffer, obj.ClearTime);
            }

            public override S2CBattleContentClearContentNtc Read(IBuffer buffer)
            {
                S2CBattleContentClearContentNtc obj = new S2CBattleContentClearContentNtc();
                obj.Unk0 = ReadUInt32(buffer);
                obj.ContentName = ReadMtString(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadBool(buffer);
                obj.ClearTime = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}

