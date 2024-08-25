using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInnHpRecoveryCompleteNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INN_HP_RECOVERY_COMPLETE_NTC;

        public C2SInnHpRecoveryCompleteNtc()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SInnHpRecoveryCompleteNtc>
        {
            public override void Write(IBuffer buffer, C2SInnHpRecoveryCompleteNtc obj)
            {
            }

            public override C2SInnHpRecoveryCompleteNtc Read(IBuffer buffer)
            {
                C2SInnHpRecoveryCompleteNtc obj = new C2SInnHpRecoveryCompleteNtc();
                return obj;
            }
        }
    }
}
