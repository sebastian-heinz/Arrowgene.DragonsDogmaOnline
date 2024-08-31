using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEntryBoardEntryBoardListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ENTRY_BOARD_ENTRY_BOARD_LIST_REQ;

        public C2SEntryBoardEntryBoardListReq()
        {
            Unk0List = new List<CDataCommonU64>(); // List of Entry ID's?
        }

        public List<CDataCommonU64> Unk0List {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardEntryBoardListReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardEntryBoardListReq obj)
            {
                WriteEntityList(buffer, obj.Unk0List);
            }

            public override C2SEntryBoardEntryBoardListReq Read(IBuffer buffer)
            {
                C2SEntryBoardEntryBoardListReq obj = new C2SEntryBoardEntryBoardListReq();
                obj.Unk0List = ReadEntityList<CDataCommonU64>(buffer);
                return obj;
            }
        }
    }
}
