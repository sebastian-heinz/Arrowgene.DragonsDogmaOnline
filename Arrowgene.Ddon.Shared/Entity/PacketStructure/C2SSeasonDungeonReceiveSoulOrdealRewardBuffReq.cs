using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonReceiveSoulOrdealRewardBuffReq : IPacketStructure
    {
        public C2SSeasonDungeonReceiveSoulOrdealRewardBuffReq()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_BUFF_REQ;

        public CDataStageLayoutId LayoutId { get; set; } // Of the Trial OM
        public uint PosId { get; set; }
        public uint BuffId { get; set; } // Seems to correspond to unk1 of the buff
        public uint Unk2 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonReceiveSoulOrdealRewardBuffReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonReceiveSoulOrdealRewardBuffReq obj)
            {
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
                WriteUInt32(buffer, obj.BuffId);
                WriteUInt32(buffer, obj.Unk2);
            }

            public override C2SSeasonDungeonReceiveSoulOrdealRewardBuffReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonReceiveSoulOrdealRewardBuffReq obj = new C2SSeasonDungeonReceiveSoulOrdealRewardBuffReq();
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                obj.BuffId = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
