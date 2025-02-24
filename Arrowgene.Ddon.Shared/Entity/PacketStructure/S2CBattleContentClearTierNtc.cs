using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentClearTierNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_BATTLE_CONTENT_TIER_CLEAR_NTC;

        public S2CBattleContentClearTierNtc()
        {
        }

        public uint Unk0 { get; set; }
        public string TierName { get; set; } = string.Empty; // Prints when the clear message is sent after killing the area boss.

        public class Serializer : PacketEntitySerializer<S2CBattleContentClearTierNtc>
        {
            public override void Write(IBuffer buffer, S2CBattleContentClearTierNtc obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteMtString(buffer, obj.TierName);
            }

            public override S2CBattleContentClearTierNtc Read(IBuffer buffer)
            {
                S2CBattleContentClearTierNtc obj = new S2CBattleContentClearTierNtc();
                obj.Unk0 = ReadUInt32(buffer);
                obj.TierName = ReadMtString(buffer);
                return obj;
            }
        }
    }
}

