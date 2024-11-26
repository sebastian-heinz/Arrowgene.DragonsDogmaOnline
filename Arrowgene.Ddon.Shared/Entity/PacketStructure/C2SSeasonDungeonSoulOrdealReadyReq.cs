using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonSoulOrdealReadyReq : IPacketStructure
    {
        public C2SSeasonDungeonSoulOrdealReadyReq()
        {
            LayoutId = new CDataStageLayoutId();
        }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_SOUL_ORDEAL_READY_REQ;

        public uint TrialId { get; set; }
        public CDataStageLayoutId LayoutId { get; set; }
        public uint PosId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonSoulOrdealReadyReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonSoulOrdealReadyReq obj)
            {
                WriteUInt32(buffer, obj.TrialId);
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
            }

            public override C2SSeasonDungeonSoulOrdealReadyReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonSoulOrdealReadyReq obj = new C2SSeasonDungeonSoulOrdealReadyReq();
                obj.TrialId = ReadUInt32(buffer);
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
