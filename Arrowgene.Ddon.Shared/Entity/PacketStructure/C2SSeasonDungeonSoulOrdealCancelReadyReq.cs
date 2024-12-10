using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonSoulOrdealCancelReadyReq : IPacketStructure
    {
        public C2SSeasonDungeonSoulOrdealCancelReadyReq()
        {
        }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_SOUL_ORDEAL_CANCEL_READY_REQ;

        public uint Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonSoulOrdealCancelReadyReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonSoulOrdealCancelReadyReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
            }

            public override C2SSeasonDungeonSoulOrdealCancelReadyReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonSoulOrdealCancelReadyReq obj = new C2SSeasonDungeonSoulOrdealCancelReadyReq();
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
