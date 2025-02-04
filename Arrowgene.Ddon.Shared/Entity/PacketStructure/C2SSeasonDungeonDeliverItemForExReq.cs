using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonDeliverItemForExReq : IPacketStructure
    {
        public C2SSeasonDungeonDeliverItemForExReq()
        {
            ItemList = new List<CDataSoulOrdealItemInfo>();
        }

        public uint EpitaphId { get; set; }
        public List<CDataSoulOrdealItemInfo> ItemList { get; set; }
        public bool Unk0 { get; set; }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_DELIVER_ITEM_FOR_EX_REQ;

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonDeliverItemForExReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonDeliverItemForExReq obj)
            {
                WriteUInt32(buffer, obj.EpitaphId);
                WriteEntityList(buffer, obj.ItemList);
                WriteBool(buffer, obj.Unk0);
            }

            public override C2SSeasonDungeonDeliverItemForExReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonDeliverItemForExReq obj = new C2SSeasonDungeonDeliverItemForExReq();
                obj.EpitaphId = ReadUInt32(buffer);
                obj.ItemList = ReadEntityList<CDataSoulOrdealItemInfo>(buffer);
                obj.Unk0 = ReadBool(buffer);

                return obj;
            }
        }
    }
}
