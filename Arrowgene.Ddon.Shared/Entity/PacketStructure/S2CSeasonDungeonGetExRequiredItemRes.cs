using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonGetExRequiredItemRes : ServerResponse
    {
        public S2CSeasonDungeonGetExRequiredItemRes()
        {
            ItemList = new List<CDataSoulOrdealItem>();
        }

        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_GET_EX_REQUIRED_ITEM_RES;

        public bool Unk0 { get; set; }
        public bool Unk1 { get; set; }
        public List<CDataSoulOrdealItem> ItemList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonGetExRequiredItemRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonGetExRequiredItemRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteBool(buffer, obj.Unk0);
                WriteBool(buffer, obj.Unk1);
                WriteEntityList(buffer, obj.ItemList);
            }

            public override S2CSeasonDungeonGetExRequiredItemRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonGetExRequiredItemRes obj = new S2CSeasonDungeonGetExRequiredItemRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadBool(buffer);
                obj.Unk1 = ReadBool(buffer);
                obj.ItemList = ReadEntityList<CDataSoulOrdealItem>(buffer);
                return obj;
            }
        }
    }
}
