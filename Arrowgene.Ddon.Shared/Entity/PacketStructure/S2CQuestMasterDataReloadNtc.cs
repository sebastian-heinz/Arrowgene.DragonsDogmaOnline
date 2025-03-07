using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestMasterDataReloadNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_MASTER_DATA_RELOAD_NTC;

        public S2CQuestMasterDataReloadNtc()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CQuestMasterDataReloadNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestMasterDataReloadNtc obj)
            {
            }

            public override S2CQuestMasterDataReloadNtc Read(IBuffer buffer)
            {
                return new S2CQuestMasterDataReloadNtc();
            }
        }
    }
}
