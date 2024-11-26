using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonSetOmStateNtc : IPacketStructure
    {
        public S2CSeasonDungeonSetOmStateNtc()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public PacketId Id => PacketId.S2C_SEASON_DUNGEON_SET_OM_STATE_NTC;

        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }
        public SoulOrdealOmState State { get; set; } // Changes marker on map and unlocks doors

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonSetOmStateNtc>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonSetOmStateNtc obj)
            {
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
                WriteByte(buffer, (byte) obj.State);
            }

            public override S2CSeasonDungeonSetOmStateNtc Read(IBuffer buffer)
            {
                S2CSeasonDungeonSetOmStateNtc obj = new S2CSeasonDungeonSetOmStateNtc();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                obj.State = (SoulOrdealOmState) ReadByte(buffer);
                return obj;
            }
        }
    }
}
