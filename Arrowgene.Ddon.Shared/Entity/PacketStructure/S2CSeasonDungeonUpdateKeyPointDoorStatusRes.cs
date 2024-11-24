using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonUpdateKeyPointDoorStatusRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_UPDATE_KEY_POINT_DOOR_STATUS_RES;

        public S2CSeasonDungeonUpdateKeyPointDoorStatusRes()
        {
            Unk1 = string.Empty;
        }

        public byte Unk0 { get; set; }
        public string Unk1 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonUpdateKeyPointDoorStatusRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonUpdateKeyPointDoorStatusRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, obj.Unk0);
                WriteMtString(buffer, obj.Unk1);
            }

            public override S2CSeasonDungeonUpdateKeyPointDoorStatusRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonUpdateKeyPointDoorStatusRes obj = new S2CSeasonDungeonUpdateKeyPointDoorStatusRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadMtString(buffer);
                return obj;
            }
        }
    }
}

