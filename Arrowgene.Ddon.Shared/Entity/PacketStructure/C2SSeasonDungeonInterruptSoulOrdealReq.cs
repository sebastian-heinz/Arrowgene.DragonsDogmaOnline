using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonInterruptSoulOrdealReq : IPacketStructure
    {
        public C2SSeasonDungeonInterruptSoulOrdealReq()
        {
        }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_INTERRUPT_SOUL_ORDEAL_REQ;

        public uint EpitaphId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonInterruptSoulOrdealReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonInterruptSoulOrdealReq obj)
            {
                WriteUInt32(buffer, obj.EpitaphId);
            }

            public override C2SSeasonDungeonInterruptSoulOrdealReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonInterruptSoulOrdealReq obj = new C2SSeasonDungeonInterruptSoulOrdealReq();
                obj.EpitaphId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
