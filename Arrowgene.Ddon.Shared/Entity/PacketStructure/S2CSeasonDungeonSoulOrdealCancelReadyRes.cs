using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonSoulOrdealCancelReadyRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_SOUL_ORDEAL_CANCEL_READY_RES;

        public S2CSeasonDungeonSoulOrdealCancelReadyRes()
        {
        }

        public bool Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonSoulOrdealCancelReadyRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonSoulOrdealCancelReadyRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteBool(buffer, obj.Unk0);
            }

            public override S2CSeasonDungeonSoulOrdealCancelReadyRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonSoulOrdealCancelReadyRes obj = new S2CSeasonDungeonSoulOrdealCancelReadyRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadBool(buffer);
                return obj;
            }
        }
    }
}
