using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardItemTimeoutTimerNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_ENTRY_BOARD_ITEM_TIMEOUT_TIMER_NTC;

        public S2CEntryBoardItemTimeoutTimerNtc()
        {
        }

        public ushort TimeOut {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardItemTimeoutTimerNtc>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardItemTimeoutTimerNtc obj)
            {
                WriteUInt16(buffer, obj.TimeOut);
            }

            public override S2CEntryBoardItemTimeoutTimerNtc Read(IBuffer buffer)
            {
                S2CEntryBoardItemTimeoutTimerNtc obj = new S2CEntryBoardItemTimeoutTimerNtc();
                obj.TimeOut = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
