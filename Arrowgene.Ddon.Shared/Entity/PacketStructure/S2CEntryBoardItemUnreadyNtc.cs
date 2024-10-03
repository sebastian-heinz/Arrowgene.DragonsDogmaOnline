using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardItemUnreadyNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_ENTRY_BOARD_ITEM_UNREADY_NTC;

        public S2CEntryBoardItemUnreadyNtc()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardItemUnreadyNtc>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardItemUnreadyNtc obj)
            {
            }

            public override S2CEntryBoardItemUnreadyNtc Read(IBuffer buffer)
            {
                S2CEntryBoardItemUnreadyNtc obj = new S2CEntryBoardItemUnreadyNtc();
                return obj;
            }
        }
    }
}
