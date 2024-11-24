using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonReceiveSoulOrdealRewardReq : IPacketStructure
    {
        public C2SSeasonDungeonReceiveSoulOrdealRewardReq()
        {
            LayoutId = new CDataStageLayoutId();
            RewardList = new List<CDataSoulOrdealRewardItem>();
        }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_REQ;

        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }
        public List<CDataSoulOrdealRewardItem> RewardList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonReceiveSoulOrdealRewardReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonReceiveSoulOrdealRewardReq obj)
            {
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
                WriteEntityList(buffer, obj.RewardList);
            }

            public override C2SSeasonDungeonReceiveSoulOrdealRewardReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonReceiveSoulOrdealRewardReq obj = new C2SSeasonDungeonReceiveSoulOrdealRewardReq();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                obj.RewardList = ReadEntityList<CDataSoulOrdealRewardItem>(buffer);
                return obj;
            }
        }
    }
}
