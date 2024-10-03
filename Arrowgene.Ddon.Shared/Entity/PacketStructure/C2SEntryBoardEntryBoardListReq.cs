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
            BoardIdList = new List<CDataCommonU64>(); // List of Board ID's?
        }

        public List<CDataCommonU64> BoardIdList {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardEntryBoardListReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardEntryBoardListReq obj)
            {
                WriteEntityList(buffer, obj.BoardIdList);
            }

            public override C2SEntryBoardEntryBoardListReq Read(IBuffer buffer)
            {
                C2SEntryBoardEntryBoardListReq obj = new C2SEntryBoardEntryBoardListReq();
                obj.BoardIdList = ReadEntityList<CDataCommonU64>(buffer);
                return obj;
            }
        }
    }
}
