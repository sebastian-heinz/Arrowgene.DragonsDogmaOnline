using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardItemInfoChangeNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_CHANGE_NTC;

        public S2CEntryBoardEntryBoardItemInfoChangeNtc()
        {
            EntryItemData = new CDataEntryItem();
        }

        public ulong BoardId {  get; set; }
        public CDataEntryItem EntryItemData { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardItemInfoChangeNtc>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardItemInfoChangeNtc obj)
            {
                WriteUInt64(buffer, obj.BoardId);
                WriteEntity(buffer, obj.EntryItemData);
            }

            public override S2CEntryBoardEntryBoardItemInfoChangeNtc Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardItemInfoChangeNtc obj = new S2CEntryBoardEntryBoardItemInfoChangeNtc();
                obj.BoardId = ReadUInt64(buffer);
                obj.EntryItemData = ReadEntity<CDataEntryItem>(buffer);
                return obj;
            }
        }
    }
}
