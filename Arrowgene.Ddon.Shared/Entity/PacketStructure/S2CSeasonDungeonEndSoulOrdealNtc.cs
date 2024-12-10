using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonEndSoulOrdealNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SEASON_DUNGEON_END_SOUL_ORDEAL_NTC;

        public S2CSeasonDungeonEndSoulOrdealNtc()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public SoulOrdealEndState EndState { get; set; }
        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }
        public SoulOrdealOmState EpitaphState { get; set; }
        public uint Unk3 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonEndSoulOrdealNtc>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonEndSoulOrdealNtc obj)
            {
                WriteByte(buffer, (byte) obj.EndState);
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
                WriteByte(buffer, (byte) obj.EpitaphState);
                WriteUInt32(buffer, obj.Unk3);

            }

            public override S2CSeasonDungeonEndSoulOrdealNtc Read(IBuffer buffer)
            {
                S2CSeasonDungeonEndSoulOrdealNtc obj = new S2CSeasonDungeonEndSoulOrdealNtc();
                obj.EndState = (SoulOrdealEndState) ReadByte(buffer);
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                obj.EpitaphState = (SoulOrdealOmState) ReadByte(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
