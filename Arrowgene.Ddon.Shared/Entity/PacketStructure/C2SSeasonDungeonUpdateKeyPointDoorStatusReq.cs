using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonUpdateKeyPointDoorStatusReq : IPacketStructure
    {
        public C2SSeasonDungeonUpdateKeyPointDoorStatusReq()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_UPDATE_KEY_POINT_DOOR_STATUS_REQ;

        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }
        public byte Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonUpdateKeyPointDoorStatusReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonUpdateKeyPointDoorStatusReq obj)
            {
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
                WriteByte(buffer, obj.Unk0);
            }

            public override C2SSeasonDungeonUpdateKeyPointDoorStatusReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonUpdateKeyPointDoorStatusReq obj = new C2SSeasonDungeonUpdateKeyPointDoorStatusReq();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                obj.Unk0 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
