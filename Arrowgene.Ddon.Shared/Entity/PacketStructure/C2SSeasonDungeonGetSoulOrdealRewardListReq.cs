using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonGetSoulOrdealRewardListReq : IPacketStructure
    {
        public C2SSeasonDungeonGetSoulOrdealRewardListReq()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_REQ;

        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonGetSoulOrdealRewardListReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonGetSoulOrdealRewardListReq obj)
            {
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
            }

            public override C2SSeasonDungeonGetSoulOrdealRewardListReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonGetSoulOrdealRewardListReq obj = new C2SSeasonDungeonGetSoulOrdealRewardListReq();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
