using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemReserveNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_RESERVE_NTC;

        public S2CEntryBoardEntryBoardItemReserveNtc()
        {
        }

        public uint NowMember { get; set; }
        public uint MaxMember { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemReserveNtc>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemReserveNtc obj)
            {
                WriteUInt32(buffer, obj.NowMember);
                WriteUInt32(buffer, obj.MaxMember);
            }

            public override S2CEntryBoardEntryBoardItemReserveNtc Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemReserveNtc obj = new S2CEntryBoardEntryBoardItemReserveNtc();
                obj.NowMember = ReadUInt32(buffer);
                obj.MaxMember = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
