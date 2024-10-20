using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardItemPartyNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_ENTRY_BOARD_ITEM_PARTY_NTC;

        public S2CEntryBoardItemPartyNtc()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardItemPartyNtc>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardItemPartyNtc obj)
            {
            }

            public override S2CEntryBoardItemPartyNtc Read(IBuffer buffer)
            {
                S2CEntryBoardItemPartyNtc obj = new S2CEntryBoardItemPartyNtc();
                return obj;
            }
        }
    }
}
