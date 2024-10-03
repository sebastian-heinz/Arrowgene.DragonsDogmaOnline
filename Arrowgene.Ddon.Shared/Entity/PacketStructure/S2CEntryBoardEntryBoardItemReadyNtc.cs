using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemReadyNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_READY_NTC;

        public S2CEntryBoardEntryBoardItemReadyNtc()
        {
        }

        public uint MaxMember {  get; set; }
        public ushort TimeOut {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemReadyNtc>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemReadyNtc obj)
            {
                WriteUInt32(buffer, obj.MaxMember);
                WriteUInt16(buffer, obj.TimeOut);
            }

            public override S2CEntryBoardEntryBoardItemReadyNtc Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemReadyNtc obj = new S2CEntryBoardEntryBoardItemReadyNtc();
                obj.MaxMember = ReadUInt32(buffer);
                obj.TimeOut = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
