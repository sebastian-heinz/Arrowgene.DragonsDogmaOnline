using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBattleContentPhaseEntryReadyCancelReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BATTLE_CONTENT_PHASE_ENTRY_READY_CANCEL_REQ;

        public C2SBattleContentPhaseEntryReadyCancelReq()
        {
        }

        public uint Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SBattleContentPhaseEntryReadyCancelReq>
        {
            public override void Write(IBuffer buffer, C2SBattleContentPhaseEntryReadyCancelReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
            }

            public override C2SBattleContentPhaseEntryReadyCancelReq Read(IBuffer buffer)
            {
                C2SBattleContentPhaseEntryReadyCancelReq obj = new()
                {
                    Unk0 = ReadUInt32(buffer),
                };

                return obj;
            }
        }
    }
}
