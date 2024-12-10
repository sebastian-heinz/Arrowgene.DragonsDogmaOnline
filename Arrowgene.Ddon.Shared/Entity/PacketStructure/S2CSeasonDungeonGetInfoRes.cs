using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonGetInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_GET_INFO_RES;

        public S2CSeasonDungeonGetInfoRes()
        {
            DungeonInfo = new CDataSeasonDungeonInfo();
            DungeonSections = new List<CDataSeasonDungeonSection>();
        }

        public CDataSeasonDungeonInfo DungeonInfo { get; set; }
        public List<CDataSeasonDungeonSection> DungeonSections { get; set;}
        public byte Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonGetInfoRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonGetInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.DungeonInfo);
                WriteEntityList(buffer, obj.DungeonSections);
                WriteByte(buffer, obj.Unk0);
            }

            public override S2CSeasonDungeonGetInfoRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonGetInfoRes obj = new S2CSeasonDungeonGetInfoRes();
                ReadServerResponse(buffer, obj);
                obj.DungeonInfo = ReadEntity<CDataSeasonDungeonInfo>(buffer);
                obj.DungeonSections = ReadEntityList<CDataSeasonDungeonSection>(buffer);
                obj.Unk0 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
