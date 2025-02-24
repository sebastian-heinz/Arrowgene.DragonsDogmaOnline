using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEntryBoardEntryBoardItemInfoChangeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_CHANGE_REQ;

        public C2SEntryBoardEntryBoardItemInfoChangeReq()
        {
            Param = new CDataEntryItemParam();
        }

        public string Password { get; set; } = string.Empty;
        public CDataEntryItemParam Param {  get; set; }

        public class Serializer : PacketEntitySerializer<C2SEntryBoardEntryBoardItemInfoChangeReq>
        {
            public override void Write(IBuffer buffer, C2SEntryBoardEntryBoardItemInfoChangeReq obj)
            {
                WriteMtString(buffer, obj.Password);
                WriteEntity(buffer, obj.Param);
            }

            public override C2SEntryBoardEntryBoardItemInfoChangeReq Read(IBuffer buffer)
            {
                C2SEntryBoardEntryBoardItemInfoChangeReq obj = new C2SEntryBoardEntryBoardItemInfoChangeReq();
                obj.Password = ReadMtString(buffer);
                obj.Param = ReadEntity<CDataEntryItemParam>(buffer);
                return obj;
            }
        }
    }
}
