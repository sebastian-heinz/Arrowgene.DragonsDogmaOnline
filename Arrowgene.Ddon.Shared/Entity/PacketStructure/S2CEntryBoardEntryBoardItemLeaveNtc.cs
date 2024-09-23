using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemLeaveNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_LEAVE_NTC;

        public S2CEntryBoardEntryBoardItemLeaveNtc()
        {
        }

        public EntryBoardLeaveType LeaveType {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemLeaveNtc>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemLeaveNtc obj)
            {
                WriteUInt32(buffer, (uint) obj.LeaveType);
            }

            public override S2CEntryBoardEntryBoardItemLeaveNtc Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemLeaveNtc obj = new S2CEntryBoardEntryBoardItemLeaveNtc();
                obj.LeaveType = (EntryBoardLeaveType) ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

