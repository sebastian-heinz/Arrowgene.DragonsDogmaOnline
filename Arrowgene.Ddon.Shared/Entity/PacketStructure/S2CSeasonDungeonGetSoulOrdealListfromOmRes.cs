using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonGetSoulOrdealListfromOmRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_GET_SOUL_ORDEAL_LIST_FROM_OM_RES;

        public S2CSeasonDungeonGetSoulOrdealListfromOmRes()
        {
            ElementParamList = new List<CDataSoulOrdealElementParam>();
        }

        public SoulOrdealOrderState Type { get; set; }
        public List<CDataSoulOrdealElementParam> ElementParamList { get; set; }
        public bool Unk2 { get; set; }
        public byte Unk3 { get; set; }
        public bool Unk4 { get; set; }
        public byte Unk5 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonGetSoulOrdealListfromOmRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonGetSoulOrdealListfromOmRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.Type);
                WriteEntityList(buffer, obj.ElementParamList);
                WriteBool(buffer, obj.Unk2);
                WriteByte(buffer, obj.Unk3);
                WriteBool(buffer, obj.Unk4);
                WriteByte(buffer, obj.Unk5);
            }

            public override S2CSeasonDungeonGetSoulOrdealListfromOmRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonGetSoulOrdealListfromOmRes obj = new S2CSeasonDungeonGetSoulOrdealListfromOmRes();
                ReadServerResponse(buffer, obj);
                obj.Type = (SoulOrdealOrderState) ReadByte(buffer);
                obj.ElementParamList = ReadEntityList<CDataSoulOrdealElementParam>(buffer);
                obj.Unk2 = ReadBool(buffer);
                obj.Unk3 = ReadByte(buffer);
                obj.Unk4 = ReadBool(buffer);
                obj.Unk5 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
