using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemMoveItemReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_MOVE_ITEM_REQ;

        public byte SourceGameStorageType { get; set; }
        public List<CDataMoveItemUIDFromTo> ItemUIDList { get; set; } = new();
        
        public class Serializer : PacketEntitySerializer<C2SItemMoveItemReq>
        {
            public override void Write(IBuffer buffer, C2SItemMoveItemReq obj)
            {
                WriteByte(buffer, obj.SourceGameStorageType);
                WriteEntityList<CDataMoveItemUIDFromTo>(buffer, obj.ItemUIDList);
            }
        
            public override C2SItemMoveItemReq Read(IBuffer buffer)
            {
                C2SItemMoveItemReq obj = new C2SItemMoveItemReq();
                obj.SourceGameStorageType = ReadByte(buffer);
                obj.ItemUIDList = ReadEntityList<CDataMoveItemUIDFromTo>(buffer);
                return obj;
            }
        }
    }
}
