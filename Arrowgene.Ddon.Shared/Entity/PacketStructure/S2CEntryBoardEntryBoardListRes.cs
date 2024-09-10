using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEntryBoardEntryBoardListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ENTRY_BOARD_ENTRY_BOARD_LIST_RES;

        public S2CEntryBoardEntryBoardListRes()
        {
            EntryList = new List<CDataEntryBoardListParam>();
        }

        public List<CDataEntryBoardListParam> EntryList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CEntryBoardEntryBoardListRes>
        {
            public override void Write(IBuffer buffer, S2CEntryBoardEntryBoardListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.EntryList);
            }

            public override S2CEntryBoardEntryBoardListRes Read(IBuffer buffer)
            {
                S2CEntryBoardEntryBoardListRes obj = new S2CEntryBoardEntryBoardListRes();
                ReadServerResponse(buffer, obj);
                obj.EntryList = ReadEntityList<CDataEntryBoardListParam>(buffer);
                return obj;
            }
        }
    }
}
