using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentPhaseEntryReadyNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_BATTLE_CONTENT_PHASE_ENTRY_READY_NTC;

        public S2CBattleContentPhaseEntryReadyNtc()
        {
        }

        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentPhaseEntryReadyNtc>
        {
            public override void Write(IBuffer buffer, S2CBattleContentPhaseEntryReadyNtc obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override S2CBattleContentPhaseEntryReadyNtc Read(IBuffer buffer)
            {
                S2CBattleContentPhaseEntryReadyNtc obj = new S2CBattleContentPhaseEntryReadyNtc
                {
                    Unk0 = ReadUInt32(buffer),
                    Unk1 = ReadUInt32(buffer)
                };
                return obj;
            }
        }
    }
}


