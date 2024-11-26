using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonGetSoulOrdealRewardListForViewReq : IPacketStructure
    {
        public C2SSeasonDungeonGetSoulOrdealRewardListForViewReq()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_FOR_VIEW_REQ;

        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }
        public uint EpitaphId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonGetSoulOrdealRewardListForViewReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonGetSoulOrdealRewardListForViewReq obj)
            {
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
                WriteUInt32(buffer, obj.EpitaphId);
            }

            public override C2SSeasonDungeonGetSoulOrdealRewardListForViewReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonGetSoulOrdealRewardListForViewReq obj = new C2SSeasonDungeonGetSoulOrdealRewardListForViewReq();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                obj.EpitaphId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
