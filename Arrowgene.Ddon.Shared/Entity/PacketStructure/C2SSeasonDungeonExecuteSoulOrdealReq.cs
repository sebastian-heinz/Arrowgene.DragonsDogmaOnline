using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSeasonDungeonExecuteSoulOrdealReq : IPacketStructure
    {
        public C2SSeasonDungeonExecuteSoulOrdealReq()
        {
            TrialCost = new List<CDataSoulOrdealItemInfo>();
            LayoutId = new CDataStageLayoutId();
        }

        public PacketId Id => PacketId.C2S_SEASON_DUNGEON_EXECUTE_SOUL_ORDEAL_REQ;

        public uint TrialId { get; set; }
        public List<CDataSoulOrdealItemInfo> TrialCost { get; set; }
        public CDataStageLayoutId LayoutId {get; set;}
        public uint PosId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSeasonDungeonExecuteSoulOrdealReq>
        {
            public override void Write(IBuffer buffer, C2SSeasonDungeonExecuteSoulOrdealReq obj)
            {
                WriteUInt32(buffer, obj.TrialId);
                WriteEntityList(buffer, obj.TrialCost);
                WriteEntity(buffer, obj.LayoutId);
                WriteUInt32(buffer, obj.PosId);
            }

            public override C2SSeasonDungeonExecuteSoulOrdealReq Read(IBuffer buffer)
            {
                C2SSeasonDungeonExecuteSoulOrdealReq obj = new C2SSeasonDungeonExecuteSoulOrdealReq();
                obj.TrialId = ReadUInt32(buffer);
                obj.TrialCost = ReadEntityList<CDataSoulOrdealItemInfo>(buffer);
                obj.LayoutId = ReadEntity<CDataStageLayoutId>(buffer);
                obj.PosId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
